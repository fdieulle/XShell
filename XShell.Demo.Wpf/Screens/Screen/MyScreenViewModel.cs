using XShell.Demo.Wpf.Services.Service;
using XShell.Mvvm;

namespace XShell.Demo.Wpf.Screens.Screen
{
    public class MyScreenViewModel : AbstractViewModel, IMyScreen
    {
        private readonly IService service;

        public MyScreenViewModel(IService service)
        {
            this.service = service;
            Title = "My screen";
        }
    }
}
