using System;
using NAudio.Wave;

namespace PluginContracts
{
    public interface IDecoder
    {
        /// <summary>
        /// Returns the name of the decoder
        /// </summary>
        /// <returns></returns>
        string GetName();
        /// <summary>
        /// Seeks to specified second.
        /// </summary>
        /// <param name="time">Second to seek to.</param>
        void Seek(int time);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        WaveStream GetWaveStream();
    }
}
