using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace CWDX {
    /// <summary>
    /// Initializes an audio processor and waveform generator for playing a sequence of MorseSymbol objects.
    /// </summary>
    class AudioHandler {

        /// <summary>
        /// Adjusts the frequency of the NAudio output generator's waveform.
        /// </summary>
        public int Frequency {
            get { return this.freq; }
            set {
                this.freq = value;
                this.outWave.Frequency = this.freq;
            }
        }
        internal int freq;

        /// <summary>
        /// Adjusts the volume of the NAudio output generator.
        /// </summary>
        public double Gain {
            get { return this.volume; }
            set {
                this.volume = value;
                this.outWave.Gain = this.volume;
            }
        }
        internal double volume;

        /// <summary>
        /// The wave-form type used to generate the audio; can be changed dynamically.
        /// </summary>
        public SignalGeneratorType WaveType {
            get { return this.waveform; }
            set {
                this.waveform = value;
                this.outWave.Type = this.waveform;
            }
        }
        internal SignalGeneratorType waveform;

        // Used to handle NAudio output processing.
        private SignalGenerator outWave;

        /// <summary>
        /// Initializes the NAudio generator to have a low(er) gain with a SINE waveform.
        /// </summary>
        public AudioHandler(int initialFrequency) {
            this.outWave = new SignalGenerator() {
                Gain = 0.6,
                Frequency = Frequency,
                Type = SignalGeneratorType.Sin
            };
            this.Frequency = initialFrequency;
        }

        /// <summary>
        /// Plays the given sequence of Morse symbols using the internal WaveGenerator module. This "queues"
        /// audio to play, rather than relying on Thread.Sleep to gap the DITs/DAHs (symbols) from each other.
        /// </summary>
        /// <param name="morseStream">The sequence of MorseSymbol items to be played.</param>
        /// <param name="cancelToken">Tracks whether or not this operation should be cancelled.</param>
        /// <see cref="MorseSymbol"/>
        /// <see cref="WaveGenerator"/>
        public void PlayWaveStream(List<MorseSymbol> morseStream, CancellationToken cancelToken) {
            // First, get the audio constructed.
            var wavFormat = new WaveAudioFormat(16000, 16, 1); //doesn't need to be the most high-def sound
            var wavType = WaveGenerator.WaveType.SINE; //always a SINE wave (for now; configurable later)
            var streamSequence = new List<WaveSample>(); //container for the resulting stream of samples
            ////Loading icon??
            foreach(MorseSymbol sym in morseStream) {
                if(cancelToken != null && cancelToken.IsCancellationRequested) { return; }
                if(sym.hasSound()) {
                    WaveAudioTools.AppendSamples(
                        ref streamSequence,
                        (100.0, WaveGenerator.CreateNewWave(wavFormat, wavType, this.Gain*100.0, sym.getDuration()/1000.0, this.Frequency))
                    );
                } else {
                    WaveAudioTools.AppendSamples(
                        ref streamSequence,
                        (100.0, WaveGenerator.CreateEmptySpace(wavFormat, sym.getDuration()/1000.0))
                    );
                }
            }
            // Construct the output stream in WAV format.
            using var finalStream = new WaveStream(streamSequence);
            ////Release loading icon??
            // Then, play the stream, while at the same time updating the text stream to sync as
            //   best as possible to the currently-playing audio.
            // Define a lambda of sorts for updating the live text view for transmissions.
            Action<MorseSymbol> updateTextStream = s => {
                Program.mainForm.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                    Program.txLiveView.AppendText(s.getRepresentation());
                });
            };
            // Construct the player.
            using var queuedStream = new System.IO.MemoryStream(finalStream.GetRawWaveStream());
            using var mediaPlayer = new System.Media.SoundPlayer(queuedStream);
            //...and...... GO!
            mediaPlayer.Play();
            int infiniteLoopPrevention = 0;
            // Give the player 2s to load the stream.
            while(!mediaPlayer.IsLoadCompleted && infiniteLoopPrevention < 100) {
                Thread.Sleep(20); 
                infiniteLoopPrevention++;
            }
            // During stream playback, send out the symbols.
            foreach(MorseSymbol sym in morseStream) {
                if(cancelToken != null && cancelToken.IsCancellationRequested) { break; }
                Thread.Sleep((int)sym.getDuration());
                try { updateTextStream(sym); } catch { }
            }
            // Regardless of the outcome (finished or cancelled), stop the sound.
            mediaPlayer.Stop();
        }

        /// <summary>
        /// Play the given symbol stream using the internal SignalGenerator provided by NAudio and initialized in the constructor.
        /// This function provides all tracking capabilities for the stream while it's being iterated (progress, cancellation, etc),
        /// and is also a cancellable event.
        /// </summary>
        /// <param name="stream">List of MorseSymbol objects to iterate. This is the sequence that is played by the audio handler.</param>
        /// <param name="cancelToken">Tracks whether or not this thread should be cancelled.</param>
        /// <seealso cref="MorseSymbol"/>
        public void PlayStream(List<MorseSymbol> stream, CancellationToken cancelToken) {
            int index = 0;
            foreach(MorseSymbol sym in stream) {
                // Lambda of sorts for updating the live text view for transmissions.
                Action<MorseSymbol> updateTextStream = s => {
                    Program.mainForm.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                        Program.txLiveView.AppendText(s.getRepresentation());
                    });
                };
                // Handle progress bar updates.
                Program.mainForm.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                    try { Program.pbTXProgress.Value = (((index * 100) / stream.Count) + 1); } catch { }
                }); index++;
                // Check for cancellations.
                if(cancelToken != null && cancelToken.IsCancellationRequested) { break; }
                if(sym.hasSound()) {
                    // Play the sound at the specified duration (synchronously).
                    //   A better way to do this eventually might be to construct a raw WAV stream and just play it.
                    //   Doing so would reduce the factor of unreliability in "live" streaming of the audio due to any lag on the executing machine.
                    //   Such stream-induced lag is VERY noticeable at high WPMs when intervals are short.
                    //   Constructing a raw WAV could be easily done by relying on another application I've written elsewhere, which I may splice in.
                    //   ^^^ REMINDER TODO: TODO: this
                    using(var wo = new WaveOut()) {
                        wo.Init(this.outWave);
                        wo.Play();
                        Thread.Sleep((int)sym.getDuration());
                        try { updateTextStream(sym); } catch { }
                        wo.Stop();
                    }
                } else {
                    Thread.Sleep((int)sym.getDuration());
                    try {
                        if(sym.getRepresentation().Trim() == "") { updateTextStream(sym); }
                    } catch { }
                }
            }
        }
    }
}
