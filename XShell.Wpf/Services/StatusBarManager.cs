using System;
using System.Windows;
using XShell.Wpf.Controls;

namespace XShell.Wpf.Services.Shell
{
    public class StatusBarManager : IDisposable
    {
        private readonly BackgroundTaskView _backgroundTaskView;
        private readonly IBackgroundTaskManager _backgroundTaskManager;

        public StatusBarManager(
            BackgroundTaskView backgroundTaskView,
            IBackgroundTaskManager backgroundTaskManager)
        {
            _backgroundTaskView = backgroundTaskView;
            _backgroundTaskManager = backgroundTaskManager;

            _backgroundTaskManager.TaskStarted += OnBackgroundTaskStarted;
            _backgroundTaskManager.ReportStateChanged += OnBackgroundReportStateChanged;
            _backgroundTaskManager.TaskCompleted += OnBackgroundTaskCompleted;
        }

        private void OnBackgroundTaskStarted(IBackgroundTask task, object state)
        {
            _backgroundTaskView.Visibility = Visibility.Visible;
            _backgroundTaskView.IsIndeterminate = task.IsIndeterminate;
            if (state != null)
                _backgroundTaskView.State = state.ToString();
        }

        private void OnBackgroundReportStateChanged(double percent, object state)
        {
            _backgroundTaskView.Percent = percent;
            if (state != null)
                _backgroundTaskView.State = state.ToString();
        }

        private void OnBackgroundTaskCompleted(IBackgroundTask arg1, object arg2)
        {
            _backgroundTaskView.Visibility = Visibility.Collapsed;
            _backgroundTaskView.IsIndeterminate = false;
            _backgroundTaskView.State = null;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _backgroundTaskManager.TaskStarted -= OnBackgroundTaskStarted;
            _backgroundTaskManager.ReportStateChanged -= OnBackgroundReportStateChanged;
            _backgroundTaskManager.TaskCompleted -= OnBackgroundTaskCompleted;
        }

        #endregion
    }
}
