using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using XShell.Services;

namespace XShell.Wpf.Controls
{
    public partial class MainWindow : IMainWindow<XDockContent>
    {
        public Menu Menu => MainMenu;

        public LayoutDocumentPane Docker => MainPane;

        public BackgroundTaskView BackgroundWorker => BackgroundWorkerView;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Overrides of Window

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Terminating?.Invoke();
        }

        #endregion

        #region Implementation of IMainWindow

        public event Action Terminating;

        public RectangleSettings GetPositionAndSize()
        {
            return new RectangleSettings
            {
                Top = Top,
                Left = Left,
                Width = ActualWidth,
                Height = ActualHeight
            };
        }

        public void SetPositionAndSize(RectangleSettings rectangle)
        {
            Top = rectangle.Top;
            Left = rectangle.Left;
            Width = rectangle.Width;
            Height = rectangle.Height;
        }

        public void SaveWorkspace(Stream stream, Encoding encoding, bool leaveOpen)
        {
            var serializer = new XmlLayoutSerializer(DockingManager);
            serializer.Serialize(stream);
        }

        public void LoadWorkspace(Stream stream, Func<string, XDockContent> createContent, bool leaveOpen)
        {
            var serializer = new XmlLayoutSerializer(DockingManager);
            serializer.LayoutSerializationCallback += (s, e) => { };
            serializer.Deserialize(stream);
        }

        #endregion
    }
}
