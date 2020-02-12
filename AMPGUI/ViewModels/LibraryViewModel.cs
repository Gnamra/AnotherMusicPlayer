using AMPGUI.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace AMPGUI.ViewModels
{
    public class LibraryViewModel : ViewModelBase
    {
        ObservableCollection<string> SongList { get; set; }
        public AnotherMusicPlayer Player { get; }
        public LibraryViewModel(AnotherMusicPlayer player)
        {
            Player = player;
            SongLoader loader = new SongLoader();
            loader.AddPath(Path.GetFullPath(@"Songs\"));
            SongList = new ObservableCollection<string>(loader.Scan());
            foreach(string s in SongList)
            {
                Player.LoadSong(s);
            }
        }

    }
}
