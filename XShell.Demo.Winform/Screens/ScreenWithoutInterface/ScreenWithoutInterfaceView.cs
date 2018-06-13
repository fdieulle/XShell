using System.Windows.Forms;

namespace XShell.Demo.Winform.Screens.ScreenWithoutInterface
{
    public partial class ScreenWithoutInterfaceView : UserControl
    {
        private readonly ScreenWithoutInterfaceController _controller;

        public ScreenWithoutInterfaceView(ScreenWithoutInterfaceController controller)
        {
            _controller = controller;
            InitializeComponent();
        }
    }
}
