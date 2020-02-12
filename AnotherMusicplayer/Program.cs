using System;

namespace AnotherMusicPlayer
{
    class Program
    {
        static void Main(string[] args)
        {

            AnotherMusicPlayer amp = new AnotherMusicPlayer(args[0]);

            Console.WriteLine("Playing Timelineの東");
            bool songLoaded = amp.LoadSong("Timelineの東.wav");
            if (!songLoaded)
            {
                Console.WriteLine("Unable to load song");
            }
            amp.Play(0);
            int printTimeX = Console.CursorTop;
            string input;

            while (true)
            {
                //amp.DisplaySongInfo();
                if (Console.KeyAvailable)
                {
                    Console.CursorTop = printTimeX + 1;
                    Console.CursorLeft = 0;
                    int positionY = Console.CursorTop;
                    input = Console.ReadLine();
                    input = input.ToUpperInvariant();
                    Console.CursorTop = positionY;
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.CursorLeft = 0;

                    string[] command = input.Split(" ");
                    switch (command[0].ToUpperInvariant())
                    {
                        case "PAUSE":
                            amp.CurrentSong.Pause();
                            break;
                        case "RESUME":
                            amp.Resume();
                            break;
                        case "PLAY":
                            amp.Play();
                            break;
                        case "STOP":
                            amp.Stop();
                            break;
                        case "VOL":
                            float volume = float.TryParse(command[1], out float vol) == true ? vol : throw new Exception("Failed to parse volume");
                            volume = volume > 100 || volume < 0 ? throw new Exception("Invalid value for volume") : volume;
                            float normalizedVolume = volume / 100;
                            amp.CurrentSong.Volume = normalizedVolume;
                            break;
                        case "SEEK":
                            amp.CurrentSong.Seek(command[1]);
                            break;
                        case "LOAD":
                            amp.LoadSong(command[1]);
                            break;
                        case "NEXT":
                            amp.NextSong();
                            break;
                        case "EXIT":
                            //amp.ExitRequested = true;
                            break;
                    }
                }
            }
        }
    }
}
