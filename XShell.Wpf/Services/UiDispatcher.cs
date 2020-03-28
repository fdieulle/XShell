using System;
using System.Windows;
using System.Windows.Threading;

namespace XShell.Wpf.Services
{
    public class UiDispatcher : IUiDispatcher
    {
        private static readonly Dispatcher dispatcher = Application.Current != null 
            ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;

        public void Dispatch(Action action) => dispatcher.BeginInvoke(DispatcherPriority.Background, action);
    }
}
