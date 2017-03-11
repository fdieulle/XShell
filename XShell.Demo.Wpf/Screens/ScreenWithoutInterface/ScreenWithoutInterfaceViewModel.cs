using XShell.Demo.Wpf.Services.Service;
using XShell.Mvvm;

namespace XShell.Demo.Wpf.Screens.ScreenWithoutInterface
{
    [ScreenMenuItem("Screens/Screen without Interface")]
    public class ScreenWithoutInterfaceViewModel : AbstractScreenViewModel
    {
        private readonly IService service;

        public ScreenWithoutInterfaceViewModel(IService service)
        {
            this.service = service;
            Title = "Screen without Interface";
        }
    }
}
