using System;
using System.Globalization;
using NAudio.Wave;
using PluginContracts;

namespace AnotherMusicPlayer
{
    class Playback
    {
        public TimeSpan Time { get { return Decoder.GetWaveStream().CurrentTime; } }
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
        private WaveOutEvent WaveOut { get; }
   
        public Playback(string pathToSong, DecoderLoader dl)
        {
            Decoder = dl.CreateWaveDecoder(pathToSong);
            WaveOut = new WaveOutEvent { NumberOfBuffers = 2 };
            WaveOut.Init(Decoder.GetWaveStream());
        }
        public void Stop() => WaveOut.Stop();
        public void Play() => WaveOut.Play();
        public void Pause() => WaveOut.Pause();
        public void Seek(string seekTime)
        {
            try
            {
                string[] timeSplit = seekTime.Contains(':') ?
                    seekTime.Split(':') :
                    throw new Exception("Input string was not in a correct format.");

                TimeSpan time = TimeSpan.ParseExact(seekTime, "%m\\:%s", CultureInfo.InvariantCulture);
                int top = Console.CursorTop;
                Console.CursorTop = 20;
                Console.WriteLine(time.Minutes * 60 + time.Seconds);
                Console.CursorTop = top;
                Decoder.Seek(time.Minutes * 60 + time.Seconds);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        } 
    }
}
