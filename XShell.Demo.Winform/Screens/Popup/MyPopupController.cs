using XShell.Core;
using XShell.Demo.Winform.Services.Service;

namespace XShell.Demo.Winform.Screens.Popup
{
    public class MyPopupController : AbstractLogic, IMyPopup
    {
        private readonly IService _service;

        public MyPopupController(IService service)
        {
            _service = service;
            Title = "My Popup";
        }
    }
}
