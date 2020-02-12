using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AMPGUI.Views
{
    public class MediaControllerView : UserControl
    {
        public MediaControllerView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}