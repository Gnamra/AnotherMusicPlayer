using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AMPGUI.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            Position = Position.WithX(5000).WithY(250);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
