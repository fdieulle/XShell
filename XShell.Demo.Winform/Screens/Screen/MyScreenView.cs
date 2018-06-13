using System.Windows.Forms;
using XShell.Winform.Binders;

namespace XShell.Demo.Winform.Screens.Screen
{
    public partial class MyScreenView : UserControl
    {
        public MyScreenView(IMyScreen logic)
        {
            InitializeComponent();

            browseFilePathButton.Bind(logic.BrowseCommand, bindName: true, toolTip: "Browse file path ...");
            filePathTextBox.Bind(logic, nameof(logic.FilePath));
        }
    }
}
