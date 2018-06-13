using XShell.Core;
using XShell.Demo.Winform.Services.Service;

namespace XShell.Demo.Winform.Screens.Screen
{
    public class MyScreenLogic : AbstractLogic, IMyScreen
    {
        private readonly IService _service;
        private readonly IViewBox _viewBox;

        public string FilePath { get; set; }

        public IRelayCommand BrowseCommand { get; }

        public MyScreenLogic(IService service, IViewBox viewBox)
        {
            _service = service;
            _viewBox = viewBox;
            Title = "My screen";

            BrowseCommand = new RelayCommand(ExecuteBrowseCommand) { Name = "..." };
        }

        private void ExecuteBrowseCommand(object p)
        {
            var filePath = _viewBox.AskFile("Text files (*.txt)|*.txt|All files (*.*)|*.*", FilePath);
            if (filePath != null) // If the user doesn't cancel dialog
            {
                FilePath = filePath;
                RaisePropertyChanged(nameof(FilePath));
            }
        }
    }
}