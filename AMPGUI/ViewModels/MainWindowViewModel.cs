using Avalonia;
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using AMPGUI.Models;

namespace AMPGUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private double scaleX;
        private double scaleY;
        public double ScaleX { get => scaleX; set => this.RaiseAndSetIfChanged(ref scaleX, value, "ScaleX"); }
        public double ScaleY { get => scaleY; set => this.RaiseAndSetIfChanged(ref scaleY, value, "ScaleY"); }

        public LibraryViewModel Library { get; }
        public MediaControllerViewModel MediaController { get; }

        private AnotherMusicPlayer Player { get; set; }
        public MainWindowViewModel()
        {
            ScaleX = 1;
            ScaleY = 1;

            Player = new AnotherMusicPlayer("Decoders\\");

            Library = new LibraryViewModel(Player);
            MediaController = new MediaControllerViewModel(Player);

        }
    }
}
