using System.ComponentModel;

namespace XShell
{
    public interface IScreen : INotifyPropertyChanged
    {
        string Title { get; }
    }
}
