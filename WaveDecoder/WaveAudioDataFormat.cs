using System;

namespace WaveDecoder
{
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
            public int SampleRate { get; set; }
            public int ByteRate { get; set; }
            public UInt16 BlockAlign { get; set; }
            public UInt16 BitsPerSample { get; set; }
        }

        //DATA
        public struct DataChunk
        {
            public string ID { get; set; }
            public int Size { get; set; }
            public Byte[] Data { get; set; }
        }

        // Other sub-chunks go here
        // ...

        public RiffChunk RIff { get; set; }
        public FormatChunk Format { get; set; }
        public DataChunk Data { get; set; }

    }
}
