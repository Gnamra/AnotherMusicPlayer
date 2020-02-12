using AMPGUI.Models;
using AMPGUI.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace AMPGUI.ViewModels
{
    public class MediaControllerViewModel : ViewModelBase
    {
        AnotherMusicPlayer player;
        public AnotherMusicPlayer Player { get => player; set => player = value; }
        public ReactiveCommand<Unit, Playback> Play { get; }
        public ReactiveCommand<Unit, Unit> Stop { get; }

        private int songLength;
        public int Volume
        {
            get => volume;
            set
            {
                this.RaiseAndSetIfChanged(ref volume, value, "Volume");
                Player.Volume = volume/100.0f;
            }
        }

        public int SongLength { get => songLength; set => this.RaiseAndSetIfChanged(ref songLength, value, "SongLength"); }
        public int SongProgress { get => songProgress; set => this.RaiseAndSetIfChanged(ref songProgress, value, "SongProgress"); }

        private int songProgress;
        private int volume;

        private Playback currentSong;

        public MediaControllerViewModel(AnotherMusicPlayer player)
        {
            Player = player;
            Play = ReactiveCommand.Create(Player.Play);

            Observable.Merge(Play).Subscribe(model =>
            {
                if (model != null)
                {
                    currentSong = model;
                    model.Elapsed += CurrentSong_Elapsed;
                }

            });

            Stop = ReactiveCommand.Create(Player.Stop);
            SongLength = 360;
            SongProgress = 0;
        }

        private void CurrentSong_Elapsed(object sender, EventArgs e)
        {
            Console.WriteLine("Updated time!");
            SongLength = (currentSong.TotalTime.Minutes * 60) + currentSong.TotalTime.Seconds;
            SongProgress = (currentSong.Time.Minutes * 60) + currentSong.Time.Seconds;
        }
    }
}
