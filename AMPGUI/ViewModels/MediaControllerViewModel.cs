using AMPGUI.Models;
using AMPGUI.ViewModels;
using NAudio.Wave;
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
        public AnotherMusicPlayer Player { get; set; }
        public ReactiveCommand<Unit, Playback> Play { get; }
        public ReactiveCommand<Unit, Unit> Stop { get; }
        public ReactiveCommand<Unit, Playback> Next { get; }
        public ReactiveCommand<Unit, Playback> Previous { get; }

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
            SongLength = 1;
            SongProgress = 0;


            Player = player;
            Play = ReactiveCommand.CreateFromTask(Player.PlayAsync);
            Stop = ReactiveCommand.Create(Player.Stop);
            Next = ReactiveCommand.Create(Player.NextSong);

            Observable.Merge(Play, Next).Subscribe(model =>
            {
                if (model != null)
                {
                    currentSong = model;
                    currentSong.Elapsed += CurrentSong_Elapsed;
                    currentSong.PlaybackStopped += CurrentSong_PlaybackStopped;
                    SongLength = (currentSong.TotalTime.Minutes * 60) + currentSong.TotalTime.Seconds;
                }
            });
        }

        private void CurrentSong_PlaybackStopped(object sender, EventArgs e)
        {
            SongLength = 1;
            SongProgress = 0;
        }

        private void CurrentSong_Elapsed(object sender, EventArgs e)
        {
            SongLength = (currentSong.TotalTime.Minutes * 60) + currentSong.TotalTime.Seconds;
            SongProgress = (currentSong.Time.Minutes * 60) + currentSong.Time.Seconds;
        }
    }
}
