using System;
using System.Diagnostics;
using System.Globalization;
using System.Timers;
using NAudio.Wave;
using PluginContracts;

namespace AnotherMusicPlayer
{
    /// <summary>
    /// Represents a playback. You can play, pause, stop and seek through a playback.
    /// </summary>
    public class Playback
    {
        /// <summary>
        /// Current time of playback
        /// </summary>
        public TimeSpan Time { get { return WaveStream.CurrentTime; } }

        /// <summary>
        /// Length of the audio file
        /// </summary>
        public TimeSpan TotalTime { get { return WaveStream.TotalTime; } }

        public Timer TimeTracker { get;  }

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

        // Events
        public event EventHandler PlaybackStopped;
        public event EventHandler Elapsed;

        public Playback(string pathToSong, DecoderLoader dl)
        {
            if (pathToSong == null)
                return;

            Decoder = dl?.GetDecoder(pathToSong);
            WaveStream = Decoder.GetWaveStream();
            WaveOut = new WaveOutEvent { NumberOfBuffers = 2 };
            WaveOut.Init(WaveStream);
            WaveOut.PlaybackStopped += WaveOut_PlaybackStopped;

            TimeTracker = new Timer(1000);
            TimeTracker.AutoReset = true;
            TimeTracker.Elapsed += TimeTracker_Elapsed;
        }

        private void TimeTracker_Elapsed(object sender, ElapsedEventArgs e)
        {
            Elapsed?.Invoke(sender, e);
        }

        private void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            PlaybackStopped?.Invoke(sender, e);
        }

        public void Stop()
        {
            TimeTracker.Stop();
            WaveOut.Stop();
            WaveOut.Dispose();
            WaveStream.Dispose();
            TimeTracker.Dispose();
        }
        public void Play()
        {
            TimeTracker.Enabled = true;
            WaveOut.Play();
        }
        public void Pause()
        {
            WaveOut.Pause();
        }
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
