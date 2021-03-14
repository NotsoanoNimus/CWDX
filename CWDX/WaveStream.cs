using System;
using System.Collections.Generic;
using System.Text;


namespace CWDX {
    /// <summary>
    /// Defines necessary methods for a section of a raw WAVE stream necessary to the file format.
    /// </summary>
    /// <seealso cref="http://soundfile.sapp.org/doc/WaveFormat/"/>
    public interface IWaveFileChunk {
        /// <summary>
        /// Sets the size chunking parameters as necessary for the WAVE stream section. Used for header information.
        /// </summary>
        /// <param name="chunkSize"></param>
        public abstract void SetChunkSize(int chunkSize);
        /// <summary>
        /// Retrieve the size of the data chunk as per the section's defined chunk size.
        /// </summary>
        /// <returns>The section's specified chunk size.</returns>
        public abstract int GetChunkSize();
        /// <summary>
        /// Gets a raw data stream for a section of a raw WAVE stream.
        /// </summary>
        /// <returns>The raw data representing the section of the WAVE stream.</returns>
        public abstract byte[] GetChunk();
    }



    /// <summary>
    /// Contains all necessary pieces needed to construct a valid WAV stream for use in-memory or as a static file.
    /// </summary>
    /// <see cref="WaveStream"/>
    public sealed class WaveFileFormat {
        public enum Endianness { BIG, LITTLE };
        /// <summary>
        /// Converts provided raw data to the specified Endianness, based on the enum choice and the system default endianness.
        /// </summary>
        /// <param name="data">The data to be arranged according to the requested byte ordering.</param>
        /// <param name="endianness">The type of Endianness required by the WAVE file format specification.</param>
        /// <returns>A byte stream in the proper order for the running system.</returns>
        public static byte[] Endian(byte[] data, Endianness endianness) {
            if((BitConverter.IsLittleEndian && endianness == Endianness.BIG) ||
                (!BitConverter.IsLittleEndian && endianness == Endianness.LITTLE)) { Array.Reverse(data);  }
            return data;
        }


        /// <summary>
        /// The RIFF section of a raw WAV stream.
        /// </summary>
        /// <see cref="http://soundfile.sapp.org/doc/WaveFormat/"/>
        public sealed class RIFF_CHUNK : IWaveFileChunk {
            private readonly byte[] bChunkId = Encoding.ASCII.GetBytes("RIFF");
            private byte[] lChunkSize = new byte[4];
            private readonly byte[] bChunkFormat = Encoding.ASCII.GetBytes("WAVE");
            public byte[] GetChunk() {
                var dataStream = new List<byte>();
                dataStream.AddRange(this.bChunkId);
                dataStream.AddRange(this.lChunkSize);
                dataStream.AddRange(this.bChunkFormat);
                return dataStream.ToArray();
            }
            public int GetChunkSize() { throw new NotImplementedException(); }
            public void SetChunkSize(int chunkSize) {
                this.lChunkSize = Endian(BitConverter.GetBytes(chunkSize), Endianness.LITTLE);
            }
        }


        /// <summary>
        /// The FMT (formatting) section of a raw WAV stream.
        /// </summary>
        /// <see cref="http://soundfile.sapp.org/doc/WaveFormat/"/>
        public sealed class FMT_CHUNK : IWaveFileChunk {
            private readonly byte[] bSubChunk1Id = Encoding.ASCII.GetBytes("fmt ");
            private byte[] lSubchunk1Size = BitConverter.GetBytes(0x00000010);
            private readonly byte[] lAudioFormat = new byte[2] { 0x01, 0x00 };
            private byte[] lNumChannels = new byte[2];
            private byte[] lSampleRate = new byte[4];
            private byte[] lByteRate = new byte[4];  // SampleRate * NumChannels * (BitsPerSample/8)
            private byte[] lBlockAlign = new byte[2]; // NumChannels * (BitsPerSample/8)
            private byte[] lBitsPerSample = new byte[2];
            public byte[] GetChunk() {
                var dataStream = new List<byte>();
                dataStream.AddRange(this.bSubChunk1Id);
                dataStream.AddRange(this.lSubchunk1Size);
                dataStream.AddRange(this.lAudioFormat);
                dataStream.AddRange(this.lNumChannels);
                dataStream.AddRange(this.lSampleRate);
                dataStream.AddRange(this.lByteRate);
                dataStream.AddRange(this.lBlockAlign);
                dataStream.AddRange(this.lBitsPerSample);
                return dataStream.ToArray();
            }
            public int GetChunkSize() {
                return BitConverter.ToInt32(this.lSubchunk1Size, 0);
            }
            public void SetChunkSize(int chunkSize) { throw new NotImplementedException(); }
            public FMT_CHUNK(WaveAudioFormat audioFormat) {
                byte[] numChannelsEncoded = BitConverter.IsLittleEndian
                    ? new byte[2] { (byte)audioFormat.ChannelCount, 0x00 }
                    : new byte[2] { 0x00, (byte)audioFormat.ChannelCount };
                this.lNumChannels = Endian(numChannelsEncoded, Endianness.LITTLE);
                this.lSampleRate = Endian(BitConverter.GetBytes(audioFormat.SampleRate), Endianness.LITTLE);
                this.lBitsPerSample = BitConverter.IsLittleEndian
                    ? new byte[2] { (byte)audioFormat.BitsPerSample, 0x00 }
                    : new byte[2] { 0x00, (byte)audioFormat.BitsPerSample };
                int byteRate = audioFormat.SampleRate * audioFormat.ChannelCount * (audioFormat.BitsPerSample / 8);
                this.lByteRate = Endian(BitConverter.GetBytes(byteRate), Endianness.LITTLE);
                short blockAlign = (short)(audioFormat.ChannelCount * (audioFormat.BitsPerSample / 8));
                this.lBlockAlign = new byte[] { (byte)blockAlign, (byte)(blockAlign >> 8) };
            }
        }


        /// <summary>
        /// The DATA portion of a raw WAV stream.
        /// </summary>
        /// <see cref="http://soundfile.sapp.org/doc/WaveFormat/"/>
        public sealed class DATA_CHUNK : IWaveFileChunk {
            private readonly byte[] bSubchunk2Id = Encoding.ASCII.GetBytes("data"); // Endian(Encoding.ASCII.GetBytes("data"), Endianness.BIG);
            private readonly byte[] lSubchunk2Size;
            private readonly byte[] lData;
            public byte[] GetChunk() {
                var dataStream = new List<byte>();
                dataStream.AddRange(this.bSubchunk2Id);
                dataStream.AddRange(this.lSubchunk2Size);
                dataStream.AddRange(this.lData);
                return dataStream.ToArray();
            }
            public int GetChunkSize() {
                return BitConverter.ToInt32(this.lSubchunk2Size, 0);
            }
            public void SetChunkSize(int chunkSize) { throw new NotImplementedException(); }
            public DATA_CHUNK(byte[] audioData) {
                this.lData = audioData;
                this.lSubchunk2Size = Endian(BitConverter.GetBytes(audioData.Length), Endianness.LITTLE);
            }
        }
    }



    /// <summary>
    /// Wrapper class to encapsulate the overall structure of an entire WAV file/stream.
    /// </summary>
    /// <see cref="http://soundfile.sapp.org/doc/WaveFormat/"/>
    /// <seealso cref="WaveFileFormat"/>
    public sealed class WaveStream : IDisposable {
        internal bool _disposed = false;
        private WaveFileFormat.RIFF_CHUNK riff;
        private WaveFileFormat.FMT_CHUNK fmt;
        private WaveFileFormat.DATA_CHUNK data;
        public WaveStream(List<WaveSample> fullAudioStream) {
            WaveAudioFormat audioFormat = fullAudioStream?[0]?.SampleFormat;
            byte[] audioData = WaveGenerator.ToByteArray(fullAudioStream.ToArray());
            this.riff = new WaveFileFormat.RIFF_CHUNK();
            this.fmt = new WaveFileFormat.FMT_CHUNK(audioFormat);
            this.data = new WaveFileFormat.DATA_CHUNK(audioData);
            this.riff.SetChunkSize(4 + (8 + this.fmt.GetChunkSize()) + (8 + this.data.GetChunkSize()));
        }

        /// <summary>
        /// Aggregate all chunk items for a WAV stream in order (RIFF->FMT->DATA), then return the byte stream.
        /// The result of this method can be used to write WAV audio directly to a file, or to memory for later (ephemeral) use.
        /// </summary>
        /// <returns>A raw stream of bytes that represent a fully-encoded WAV sound format.</returns>
        public byte[] GetRawWaveStream() {
            var rawWaveStream = new List<byte>();
            rawWaveStream.AddRange(this.riff.GetChunk());
            rawWaveStream.AddRange(this.fmt.GetChunk());
            rawWaveStream.AddRange(this.data.GetChunk());
            return rawWaveStream.ToArray();
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        internal void Dispose(bool disposing) {
            if(!this._disposed) {
                if(disposing) { this.riff = null;  this.fmt = null; this.data = null; }
                // Indicate the instance has been disposed.
                this._disposed = true;
            }
        }
    }
}
