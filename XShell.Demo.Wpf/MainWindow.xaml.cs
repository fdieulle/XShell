using System.Windows;

namespace XShell.Demo.Wpf
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly XShellModule module;

        public MainWindow()
        {
            InitializeComponent();

            this.module = new XShellModule(this);
        }
    }
}
