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

        public int Volume
        {
            get => volume;
            set
            {
                this.RaiseAndSetIfChanged(ref volume, value, "Volume");
                Player.Volume = volume/100.0f;
            }
        }
        public double SongLength { get => songLength; set => this.RaiseAndSetIfChanged(ref songLength, value, "SongLength"); }
        public double SongProgress { get => songProgress; set => this.RaiseAndSetIfChanged(ref songProgress, value, "SongProgress"); }

        private double songProgress;
        private double songLength;
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
                    if (currentSong != null)
                    {
                        currentSong.Stop();
                        currentSong.Elapsed -= CurrentSong_Elapsed;
                        currentSong.PlaybackStopped -= CurrentSong_PlaybackStopped;
                    }
                    currentSong = model;
                    currentSong.Elapsed += CurrentSong_Elapsed;
                    currentSong.PlaybackStopped += CurrentSong_PlaybackStopped;
                    SongLength = (currentSong.TotalTime.Minutes * 60000) + currentSong.TotalTime.Seconds * 1000 + currentSong.TotalTime.Milliseconds;

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
            SongProgress = (currentSong.Time.Minutes * 60000) + currentSong.Time.Seconds * 1000 + currentSong.Time.Milliseconds;
        }
    }
}
