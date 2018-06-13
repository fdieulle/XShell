using System.Windows.Forms;

namespace XShell.Demo.Winform.Screens.ScreenWithoutInterface
{
    public partial class ScreenWithoutInterfaceView : UserControl
    {
        private readonly ScreenWithoutInterfaceLogic _logic;

        public ScreenWithoutInterfaceView(ScreenWithoutInterfaceLogic logic)
        {
            _logic = logic;
            InitializeComponent();
        }
    }
}
