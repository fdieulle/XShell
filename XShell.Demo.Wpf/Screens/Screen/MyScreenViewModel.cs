using XShell.Core;
using XShell.Demo.Wpf.Services.Service;

namespace XShell.Demo.Wpf.Screens.Screen
{
    public class MyScreenViewModel : AbstractLogic, IMyScreen
    {
        private readonly IService _service;

        public MyScreenViewModel(IService service)
        {
            _service = service;
            Title = "My screen";
        }
    }
}
