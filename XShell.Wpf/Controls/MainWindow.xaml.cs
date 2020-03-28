using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Xceed.Wpf.AvalonDock.Layout;

namespace XShell.Wpf.Controls
{
    public partial class MainWindow : Window
    {
        public Menu Menu => MainMenu;

        public LayoutDocumentPane Docker => MainPane;

        public StatusBar StatusBar => MainStatusBar;

        public BackgroundTaskView BackgroundWorker => BackgroundWorkerView;

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
