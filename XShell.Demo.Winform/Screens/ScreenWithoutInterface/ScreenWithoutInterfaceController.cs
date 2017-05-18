using XShell.Core;
using XShell.Demo.Winform.Services.Service;

namespace XShell.Demo.Winform.Screens.ScreenWithoutInterface
{
    [ScreenMenuItem("Screens/Screen without Interface")]
    public class ScreenWithoutInterfaceController : AbstractLogic
    {
        private readonly IService service;

        public ScreenWithoutInterfaceController(IService service)
        {
            this.service = service;

            Title = "Screen without Interface";
        }
    }
}
