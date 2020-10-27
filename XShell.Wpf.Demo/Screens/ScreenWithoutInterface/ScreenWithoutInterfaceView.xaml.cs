using System.Windows.Controls;

namespace XShell.Demo.Wpf.Screens.ScreenWithoutInterface
{
    /// <summary>
    /// Interaction logic for ScreenWithoutInterfaceView.xaml
    /// </summary>
    public partial class ScreenWithoutInterfaceView : UserControl
    {
        public ScreenWithoutInterfaceView(ScreenWithoutInterfaceViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
