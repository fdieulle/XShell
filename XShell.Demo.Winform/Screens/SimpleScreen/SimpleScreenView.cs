using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using XShell.Demo.Winform.Annotations;
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

        public string Title { get { return "Simple Screen"; } }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
