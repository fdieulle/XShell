using System.Windows.Controls;

namespace XShell.Demo.Wpf.Screens.Popup
{
    /// <summary>
    /// Interaction logic for MyPopupView.xaml
    /// </summary>
    public partial class MyPopupView : UserControl
    {
        public MyPopupView(IMyPopup viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
