using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using PluginContracts;
using NAudio.Wave;


namespace AnotherMusicplayer
{
    class AnotherMusicPlayer
    {
        public AnotherMusicPlayer()
        {

        }
    }

    class DecoderLoader
    {
        private string pluginFolderPath { get; }
        private ICollection<Assembly> assemblies { get; }
        private ICollection<Type> pluginTypes { get; }
        public List<IDecoder> decoders { get; }
        public DecoderLoader(string PathToPluginFolder)
        {
            pluginFolderPath = PathToPluginFolder;
            assemblies = LoadPluginAssemblies();
            pluginTypes = GetPluginTypes();
            decoders = GetDecoders();
        }

        private List<IDecoder> GetDecoders()
        {
            List<IDecoder> decs = new List<IDecoder>(pluginTypes.Count);
            foreach (Type type in pluginTypes)
            {
                IDecoder decoder = (IDecoder)Activator.CreateInstance(type, "Timelineの東.wav");
                decs.Add(decoder);

            }
            return decs;
        }
        private ICollection<Type> GetPluginTypes()
        {
            Type pluginType = typeof(IDecoder);
            ICollection<Type> pluginTypes = new List<Type>();

            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }
            }
            return pluginTypes;
        }
        private ICollection<Assembly> LoadPluginAssemblies()
        {
            try
            {
                string[] decoderFileNames = Directory.GetFiles(pluginFolderPath, "*.dll");
                ICollection<Assembly> assemblies = new List<Assembly>(decoderFileNames.Length);

                foreach(string decoder in decoderFileNames)
                {
                    Assembly assembly = Assembly.LoadFrom(pluginFolderPath + AssemblyName.GetAssemblyName(decoder).Name + ".dll" );
                    assemblies.Add(assembly);
                }
                return assemblies;
            }
            catch(Exception e)
            {
                Console.WriteLine(" =========== Failed to load plugin assemblies =========== ");
                Console.WriteLine(e.Message);
                throw e;
            }
        }
    }

    

    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            DecoderLoader pl = null;
            try
            {
                pl = args.Length > 0 ?
                new DecoderLoader(args[0]) : throw new Exception("No arguments provided");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }


            var waveOut = new WaveOutEvent
            {
                NumberOfBuffers = 2
            };

            
            waveOut.Init(pl.decoders[0].GetWaveStream());
            waveOut.Play();

            Console.WriteLine("Playing Timelineの東");

            int printTimeX = Console.CursorTop;
            string input = "";
            while (true)
            {
                Console.CursorTop = printTimeX;
                Console.Write(new string(' ', 10));
                Console.CursorLeft = 0;
                var time = pl.decoders[0].GetWaveStream().CurrentTime;
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
                        waveOut.Pause();
                    }
                    else if(input.ToLower().Equals("play"))
                    {
                        waveOut.Play();
                    }
                    else if(input.ToLower().Equals("stop"))
                    {
                        waveOut.Stop();
                    }
                    else if(input.ToLower().Contains("vol"))
                    {
                        try
                        {
                            string unparsedVolume = input.Substring(
                                4, 
                                input.Length - 4
                                );
                            float volume = float.TryParse(unparsedVolume, out float vol) == true ? vol : throw new Exception("Failed to parse volume");
                            volume = volume > 100 || volume < 0 ? throw new Exception("Invalid value for volume") : volume;
                            float normalizedVolume = volume / 100;
                            waveOut.Volume = normalizedVolume;
                            
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                if(waveOut.PlaybackState == PlaybackState.Stopped)
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
                        pl.decoders[0].GetWaveStream().Position = 0;
                        waveOut.Play();
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
