using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnotherMusicPlayer.Decoders.Wave
{

    /// <summary>
    /// Class that represents the wave audio file format.
    /// </summary>
    class WaveAudioDataFormat
    {
        // http://soundfile.sapp.org/doc/WaveFormat/
        // RIFF
        public struct RiffChunk
        {
            public string ID { get; set; }
            public int Size { get; set; }
            public string Format { get; set; }
        } 

        //FORMAT
        public struct FormatChunk
        {
            public string ID { get; set; }
            public int Size { get; set; }
            public UInt16 AudioFormat { get; set; }
            public UInt16 NumChannels { get; set; }
            public UInt32 SampleRate { get; set; }
            public UInt32 ByteRate { get; set; }
            public UInt16 BlockAlign { get; set; }
            public UInt16 BitsPerSample { get; set; }
        }

        //DATA
        public struct DataChunk
        {
            public string ID { get; set;}
            public int Size { get; set; }
            public Byte[] Data { get; set; }
        }

        // Other sub-chunks go here
        // ...

        public RiffChunk RIff { get; set; }
        public FormatChunk Format { get; set; }
        public DataChunk Data { get; set; }

    }


    /// <summary>
    /// Decodes and provides information about Wave files
    /// </summary>
    class WaveDecoder
    {
        private BinaryReader AudioFileReader { get; set; }
        private WaveAudioDataFormat AudioFile { get; set; }
        private string CurrentChunkId { get; set; }
        private int CurrentChunkSize { get; set; }
        private int CurrentChunkStart { get; set; }

        public WaveDecoder(string file)
        {
            CurrentChunkId = "";
            CurrentChunkSize = 0;
            AudioFile = new WaveAudioDataFormat();

            try
            {
                using (AudioFileReader = new BinaryReader(File.OpenRead(file)))
                {
                    AudioFile.RIff = ExtractRiffData();
                    AudioFile.Format = ExctractFormatData();
                    AudioFile.Data = ExtractAudioData();
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Program shutting down.");
                Environment.Exit(1);
            }
        }

        private void NextChunk()
        {
            // Add check to see if at the start of a chunk or not

            CurrentChunkStart = (int)AudioFileReader.BaseStream.Position;
            CurrentChunkId = new string(AudioFileReader.ReadChars(4));
            Console.WriteLine(CurrentChunkId);

            if (CurrentChunkId.Equals("RIFF"))
            {
                CurrentChunkSize = AudioFileReader.ReadInt32();
                var type = new string(AudioFileReader.ReadChars(4));
            }
            else
            {
                // Doesn't account for pad bytes
                CurrentChunkSize = AudioFileReader.ReadInt32();
                AudioFileReader.ReadBytes(CurrentChunkSize);
            }
        }
        private WaveAudioDataFormat.RiffChunk ExtractRiffData()
        {
            WaveAudioDataFormat.RiffChunk data = new WaveAudioDataFormat.RiffChunk
            {
                ID = new string(AudioFileReader.ReadChars(4)),
                Size = AudioFileReader.ReadInt32(),
                Format = new string(AudioFileReader.ReadChars(4))
            };

            return data;
        }

        private WaveAudioDataFormat.FormatChunk ExctractFormatData()
        {
            while (!CurrentChunkId.Equals("fmt ")) { NextChunk(); };
            AudioFileReader.BaseStream.Position = CurrentChunkStart + 8;
            WaveAudioDataFormat.FormatChunk data = new WaveAudioDataFormat.FormatChunk
            {
                ID = CurrentChunkId,
                Size = CurrentChunkSize,
                AudioFormat = AudioFileReader.ReadUInt16(),
                NumChannels = AudioFileReader.ReadUInt16(),
                SampleRate = AudioFileReader.ReadUInt32(),
                ByteRate = AudioFileReader.ReadUInt32(),
                BlockAlign = AudioFileReader.ReadUInt16(),
                BitsPerSample = AudioFileReader.ReadUInt16()
            };
            Console.WriteLine("========= FORMAT CHUNK =========");
            Console.WriteLine("0: Chunk id: " + new string(data.ID));
            Console.WriteLine($"4: File size: {data.Size}");
            Console.WriteLine($"6: Audio format: {data.AudioFormat}");
            Console.WriteLine($"8: Channels: {data.NumChannels}");
            Console.WriteLine($"12: Sample rate: {data.SampleRate}");
            Console.WriteLine($"16: Byte rate: {data.ByteRate}");
            Console.WriteLine($"18: Block alignment: {data.BlockAlign}");
            Console.WriteLine($"20: Bits per sample: {data.BitsPerSample}");

            return data;
        }

        private WaveAudioDataFormat.DataChunk ExtractAudioData()
        {
            CurrentChunkStart = (int)AudioFileReader.BaseStream.Position;
            while (!CurrentChunkId.Equals("data")) { NextChunk(); };
            AudioFileReader.BaseStream.Position = CurrentChunkStart;
            WaveAudioDataFormat.DataChunk data = new WaveAudioDataFormat.DataChunk
            {
                ID = CurrentChunkId,
                Size = CurrentChunkSize,
                Data = AudioFileReader.ReadBytes(CurrentChunkSize)
            };
            Console.WriteLine("========= DATA CHUNK =========");
            Console.WriteLine("0: Chunk id: " + data.ID);
            Console.WriteLine($"4: Chunk size: {data.Size}");
            Console.WriteLine($"8: Chunk Data: {data.Data}");
            return data;

        }

        public Byte[] GetAudioData()
        {
            return AudioFile.Data.Data;
        }
    }
}
