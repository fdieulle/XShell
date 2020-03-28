using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace XShell.Winform.Controls
{
    public partial class MainForm : Form
    {
        public DockPanel MainDockPanel => mainDockPanel;

        public StatusStrip MainStatusBar => mainStatusBar;

        public ToolStripProgressBar StatusProgressBar => progressBar;
        public ToolStripStatusLabel StatusProgressLabel => progressLabel;

        public MainForm()
        {
            InitializeComponent();
        }
    }
}
