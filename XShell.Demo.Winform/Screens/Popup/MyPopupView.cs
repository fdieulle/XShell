using System.Windows.Forms;

namespace XShell.Demo.Winform.Screens.Popup
{
    public partial class MyPopupView : UserControl
    {
        private readonly IMyPopup controller;

        public MyPopupView(IMyPopup controller)
        {
            this.controller = controller;
            InitializeComponent();
        }
    }
}
