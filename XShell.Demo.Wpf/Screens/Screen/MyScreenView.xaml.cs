using System.Windows.Controls;

namespace XShell.Demo.Wpf.Screens.Screen
{
    /// <summary>
    /// Interaction logic for MyScreenView.xaml
    /// </summary>
    public partial class MyScreenView : UserControl
    {
        public MyScreenView(IMyScreen screen)
        {
            DataContext = screen;
            InitializeComponent();
        }
    }
}
