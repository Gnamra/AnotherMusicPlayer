using Eto.Drawing;
using Eto.Forms;
using System;

namespace AnotherMusicPlayer
{
    public class MyCommand : Command
    {
        public MyCommand()
        {
            MenuText = "C&lick Me, Command";
            ToolBarText = "Click Me";
            ToolTip = "This shows a dialog for no reason";
            //Image = Icon.FromResource ("MyResourceName.ico");
            //Image = Bitmap.FromResource ("MyResourceName.png");
            Shortcut = Application.Instance.CommonModifier | Keys.M;  // control+M or cmd+M
        }

        protected override void OnExecuted(EventArgs e)
        {
            base.OnExecuted(e);

            MessageBox.Show(Application.Instance.MainForm, "You clicked me!", "Tutorial 2", MessageBoxButtons.OK);
        }
    }
    public class MyForm : Form
    {
        public MyForm()
        {
            ClientSize = new Size(600, 400);
            Title = "Menus and Toolbars";

            // create menu
            Menu = new MenuBar
            {
                Items =
                {
                    new ButtonMenuItem
                    {
                        Text = "&File",
                        Items =
                        { 
							// you can add commands or menu items
							new MyCommand(),
                            new ButtonMenuItem { Text = "Click Me, MenuItem" }
                        }
                    }
                },
                // quit item (goes in Application menu on OS X, File menu for others)
                QuitItem = new Command((sender, e) => Application.Instance.Quit())
                {
                    MenuText = "Quit",
                    Shortcut = Application.Instance.CommonModifier | Keys.Q
                },
                // about command (goes in Application menu on OS X, Help menu for others)
                AboutItem = new Command((sender, e) => new Dialog { Content = new Label { Text = "About my app..." }, ClientSize = new Size(200, 200) }.ShowModal(this))
                {
                    MenuText = "About my app"
                }
            };

            // create toolbar
            ToolBar = new ToolBar
            {
                Items =
                {
                    new MyCommand(),
                    new SeparatorToolItem(),
                    new ButtonToolItem { Text = "Click Me, ToolItem" }
                }
            };
        }
    }


    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Eto.Forms.Application().Run(new MyForm());
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
