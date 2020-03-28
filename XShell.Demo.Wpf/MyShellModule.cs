using DryIoc;
using XShell.Demo.Wpf.Screens.Popup;
using XShell.Demo.Wpf.Screens.Screen;
using XShell.Demo.Wpf.Screens.ScreenWithoutInterface;
using XShell.Demo.Wpf.Screens.SimpleScreen;
using XShell.Demo.Wpf.Services.Service;
using XShell.Wpf;

namespace XShell.Demo.Wpf
{
    public class MyXShellModule : XShellModule
    {
        public MyXShellModule() : base("XShell Demo") { }

        #region Overrides of ShellModule

        protected override void SetupServices(IContainer container)
        {
            container.RegisterInstance<IService>(new ServiceImpl());
        }

        protected override void SetupScreens(IScreenContainer container)
        {
            container.Register<SimpleScreenView>();
            container.Register<ScreenWithoutInterfaceView, ScreenWithoutInterfaceViewModel>();
            container.Register<IMyScreen, MyScreenView, MyScreenViewModel>();
            container.Register<IMyPopup, MyPopupView, MyPopupViewModel>();
        }

        protected override void Initialize(IContainer container)
        {
        }

        #endregion
    }
}
