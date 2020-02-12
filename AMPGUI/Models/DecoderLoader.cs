using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NAudio.Wave;
using PluginContracts;


namespace AMPGUI.Models
{
    public class DecoderLoader
    {

        private string PluginFolderPath { get; }
        private ICollection<Assembly> Assemblies { get; }
        private List<Type> PluginTypes { get; }
        
        public DecoderLoader(string PathToPluginFolder)
        {
            PluginFolderPath = PathToPluginFolder;
            Assemblies = LoadPluginAssemblies();
            PluginTypes = GetPluginTypes();
        }

        public IDecoder GetDecoder(string file)
        {
            string filetype = file?.Substring(file.Length - 3, 3).ToUpperInvariant();
            Console.WriteLine("Type: " + filetype);
            switch(filetype)
            {
                case "WAV":
                    return CreateWaveDecoder(file);
                case "MP3":
                    Mp3FileReader fr = new Mp3FileReader(file);
                    return (IDecoder)fr;
                default:
                    throw new Exception("Decoder not found!");
            }
        }

        private List<Type> GetPluginTypes()
        {
            Type pluginType = typeof(IDecoder);
            List<Type> pluginTypes = new List<Type>();

            foreach (Assembly assembly in Assemblies)
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
                string[] decoderFileNames = Directory.GetFiles(PluginFolderPath, "*.dll");
                ICollection<Assembly> assemblies = new List<Assembly>(decoderFileNames.Length);

                foreach(string decoder in decoderFileNames)
                {
                    Assembly assembly = Assembly.LoadFrom(PluginFolderPath + AssemblyName.GetAssemblyName(decoder).Name + ".dll" );
                    assemblies.Add(assembly);
                }
                return assemblies;
            }
            catch(Exception e)
            {
                Console.WriteLine(" =========== Failed to load plugin assemblies =========== ");
                Console.WriteLine(e.Message);
                throw;
            }
        }
        private IDecoder CreateWaveDecoder(string file)
        {
            Type waveDecoderType = PluginTypes.Find((x) => x.Name.Equals("WaveDecoder")) ??
                throw new Exception($"Unable to load {file}. Decoder unavailable.");
            IDecoder decoder = (IDecoder)Activator.CreateInstance(waveDecoderType, file);
            return decoder;
        }
    }
}
