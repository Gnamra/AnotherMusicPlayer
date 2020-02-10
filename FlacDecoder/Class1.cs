using NAudio.Wave;
using PluginContracts;
using System;

namespace FlacDecoder
{
    public class FlacDecoder : WaveStream, IDecoder
    {
        public override WaveFormat WaveFormat => throw new NotImplementedException();

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string GetName()
        {
            return "Flac Decoder";
        }

        public WaveStream GetWaveStream()
        {
            return this;
        }

        public void Seek(int time)
        {

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
