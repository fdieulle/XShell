using System.Windows.Forms;

namespace XShell.Demo.Winform.Screens.ScreenWithoutInterface
{
    public partial class ScreenWithoutInterfaceView : UserControl
    {
        private readonly ScreenWithoutInterfaceController controller;

        public ScreenWithoutInterfaceView(ScreenWithoutInterfaceController controller)
        {
            this.controller = controller;
            InitializeComponent();
        }
    }
}
