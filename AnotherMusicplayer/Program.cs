using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using PluginContracts;
using NAudio.Wave;

namespace AnotherMusicplayer
{

    class Program
    {
        static void Main(string[] args)
        {

            #region Put this into plugin loader class
            string[] decoderFileNames = null;
            if (Directory.Exists(@"C:\Users\magnar.kleppe\source\repos\AnotherMusicplayer\AnotherMusicplayer\Decoders"))
            {
                decoderFileNames = Directory.GetFiles(@"C:\Users\magnar.kleppe\source\repos\AnotherMusicplayer\AnotherMusicplayer\Decoders", "*.dll");
            }
            else
                Environment.Exit(0);

            ICollection<Assembly> assemblies = new List<Assembly>(decoderFileNames.Length);
            foreach (string decoder in decoderFileNames)
            {
                try
                {
                    Console.WriteLine("Found: " + decoder);
                    AssemblyName an = AssemblyName.GetAssemblyName(decoder);

                    Console.WriteLine("Assembly name: " + an.Name);
                    Assembly assembly = Assembly.LoadFrom(@"C:\Users\magnar.kleppe\source\repos\AnotherMusicplayer\AnotherMusicplayer\Decoders\" + an.Name + ".dll");

                    assemblies.Add(assembly);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Type pluginType = typeof(IDecoder);
            ICollection<Type> pluginTypes = new List<Type>();

            foreach(Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach(Type type in types)
                    {
                        if(type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if(type.GetInterface(pluginType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }
            }

            List<IDecoder> decoders = new List<IDecoder>(pluginTypes.Count);
            foreach(Type type in pluginTypes)
            {
                IDecoder decoder = (IDecoder)Activator.CreateInstance(type, "Timelineの東.wav");
                decoders.Add(decoder);
                Console.WriteLine(decoder.GetName());

            }
            #endregion

            var waveOut = new WaveOutEvent
            {
                NumberOfBuffers = 2
            };

            
            waveOut.Init(decoders[0].GetWaveStream());
            waveOut.Play();

            Console.WriteLine("Playing song!");

            string input = "";
            while (true)
            {
                if(Console.KeyAvailable)
                {
                    input = Console.ReadLine();
                    Console.WriteLine($"\ninput: {input}");
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
                    string answer = Console.ReadLine();
                    if(answer.ToLower().Equals("y"))
                    {
                        decoders[0].GetWaveStream().Position = 0;
                        waveOut.Play();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public void ParseInput(string input)
        {

        }
    }
}
