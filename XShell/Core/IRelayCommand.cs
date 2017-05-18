using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace XShell.Core
{
    public interface IRelayCommand : ICommand, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the command name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets if the commond is working.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Force the command invalidation by calling canExecute predicate.
        /// </summary>
        void InvalidateCanExecute();
    }

    public interface IRelayCommand<T> : IRelayCommand
    {
        /// <summary>
        /// Condition to allow the command execution.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>If the condition is filled.</returns>
        bool CanExecute(T parameter);

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        void Execute(T parameter);

        /// <summary>
        /// Add a condition on CanExecute predicate.
        /// </summary>
        /// <param name="predicate">The new condition.</param>
        void AddCanExecute(Func<T, bool> predicate);

        /// <summary>
        /// Remove a condition from CanExecute predicate.
        /// </summary>
        /// <param name="predicate">the condition to remove.</param>
        void RemoveCanExecute(Func<T, bool> predicate);
    }

    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. 
    /// The default return value for the CanExecute method is 'true'.
    /// </summary>
    public class RelayCommand<T> : AbstractNpc, IRelayCommand<T>
    {
        private static readonly Action<T> defaultExecute = p => { };
        private static readonly Func<T, bool> defaultCanExecute = p => true;

        private readonly Action<T> execute;
        private readonly Func<T, bool> canExecute;
        private readonly Func<object, T> adaptParameter;
        private List<Func<T, bool>> dynamicPredicates;

        #region Properties

        private static readonly PropertyChangedEventArgs namePropertyChanged = new PropertyChangedEventArgs("Name");
        private string name;
        /// <summary>
        /// Name of the command.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;

                name = value;
                RaisePropertyChanged(namePropertyChanged);
            }
        }

        private static readonly PropertyChangedEventArgs isRunningPropertyChanged = new PropertyChangedEventArgs("IsRunning");
        private bool isRunning;
        /// <summary>
        /// Indicate if the command is in progress.
        /// </summary>
        public bool IsRunning
        {
            get { return isRunning; }
            private set
            {
                if (isRunning == value) return;

                isRunning = value;
                RaisePropertyChanged(isRunningPropertyChanged);
                InvalidateCanExecute();
            }
        }

        #endregion // Properties

        #region Ctors

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<T> execute)
            : this(execute, null) { }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute ?? defaultExecute;
            this.canExecute = canExecute ?? defaultCanExecute;
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <param name="adaptParameter">Parameter adaptation.</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute, Func<object, T> adaptParameter)
        {
            this.execute = execute ?? defaultExecute;
            this.canExecute = canExecute ?? defaultCanExecute;
            this.adaptParameter = adaptParameter;
        }

        #endregion // Ctors

        #region Implementation of IRelayCommand

        public bool CanExecute(T parameter)
        {
            try
            {
                if (isRunning)
                    return false;

                var result = canExecute(parameter);

                if (dynamicPredicates != null)
                    dynamicPredicates.ForEach(p => result &= p(parameter));

                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Execute(T parameter)
        {
            IsRunning = true;

            // Raise execute action
            execute(parameter);

            IsRunning = false;
        }

        /// <summary>
        /// Force the command invalidation by calling canExecute predicate.
        /// </summary>
        public void InvalidateCanExecute()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public void AddCanExecute(Func<T, bool> predicate)
        {
            if (dynamicPredicates == null)
                dynamicPredicates = new List<Func<T, bool>>();

            dynamicPredicates.Add(predicate);
            InvalidateCanExecute();
        }

        public void RemoveCanExecute(Func<T, bool> predicate)
        {
            if (dynamicPredicates == null) return;

            if (dynamicPredicates.Remove(predicate))
                InvalidateCanExecute();
        }

        #endregion

        #region Implementation of ICommand

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(AdaptParameter(parameter));
        }

        void ICommand.Execute(object parameter)
        {
            Execute(AdaptParameter(parameter));
        }

        public event EventHandler CanExecuteChanged;

        #endregion

        /// <summary>
        /// Adapt the parameter instance to generic parameter type.
        /// </summary>
        /// <param name="parameter">Parameter instance to adapt</param>
        /// <returns>Parameter instance adapted</returns>
        protected virtual T AdaptParameter(object parameter)
        {
            if (adaptParameter != null)
                return adaptParameter(parameter);

            if (parameter is T)
                return (T)parameter;

            return default(T);
        }
    }

    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. 
    /// The default return value for the CanExecute method is 'true'.
    /// </summary>
    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action<object> execute)
            : base(execute) { }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
            : base(execute, canExecute) { }
    }
}
