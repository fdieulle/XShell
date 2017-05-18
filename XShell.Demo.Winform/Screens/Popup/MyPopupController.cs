using XShell.Core;
using XShell.Demo.Winform.Services.Service;

namespace XShell.Demo.Winform.Screens.Popup
{
    public class MyPopupController : AbstractLogic, IMyPopup
    {
        private readonly IService service;

        public MyPopupController(IService service)
        {
            this.service = service;
            Title = "My Popup";
        }
    }
}
