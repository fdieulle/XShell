using XShell.Core;
using XShell.Demo.Wpf.Services.Service;

namespace XShell.Demo.Wpf.Screens.Screen
{
    public class MyScreenViewModel : AbstractLogic, IMyScreen
    {
        private readonly IService service;

        public MyScreenViewModel(IService service)
        {
            this.service = service;
            Title = "My screen";
        }
    }
}
