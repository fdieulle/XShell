using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace XShell.Wpf.Controls
{
    public partial class MainWindow
    {
        public Menu Menu => MainMenu;

        public LayoutDocumentPane Docker => MainPane;

        public BackgroundTaskView BackgroundWorker => BackgroundWorkerView;

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
