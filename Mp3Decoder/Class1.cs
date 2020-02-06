using NAudio.Wave;
using PluginContracts;
using System;

namespace MP3Decoder
{
    public class Class1 : IDecoder
    {
        public string GetName()
        {
            throw new NotImplementedException();
        }

        public WaveStream GetWaveStream()
        {
            throw new NotImplementedException();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public void Seek(int time)
        {
            throw new NotImplementedException();
        }
    }
}
