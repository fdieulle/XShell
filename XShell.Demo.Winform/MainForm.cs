using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace XShell.Demo.Winform
{
    public partial class MainForm : Form
    {
        public DockPanel MainDockPanel { get { return mainDockPanel; } }

        public StatusStrip MainStatusBar { get { return mainStatusBar; } }

        public ToolStripProgressBar StatusProgressBar { get { return progressBar; } }
        public ToolStripStatusLabel StatusProgressLabel { get { return progressLabel; } }

        public MainForm()
        {
            InitializeComponent();
        }
    }
}
