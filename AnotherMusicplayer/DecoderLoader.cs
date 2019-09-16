    using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using PluginContracts;


namespace AnotherMusicPlayer
{
    class DecoderLoader
    {
        private string PluginFolderPath { get; }
        private ICollection<Assembly> Assemblies { get; }
        private List<Type> PluginTypes { get; }
        public DecoderLoader(string PathToPluginFolder)
        {
            PluginFolderPath = PathToPluginFolder;
            Assemblies = LoadPluginAssemblies();
            PluginTypes = GetPluginTypes();
           // Decoders = GetDecoders();
        }

        public List<IDecoder> GetDecoders()
        {
            List<IDecoder> decs = new List<IDecoder>(PluginTypes.Count);
            foreach (Type type in PluginTypes)
            {
                Console.WriteLine(type.Name);
                IDecoder decoder = (IDecoder)Activator.CreateInstance(type, "Timelineの東.wav");
                decs.Add(decoder);

            }
            return decs;
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
                throw e;
            }
        }

        public IDecoder CreateWaveDecoder(string file)
        {
            Type waveDecoderType = PluginTypes.Find((x) => x.Name.Equals("WaveDecoder")) ??
                throw new Exception($"Unable to load {file}. Decoder unavailable.");

            IDecoder decoder = (IDecoder)Activator.CreateInstance(waveDecoderType, file);
            return decoder;
        }
    }
}
