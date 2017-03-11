using XShell.Demo.Winform.Services.Service;
using XShell.Mvc;

namespace XShell.Demo.Winform.Screens.Popup
{
    public class MyPopupController : AbstractScreenController, IMyPopup
    {
        private readonly IService service;

        public MyPopupController(IService service)
        {
            this.service = service;
            Title = "My Popup";
        }
    }
}
