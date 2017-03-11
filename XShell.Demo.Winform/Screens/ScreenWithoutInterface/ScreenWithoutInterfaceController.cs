using XShell.Demo.Winform.Services.Service;
using XShell.Mvc;

namespace XShell.Demo.Winform.Screens.ScreenWithoutInterface
{
    [ScreenMenuItem("Screens/Screen without Interface")]
    public class ScreenWithoutInterfaceController : AbstractScreenController
    {
        private readonly IService service;

        public ScreenWithoutInterfaceController(IService service)
        {
            this.service = service;

            Title = "Screen without Interface";
        }
    }
}
