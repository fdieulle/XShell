//using System;
//using System.Collections.Generic;
//using System.ComponentModel;

//namespace XShell.Core
//{
//    public interface ICollectionEditor : ICollectionSelector
//    {
//        bool AllowAddNew { get; set; }
//        IRelayCommand AddNewCommand { get; }
//        bool AllowClone { get; set; }
//        IRelayCommand CloneCommand { get; }
//        IRelayCommand CloneAtCommand { get; }
//        bool AllowRemove { get; set; }
//        IRelayCommand RemoveCommand { get; }
//        IRelayCommand RemoveAtCommand { get; }
//        bool AllowClear { get; set; }
//        IRelayCommand ClearCommand { get; }
//        bool AllowMoveUp { get; set; }
//        IRelayCommand MoveUpCommand { get; }
//        IRelayCommand MoveIndexUpCommand { get; }
//        bool AllowMoveDown { get; set; }
//        IRelayCommand MoveDownCommand { get; }
//        IRelayCommand MoveIndexDownCommand { get; }
//        bool AllowEditSelectedObject { get; set; }
//        IObjectEditor SelectedObjectEditor { get; }
//    }
//    public class CollectionEditor<T> : CollectionSelector<T>, ICollectionEditor
//    {
//        private static readonly PropertyChangedEventArgs factoryPropertyChanged = new PropertyChangedEventArgs("Factory");
//        private Func<T> factory;
//        public Func<T> Factory
//        {
//            get { return factory; }
//            set
//            {
//                if (ReferenceEquals(factory, value)) return;
//                factory = value;

//                RaisePropertyChanged(factoryPropertyChanged);
//                AddNewCommand.InvalidateCanExecute();
//            }
//        }

//        private static readonly PropertyChangedEventArgs clonePropertyChanged = new PropertyChangedEventArgs("Clone");
//        private Func<T, T> clone;
//        private readonly bool raiseItemsWhenListIsModified;

//        public Func<T, T> Clone
//        {
//            get { return clone; }
//            set
//            {
//                if (ReferenceEquals(clone, value)) return;
//                clone = value;

//                RaisePropertyChanged(clonePropertyChanged);
//                CloneCommand.InvalidateCanExecute();
//            }
//        }

//        private static readonly PropertyChangedEventArgs allowAddNewPropertyChanged = new PropertyChangedEventArgs("AllowAddNew");
//        private bool allowAddNew = true;
//        public bool AllowAddNew
//        {
//            get { return allowAddNew; }
//            set
//            {
//                if (allowAddNew == value) return;
//                allowAddNew = value;
//                RaisePropertyChanged(allowAddNewPropertyChanged);
//                AddNewCommand.InvalidateCanExecute();
//            }
//        }

//        public IRelayCommand AddNewCommand { get; private set; }

//        private static readonly PropertyChangedEventArgs allowClonePropertyChanged = new PropertyChangedEventArgs("AllowClone");
//        private bool allowClone = true;
//        public bool AllowClone
//        {
//            get { return allowClone; }
//            set
//            {
//                if (allowClone == value) return;
//                allowClone = value;
//                RaisePropertyChanged(allowClonePropertyChanged);
//                CloneCommand.InvalidateCanExecute();
//                CloneAtCommand.InvalidateCanExecute();
//            }
//        }

//        public IRelayCommand CloneCommand { get; private set; }

//        public IRelayCommand CloneAtCommand { get; private set; }

//        private static readonly PropertyChangedEventArgs allowRemovePropertyChanged = new PropertyChangedEventArgs("AllowRemove");
//        private bool allowRemove = true;
//        public bool AllowRemove
//        {
//            get { return allowRemove; }
//            set
//            {
//                if (allowRemove == value) return;
//                allowRemove = value;
//                RaisePropertyChanged(allowRemovePropertyChanged);
//                RemoveCommand.InvalidateCanExecute();
//                RemoveAtCommand.InvalidateCanExecute();
//            }
//        }

//        public IRelayCommand RemoveCommand { get; private set; }

//        public IRelayCommand RemoveAtCommand { get; private set; }

//        private static readonly PropertyChangedEventArgs allowClearPropertyChanged = new PropertyChangedEventArgs("AllowClear");
//        private bool allowClear = true;
//        public bool AllowClear
//        {
//            get { return allowClear; }
//            set
//            {
//                if (allowClear == value) return;
//                allowClear = value;
//                RaisePropertyChanged(allowClearPropertyChanged);
//                ClearCommand.InvalidateCanExecute();
//            }
//        }

//        public IRelayCommand ClearCommand { get; private set; }

//        private static readonly PropertyChangedEventArgs allowMoveUpPropertyChanged = new PropertyChangedEventArgs("AllowMoveUp");
//        private bool allowMoveUp = true;
//        public bool AllowMoveUp
//        {
//            get { return allowMoveUp; }
//            set
//            {
//                if (allowMoveUp == value) return;
//                allowMoveUp = value;
//                RaisePropertyChanged(allowMoveUpPropertyChanged);
//                MoveUpCommand.InvalidateCanExecute();
//                MoveIndexUpCommand.InvalidateCanExecute();
//            }
//        }

//        public IRelayCommand MoveUpCommand { get; private set; }

//        private static readonly PropertyChangedEventArgs allowMoveDownPropertyChanged = new PropertyChangedEventArgs("AllowMoveDown");
//        private bool allowMoveDown = true;
//        public bool AllowMoveDown
//        {
//            get { return allowMoveDown; }
//            set
//            {
//                if (allowMoveDown == value) return;
//                allowMoveDown = value;
//                RaisePropertyChanged(allowMoveDownPropertyChanged);
//                MoveDownCommand.InvalidateCanExecute();
//                MoveIndexDownCommand.InvalidateCanExecute();
//            }
//        }

//        public IRelayCommand MoveDownCommand { get; private set; }

//        public IRelayCommand MoveIndexUpCommand { get; private set; }

//        public IRelayCommand MoveIndexDownCommand { get; private set; }

//        private static readonly PropertyChangedEventArgs allowEditSelectedObjectPropertyChanged = new PropertyChangedEventArgs("AllowEditSelectedObject");
//        private bool allowEditSelectedObject = true;
//        public bool AllowEditSelectedObject
//        {
//            get { return allowEditSelectedObject; }
//            set
//            {
//                if (allowEditSelectedObject == value) return;
//                allowEditSelectedObject = value;
//                RaisePropertyChanged(allowEditSelectedObjectPropertyChanged);
//            }
//        }

//        public ObjectEditor<T> SelectedObjectEditor { get; private set; } 

//        public CollectionEditor(IList<T> items = null, Func<T> factory = null, Func<T, T> clone = null, bool raiseItemsWhenListIsModified = true)
//        {
//            this.items = items;
//            this.factory = factory;
//            this.clone = clone;
//            this.raiseItemsWhenListIsModified = raiseItemsWhenListIsModified;

//            AddNewCommand = new RelayCommand(ExecuteAddNewCommand, p => this.items != null && this.factory != null && allowAddNew);
//            CloneCommand = new RelayCommand(p => ExecuteCloneCommand(this.selectedItem), p => this.items != null && this.clone != null && allowClone);
//            CloneAtCommand = new RelayCommand(p => ExecuteCloneCommand(this.items[this.selectedIndex])), p => this.items != null && this.clone != null && this.selectedIndex >= 0 && this.selectedIndex < this.items.Count && allowClone);
//            RemoveCommand = new RelayCommand(ExecuteRemoveCommand, p => this.items != null && this.selectedIndex >= 0 && this.selectedIndex < this.items.Count && allowRemove);
//            RemoveAtCommand = new RelayCommand(ExecuteRemoveAtCommand, p => this.items != null && allowRemove);
//            ClearCommand = new RelayCommand(ExecuteClearCommand, p => this.items != null && allowClear);
//            MoveUpCommand = new RelayCommand(p => MoveUp(this.items.IndexOf(this.selectedItem)), p => this.items != null && allowMoveUp);
//            MoveDownCommand = new RelayCommand(p => MoveDown(this.items.IndexOf(this.selectedItem)), p => this.items != null && allowMoveDown);
//            MoveIndexUpCommand = new RelayCommand(p => MoveUp(this.selectedIndex), p => this.items != null && this.selectedIndex > 0 && allowMoveUp);
//            MoveIndexDownCommand = new RelayCommand(p => MoveDown(this.selectedIndex), p => this.items != null && this.selectedIndex < this.items.Count - 1 && allowMoveDown);

//            SelectedObjectEditor = new ObjectEditor<T>(clone);
//            SelectedObjectEditor.ApplyExecuted += OnSelectedObjectEditorApplyExecuted;
//        }

//        private void ExecuteAddNewCommand(object parameter)
//        {
//            var newItem = this.factory();
//            this.items.Add(newItem);
//            if (this.raiseItemsWhenListIsModified)
//                RaisePropertyChanged(itemsPropertyChanged);
//            SelectedItem = newItem;
//        }

//        private void ExecuteCloneCommand(T item)
//        {
//            var cloneItem = this.clone(item);
//            this.items.Add(cloneItem);
//            if (this.raiseItemsWhenListIsModified)
//                RaisePropertyChanged(itemsPropertyChanged);
//            SelectedItem = cloneItem;
//        }

//        private void ExecuteRemoveCommand(object parameter)
//        {
//            this.items.Remove(this.selectedItem);
//            if (this.raiseItemsWhenListIsModified)
//                RaisePropertyChanged(itemsPropertyChanged);
//            SelectedItem = default(T);
//        }

//        private void ExecuteRemoveAtCommand(object parameter)
//        {
//            this.items.RemoveAt(this.selectedIndex);
//            if (this.raiseItemsWhenListIsModified)
//                RaisePropertyChanged(itemsPropertyChanged);
//            SelectedItem = default(T);
//        }

//        private void ExecuteClearCommand(object parameter)
//        {
//            this.items.Clear();
//            if (this.raiseItemsWhenListIsModified)
//                RaisePropertyChanged(itemsPropertyChanged);
//        }

//        private void OnSelectedObjectEditorApplyExecuted(T oldvalue, T newvalue)
//        {
//            SelectedItem = newvalue;
//            if (items != null && SelectedIndex >= 0 && SelectedIndex < items.Count)
//            {
//                items[SelectedIndex] = newvalue;
//                if (this.raiseItemsWhenListIsModified)
//                    RaisePropertyChanged(itemsPropertyChanged);
//            }
//        }

//        public void InvalidateCommands()
//        {
//            AddNewCommand.InvalidateCanExecute();
//            CloneCommand.InvalidateCanExecute();
//            CloneAtCommand.InvalidateCanExecute();
//            RemoveCommand.InvalidateCanExecute();
//            RemoveAtCommand.InvalidateCanExecute();
//            ClearCommand.InvalidateCanExecute();
//            MoveUpCommand.InvalidateCanExecute();
//            MoveDownCommand.InvalidateCanExecute();
//            MoveIndexUpCommand.InvalidateCanExecute();
//            MoveIndexDownCommand.InvalidateCanExecute();
//        }

//        private void MoveUp(int index)
//        {
//            if (index <= 0) return;

//            var swap = this.items[index - 1];
//            this.items[index - 1] = this.items[index];
//            this.items[index] = swap;

//            if (this.raiseItemsWhenListIsModified)
//                RaisePropertyChanged(itemsPropertyChanged);
//        }

//        private void MoveDown(int index)
//        {
//            if (index >= this.items.Count) return;

//            var swap = this.items[index];
//            this.items[index] = this.items[index + 1];
//            this.items[index + 1] = swap;

//            if (this.raiseItemsWhenListIsModified)
//                RaisePropertyChanged(itemsPropertyChanged);
//        }

//        protected override void RaisePropertyChanged(PropertyChangedEventArgs e)
//        {
//            base.RaisePropertyChanged(e);
//            switch (e.PropertyName)
//            {
//                case "Items":
//                    InvalidateCommands();
//                    break;
//                case "SelectedItem":
//                    CloneCommand.InvalidateCanExecute();
//                    RemoveCommand.InvalidateCanExecute();
//                    MoveUpCommand.InvalidateCanExecute();
//                    MoveDownCommand.InvalidateCanExecute();
//                    break;
//                case "SelectedIndex":
//                    CloneAtCommand.InvalidateCanExecute();
//                    RemoveAtCommand.InvalidateCanExecute();
//                    MoveIndexUpCommand.InvalidateCanExecute();
//                    MoveIndexDownCommand.InvalidateCanExecute();
//                    break;
//            }
//        }

//        IObjectEditor ICollectionEditor.SelectedObjectEditor { get { return SelectedObjectEditor; } }
//    }
//}