using Eto.Drawing;
using Eto.Forms;
using System.IO;

namespace AnotherMusicPlayer
{
    public class ControlPanel : Panel
    {
        private TableLayout layout;
        public Button Play { get; set; }
        public Button Stop { get; set; }
        public Slider VolumeSlider { get; set; }
        public Slider PlaybackSlider { get; set; }
        public ControlPanel()
        {
            layout = new TableLayout();
            layout.Spacing = new Size(5, 5);
            layout.Padding = new Padding(10, 10, 10, 10);

            // create and set up buttons
            Play = new Button();
            Play.Image = new Bitmap(Path.GetFullPath(@"Resources\btnPlay.bmp"));
            Stop = new Button();
            Stop.Image = new Bitmap(Path.GetFullPath(@"Resources\btnStop.bmp"));

            VolumeSlider = new Slider();
            VolumeSlider.MinValue = 0;
            VolumeSlider.MaxValue = 100;
            VolumeSlider.Value = 50;

            PlaybackSlider = new Slider();

            layout.Rows.Add(new TableRow(
                new TableCell(Play),
                new TableCell(Stop),
                new TableCell(VolumeSlider),
                new TableCell(PlaybackSlider),
                null));
            Content = layout;
        }
    }
}
