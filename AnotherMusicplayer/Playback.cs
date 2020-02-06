using System;
using System.Globalization;
using NAudio.Wave;
using PluginContracts;

namespace AnotherMusicPlayer
{
    /// <summary>
    /// Represents a playback. You can play, pause, stop and seek through a playback.
    /// </summary>
    public class Playback
    {
        public TimeSpan Time { get { return WaveStream.CurrentTime; } }
        public long Position
        {
            get { return Decoder.GetWaveStream().Position; }
            set { Decoder.GetWaveStream().Position = value; }
        }
        public float Volume
        {
            get { return WaveOut.Volume; }
            set { WaveOut.Volume = value; }
        }
        public PlaybackState State { get { return WaveOut.PlaybackState; } }

        private IDecoder Decoder { get; }
        private WaveStream WaveStream { get; }
        private WaveOutEvent WaveOut { get; }
   
        public Playback(string pathToSong, DecoderLoader dl)
        {
            Decoder = dl?.GetDecoder(pathToSong);
            WaveStream = Decoder.GetWaveStream();
            WaveOut = new WaveOutEvent { NumberOfBuffers = 2 };
            WaveOut.Init(WaveStream);
        }
        public void Stop()
        {
            WaveOut.Stop();
            WaveOut.Dispose();
            WaveStream.Dispose();
        }
        public void Play() => WaveOut.Play();
        public void Pause() => WaveOut.Pause();
        public void Seek(string seekTime)
        {
            try
            {
                string[] timeSplit = seekTime?.Contains(':') == true ?
                    seekTime.Split(':') :
                    throw new Exception("Input string was not in a correct format.");

                TimeSpan time = TimeSpan.ParseExact(seekTime, "%m\\:%s", CultureInfo.InvariantCulture);
               
                Decoder.Seek(time.Minutes * 60 + time.Seconds);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        } 
    }
}
