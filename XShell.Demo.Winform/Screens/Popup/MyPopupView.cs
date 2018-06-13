using System.Windows.Forms;

namespace XShell.Demo.Winform.Screens.Popup
{
    public partial class MyPopupView : UserControl
    {
        private readonly IMyPopup _controller;

        public MyPopupView(IMyPopup controller)
        {
            _controller = controller;
            InitializeComponent();
        }
    }
}
