using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XShell.Services
{
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        private readonly IUiDispatcher _uiDispatcher;
        private readonly Queue<Action> _tasks = new Queue<Action>();
        private bool _taskIsRunning;

        public event Action<IBackgroundTask, object> TaskStarted;

        public event Action<double, object> ReportStateChanged;

        public event Action<IBackgroundTask, object> TaskCompleted;

        public BackgroundTaskManager(IUiDispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
        }

        public void Dispatch<TResult>(Func<IBackgroundTask, object, TResult> onWork, Action<TResult, object> onCompleted = null, object state = null)
        {
            var task = new BackgroundTask<TResult>(_uiDispatcher, this, onWork, onCompleted, state, false);
            Run(task.Run);
        }

        public void Dispatch<TResult>(Func<object, TResult> onWork, Action<TResult, object> onCompleted = null, object state = null)
        {
            var task = new BackgroundTask<TResult>(_uiDispatcher, this, (t,s) => onWork(s), onCompleted, state, true);
            Run(task.Run);
        }

        private void Run(Action run)
        {
            lock (_tasks)
            {
                if (_taskIsRunning)
                    _tasks.Enqueue(run);
                else
                {
                    _taskIsRunning = true;
                    run();
                }
            }
        }

        private void RaiseTaskStarted(IBackgroundTask task, object state) 
            => TaskStarted?.Invoke(task, state);

        private void RaiseReportStateChanged(double percent, object state) 
            => ReportStateChanged?.Invoke(percent, state);

        private void RaiseTaskCompleted(IBackgroundTask task, object state)
        {
            TaskCompleted?.Invoke(task, state);

            lock (_tasks)
            {
                if(_tasks.Count > 0) _uiDispatcher.Dispatch(() => _tasks.Dequeue());
                else _taskIsRunning = false;
            }
        }

        private class BackgroundTask<TResult> : IBackgroundTask
        {
            private readonly IUiDispatcher _uiDispatcher;
            private readonly BackgroundTaskManager _manager;
            private readonly Func<IBackgroundTask, object, TResult> _onWork;
            private readonly Action<TResult, object> _onCompleted;
            private readonly object _state;
            private readonly bool _isIndeterminate;

            public BackgroundTask(
                IUiDispatcher uiDispatcher,
                BackgroundTaskManager manager,
                Func<IBackgroundTask, object, TResult> onWork,
                Action<TResult, object> onCompleted,
                object state,
                bool isIndeterminate)
            {
                _uiDispatcher = uiDispatcher;
                _manager = manager;
                _onWork = onWork;
                _onCompleted = onCompleted;
                _state = state;
                _isIndeterminate = isIndeterminate;
            }

            public void Run()
            {
                _uiDispatcher.Dispatch(() =>
                {
                    _manager.RaiseTaskStarted(this, _state);
                    Task.Factory.StartNew(s =>
                    {
                        var result = _onWork(this, s);
                        _uiDispatcher.Dispatch(() =>
                        {
                            _onCompleted?.Invoke(result, s);

                            _manager.RaiseTaskCompleted(this, _state);
                        });
                    }, _state); 
                });
            }

            public bool IsIndeterminate => _isIndeterminate;

            public void ReportState(double percent, object s)
            {
                _uiDispatcher.Dispatch(() => _manager.RaiseReportStateChanged(percent, s));
            }
        }
    }
}
