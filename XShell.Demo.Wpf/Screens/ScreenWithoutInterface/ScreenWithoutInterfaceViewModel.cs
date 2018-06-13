using XShell.Core;
using XShell.Demo.Wpf.Services.Service;

namespace XShell.Demo.Wpf.Screens.ScreenWithoutInterface
{
    [ScreenMenuItem("Screens/Screen without Interface")]
    public class ScreenWithoutInterfaceViewModel : AbstractLogic
    {
        private readonly IService _service;

        public ScreenWithoutInterfaceViewModel(IService service)
        {
            _service = service;
            Title = "Screen without Interface";
        }
    }
}
