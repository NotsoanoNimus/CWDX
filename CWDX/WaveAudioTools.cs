using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CWDX {
    /// <summary>
    /// A class that provides static methods for manipulating PCM-encoded audio data.
    /// </summary>
    public static class WaveAudioTools {

        /// <summary>
        /// Mixes different audio streams, represented as lists of WaveSample objects and their corresponding amplitude weighting, into an overall stream of sound.
        /// The resulting stream duration will equal the longest stream in the "samples" list, so varying stream durations can be easily mixed together; simply note that
        /// the final stream starts all component streams at the "0" duration mark.
        /// </summary>
        /// <param name="audioFormat">The format used for aggregating all stream data. This MUST be the same format used across all streams -- any stream NOT matching the provided format will be DISCARDED from the final result.</param>
        /// <param name="peakOutputAmplitudePercentage">A scaling percentage for the final stream's peak output amplitude. Defaults to 100%, which represents no change in the mixed stream's volume.</param>
        /// <param name="componentStreamCollection">Any amount of tuples consisting of the amplitude (volume) weighting within all mixed samples, and the audio sample itself. NOTE: The weighting is NOT a percentage value!</param>
        /// <returns>An audio stream (stream of samples) equal to the combination of all provided component streams at their given amplitude weightings.</returns>
        /// <see cref="WaveSample"/>
        public static List<WaveSample> MixSamples(
                WaveAudioFormat audioFormat,
                double peakOutputAmplitudePercentage = 100.0,
                params (double, List<WaveSample>)[] componentStreamCollection
        ) {
            // Cap the peak audio amplitude at 100%.
            peakOutputAmplitudePercentage = Math.Min(Math.Abs(peakOutputAmplitudePercentage), 100.0);
            // Create some tracking local variables.
            int longestSample = 0;  //the longest component stream length
            double totalAmplitudeWeights = 0.0;  //the combined weights of all provided component streams (used for weight ratio calculations)
            // Filter all component streams for (1) empty lists, and (2) bad formatting. And while iterating (if the object isn't skipped), get stats.
            var filteredComponentStreams = new List<(double, List<WaveSample>)>();
            foreach((double amplitudeWeight, List<WaveSample> componentStream) in componentStreamCollection) {
                if(
                        componentStream.Count < 1
                        || amplitudeWeight == 0.0
                        || componentStream[0].SampleFormat != audioFormat) {
                    continue;
                }
                filteredComponentStreams.Add((amplitudeWeight, componentStream));
                // Mark the amplitude weighting and get the longest sample.
                totalAmplitudeWeights += amplitudeWeight;
                longestSample = Math.Max(componentStream.Count, longestSample);
            }
            // Ready the output samples stream and initialize all values to 0.
            var finalStream = new WaveSample[longestSample];
            for(int i = 0; i < longestSample; i++) { finalStream[i] = new WaveSample(audioFormat, 0); }
            finalStream.Select((x, i) => finalStream[i] = new WaveSample(audioFormat, 0));
            // If the total amplitude weightings add up to zero for some reason, or no streams are left over after filtering,
            //   return an empty stream at the right sample length.
            if(totalAmplitudeWeights <= 0.0 || filteredComponentStreams.Count <= 0) { return new List<WaveSample>(finalStream); }
            // Iterate each component stream...
            for(int j = 0; j < filteredComponentStreams.Count; j++) {
                (double amplitudeWeight, List<WaveSample> componentStream) = filteredComponentStreams[j];
                // Get the ratio of stream weight to total as a percentage.
                double amplitudeRatioPerc = (amplitudeWeight / totalAmplitudeWeights) * 100.0;
                // Use that percentage to change the stream's volume (will always be < 100% for multiple component streams).
                WaveAudioTools.ChangeVolume(amplitudeRatioPerc, ref componentStream);
                // For each sample in the stream, use the WaveSample "CombineWaveSamples" method to add onto the finalStream's value.
                for(int k = 0; k < componentStream.Count; k++) {
                    finalStream[k] = WaveSample.CombineWaveSamples(audioFormat, finalStream[k], componentStream[k]);
                }
            }
            // Convert the finalStream into a list object.
            var finalStreamAsList = new List<WaveSample>(finalStream);
            // Adjust the finalStream object's samples to the final volume scaling.
            WaveAudioTools.ChangeVolume(peakOutputAmplitudePercentage, ref finalStreamAsList);
            // Finally, return the completed stream.
            return finalStreamAsList;
        }


        /// <summary>
        /// Changes the volume for a referenced audio stream based on the provided percentage. This is NOT multiplicative: the percentage
        /// provided in the parameter is based on 0 to 100% the POSSIBLE volume of the audio format.
        /// </summary>
        /// <param name="newAmplitudePerc">The new amplitude percentage, between 0 and 100%.</param>
        /// <param name="audioStream">A pointer to an audio stream whose values should be altered to match the new amplitude scaling.</param>
        public static void ChangeVolume(double newAmplitudePerc, ref List<WaveSample> audioStream) {
            if(audioStream?.Count <= 0) {
                throw new Exception("ChangeVolume: The stream to modify must have at least one sample.");
            }
            WaveAudioFormat audioFormat = audioStream[0].SampleFormat;
            double maxAmplitude = audioFormat.GetPeakAmplitude();
            double minAmplitude = 0 - maxAmplitude - 1; //min peak is always negated positive peak, minus 1
            double newAmplitudeRatio = (0.01 * newAmplitudePerc); //from % to ratio
            foreach(WaveSample sample in audioStream) {
                // Change the amplitude per-channel.
                int[] channelValues = sample.GetAllChannelValues();
                for(int channel = 0; channel < channelValues.Length; channel++) {
                    int newSampleValueForChannel = (int)Math.Floor(channelValues[channel] * newAmplitudeRatio);
                    // Constrain the value between the peak amplitudes.
                    newSampleValueForChannel = (int)Math.Min(maxAmplitude, Math.Max(minAmplitude, newSampleValueForChannel));
                    sample.SetChannelValue(channel, ref newSampleValueForChannel);
                }
            }
        }


        public static void AppendSamples(ref List<WaveSample> streamToExtend,
                params (double?, List<WaveSample>)[] appendedStreams) {
            if(streamToExtend?.Count <= 0) {
                throw new Exception("AppendSamples: The stream to extend must have at least one sample in the dataset.");
            }
            for(int i = 0; i < appendedStreams?.Length; i++) {
                // Get the stream.
                (double? newVolumePercentage, List<WaveSample> appendMe) = appendedStreams[i];
                if(appendMe.Count <= 0) { continue; }
                else if(appendMe[0].SampleFormat != streamToExtend[0].SampleFormat) { continue; }
                // Set the volume (unchanged if the % provided is missing/null).
                WaveAudioTools.ChangeVolume(newVolumePercentage ?? 100.0, ref appendMe);
                // Append the samples onto the original stream being extended.
                streamToExtend.AddRange(appendMe);
            }
        }


        /// <summary>
        /// Normalizes the peak amplitudes within the referenced audio stream.
        /// </summary>
        /// <param name="audioFormat">The format to use for normalization.</param>
        /// <param name="audioStream"></param>
        public static void NormalizePeaks(ref List<WaveSample> audioStream) {

        }

    }
}
