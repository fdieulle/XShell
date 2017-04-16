using System;
using System.Windows;
using System.Windows.Threading;

namespace XShell.Demo.Wpf.Services.Shell
{
    public class UiDispatcher : IUiDispatcher
    {
        private static readonly Dispatcher dispatcher = Application.Current != null 
            ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher;

        #region Implementation of IUiDispatcher

        public void Dispatch(Action action)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Background, action);
        }

        #endregion
    }
}
