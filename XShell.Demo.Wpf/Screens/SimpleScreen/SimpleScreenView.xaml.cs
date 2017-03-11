using System;
using System.Windows.Controls;
using XShell.Demo.Wpf.Services.Service;

namespace XShell.Demo.Wpf.Screens.SimpleScreen
{
    /// <summary>
    /// Interaction logic for SimpleScreenView.xaml
    /// </summary>
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
