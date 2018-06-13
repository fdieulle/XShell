using XShell.Core;
using XShell.Demo.Winform.Services.Service;

namespace XShell.Demo.Winform.Screens.Popup
{
    public class MyPopupLogic : AbstractLogic, IMyPopup
    {
        private readonly IService _service;

        public MyPopupLogic(IService service)
        {
            _service = service;
            Title = "My Popup";
        }
    }
}
