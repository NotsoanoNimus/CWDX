using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace CWDX {
    class AudioHandler {
        internal int freq; public int Frequency {
            get { return this.freq; }
            set {
                this.freq = value;
                this.outWave.Frequency = this.freq;
            }
        }
        internal double volume;  public double Gain {
            get { return this.volume; }
            set {
                this.volume = value;
                this.outWave.Gain = this.volume;
            }
        }
        internal SignalGeneratorType waveform;
        public SignalGeneratorType WaveType {
            get { return this.waveform; }
            set {
                this.waveform = value;
                this.outWave.Type = this.waveform;
            }
        }
        private SignalGenerator outWave;

        public AudioHandler() {
            this.outWave = new SignalGenerator() {
                Gain = 0.2,
                Frequency = Frequency,
                Type = SignalGeneratorType.Sin
            };
        }

        public void PlayStream(List<MorseSymbol> stream, CancellationToken cancelToken) {
            int index = 0;
            foreach(MorseSymbol sym in stream) {
                Program.mainForm.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                    try { Program.pbTXProgress.Value = (((index * 100) / stream.Count) + 1); } catch { }
                }); index++;
                if(cancelToken != null && cancelToken.IsCancellationRequested) { break; }
                if(sym.hasSound()) {
                    using(var wo = new WaveOut()) {
                        wo.Init(this.outWave);
                        wo.Play();
                        Thread.Sleep((int)sym.getDuration());
                        try {
                            Program.mainForm.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                                Program.txLiveView.AppendText(sym.getRepresentation());
                            });
                        } catch { }
                        wo.Stop();
                    }
                } else {
                    Thread.Sleep((int)sym.getDuration());
                    try {
                        if(sym.getRepresentation() == " " || sym.getRepresentation() == "  ") {
                            Program.mainForm.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                                Program.txLiveView.AppendText(sym.getRepresentation());
                            });
                        }
                    } catch { }
                }
            }
        }
    }
}
