using XShell.Demo.Winform.Services.Service;
using XShell.Mvc;

namespace XShell.Demo.Winform.Screens.Screen
{
    public class MyScreenController : AbstractScreenController, IMyScreen
    {
        private readonly IService service;

        public MyScreenController(IService service)
        {
            this.service = service;
            Title = "My screen";
        }
    }
}
