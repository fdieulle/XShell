using XShell.Core;
using XShell.Demo.Wpf.Services.Service;

namespace XShell.Demo.Wpf.Screens.Popup
{
    public class MyPopupViewModel : AbstractLogic, IMyPopup
    {
        private readonly IService service;

        public MyPopupViewModel(IService service)
        {
            this.service = service;
            Title = "My Popup";
        }
    }
}
