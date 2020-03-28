using System;

namespace XShell
{
    /// <summary>
    /// This service allows you to execute big tasks in asynchronously. 
    /// Each steps of your task is reported in the main ui thread to allows you to display 
    /// the state of your asynchronous task.
    /// </summary>
    public interface IBackgroundTaskManager
    {
        /// <summary>
        /// Raises in the main ui thread when a new task is started
        /// </summary>
        event Action<IBackgroundTask, object> TaskStarted;

        /// <summary>
        /// Raises in the main ui thread when a task state is reported
        /// </summary>
        event Action<double, object> ReportStateChanged;

        /// <summary>
        /// Raises in the main ui thread when a task is completed.
        /// </summary>
        event Action<IBackgroundTask, object> TaskCompleted;

        /// <summary>
        /// Run asynchronously a task in another thread. You can report at any time the state of your background task 
        /// through the <see cref="IBackgroundTask"/> interface given in onWork function parameter. 
        /// The report callback is send in the main ui thread.
        /// </summary>
        /// <typeparam name="TResult">Background work result type.</typeparam>
        /// <param name="onWork">Function which is executed asynchronously. The first parameter allows you to report the state of the background work. The 2nd parameter is the state given on the Dispatch method.</param>
        /// <param name="onCompleted">Callback called in the main ui thread when the work is finished. We provide the result and the state as parameters.</param>
        /// <param name="state">User instance state forward on the onWork and onComplete callbacks.</param>
        void Dispatch<TResult>(Func<IBackgroundTask, object, TResult> onWork, Action<TResult, object> onCompleted = null, object state = null);

        /// <summary>
        /// Run asynchronously a task in another thread with an indeterminate time of work. 
        /// </summary>
        /// <typeparam name="TResult">Background work result type.</typeparam>
        /// <param name="onWork">Function which is executed asynchronously. The 1st parameter is the state given on the Dispatch method.</param>
        /// <param name="onCompleted">Callback called in the main ui thread when the work is finished. We provide the result and the state as parameters.</param>
        /// <param name="state">User instance state forward on the onWork and onComplete callbacks.</param>
        void Dispatch<TResult>(Func<object, TResult> onWork, Action<TResult, object> onCompleted = null, object state = null);
    }
}
