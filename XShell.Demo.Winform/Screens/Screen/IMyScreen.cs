using XShell.Core;

namespace XShell.Demo.Winform.Screens.Screen
{
    [ScreenMenuItem("Screens/My Screen")]
    public interface IMyScreen : IScreen
    {
        string FilePath { get; set; }
        IRelayCommand BrowseCommand { get; }
    }
}
