using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XShell.Core
{
    public interface ICollectionEditor : ICollectionSelector
    {
        bool AllowAdd { get; set; }
        IRelayCommand AddCommand { get; }

        bool AllowRemove { get; set; }
        IRelayCommand RemoveCommand { get; }
        
        bool AllowClone { get; set; }
        IRelayCommand CloneCommand { get; }
        
        bool AllowClear { get; set; }
        IRelayCommand ClearCommand { get; }
        
        bool AllowMove { get; set; }
        IRelayCommand MoveUpCommand { get; }
        IRelayCommand MoveDownCommand { get; }
    }

    public interface ICollectionEditor<T> : ICollectionEditor
    {
        Func<T> Factory { get; set; }

        Func<T, T> Clone { get; set; } 
    }

    public class CollectionEditor<T> : CollectionSelector<T>, ICollectionEditor<T>
    {
        public CollectionEditor(IEnumerable<T> source = null, Func<T> factory = null)
            : this(source != null ? new ObservableCollection<T>(source) : null, factory) { }

        public CollectionEditor(IList<T> source, Func<T> factory = null)
            : base(source)
        {
            this.factory = factory;

            this.AddCommand = new RelayCommand(ExecuteAddCommand, CanExecuteAddCommand) { Name = "Add" };
            this.RemoveCommand = new RelayCommand(ExecuteRemoveCommand, CanExecuteRemoveCommand) { Name = "Remove" };
            this.CloneCommand = new RelayCommand(ExecuteCloneCommand, CanExecuteCloneCommand) { Name = "Clone" };
            this.MoveUpCommand = new RelayCommand(ExecuteMoveUpCommand, CanExecuteMoveUpCommand) { Name = "Move Up" };
            this.MoveDownCommand = new RelayCommand(ExecuteMoveDownCommand, CanExecuteMoveDownCommand) { Name = "Move Down" };
            this.ClearCommand = new RelayCommand(ExecuteClearCommand, CanExecuteClearCommand) { Name = "Clear" };
        }

        protected override void OnItemsChanged(IList<T> oldValue, IList<T> newValue)
        {
            base.OnItemsChanged(oldValue, newValue);

            this.AddCommand.InvalidateCanExecute();
            this.RemoveCommand.InvalidateCanExecute();
            this.CloneCommand.InvalidateCanExecute();
            this.MoveUpCommand.InvalidateCanExecute();
            this.MoveDownCommand.InvalidateCanExecute();
            this.ClearCommand.InvalidateCanExecute();
        }

        protected override void OnSelectedIndexChanged(int oldValue, int newValue)
        {
            base.OnSelectedIndexChanged(oldValue, newValue);

            this.RemoveCommand.InvalidateCanExecute();
            this.CloneCommand.InvalidateCanExecute();
            this.MoveUpCommand.InvalidateCanExecute();
            this.MoveDownCommand.InvalidateCanExecute();
        }

        #region Add

        private static readonly bool hasDefaultCtor = typeof(T).GetConstructors().Any(p => p.GetParameters().Length == 0);

        private Func<T> factory;
        public Func<T> Factory
        {
            get { return factory; }
            set
            {
                if (ReferenceEquals(factory, value)) return;
                factory = value;

                RaisePropertyChanged(Properties.FactoryPropertyChanged);
                this.AddCommand.InvalidateCanExecute();
            }
        }

        private bool allowAdd = true;
        public bool AllowAdd
        {
            get { return this.allowAdd; }
            set
            {
                if (this.allowAdd == value) return;
                this.allowAdd = value;

                this.RaisePropertyChanged(Properties.AllowAddPropertyChanged);
                this.AddCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand AddCommand { get; private set; }

        private bool CanExecuteAddCommand(object parameter)
        {
            return this.allowAdd && this.items != null && (this.factory != null || hasDefaultCtor);
        }

        private void ExecuteAddCommand(object parameter)
        {
            T newItem;
            if (this.factory != null)
                newItem = this.factory();
            else if (hasDefaultCtor) 
                newItem = Activator.CreateInstance<T>();
            else return;

            this.items.Add(newItem);
            this.SelectedIndex = this.items.Count - 1;
        }

        #endregion

        #region Remove

        private bool allowRemove = true;
        public bool AllowRemove
        {
            get { return allowRemove; }
            set
            {
                if (this.allowRemove == value) return;
                this.allowRemove = value;

                this.RaisePropertyChanged(Properties.AllowRemovePropertyChanged);
                this.RemoveCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand RemoveCommand { get; private set; }

        private bool CanExecuteRemoveCommand(object parameter)
        {
            return this.allowRemove && this.items != null && this.selectedIndex >= 0 && this.selectedIndex < this.items.Count;
        }

        private void ExecuteRemoveCommand(object parameter)
        {
            this.items.RemoveAt(this.selectedIndex);

            var mem = Math.Min(this.selectedIndex, this.items.Count - 1);
            this.selectedIndex = -1;
            this.SelectedIndex = mem;
        }

        #endregion

        #region Clone

        private Func<T, T> clone;
        public Func<T, T> Clone
        {
            get { return clone; }
            set
            {
                if (ReferenceEquals(this.clone, value)) return;
                this.clone = value;

                this.RaisePropertyChanged(Properties.ClonePropertyChanged);
                this.CloneCommand.InvalidateCanExecute();
            }
        }

        private bool allowClone = true;
        public bool AllowClone
        {
            get { return allowClone; }
            set
            {
                if (this.allowClone == value) return;
                this.allowClone = value;

                this.RaisePropertyChanged(Properties.AllowClonePropertyChanged);
                this.CloneCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand CloneCommand { get; private set; }

        private bool CanExecuteCloneCommand(object parameter)
        {
            return this.allowClone && this.clone != null && this.items != null && this.selectedIndex >= 0 && this.selectedIndex < this.items.Count;
        }

        private void ExecuteCloneCommand(object parameter)
        {
            var cloned = this.clone(this.items[this.selectedIndex]);
            this.items.Insert(this.selectedIndex + 1, cloned);
            this.SelectedIndex = this.SelectedIndex + 1;
        }

        #endregion

        #region Move

        private bool allowMove = true;
        public bool AllowMove
        {
            get { return this.allowMove; }
            set 
            { 
                if (this.allowMove == value) return;
                this.allowMove = value;

                this.RaisePropertyChanged(Properties.AllowMovePropertyChanged);
                this.MoveUpCommand.InvalidateCanExecute();
                this.MoveDownCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand MoveUpCommand { get; private set; }
        
        public IRelayCommand MoveDownCommand { get; private set; }

        private bool CanExecuteMoveUpCommand(object parameter)
        {
            return this.allowMove && this.items != null && this.selectedIndex > 0 && this.selectedIndex < this.items.Count;
        }

        private void ExecuteMoveUpCommand(object parameter)
        {
            var oc = this.items as ObservableCollection<T>;
            if (oc != null) oc.Move(this.selectedIndex, this.selectedIndex - 1);
            else
            {
                var swap = this.items[this.selectedIndex - 1];
                this.items[this.selectedIndex - 1] = this.items[this.selectedIndex];
                this.items[this.selectedIndex] = swap;
            }
            this.SelectedIndex = this.selectedIndex - 1;
        }

        private bool CanExecuteMoveDownCommand(object parameter)
        {
            return this.allowMove && this.items != null && this.selectedIndex >= 0 && this.selectedIndex < this.items.Count - 1;
        }

        private void ExecuteMoveDownCommand(object parameter)
        {
            var oc = this.items as ObservableCollection<T>;
            if (oc != null) oc.Move(this.selectedIndex, this.selectedIndex + 1);
            else
            {
                var swap = this.items[this.selectedIndex];
                this.items[this.selectedIndex] = this.items[this.selectedIndex + 1];
                this.items[this.selectedIndex + 1] = swap;
            }
            this.SelectedIndex = this.selectedIndex + 1;
        }

        #endregion

        #region Clear

        private bool allowClear = true;
        public bool AllowClear
        {
            get { return this.allowClear; }
            set
            {
                if (this.allowClear == value) return;
                this.allowClear = value;
                
                this.RaisePropertyChanged(Properties.AllowClearPropertyChanged);
                this.ClearCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand ClearCommand { get; private set; }

        private bool CanExecuteClearCommand(object parameter)
        {
            return this.allowClear && this.items != null;
        }

        private void ExecuteClearCommand(object parameter)
        {
            this.items.Clear();
            this.SelectedIndex = -1;
        }

        #endregion
    }
}