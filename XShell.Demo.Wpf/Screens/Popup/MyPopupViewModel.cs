using XShell.Demo.Wpf.Services.Service;
using XShell.Mvvm;

namespace XShell.Demo.Wpf.Screens.Popup
{
    public class MyPopupViewModel: AbstractViewModel, IMyPopup
    {
        private readonly IService service;

        public MyPopupViewModel(IService service)
        {
            this.service = service;
            Title = "My Popup";
        }
    }
}
