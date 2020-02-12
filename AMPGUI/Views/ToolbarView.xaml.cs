using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AMPGUI.Views
{
    public class ToolbarView : UserControl
    {
        public ToolbarView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}