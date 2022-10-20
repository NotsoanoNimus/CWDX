using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using ToneBaker.PCM;


namespace CWDX {
    public partial class TransmitForm : Form {

        private Task<string> MorseTransmitTask;
        private AudioHandler AudioHandler { get; set; }
        private CancellationTokenSource AudioCancellationToken;


        public TransmitForm() {
            this.InitializeComponent();
            
            // Populate the cbWaveForm list and attempt to select the SIN wave.
            foreach ( var e in Enum.GetValues(typeof(WaveGenerator.WaveType)) )
                this.cbWaveType.Items.Add( e );

            // Set up configuration information.
            this.PopulateFromConfiguration();

            // Engage these two actions just once.
            this.tbGain_Scroll(null, null);
            this.tbFreq_Scroll(null, null);

            // Set the static reference to the text field and expose it so other threads can access it.
            //   Hacky, but it gets the job done...
            Program.txLiveView = this.tTXLive;
            Program.pbTXProgress = this.pbTXTime;
        }


        // Refreshes all or a specific section of form elements/controls.
        private void PopulateFromConfiguration( int sectionId = -1) {
            // Identity.
            if ( sectionId == -1 || sectionId == 1 ) {
                this.tCallSignMe.Text = Properties.Settings.Default.CallSign ?? "";
                this.tNameMe.Text = Properties.Settings.Default.Name ?? "";
                this.tQTHMe.Text = Properties.Settings.Default.QTH ?? "";
                this.tGridMe.Text = Properties.Settings.Default.Grid ?? "";
            }

            // TX Settings.
            if ( sectionId == -1 || sectionId == 2 ) {
                string waveType = Properties.Settings.Default.TXWaveType;
                try {
                    for ( int i = 0; i < this.cbWaveType.Items.Count; i++ )
                        if ( this.cbWaveType.Items[i].ToString() == waveType )
                            this.cbWaveType.SelectedIndex = i;
                    //this.cbWaveType.SelectedIndex = this.cbWaveType.Items.IndexOf( (WaveGenerator.WaveType)waveType );
                } catch ( Exception e ) {
                    this.cbWaveType.SelectedIndex = 0;
                }
                
                this.tbTXWPM.Value = Properties.Settings.Default.TXWPM;
                this.lblTXWPM.Text = string.Format("{0} WPM", this.tbTXWPM.Value);
                
                this.tbFreq.Value = Properties.Settings.Default.TXFreq;
                this.lblFreq.Text = string.Format("Frequency: {0}Hz", this.tbFreq.Value);

                this.tbGain.Value = (int)(Properties.Settings.Default.TXVolume);
                this.lblGain.Text = string.Format("Volume: {0}%", this.tbGain.Value);
                
                this.chbKeepText.Checked = Properties.Settings.Default.TXPreserve;
            }
        }


        /// <summary>
        /// Disable/Lock all transmit fields during transmission.
        /// </summary>
        private void TXLock( bool doLock ) {
            this.btnSendTX.Enabled = this.tTXText.Enabled = this.tbTXWPM.Enabled = !doLock;
            this.btnCancelTX.Enabled = doLock;
        }


        /// <summary>
        /// Wrapper method which actually plays the encoded Morse stream. Called as a Task.
        /// </summary>
        /// <param name="txText">Text to transmit.</param>
        /// <param name="wpmValue">Speed in words per minute.</param>
        /// <param name="cancelToken">Cancellation token.</param>
        /// <returns>The generated Morse string from the player.</returns>
        private string TransmitText( string txText, int wpmValue, CancellationToken cancelToken) {
            try {

                (MorseCharacter[] seq, string outputText) = Morse.GetMorseSequence( txText );

                this.AudioHandler.PlayMorseStream( wpmValue, seq, cancelToken );

                return outputText;

            } catch ( Exception e ) {

                // Add error information to the interface and cancel the transmission.
                MessageBox.Show( $"\nProblem creating the Morse audio transmission:\n{e.Message}",
                    "Transmission Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning );

            }

            return null;
        }


        // Form load.
        private void TransmitForm_Load(object sender, EventArgs e) {
            // Set up the AudioHandler object.
            this.AudioHandler = new AudioHandler() {
                Frequency = this.tbFreq.Value,
                Gain = this.tbGain.Value,
                WaveType = (WaveGenerator.WaveType)this.cbWaveType.SelectedIndex
            };
        }

        // Transmit button clicked.
        private async void btnSendTX_Click(object sender, EventArgs e) {
            if ( string.IsNullOrEmpty( this.tTXText.Text.Trim() ) ) return;

            // Lock the TX interface.
            this.TXLock(true);

            // Start a new TX log entry.
            this.tTXLive.Text += string.Format( "{0} UTC: ", System.DateTime.UtcNow.ToString() );

            // Create a new Task/thread for playing the sound.
            int txWPM = this.tbTXWPM.Value;
            
            this.AudioCancellationToken = new CancellationTokenSource();
            var ct = this.AudioCancellationToken.Token;

            this.MorseTransmitTask = new Task<string>( () => this.TransmitText( this.tTXText.Text, txWPM, ct ), ct );

            // Run it, wait (or catch a cancellation), then dispose of it.
            this.MorseTransmitTask?.Start();
            await MorseTransmitTask;
            this.MorseTransmitTask?.Dispose();

            // Append transmitted text.
            this.tTXLive.Text += $"{(ct.IsCancellationRequested ? "[[CANCELLED]]" : "")}"
                + Environment.NewLine
                + $"OriginalMessage: {this.tTXText.Text}"
                + Environment.NewLine + Environment.NewLine;

            // Refresh values where applicable.
            if ( !this.chbKeepText.Checked )
                this.tTXText.Text = "";

            this.pbTXTime.Value = 0;
            
            // Unlock the TX interface.
            this.TXLock(false);
        }

        // Cancellation of the Transmit task.
        private void btnCancelTX_Click( object sender, EventArgs e ) {
            if ( !this.MorseTransmitTask.IsCompleted ) this.AudioCancellationToken.Cancel();

            this.TXLock( false );
        }

        // Combo-box selection of wave type.
        private void cbWaveType_SelectedIndexChanged( object sender, EventArgs e ) {
            if ( this.AudioHandler == null ) return;

            this.AudioHandler.WaveType = (WaveGenerator.WaveType)this.cbWaveType.SelectedIndex;
        }

        // WPM slider change.
        private void tbTXWPM_Scroll( object sender, EventArgs e ) => this.lblTXWPM.Text = $"{this.tbTXWPM.Value} WPM";

        // Frequency slider change.
        private void tbFreq_Scroll( object sender, EventArgs e ) {
            if ( this.AudioHandler == null ) return;

            this.AudioHandler.Frequency = this.tbFreq.Value;

            this.lblFreq.Text = $"Frequency: {this.tbFreq.Value}Hz";
        }

        // Gain slider change.
        private void tbGain_Scroll( object sender, EventArgs e ) {
            if ( this.AudioHandler == null ) return;

            this.AudioHandler.Gain = this.tbGain.Value;

            this.lblGain.Text = $"Volume: {this.tbGain.Value}%";
        }

        // TX Text Live view change.
        private void tTXLive_TextChanged( object sender, EventArgs e ) {
            this.tTXLive.SelectionStart = this.tTXLive.Text.Length;
            this.tTXLive.SelectionLength = 0;
            this.tTXLive.ScrollToCaret();
        }

        // Edit identity information button click.
        private void btnEditMe_Click( object sender, EventArgs e ) {
            if ( this.btnEditMe.Text == "Cancel" ) {
                // Already in "edit" mode, cancel was pressed. Hacky, but no need to add another boolean here to toggle I guess.
                this.btnSaveMe.Enabled = false;
                this.btnEditMe.Text = "Edit";

                this.tCallSignMe.Enabled = this.tQTHMe.Enabled = this.tNameMe.Enabled = this.tGridMe.Enabled = false;
                
                this.PopulateFromConfiguration( 1 );   // Repopulate the identity controls from the config
            } else {
                // Enable "edit" mode.
                this.btnSaveMe.Enabled = true;
                this.btnEditMe.Text = "Cancel";

                this.tCallSignMe.Enabled = this.tQTHMe.Enabled = this.tNameMe.Enabled = this.tGridMe.Enabled = true;
            }
        }

        // Save Identity fields.
        private void btnSaveMe_Click( object sender, EventArgs e ) {
            Properties.Settings.Default.CallSign = this.tCallSignMe.Text;
            Properties.Settings.Default.Grid = this.tGridMe.Text;
            Properties.Settings.Default.QTH = this.tQTHMe.Text;
            Properties.Settings.Default.Name = this.tNameMe.Text;
            Properties.Settings.Default.Save();

            this.tCallSignMe.Enabled = this.tQTHMe.Enabled = this.tNameMe.Enabled = this.tGridMe.Enabled = false;
            this.btnEditMe.Text = "Edit";
        }

        // Checkbox for keeping text change (does nothing right now; TODO).
        private void chbKeepText_CheckedChanged( object sender, EventArgs e ) { }

        // Save button for TX settings click.
        private void btnSaveTX_Click( object sender, EventArgs e ) {
            Properties.Settings.Default.TXWaveType = ((WaveGenerator.WaveType)this.cbWaveType.SelectedItem).ToString();
            Properties.Settings.Default.TXWPM = this.tbTXWPM.Value;
            Properties.Settings.Default.TXFreq = this.tbFreq.Value;
            Properties.Settings.Default.TXVolume = this.tbGain.Value;
            Properties.Settings.Default.TXPreserve = this.chbKeepText.Checked;
            Properties.Settings.Default.Save();
        }

        // Revert TX settings click.
        private void btnRevertTX_Click(object sender, EventArgs e) => this.PopulateFromConfiguration( 2 );
    }
}
