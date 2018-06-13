using System.Windows.Forms;
using XShell.Winform.Binders;

namespace XShell.Demo.Winform.Screens.Screen
{
    public partial class MyScreenView : UserControl
    {
        public MyScreenView(IMyScreen logic)
        {
            InitializeComponent();

            this.browseFilePathButton.Bind(logic.BrowseCommand, bindName: true, toolTip: "Browse file path ...");
            this.filePathTextBox.Bind(logic, nameof(logic.FilePath));
        }
    }
}
