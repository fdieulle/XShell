using System;
using System.Windows.Forms;
using XShell.Demo.Winform.Services.Service;

namespace XShell.Demo.Winform.Screens.SimpleScreen
{
    [ScreenMenuItem("Screens/Simple Screen")]
    public partial class SimpleScreenView : UserControl, IScreen
    {
        private readonly IService service;

        public SimpleScreenView(IService service)
        {
            this.service = service;
            InitializeComponent();
        }

        #region Implementation of IScreen

        public event Action TitleChanged;

        public string Title { get { return "Simple Screen"; } }

        #endregion
    }
}
