using System;
using System.Windows;
using XShell.Wpf.Controls;

namespace XShell.Wpf.Services.Shell
{
    public class StatusBarManager : IDisposable
    {
        private readonly BackgroundTaskView backgroundTaskView;
        private readonly IBackgroundTaskManager backgroundTaskManager;

        public StatusBarManager(
            BackgroundTaskView backgroundTaskView,
            IBackgroundTaskManager backgroundTaskManager)
        {
            this.backgroundTaskView = backgroundTaskView;
            this.backgroundTaskManager = backgroundTaskManager;

            this.backgroundTaskManager.TaskStarted += OnBackgroundTaskStarted;
            this.backgroundTaskManager.ReportStateChanged += OnBackgroundReportStateChanged;
            this.backgroundTaskManager.TaskCompleted += OnBackgroundTaskCompleted;
        }

        private void OnBackgroundTaskStarted(IBackgroundTask task, object state)
        {
            backgroundTaskView.Visibility = Visibility.Visible;
            backgroundTaskView.IsIndeterminate = task.IsIndeterminate;
            if (state != null)
                backgroundTaskView.State = state.ToString();
        }

        private void OnBackgroundReportStateChanged(double percent, object state)
        {
            backgroundTaskView.Percent = percent;
            if (state != null)
                backgroundTaskView.State = state.ToString();
        }

        private void OnBackgroundTaskCompleted(IBackgroundTask arg1, object arg2)
        {
            backgroundTaskView.Visibility = Visibility.Collapsed;
            backgroundTaskView.IsIndeterminate = false;
            backgroundTaskView.State = null;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            this.backgroundTaskManager.TaskStarted -= OnBackgroundTaskStarted;
            this.backgroundTaskManager.ReportStateChanged -= OnBackgroundReportStateChanged;
            this.backgroundTaskManager.TaskCompleted -= OnBackgroundTaskCompleted;
        }

        #endregion
    }
}
