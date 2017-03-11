using System.Windows.Forms;

namespace XShell.Demo.Winform.Screens.Screen
{
    public partial class MyScreenView : UserControl
    {
        private readonly IMyScreen controller;

        public MyScreenView(IMyScreen controller)
        {
            this.controller = controller;
            InitializeComponent();
        }
    }
}
