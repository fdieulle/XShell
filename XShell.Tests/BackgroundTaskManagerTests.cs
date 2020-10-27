using System;
using System.Collections.Concurrent;
using System.Threading;
using XShell.Services;
using Xunit;

namespace XShell.Tests
{
    public class BackgroundTaskManagerTests
    {
        [Fact]
        public void DispatchIndeterminateTest()
        {
            var uiDispatcher = new MockUiDispatcher();
            var manager = new BackgroundTaskManager(uiDispatcher);

            var countdown = new CountdownEvent(4);
            
            var state = "State";

            manager.TaskStarted += (t, s) =>
            {
                Assert.True(t.IsIndeterminate);
                Assert.Equal(s, state);
                Assert.Equal(uiDispatcher.ThreadId, Thread.CurrentThread.ManagedThreadId);
                Assert.Equal(4, countdown.CurrentCount);
                countdown.Signal();
            };

            manager.TaskCompleted += (t, s) =>
            {
                Assert.True(t.IsIndeterminate);
                Assert.Equal(s, state);
                Assert.Equal(uiDispatcher.ThreadId, Thread.CurrentThread.ManagedThreadId);
                Assert.Equal(1, countdown.CurrentCount);
                countdown.Signal();
            };

            manager.Dispatch(s =>
            {
                Assert.Equal(state, s);
                Assert.NotEqual(uiDispatcher.ThreadId, Thread.CurrentThread.ManagedThreadId);
                Assert.Equal(3, countdown.CurrentCount);
                countdown.Signal();
                return "Result";
            }, (r, s) =>
            {
                Assert.Equal("Result", r);
                Assert.Equal(uiDispatcher.ThreadId, Thread.CurrentThread.ManagedThreadId);
                Assert.Equal(2, countdown.CurrentCount);
                countdown.Signal();
            }, state);

            Assert.True(countdown.Wait(5000));
        }

        [Fact]
        public void DispatchAndReportTest()
        {
            var uiDispatcher = new MockUiDispatcher();
            var manager = new BackgroundTaskManager(uiDispatcher);

            const int steps = 10;

            var countdown = new CountdownEvent(4 + steps);

            var state = "State";

            manager.TaskStarted += (t, s) =>
            {
                Assert.False(t.IsIndeterminate);
                Assert.Equal(s, state);
                Assert.Equal(uiDispatcher.ThreadId, Thread.CurrentThread.ManagedThreadId);
                Assert.Equal(4 + steps, countdown.CurrentCount);
                countdown.Signal();
            };

            manager.ReportStateChanged += (p, s) =>
            {
                Assert.Equal(p + " %", s);
                Assert.Equal(uiDispatcher.ThreadId, Thread.CurrentThread.ManagedThreadId);
                Assert.Equal(3 + steps - p / steps, countdown.CurrentCount);
                countdown.Signal();
            };

            manager.TaskCompleted += (t, s) =>
            {
                Assert.False(t.IsIndeterminate);
                Assert.Equal(s, state);
                Assert.Equal(uiDispatcher.ThreadId, Thread.CurrentThread.ManagedThreadId);
                Assert.Equal(1, countdown.CurrentCount);
                countdown.Signal();
            };

            manager.Dispatch((t, s) =>
            {
                Assert.Equal(state, s);
                Assert.NotEqual(uiDispatcher.ThreadId, Thread.CurrentThread.ManagedThreadId);

                for (var i = 0; i < steps; i++)
                {
                    t.ReportState(i * 10, (i*10)+" %");
                    Thread.Sleep(10);
                }

                Assert.Equal(3, countdown.CurrentCount);
                countdown.Signal();
                return "Result";
            }, (r, s) =>
            {
                Assert.Equal("Result", r);
                Assert.Equal(uiDispatcher.ThreadId, Thread.CurrentThread.ManagedThreadId);
                Assert.Equal(2, countdown.CurrentCount);
                countdown.Signal();
            }, state);

            Assert.True(countdown.Wait(5000));
        }

        public class MockUiDispatcher : IUiDispatcher, IDisposable
        {
            private readonly ManualResetEvent _waiter = new ManualResetEvent(false);
            private readonly Thread _thread;
            private readonly ConcurrentQueue<Action> _tasks = new ConcurrentQueue<Action>();
            private bool _isRunning = true;

            public int ThreadId => _thread.ManagedThreadId;

            public MockUiDispatcher()
            {
                _thread = new Thread(Work){ IsBackground = true };
                _thread.Start();
            }

            private void Work()
            {
                while (_isRunning)
                {
                    _waiter.WaitOne();

                    while (_tasks.Count > 0)
                    {
                        if (_tasks.TryDequeue(out var action))
                            action();
                    }

                    _waiter.Reset();
                }
            }

            #region Implementation of IUiDispatcher

            public void Dispatch(Action action)
            {
                if (action == null) return;

                _tasks.Enqueue(action);
                _waiter.Set();
            }

            #endregion

            #region Implementation of IDisposable

            public void Dispose()
            {
                _isRunning = false;
            }

            #endregion
        }
    }
}
