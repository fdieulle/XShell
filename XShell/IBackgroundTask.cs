namespace XShell
{
    public interface IBackgroundTask
    {
        /// <summary>
        /// Defines if the task can report its state.
        /// </summary>
        bool IsIndeterminated { get; }

        /// <summary>
        /// Reports current task state.
        /// </summary>
        /// <param name="percent">Percentage of task completion.</param>
        /// <param name="state">User state</param>
        void ReportState(double percent, object state);
    }
}
