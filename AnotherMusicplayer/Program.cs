using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace AnotherMusicPlayer
{
    public class btnPlay : Button
    {
        public btnPlay()
        {
            Image = new Bitmap(Path.GetFullPath(@"Resources\btnPlay.bmp"));
        }
    }

    public class ControlPanel : Panel
    {
        private enum ResizeTargetEnum
        {
            Top,
            Bottom,
            None
        }
        private ResizeTargetEnum ResizeTarget { get; set; }
        private TableLayout layout;
        public ControlPanel()
        {
            layout = new TableLayout();
            layout.Spacing = new Size(5, 5);
            layout.Padding = new Padding(10, 10, 10, 10);


            var btnPlay = new btnPlay();
            
            var cellBtnPlay = new TableCell(btnPlay);
            cellBtnPlay.ScaleWidth = true;
            layout.Rows.Add(new TableRow(
                cellBtnPlay));
            Content = layout;
       //     MouseMove += OnMouseMove;
        //    MouseUp += OnMouseUp;
        //    MouseDown += OnMouseDown;
        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Mouse released!");
            ResizeTarget = ResizeTargetEnum.None;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Mouse clicked!");
            if (e.Location.Y < 10) ResizeTarget = ResizeTargetEnum.Top;
            else if (e.Location.Y > Height - 10) ResizeTarget = ResizeTargetEnum.Bottom;

        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Location.Y <= 10 || e.Location.Y >= ((ControlPanel)sender).Height - 10)
            {
                Mouse.SetCursor(new Cursor(CursorType.HorizontalSplit));
            }
            if (ResizeTarget == ResizeTargetEnum.Bottom)
            {
                Height = e.Location.Y > 50 ? (int)e.Location.Y : 50;
            }
            else if(ResizeTarget == ResizeTargetEnum.Top)
            {
                var location = Location;
                location.Y = (int)e.Location.Y;
                Height -= (int)e.Delta.Height;
            }
        }
    }

    public class MyForm : Form
    {
        Scrollable scrLibrary;
        Splitter splitter1;
        Splitter splitter2;
        public MyForm()
        {
            ClientSize = new Size(600, 400);
            Title = "Another Music Player";

            var layout = new TableLayout();
            layout.Spacing = new Size(5, 5);
            layout.Padding = new Padding(10, 10, 10, 10);

            var lbxLibrary = new ListBox();
            var library = new List<string>();
            var controlPanel = new ControlPanel();
            library.Add("Timelineの東.wav");
            library.Add("Timelineの東2.wav");
            library.Add("Timelineの東3.wav");
            library.Add("Timelineの東4.wav");
            library.Add("Timelineの東5.wav");
            library.Add("Timelineの東6.wav");
            library.Add("Timelineの東7.wav");
            library.Add("Timelineの東8.wav");
            library.Add("Timelineの東1.wav");
            library.Add("Timelineの東2.wav");
            library.Add("Timelineの東3.wav");
            library.Add("Timelineの東4.wav");
            library.Add("Timelineの東12.wav");
            library.Add("Timelineの東3.wav");
            library.Add("Timelineの東21.wav");
            library.Add("Timelineの東.w3av");
            library.Add("Timelineの東.w21av");
            library.Add("Timelineの東.wa132v");
            library.Add("Timelineの東2.wav");
            library.Add("Timelineの東.wav");
            library.Add("Timelineの2東.wav");
            library.Add("Timelineの1東.wav");
            library.Add("Timeline5の東.wav");
            library.Add("Timelineの東.wav");

            lbxLibrary.DataStore = library;
            var btn2 = new btnPlay();
            var btn3 = new btnPlay();
            splitter1 = new Splitter();
            splitter2 = new Splitter();
            scrLibrary = new Scrollable();
            scrLibrary.Content = lbxLibrary;
            scrLibrary.Border = BorderType.Line;
            scrLibrary.MouseWheel += ScrLibrary_MouseWheel;
            splitter1.Panel1 = scrLibrary;
            splitter1.Panel1MinimumSize = 50;
            splitter1.Panel2 = controlPanel;
            splitter1.Panel2MinimumSize = 50;
            splitter1.Orientation = Orientation.Vertical;
            splitter1.SplitterWidth = 10;
          //  layout.Rows.Add(scrLibrary);
            layout.Rows.Add(splitter1);
            //layout.Rows.Add(null);

            splitter2.Panel1 = controlPanel;
            splitter2.Panel1MinimumSize = 50;
            splitter2.Panel2 = btn3;
            splitter2.Panel2MinimumSize = 50;
            splitter2.Orientation = Orientation.Vertical;
            splitter2.SplitterWidth = 10;
          //  layout.Rows.Add(splitter2);

            //var stack = new StackLayout();
            //stack.Orientation = Orientation.Vertical;
            //stack.VerticalContentAlignment = VerticalAlignment.Center;
            //var stackItem1 = new StackLayoutItem(scrLibrary);
            //var stackItem2 = new StackLayoutItem(controlPanel);
            //var stackItem3 = new StackLayoutItem(btn2);
            //stack.Items.Add(stackItem1);
            //stack.Items.Add(stackItem2);
            //stack.Items.Add(stackItem3);

            Content = layout;
           // scrLibrary.UpdateScrollSizes();
        }

        private void ScrLibrary_MouseWheel(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Scroll!");
            int y = scrLibrary.ScrollPosition.Y + (10 * (int)-e.Delta.Height);
            scrLibrary.ScrollPosition = new Point(scrLibrary.ScrollPosition.X, y);
        }

        private void ScrLibrary_Scroll(object sender, ScrollEventArgs e)
        {
            Console.WriteLine("Scroll!");
            //scrLibrary.UpdateScrollSizes();
        }
    }


    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Application().Run(new MyForm());
            Console.CursorVisible = false;

            AnotherMusicPlayer amp = new AnotherMusicPlayer(args[0]);

            Console.WriteLine("Playing Timelineの東");
            bool songLoaded = amp.LoadSong("Timelineの東.wav");
            if(!songLoaded)
            {
                Console.WriteLine("Unable to load song");
            }
            amp.Play(0);
            int printTimeX = Console.CursorTop;
            string input;

            while (!amp.ExitRequested)
            {
                amp.DisplaySongInfo();
                if (Console.KeyAvailable)
                {
                    Console.CursorTop = printTimeX + 1;
                    Console.CursorLeft = 0;
                    int positionY = Console.CursorTop;
                    input = Console.ReadLine();
                    input = input.ToUpperInvariant();
                    Console.CursorTop = positionY;
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.CursorLeft = 0;

                    string[] command = input.Split(" ");
                    switch (command[0].ToUpperInvariant())
                    {
                        case "PAUSE":
                            amp.CurrentSong.Pause();
                            break;
                        case "RESUME":
                            amp.Resume();
                            break;
                        case "PLAY":
                            amp.Play();
                            break;
                        case "STOP":
                            amp.Stop();
                            break;
                        case "VOL":
                            float volume = float.TryParse(command[1], out float vol) == true ? vol : throw new Exception("Failed to parse volume");
                            volume = volume > 100 || volume < 0 ? throw new Exception("Invalid value for volume") : volume;
                            float normalizedVolume = volume / 100;
                            amp.CurrentSong.Volume = normalizedVolume;
                            break;
                        case "SEEK":
                            amp.CurrentSong.Seek(command[1]);
                            break;
                        case "LOAD":
                            amp.LoadSong(command[1]);
                            break;
                        case "NEXT":
                            amp.NextSong();
                            break;
                        case "EXIT":
                            amp.ExitRequested = true;
                            break;
                    }
                }
            }
        }
    }
}
