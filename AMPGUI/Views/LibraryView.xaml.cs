using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AMPGUI.Views
{
    public class LibraryView : UserControl
    {
        public LibraryView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}