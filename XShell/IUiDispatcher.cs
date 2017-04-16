using System;

namespace XShell
{
    /// <summary>
    /// Main ui thread dispatcher. This interface has to be implemented by each ui framework you want to use.
    /// </summary>
    public interface IUiDispatcher
    {
        /// <summary>
        /// Execution the action in the main ui thread.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        void Dispatch(Action action);
    }
}
