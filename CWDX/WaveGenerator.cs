using System;
using System.Collections.Generic;

namespace CWDX {

    /// <summary>
    /// A generic "audio format" class that allows define-once objects to act as a set of immutable audio properties.
    /// All values are represented and used exactly as read in variable names.
    /// </summary>
    public sealed class WaveAudioFormat {
        public readonly int MaxSampleValue, MinSampleValue;
        public readonly int SampleRate;
        public readonly int BytesPerSample, BitsPerSample;
        public readonly int ChannelCount;
        public WaveAudioFormat(int sampleRate, int bitsPerSample, int channelCount) {
            this.SampleRate = sampleRate;
            this.BitsPerSample = bitsPerSample % 32;
            this.BytesPerSample = (int)(bitsPerSample / 8) % 4;
            this.ChannelCount = channelCount;
            // Set min/max sample bounds and clip the value of the sample by reference.
            this.MaxSampleValue = (1 << (this.BitsPerSample - 1)) - 1;
            this.MinSampleValue = 0 - this.MaxSampleValue - 1;
        }
        /// <summary>
        /// Calculates the peak amplitude at the given percentage for the audio format.
        /// </summary>
        /// <param name="forPercentage">Defaults to 100%. Can be used to get the peak amplitude at any volume percentage under 100%.</param>
        /// <returns>A value representing the highest (and lowest, if negated) travel an audio format can have based on the Min/Max SampleValue variables.</returns>
        public double GetPeakAmplitude(double? forPercentage = 100.0) {
            double atPercentage = Math.Max(Math.Abs(forPercentage ?? 100.0), 100.0);
            return (atPercentage * 0.01) * this.MaxSampleValue;
        }
    }



    /// <summary>
    /// Respresents an individual PCM unit (i.e. sample) in a raw WAV stream.
    /// </summary>
    /// <seealso cref="WaveGenerator"/>
    public sealed class WaveSample {
        /// <summary>
        /// Static method to combine lists of multiple WaveSample objects into a single WaveSample.
        /// </summary>
        /// <returns>A single WaveSample representing the mixing of all provided samples.</returns>
        public static WaveSample CombineWaveSamples(WaveAudioFormat audioFormat, params WaveSample[] samples) {
            var combinedSamples = new int[audioFormat.ChannelCount];
            var newSample = new WaveSample(audioFormat, 0);
            // For each sample, add the channel values onto the aggregate total.
            foreach(WaveSample smp in samples) {
                int[] channelValues = smp.GetAllChannelValues();
                for(int i = 0; i < channelValues.Length; i++) { combinedSamples[i] += channelValues[i]; }
            }
            // Now, set the new sample's channel values to be mapped to the final totals for each channel.
            for(int channel = 0; channel < combinedSamples.Length; channel++) {
                newSample.SetChannelValue(channel, ref combinedSamples[channel]);
            }
            return newSample;
        }
        /// <summary>
        /// The value applied to the sample initially, across all channels simultaneously.
        /// </summary>
        public int SampleValue { get; private set; }
        /// <summary>
        /// The raw sample data.
        /// </summary>
        public byte[] Sample { get; private set; }
        /// <summary>
        /// The sample formatting used to construct the sample data, including the bit-rate, channel-count, etc.
        /// </summary>
        /// <see cref="WaveAudioFormat"/>
        public WaveAudioFormat SampleFormat { get; private set; }
        public WaveSample(WaveAudioFormat sampleFormat, int sampleValue) {
            this.SampleFormat = sampleFormat;
            // Initialize the Sample byte array to be the size of ===>   FULL_SAMPLE = [CHANNELS x CHANNEL_SIZE]
            this.Sample = new byte[this.SampleFormat.BytesPerSample * this.SampleFormat.ChannelCount];
            // Clip the provided sample value to constrain it to an acceptable audio value, then set it on the object.
            this.ClipSample(ref sampleValue);
            this.SampleValue = sampleValue;
            // Set the sample values equal across all channels on instantiation.
            int discardClipping = sampleValue; //don't care about the ref, just can't use a `this`
            this.SetAllChannelValues(ref discardClipping);
        }

        /// <summary>
        /// Clips sample values by reference to constrain them between the min/max sample values.
        /// </summary>
        /// <param name="sampleValue">A pointer to the sample value to clip.</param>
        internal void ClipSample(ref int sampleValue) {
            if(sampleValue != 0) {
                sampleValue = sampleValue > 0
                    ? Math.Min(this.SampleFormat.MaxSampleValue, sampleValue)
                    : Math.Max(this.SampleFormat.MinSampleValue, sampleValue);
            }
        }

        /// <summary>
        /// Wrapper function to equalize the sample value across all channels.
        /// </summary>
        /// <param name="sampleValue">The PCM audio sample value.</param>
        public void SetAllChannelValues(ref int sampleValue) {
            for(int r = 0; r < this.SampleFormat.ChannelCount; r++) {
                this.SetChannelValue(r, ref sampleValue);
            }
        }

        /// <summary>
        /// Gets the sample values from each channel in an ascending array.
        /// </summary>
        /// <returns>An integer array mapped to each channel's value.</returns>
        public int[] GetAllChannelValues() {
            var x = new int[this.SampleFormat.ChannelCount];
            for(int r = 0; r < this.SampleFormat.ChannelCount; r++) {
                x[r] = this.GetChannelValue(r);
            }
            return x;
        }

        /// <summary>
        /// Change the sample value for a specific channel of the overall audio sample.
        /// </summary>
        /// <param name="channelNumber"></param>
        /// <param name="sampleValue"></param>
        public void SetChannelValue(in int channelNumber, ref int sampleValue) {
            this.ClipSample(ref sampleValue);
            for(int r = 0; r < this.SampleFormat.BytesPerSample; r++) {
                this.Sample[r + (channelNumber * this.SampleFormat.BytesPerSample)] =
                    (byte)((sampleValue >> (r * 8)) & 0xFF);
            }
        }

        /// <summary>
        /// Retrieves the sample value of a selected channel from the audio sample.
        /// </summary>
        /// <param name="channelNumber">The channel to select from the audio sample.</param>
        /// <returns>The value of the sample for the chosen channel.</returns>
        public int GetChannelValue(in int channelNumber) {
            try {
                // Get whether the most-significant-byte of the sample is set --> [firstByte] & 0x80 == 0x80
                //// Consider a 2ch, 4byte-per-sample audio sample: [x x x x] [y y y y]
                //// Say the target MSB is in channel #1 (the second channel, counting starts from 0)
                //// Then the index into the sample would become: (4bps - 1) + (1 * 4bps) ==> (3)+(4) ==> index 7 as msb on channel 1
                bool msbSet = (
                        this.Sample[
                            (this.SampleFormat.BytesPerSample - 1) + (channelNumber * this.SampleFormat.BytesPerSample)
                        ] & 0x80
                    ) == 0x80;
                // Create a dummy buffer and isolate the potion/channel that's of interest.
                byte[] DWORD_sampleValueInChanel = new byte[4] { 0, 0, 0, 0 };
                for(int i = 0; i < this.SampleFormat.BytesPerSample; i++) {
                    DWORD_sampleValueInChanel[i] = this.Sample[i + (channelNumber * this.SampleFormat.BytesPerSample)];
                }
                // If for some reason, the system is big-endian, then reverse the byte array.
                if(!BitConverter.IsLittleEndian) { Array.Reverse(DWORD_sampleValueInChanel); }
                // If the most significant byte is set, then invert every byte in the array, to make the returned value negative.
                if(msbSet && this.SampleFormat.BytesPerSample < 4) {
                    for(int i = this.SampleFormat.BytesPerSample; i < DWORD_sampleValueInChanel.Length; i++) {
                        DWORD_sampleValueInChanel[i] ^= 0xFF;
                    }
                }
                // Using ToInt32 here.
                return BitConverter.ToInt32(DWORD_sampleValueInChanel, 0);
            } catch { return 0; }
        }
    }


    /// <summary>
    /// Provides methods used to generate sequences of WaveSample objects that can be put into a raw WAVE stream.
    /// </summary>
    /// <see cref="WaveSample"/>
    /// <see cref="WaveStream"/>
    public static class WaveGenerator {

        /// <summary>
        /// Amount of periods per generated wave for which the signal is attenuated. This is in hopes to
        /// reduce any hard clicks between signals.
        /// </summary>
        /// <see cref="CreateNewWave(WaveAudioFormat, WaveType, double, double, double)"/>
        public static int ATTENUATION_CYCLES = 2;

        /// <summary>
        /// Defines multiple different waveforms available for the generator to build.
        /// </summary>
        /// <seealso cref="WaveGenerator"/>
        public enum WaveType { SINE, SAWTOOTH, SQUARE, TRIANGLE };

        /// <summary>
        /// Converts a stream of WaveSample objects into a raw byte array.
        /// </summary>
        /// <param name="audioStream">The stream to convert.</param>
        /// <returns>A raw byte array representing the provided WaveSample stream.</returns>
        public static byte[] ToByteArray(WaveSample[] audioStream) {
            var returnStream = new List<byte>();
            foreach(WaveSample sample in audioStream) { returnStream.AddRange(sample.Sample); }
            return returnStream.ToArray();
        }

        /// <summary>
        /// Create and return an audio stream of the specified duration that has no sound.
        /// </summary>
        /// <param name="audioFormat">The format in which the silence should be created.</param>
        /// <param name="spaceDurationSec">The amount of time in seconds for which to create total silence.</param>
        /// <returns>Zeroed set of WaveSample objects that will not make any sound if played.</returns>
        /// <see cref="WaveSample"/>
        public static List<WaveSample> CreateEmptySpace(WaveAudioFormat audioFormat, double spaceDurationSec) {
            double totalSamples = Math.Ceiling(spaceDurationSec * (double)audioFormat.SampleRate);
            WaveSample[] silentAudioStream = new WaveSample[(int)totalSamples];
            for(int i = 0; i < (int)totalSamples; i++) {
                silentAudioStream[i] = new WaveSample(audioFormat, 0);
            }
            return new List<WaveSample>(silentAudioStream);
        }

        /// <summary>
        /// Generates a new wave-form as an audio stream (list of WaveSample objects).
        /// </summary>
        /// <param name="audioFormat">The format used to sample the waveform.</param>
        /// <param name="wavePattern">The type of wave to generate.</param>
        /// <param name="amplitudePerc">A percentage of the audio format's MaxAmplitude (0 to 100%).</param>
        /// <param name="durationSec">How long (in seconds) the sound will be generated for.</param>
        /// <param name="frequencyHz">The frequency at which the sound should be played.</param>
        /// <returns>A new list of WaveSample objects that represent the sound.</returns>
        /// <see cref="WaveSample"/>
        /// <see cref="WaveAudioFormat"/>
        public static List<WaveSample> CreateNewWave(WaveAudioFormat audioFormat, WaveType wavePattern,
                double amplitudePerc, double durationSec, double frequencyHz) {
            // Cap the amplitude at 100%.
            amplitudePerc = Math.Min(100.0, Math.Abs(amplitudePerc));
            // Samples required to complete one wave period at the given frequency.
            double samplesPerWaveCycle = (double)(audioFormat.SampleRate / frequencyHz);
            // Total amount of samples for the wave duration in its entirety.
            double totalSamples = durationSec * (double)audioFormat.SampleRate;
            // Additional amount of samples to append onto the stream until the final wave/period completes.
            //   This is aimed at keeping sudden adjacent tones from causing hard key clicks when transitioning.
            double extraSamples = samplesPerWaveCycle - (totalSamples % samplesPerWaveCycle);
            double waveAttenuationSamplesCount = (samplesPerWaveCycle * WaveGenerator.ATTENUATION_CYCLES);
            // Short-handing some variables from the audio format as needed...
            int sampleRate = audioFormat.SampleRate;
            // Get the peak amplitude permitted based on the amplitudePerc parameter.
            double peakAmplitude = audioFormat.GetPeakAmplitude(amplitudePerc);
            // Initialize the new stream object.
            var newWaveStream = new List<WaveSample>();
            // Create the wave stream based on the requested wave type.
            Func<int, double> sampleCalculation = wavePattern switch {
                // f(x) = amplitude * SIN(2pi * x * frequency / sampleRate)
                WaveType.SINE => (currSample =>
                    peakAmplitude * Math.Sin((2*Math.PI * currSample * frequencyHz) / sampleRate)
                ),
                // f(x) = abs(amplitude), when 0 <= x < period/2 ;; f(x) = neg(amplitude), when period/2 <= x < period
                // My implementation factors amplitude out of: f(x) = (({2*[amp/samplesPerPeriod]*x + amp} % [amp*2]) - (amp)
                WaveType.SAWTOOTH => (currSample =>
                    ((peakAmplitude * ((2 * currSample / samplesPerWaveCycle) + 1)) % (peakAmplitude * 2)) - peakAmplitude
                ),
                // f(x) = x, when 0 <= x < pi ;; f(x) = x-2pi, when pi <= x =< 2pi
                WaveType.SQUARE => (currSample =>
                    (currSample % samplesPerWaveCycle) < (samplesPerWaveCycle / 2) ? peakAmplitude : 0 - peakAmplitude
                ),
                // f(x) = [2*amp / pi] * [ arcsin( sin( {2pi * freq}/sampleRate ) * x ) ]
                // Definitely had to look this one up...
                WaveType.TRIANGLE => (currSample => 
                    ((2 * peakAmplitude) / Math.PI) 
                    * Math.Asin(Math.Sin(((2 * Math.PI * frequencyHz) / sampleRate) * currSample))
                ),
                // If no other enum matched, throw exception
                _ => throw new Exception("CreateNewWave: Invalid wave pattern selection: " + wavePattern.ToString())
            };
            for(int currentSample = 0; currentSample < totalSamples + extraSamples; currentSample++) {
                double sampleValue = sampleCalculation(currentSample);
                if(currentSample < waveAttenuationSamplesCount) {
                    // For the first few wave cycles, attenuate the max amplitude by a
                    //   ratio to gradually introduce the signal.
                    //   Again, this is aimed to help prevent clicking in the resulting sample
                    sampleValue *= (double)((double)currentSample / waveAttenuationSamplesCount);
                } else if(currentSample > ((totalSamples + extraSamples) - waveAttenuationSamplesCount)) {
                    // Likewise, attenuate the end of the signal for the sample.
                    sampleValue *= (double)((double)((totalSamples + extraSamples) - currentSample) / waveAttenuationSamplesCount);
                }
                newWaveStream.Add(new WaveSample(audioFormat, (int)sampleValue));
            }
            // Finally, return the new stream.
            return newWaveStream;
        }

    }

}
