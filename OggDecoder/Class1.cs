using NAudio.Wave;
using PluginContracts;
using System;

namespace OggDecoder
{
    public class OggDecoder : IDecoder
    {
        public string GetName()
        {
            return "OggDecoder";
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
