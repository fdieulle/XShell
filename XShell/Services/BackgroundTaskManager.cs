using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XShell.Services
{
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private readonly IUiDispatcher uiDispatcher;
        private readonly Queue<Action> tasks = new Queue<Action>();
        private bool taskIsRunning;

        public event Action<IBackgroundTask, object> TaskStarted;

        public event Action<double, object> ReportStateChanged;

        public event Action<IBackgroundTask, object> TaskCompleted;

        public BackgroundTaskManager(IUiDispatcher uiDispatcher)
        {
            this.uiDispatcher = uiDispatcher;
        }

        public void Dispatch<TResult>(Func<IBackgroundTask, object, TResult> onWork, Action<TResult, object> onCompleted = null, object state = null)
        {
            var task = new BackgroundTask<TResult>(uiDispatcher, this, onWork, onCompleted, state, false);
            Run(task.Run);
        }

        public void Dispatch<TResult>(Func<object, TResult> onWork, Action<TResult, object> onCompleted = null, object state = null)
        {
            var task = new BackgroundTask<TResult>(uiDispatcher, this, (t,s) => onWork(s), onCompleted, state, true);
            Run(task.Run);
        }

        private void Run(Action run)
        {
            lock (tasks)
            {
                if (taskIsRunning)
                    tasks.Enqueue(run);
                else
                {
                    taskIsRunning = true;
                    run();
                }
            }
        }

        private void RaiseTaskStarted(IBackgroundTask task, object state)
        {
            var handler = TaskStarted;
            if (handler != null) handler(task, state);
        }

        private void RaiseReportStateChanged(double percent, object state)
        {
            var handler = ReportStateChanged;
            if (handler != null) handler(percent, state);
        }

        private void RaiseTaskCompleted(IBackgroundTask task, object state)
        {
            var handler = TaskCompleted;
            if (handler != null) handler(task, state);

            lock (tasks)
            {
                if(tasks.Count > 0) uiDispatcher.Dispatch(() => tasks.Dequeue());
                else taskIsRunning = false;
            }
        }

        private class BackgroundTask<TResult> : IBackgroundTask
        {
            private readonly IUiDispatcher uiDispatcher;
            private readonly BackgroundTaskManager manager;
            private readonly Func<IBackgroundTask, object, TResult> onWork;
            private readonly Action<TResult, object> onCompleted;
            private readonly object state;
            private readonly bool isIndeterminated;

            public BackgroundTask(
                IUiDispatcher uiDispatcher,
                BackgroundTaskManager manager,
                Func<IBackgroundTask, object, TResult> onWork,
                Action<TResult, object> onCompleted,
                object state,
                bool isIndeterminated)
            {
                this.uiDispatcher = uiDispatcher;
                this.manager = manager;
                this.onWork = onWork;
                this.onCompleted = onCompleted;
                this.state = state;
                this.isIndeterminated = isIndeterminated;
            }

            public void Run()
            {
                uiDispatcher.Dispatch(() =>
                {
                    manager.RaiseTaskStarted(this, state);
                    Task.Factory.StartNew(s =>
                    {
                        var result = onWork(this, s);
                        uiDispatcher.Dispatch(() =>
                        {
                            if (onCompleted != null)
                                onCompleted(result, s);

                            manager.RaiseTaskCompleted(this, state);
                        });
                    }, state); 
                });
            }

            public bool IsIndeterminated { get { return isIndeterminated; } }

            public void ReportState(double percent, object s)
            {
                uiDispatcher.Dispatch(() => manager.RaiseReportStateChanged(percent, s));
            }
        }
    }
}
