using System;
using NAudio.Wave;
using PluginContracts;

namespace AnotherMusicPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            AnotherMusicPlayer amp = new AnotherMusicPlayer(args[0]);

            Console.WriteLine("Playing Timelineの東");
            Playback song = amp.LoadSong("Timelineの東.wav");
            song.Play();
            int printTimeX = Console.CursorTop;
            string input = "";

            while (true)
            {
                Console.CursorTop = printTimeX;
                Console.Write(new string(' ', 10));
                Console.CursorLeft = 0;
                var time = song.Time;
                Console.Write(time.ToString(@"m\:ss"));
                if (Console.KeyAvailable)
                {
                    Console.CursorTop = printTimeX + 1;
                    Console.CursorLeft = 0;
                    int positionY = Console.CursorTop;
                    input = Console.ReadLine();
                    Console.CursorTop = positionY;
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.CursorLeft = 0;
                    if (input.ToLower().Equals("pause"))
                    {
                        song.Pause();
                    }
                    else if(input.ToLower().Equals("play"))
                    {
                        song.Play();
                    }
                    else if(input.ToLower().Equals("stop"))
                    {
                        song.Stop();
                    }
                    else if(input.ToLower().Contains("vol"))
                    {
                        try
                        {
                            // 4, because "vol " is 4 characters.
                            string unparsedVolume = input.Substring(4, input.Length - 4);
                            float volume = float.TryParse(unparsedVolume, out float vol) == true ? vol : throw new Exception("Failed to parse volume");
                            volume = volume > 100 || volume < 0 ? throw new Exception("Invalid value for volume") : volume;
                            float normalizedVolume = volume / 100;
                            song.Volume = normalizedVolume;
                            
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    else if(input.ToLower().Contains("seek"))
                    {
                        try
                        {   // 5, because "seek " is 5 characters.
                            string seekTime = input.Substring(5, input.Length - 5);
                            song.Seek(seekTime);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    else if(input.ToLower().Contains("load"))
                    {
                        try
                        {
                            // 5, because "load " is 5 characters.
                            string filePath = input.Substring(5, input.Length - 5);
                            song.Stop();
                            song = amp.LoadSong(filePath);

                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }

                if(song.State == PlaybackState.Stopped)
                {
                    Console.WriteLine("Restart? (Y / N)");
                    int positionY = Console.CursorTop - 1;
                    string answer = Console.ReadLine();
                    Console.CursorTop = positionY;

                    Console.WriteLine(new string(' ', Console.WindowWidth));
                    Console.WriteLine(new string(' ', Console.WindowWidth));
                    Console.CursorTop = positionY;
                    Console.CursorLeft = 0;
                    if (answer.ToLower().Equals("y"))
                    {
                        song.Position = 0;
                        song.Play();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
