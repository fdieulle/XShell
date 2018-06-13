using System.Windows;

namespace XShell.Demo.Wpf
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly XShellModule _module;

        public MainWindow()
        {
            InitializeComponent();

            _module = new XShellModule(this);
        }
    }
}
