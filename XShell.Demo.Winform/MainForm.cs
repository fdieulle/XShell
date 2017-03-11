using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace XShell.Demo.Winform
{
    public partial class MainForm : Form
    {
        public DockPanel MainDockPanel { get { return mainDockPanel; } }

        public MainForm()
        {
            InitializeComponent();
        }
    }
}
