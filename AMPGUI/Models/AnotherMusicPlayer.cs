using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMPGUI.Models
{
    public struct LibraryEntry : IEquatable<LibraryEntry>
    {
        public string Path { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator !=(LibraryEntry left, LibraryEntry right)
        {
            return !(left == right);
        }

        public static bool operator ==(LibraryEntry left, LibraryEntry right)
        {
            return !(left != right);
        }

        public bool Equals(LibraryEntry other)
        {
            return base.Equals(other);
        }
    }

    public enum PlayState
    {
        Playing,
        Paused,
        Stopped
    }

    public class AnotherMusicPlayer
    {
        private DecoderLoader DecoderLoader { get; }
        private List<LibraryEntry> Library { get; }
        public bool RepeatSong { get; set; }
        private int CurrentSongIndex { get; set; }
        public Playback CurrentSong { get; set; }
        public PlayState State { get; private set; }

        public float Volume
        {
            get
            {
                return CurrentSong.Volume;
            }
            set
            {
                if (value > 1.0f) value = 1.0f;
                else if (value < 0.0f) value = 0.0f;
                if(CurrentSong != null) CurrentSong.Volume = value;
            }
        }
        public AnotherMusicPlayer(string path)
        {
            try
            {
                DecoderLoader = path != null ?
                new DecoderLoader(path) : throw new Exception("No filepath provided");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            Library = new List<LibraryEntry>();
            State = PlayState.Stopped;
        }

        public bool LoadSong(string path)
        {
            string name = path?.Substring(0, path.Length - 4);
            Library.Add(new LibraryEntry { Name = name, Path = path});
            return true;
        }

        public void Stop()
        {
            Console.WriteLine("Song stopped!");
            State = PlayState.Stopped;
            CurrentSong?.Stop();
            CurrentSong = null;
        }
        public void Pause()
        {
            CurrentSong?.Pause();
            State = PlayState.Paused;
        }
        public void Resume()
        {
            if(CurrentSong == null)
                Play();
            else
                CurrentSong?.Play();
            State = PlayState.Playing;

        }
        public Playback Play()
        {
            Stop();
            CurrentSong = new Playback(Library[CurrentSongIndex].Path, DecoderLoader);
            CurrentSong.PlaybackStopped += CurrentSong_PlaybackStopped;
            CurrentSong.Play();
            State = PlayState.Playing;
            return CurrentSong;
        }

        public async Task<Playback> PlayAsync()
        {
            Stop();
            CurrentSong = await Task.Run(() =>
            {
                return new Playback(Library[CurrentSongIndex].Path, DecoderLoader);
            });
            CurrentSong.PlaybackStopped += CurrentSong_PlaybackStopped;
            CurrentSong.Play();
            State = PlayState.Playing;
            return CurrentSong;
        }

        public void Play(string path)
        {
            Stop();
            CurrentSong = new Playback(path, DecoderLoader);
            CurrentSong.PlaybackStopped += CurrentSong_PlaybackStopped;
            CurrentSong.Play();
            State = PlayState.Playing;
        }

        private void CurrentSong_PlaybackStopped(object sender, EventArgs e)
        {
            if(State != PlayState.Stopped)
                NextSong();
        }

        public void Play(int songIndex)
        {
            if (songIndex < 0 || songIndex > Library.Count) throw new Exception("Invalid song index");
            CurrentSongIndex = songIndex;
            Stop();
            Play();
        }
        public Playback NextSong()
        {
            Stop();
            CurrentSongIndex = CurrentSongIndex == Library.Count-1 ? 0 : ++CurrentSongIndex;
            return Play();
        }

    }
}
