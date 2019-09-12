using System;
using NAudio.Wave;

namespace PluginContracts
{
    public interface IDecoder
    {
        string GetName();
        int Read(byte[] buffer, int offset, int count);
        WaveStream GetWaveStream();
    }
}
