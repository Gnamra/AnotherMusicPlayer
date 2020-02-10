using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace AnotherMusicPlayer
{
    public class AMPGui : Form
    {

        AnotherMusicPlayer amp;
        SongLoader songLoader;

        Scrollable scrLibrary;
        Splitter splitter1;
        ControlPanel ctrlPanel;

        int currentTime;

        public AMPGui()
        {
            amp = new AnotherMusicPlayer(@"Decoders\");
            songLoader = new SongLoader();
            songLoader.AddPath(Path.GetFullPath(@"Songs\"));
            ClientSize = new Size(600, 400);
            Title = "Another Music Player";

            var layout = new TableLayout();
            layout.Spacing = new Size(5, 5);
            layout.Padding = new Padding(10, 10, 10, 10);
            layout.BackgroundColor = Colors.LightGrey;
            var lbxLibrary = new ListBox();
            var library = new List<string>();
            ctrlPanel = new ControlPanel();
            var songs = songLoader.Scan();

            library.AddRange(songs);
            foreach(string s in songs)
            {
                amp.LoadSong(s);
            }

            lbxLibrary.DataStore = library;

            ctrlPanel.Play.Click += Play_Click;
            ctrlPanel.Stop.Click += Stop_Click;
            ctrlPanel.VolumeSlider.ValueChanged += (s, a) => amp.Volume = ctrlPanel.VolumeSlider.Value / 100.0f;
            
            splitter1 = new Splitter();
            scrLibrary = new Scrollable();

            scrLibrary.Content = lbxLibrary;
            scrLibrary.Border = BorderType.Line;
            scrLibrary.MouseWheel += ScrLibrary_MouseWheel;


            scrLibrary.Content.MouseDoubleClick += ScrLibrary_MouseDoubleClick;
            splitter1.Panel1 = scrLibrary;
            splitter1.Panel1MinimumSize = 100;
            splitter1.Panel2 = ctrlPanel;
            splitter1.Panel2MinimumSize = 100;
            splitter1.Orientation = Orientation.Vertical;
            splitter1.SplitterWidth = 10;

            layout.Rows.Add(splitter1);

            Content = layout;
        }

        private void ScrLibrary_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            amp.Stop();
            amp.Play(((ListBox)scrLibrary.Content).SelectedKey);
            ctrlPanel.Play.Image = new Bitmap(Path.GetFullPath(@"Resources\btnPause.bmp"));
            ctrlPanel.PlaybackSlider.MinValue = 0;
            ctrlPanel.PlaybackSlider.MaxValue = (amp.CurrentSong.TotalTime.Minutes * 60 + amp.CurrentSong.TotalTime.Seconds);
            ctrlPanel.PlaybackSlider.Value = currentTime;
            amp.CurrentSong.Elapsed += CurrentSong_Elapsed;
        }

        private void CurrentSong_Elapsed(object sender, EventArgs e)
        {
            ctrlPanel.PlaybackSlider.Value = (amp.CurrentSong.Time.Minutes * 60 + amp.CurrentSong.Time.Seconds);
        }


        private void Stop_Click(object sender, EventArgs e)
        {
            if (amp.State == PlayState.Playing || amp.State == PlayState.Paused)
            {
                amp.Stop();
                ctrlPanel.Play.Image = new Bitmap(Path.GetFullPath(@"Resources\btnPlay.bmp"));
            }
        }

        private void Play_Click(object sender, EventArgs e)
        {
            if (amp.State == PlayState.Playing)
            {
                amp.Pause();
                ctrlPanel.Play.Image = new Bitmap(Path.GetFullPath(@"Resources\btnPlay.bmp"));
            }
            else
            {
                if (amp.CurrentSong != null && amp.State == PlayState.Stopped)
                {
                    amp.Play(((ListBox)scrLibrary.Content).SelectedKey);
                    amp.CurrentSong.TimeTracker.Elapsed += (o, e) => ctrlPanel.PlaybackSlider.Value = (amp.CurrentSong.Time.Minutes * 60 + amp.CurrentSong.Time.Seconds);
                    ctrlPanel.PlaybackSlider.MinValue = 0;
                    ctrlPanel.PlaybackSlider.MaxValue = (amp.CurrentSong.TotalTime.Minutes * 60 + amp.CurrentSong.TotalTime.Seconds);
                    ctrlPanel.PlaybackSlider.Value = 0;
                }
                else amp.Resume();
                ctrlPanel.Play.Image = new Bitmap(Path.GetFullPath(@"Resources\btnPause.bmp"));
            }
        }

        private void ScrLibrary_MouseWheel(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Scroll!");
            int y = scrLibrary.ScrollPosition.Y + (10 * (int)-e.Delta.Height);
            scrLibrary.ScrollPosition = new Point(scrLibrary.ScrollPosition.X, y);
        }
    }
}
