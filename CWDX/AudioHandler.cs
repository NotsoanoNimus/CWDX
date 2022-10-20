using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

using ToneBaker.PCM;
using ToneBaker.WAV;


namespace CWDX {
    /// <summary>
    /// Initializes an audio processor and waveform generator for playing a sequence of MorseSymbol objects.
    /// </summary>
    public sealed class AudioHandler {

        /// <summary>
        /// Adjusts the frequency of the NAudio output generator's waveform.
        /// </summary>
        public double Frequency { get; set; } = 700.0;

        /// <summary>
        /// Adjusts the volume of the NAudio output generator.
        /// </summary>
        public double Gain {
            get { return this._gain; }
            set { this._gain = Math.Min(value, 100.0); }
        }
        internal double _gain = 60.0;

        /// <summary>
        /// The wave-form type used to generate the audio; can be changed dynamically.
        /// </summary>
        public WaveGenerator.WaveType WaveType { get; set; } = WaveGenerator.WaveType.SINE;

        /// <summary>
        /// The properties used for playing the generated audio streams.
        /// </summary>
        public AudioFormat AudioFormat { get; set; } = new AudioFormat( 16000, 16, 1 );

        /// <summary>
        /// Initializes the audio handler to have a low(er) gain with a SINE waveform.
        /// </summary>
        public AudioHandler() { }


        public void PlayMorseStream( int wpm, MorseCharacter[] MorseStream, CancellationToken? CancelToken ) {
            double wpmMultiplier = Morse.GetMillisecondsPerSymbolAtWPM( wpm );
            var finalAudioStream = new List<PCMSample>();

            // Define some useful constants that shouldn't be reevaluted on each loop.
            //   More info: https://en.wikipedia.org/wiki/Morse_code#Timing
            var perSymbolGap = PCMAudioTools.CreateEmptySpace( this.AudioFormat, ((MorseSymbolDuration.PER_SYMBOL * wpmMultiplier) / 1000) );
            // (special note): the below is done in order to account for extra trailing PER_SYMBOL gaps added onto each symbol at the end of a MorseCharacter.
            var perCharacterGap = PCMAudioTools.CreateEmptySpace( this.AudioFormat, (((MorseSymbolDuration.PER_CHARACTER) * wpmMultiplier) / 1000) );

            // Get each character from the stream...
            foreach ( var morseChar in MorseStream ) {

                // Get each symbol in the character...
                foreach ( var morseSymbol in morseChar.Sequence ) {

                    // Check for cancellations.
                    if ( CancelToken?.IsCancellationRequested ?? true ) return;

                    // Get the duration and instantiate a local list to get the wave samples.
                    double scaledDuration = (morseSymbol.Duration * wpmMultiplier) / 1000;   // to milliseconds and back
                    var addedSamples = new List<PCMSample>();

                    // If it's a space, it's dead air. If it's not, make a wave.
                    if ( morseSymbol.Representation == MorseSymbolCharacter.SPACE_CHAR ) {
                        addedSamples = PCMAudioTools.CreateEmptySpace( this.AudioFormat, scaledDuration );
                    } else {
                        addedSamples = WaveGenerator.CreateNewWave( this.AudioFormat, this.WaveType, this.Gain, scaledDuration, this.Frequency ); 
                    }

                    // Always add the per_symbol dead-space here.
                    addedSamples.AddRange( perSymbolGap );

                    // Now add the local samples onto the final audio.
                    finalAudioStream.AddRange( addedSamples );

                }

                // Add the per-character spacing.
                finalAudioStream.AddRange( perCharacterGap );
            }

            // Nothing to play? Leave.
            if ( finalAudioStream.Count < 1 ) return;

            // Create the RIFF (wav) stream, map it into memory, and play it using the soundplayer.
            using var outStream = new RiffStream( finalAudioStream );
            using var memStream = new System.IO.MemoryStream( outStream.GetRawWaveStream() );
            using var soundPlayer = new System.Media.SoundPlayer( memStream ) { LoadTimeout = 100 * 1000 };

            // Load the in-memory sound buffer into the player.
            int infinitePrevention = 0;
            soundPlayer.LoadAsync();

            // Wait for it to finish loading.
            while ( !soundPlayer.IsLoadCompleted && infinitePrevention < 100000 ) {
                if ( infinitePrevention == (100000-1) || (CancelToken?.IsCancellationRequested ?? true) ) {
                    soundPlayer.Stop();
                    soundPlayer.Dispose();
                    return;
                }

                Thread.Sleep( 20 );
                infinitePrevention++;
            }

            // Now that it's loaded, play the stream, while at the same time updating the text stream
            //   to sync as best as possible to the currently-playing audio.

            // Define a lambda of sorts for updating the live text view for transmissions.
            Action<string> updateTextStreamString = s => {
                Program.mainForm.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                    Program.txLiveView.AppendText( s );
                });
            };
            Action<MorseSymbol> updateTextStream = s => updateTextStreamString( s.Representation.ToString() );

            soundPlayer.Play();
            for ( int idx = 0; idx < MorseStream.Length; idx++ ) {

                foreach ( var sym in MorseStream[idx].Sequence ) {
                    // Keep abreast for cancellations.
                    if(CancelToken?.IsCancellationRequested ?? true) {
                        soundPlayer.Stop();
                        soundPlayer.Dispose();
                        return;
                    }

                    // Update the textbox.
                    updateTextStream( sym );

                    // Wait the duration.
                    Task.Delay( (int)((sym.Duration + MorseSymbolDuration.PER_SYMBOL) * wpmMultiplier) ).Wait();
                }

                // Update again for spacing.
                updateTextStreamString( "   " );

                // Wait.
                Task.Delay( (int)(MorseSymbolDuration.PER_CHARACTER * wpmMultiplier) ).Wait();

                // Handle progress bar updates.
                Program.mainForm.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                    try { Program.pbTXProgress.Value = (((idx * 100) / MorseStream.Length) + 1); } catch { }
                });
            }
        }

    }
}
