using XShell.Core;
using XShell.Demo.Winform.Services.Service;

namespace XShell.Demo.Winform.Screens.Screen
{
    public class MyScreenController : AbstractLogic, IMyScreen
    {
        private readonly IService service;

        public MyScreenController(IService service)
        {
            this.service = service;
            Title = "My screen";
        }
    }
}
