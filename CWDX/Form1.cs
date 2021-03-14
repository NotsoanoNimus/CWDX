using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave.SampleProviders;

namespace CWDX {
    public partial class TransmitForm : Form {
        private Task<string> playMorse;
        private AudioHandler audioHandler;
        private CancellationTokenSource txCancelAudio;

        public TransmitForm() {
            InitializeComponent();
            // Populate the cbWaveForm list and attempt to select the SIN wave.
            foreach(SignalGeneratorType e in Enum.GetValues(typeof(SignalGeneratorType))) { this.cbWaveType.Items.Add(e); }
            // Set up the AudioHandler object.
            this.audioHandler = new AudioHandler(tbFreq.Value) {
                Frequency = tbFreq.Value, Gain = tbGain.Value, WaveType = SignalGeneratorType.Sin
            };
            // Engage these two actions just once.
            this.tbGain_Scroll(null, null); this.tbFreq_Scroll(null, null);
            this.cbWaveType.SelectedItem = SignalGeneratorType.Sin;
            // Set the static reference to the text field and expose it so other threads can access it...
            // WARNING!!! THIS IS DANGEROUS [but it works ):]
            Program.txLiveView = this.tTXLive;
            Program.pbTXProgress = this.pbTXTime;
            // Set up configuration information.
            this.PopulateFromConfiguration();



            var wavFormat = new WaveAudioFormat(16000, 16, 2);
            var wavType = WaveGenerator.WaveType.SINE;
            var lowTriad = WaveGenerator.CreateNewWave(wavFormat, wavType, 100.0, 3.2, 220);
            var medTriad = WaveGenerator.CreateNewWave(wavFormat, wavType, 100.0, 4.2, 261.6256);
            var highTriad = WaveGenerator.CreateNewWave(wavFormat, wavType, 100.0, 3.2, 329.6276);
            var chord = WaveAudioTools.MixSamples(wavFormat, 100.0, (10.0, lowTriad), (30.0, medTriad), (20.0, highTriad));
            WaveAudioTools.ChangeVolume(100.0, ref chord); // testing volume change
            var EAS = WaveGenerator.CreateNewWave(wavFormat, wavType, 100.0, 4.2, 1200);
            var CWTone = WaveGenerator.CreateNewWave(wavFormat, wavType, 100.0, 1.2, 780);
            WaveAudioTools.AppendSamples(ref chord, (100.0, EAS), (20.0, CWTone));
            // Play the sound and time its duration (to the best possible).
            System.Diagnostics.Stopwatch c = new System.Diagnostics.Stopwatch();
            using(var finalFile = new WaveStream(chord)) {
                using(System.IO.MemoryStream m = new System.IO.MemoryStream(finalFile.GetRawWaveStream())) {
                    using(System.Media.SoundPlayer player = new System.Media.SoundPlayer(m)) {
                        c.Start();
                        player.Play();
                        c.Stop();
                    }
                }
                // Show the time.
                //MessageBox.Show(c.ElapsedMilliseconds.ToString());
                // Write the file to the desktop for viewing in Audacity.
                System.IO.File.WriteAllBytes(Environment.GetEnvironmentVariable("USERPROFILE") + "\\Desktop\\testingCode.wav", finalFile.GetRawWaveStream());
            }
        }

        internal void PopulateFromConfiguration(bool specificSection = false, int sectionId = -1) {
            if(!specificSection || (specificSection && sectionId == 1)) {
                this.tCallSignMe.Text = Properties.Settings.Default.CallSign ?? "";
                this.tNameMe.Text = Properties.Settings.Default.Name ?? "";
                this.tQTHMe.Text = Properties.Settings.Default.QTH ?? "";
                this.tGridMe.Text = Properties.Settings.Default.Grid ?? "";
            }
            if(!specificSection || (specificSection && sectionId == 2)) {
                this.cbWaveType.SelectedItem = Properties.Settings.Default.TXWaveType;
                this.tbTXWPM.Value = Properties.Settings.Default.TXWPM;
                this.lblTXWPM.Text = string.Format("{0} WPM", this.tbTXWPM.Value);
                this.tbFreq.Value = Properties.Settings.Default.TXFreq;
                this.lblFreq.Text = string.Format("Frequency: {0}Hz", this.tbFreq.Value);
                this.tbGain.Value = (int)(Properties.Settings.Default.TXVolume * 100);
                this.lblGain.Text = string.Format("Volume: {0}%", this.tbGain.Value);
                this.chbKeepText.Checked = Properties.Settings.Default.TXPreserve;
            }
        }

        private async void btnSendTX_Click(object sender, EventArgs e) {
            // Skip if the text field is empty.
            if(string.IsNullOrEmpty(this.tTXText.Text)) { return; }
            // Lock the TX interface.
            TXLock(true);
            // Start a new TX log entry
            this.tTXLive.Text += string.Format("{0} UTC: ", System.DateTime.UtcNow.ToString());
            // Create a new Task/thread for playing the sound.
            string txText = this.tTXText.Text; int txWPM = this.tbTXWPM.Value;
            this.txCancelAudio = new CancellationTokenSource();
            var ct = this.txCancelAudio.Token;
            this.playMorse = new Task<string>(() => { return TransmitText(txText, txWPM, ct); }, ct);
            this.playMorse?.Start();
            await playMorse;
            this.playMorse?.Dispose();
            this.tTXLive.Text +=
                (ct.IsCancellationRequested ? "[[CANCELLED]]" : "")
                + Environment.NewLine + "Original Message: "
                + this.tTXText.Text + Environment.NewLine + Environment.NewLine;
            if(!this.chbKeepText.Checked) { this.tTXText.Text = ""; }
            this.pbTXTime.Value = 0;
            // Unlock the TX interface.
            TXLock(false);
        }

        private void TXLock(bool doLock) {
            this.btnSendTX.Enabled = this.tTXText.Enabled = this.tbTXWPM.Enabled = !doLock;
            this.btnCancelTX.Enabled = doLock;
        }

        private string TransmitText(string txText, int wpmValue, CancellationToken cancelToken) {
            try {
                (List<MorseSymbol> symbols, string outputText) = Morse.GetTimeSequence(txText, wpmValue);
                this.audioHandler.PlayWaveStream(symbols, cancelToken);
                //this.audioHandler.PlayStream(symbols, cancelToken);
                return outputText;
            } catch(Exception e) {
                // Add error information to the interface and cancel the transmission.
            }
            return "";
        }

        private void btnCancelTX_Click(object sender, EventArgs e) {
            if(!this.playMorse.IsCompleted) {
                this.txCancelAudio.Cancel();
            }
            TXLock(false);
        }

        private void cbWaveType_SelectedIndexChanged(object sender, EventArgs e) {
            if(this.audioHandler == null) { return; }
            this.audioHandler.WaveType = (SignalGeneratorType)(this.cbWaveType.SelectedIndex);
        }

        private void tbTXWPM_Scroll(object sender, EventArgs e) {
            this.lblTXWPM.Text = string.Format("{0} WPM", this.tbTXWPM.Value);
        }

        private void tbFreq_Scroll(object sender, EventArgs e) {
            if(this.audioHandler == null) { return; }
            this.audioHandler.Frequency = this.tbFreq.Value;
            this.lblFreq.Text = string.Format("Frequency: {0}Hz", this.tbFreq.Value);
        }

        private void tbGain_Scroll(object sender, EventArgs e) {
            if(this.audioHandler == null) { return; }
            this.audioHandler.Gain = (double)((double)this.tbGain.Value * 0.01);
            this.lblGain.Text = string.Format("Volume: {0}%", this.tbGain.Value);
        }

        private void tTXLive_TextChanged(object sender, EventArgs e) {
            this.tTXLive.SelectionStart = this.tTXLive.Text.Length;
            this.tTXLive.SelectionLength = 0;
            this.tTXLive.ScrollToCaret();
        }

        private void btnEditMe_Click(object sender, EventArgs e) {
            if(this.btnEditMe.Text == "Cancel") {
                // Already in "edit" mode, cancel was pressed.
                this.btnSaveMe.Enabled = false;
                this.btnEditMe.Text = "Edit";
                this.tCallSignMe.Enabled = this.tQTHMe.Enabled = this.tNameMe.Enabled = this.tGridMe.Enabled = false;
                this.PopulateFromConfiguration(specificSection: true, 1);   //repopulate the text fields from its config
            } else {
                // Enable "edit" mode.
                this.btnSaveMe.Enabled = true;
                this.btnEditMe.Text = "Cancel";
                this.tCallSignMe.Enabled = this.tQTHMe.Enabled = this.tNameMe.Enabled = this.tGridMe.Enabled = true;
            }
        }

        private void btnSaveMe_Click(object sender, EventArgs e) {
            Properties.Settings.Default.CallSign = this.tCallSignMe.Text;
            Properties.Settings.Default.Grid = this.tGridMe.Text;
            Properties.Settings.Default.QTH = this.tQTHMe.Text;
            Properties.Settings.Default.Name = this.tNameMe.Text;
            Properties.Settings.Default.Save();
            this.tCallSignMe.Enabled = this.tQTHMe.Enabled = this.tNameMe.Enabled = this.tGridMe.Enabled = false;
            this.btnEditMe.Text = "Edit";
        }

        private void chbKeepText_CheckedChanged(object sender, EventArgs e) {
        }

        private void btnSaveTX_Click(object sender, EventArgs e) {
            Properties.Settings.Default.TXWaveType = ((SignalGeneratorType)this.cbWaveType.SelectedItem).ToString();
            Properties.Settings.Default.TXWPM = this.tbTXWPM.Value;
            Properties.Settings.Default.TXFreq = this.tbFreq.Value;
            Properties.Settings.Default.TXVolume = (double)this.tbGain.Value / 100;
            Properties.Settings.Default.TXPreserve = this.chbKeepText.Checked;
            Properties.Settings.Default.Save();
        }

        private void btnRevertTX_Click(object sender, EventArgs e) {
            this.PopulateFromConfiguration(specificSection: true, 2);
        }
    }
}
