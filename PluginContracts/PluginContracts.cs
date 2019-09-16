using System;
using NAudio.Wave;

namespace PluginContracts
{
    public interface IDecoder
    {
        string GetName();
        int Read(byte[] buffer, int offset, int count);
        void Seek(int time);
        WaveStream GetWaveStream();
    }
}
