using System.Collections.Generic;
using System.IO;

namespace AMPGUI.Models
{
    public class SongLoader
    {
        private List<string> Paths { get; set; }
        public SongLoader()
        {
            Paths = new List<string>();
        }
        public void AddPath(string path)
        {
            Paths.Add(path);
        }
        public void RemovePath(string path)
        {
            Paths.Remove(path);
        }

        public List<string> Scan()
        {
            List<string> result = new List<string>();
            string[] wavFiles = null;

            foreach(string path in Paths)
            {
                wavFiles = Directory.GetFiles(path, "*.wav");
            }

            if(wavFiles != null)
                result.AddRange(wavFiles);
            return result;
        }
    }
}
