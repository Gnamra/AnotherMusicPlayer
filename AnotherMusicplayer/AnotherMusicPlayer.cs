using System;
using System.Collections.Generic;

namespace AnotherMusicPlayer
{
    class AnotherMusicPlayer
    {
        DecoderLoader DecoderLoader { get; }
        private LinkedList<Playback> Playbacklist { get; set; }

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
            Playbacklist = new LinkedList<Playback>();
        }

        public Playback LoadSong(string pathToSong)
        {
            Console.Clear();
            Playback p = new Playback(pathToSong, DecoderLoader);
            Playbacklist.AddLast(p);
            return p;
        }

        public Playback GetCurrentSong()
        {
            return Playbacklist.Last.Value;
        }
    }
}
