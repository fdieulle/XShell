using DryIoc;
using XShell.Demo.Winform.Screens.Popup;
using XShell.Demo.Winform.Screens.Screen;
using XShell.Demo.Winform.Screens.ScreenWithoutInterface;
using XShell.Demo.Winform.Screens.SimpleScreen;
using XShell.Demo.Winform.Services.Service;
using XShell.Winform;

namespace XShell.Demo.Winform
{
    public class MyXShellModule : XShellModule
    {
        public MyXShellModule() : base("XShell for Windows Forms Demo") { }

        #region Overrides of ShellModule

        protected override void SetupServices(IContainer container)
        {
            container.RegisterInstance<IService>(new ServiceImpl());
        }

        protected override void SetupScreens(IScreenContainer container)
        {
            container.Register<SimpleScreenView>();
            container.Register<ScreenWithoutInterfaceView, ScreenWithoutInterfaceLogic>();
            container.Register<IMyScreen, MyScreenView, MyScreenLogic>();
            container.Register<IMyPopup, MyPopupView, MyPopupLogic>();
        }

        protected override void Initialize(IContainer container)
        {
            
        }

        #endregion
    }
}
