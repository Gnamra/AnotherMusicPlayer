using System;
using NAudio.Wave;

namespace PluginContracts
{
    public interface IDecoder
    {
        string GetName();
        void Seek(int time);
        WaveStream GetWaveStream();
    }
}
