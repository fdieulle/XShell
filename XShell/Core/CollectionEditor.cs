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
        
        bool AllowMove { get; set; }
        IRelayCommand MoveUpCommand { get; }
        IRelayCommand MoveDownCommand { get; }

        bool AllowClear { get; set; }
        IRelayCommand ClearCommand { get; }

        bool AllowImport { get; set; }
        IRelayCommand ImportCommand { get; }

        bool AllowExport { get; set; }
        IRelayCommand ExportCommand { get; }
    }

    public interface ICollectionEditor<T> : ICollectionEditor
    {
        Func<T> ItemFactory { get; set; }

        Func<T, T> ItemCloner { get; set; }

        Func<IEnumerable<T>> Importer { get; set; }

        Action<IEnumerable<T>> Exporter { get; set; }
    }

    public class CollectionEditor<T> : CollectionSelector<T>, ICollectionEditor<T>
    {
        public CollectionEditor(IEnumerable<T> source = null, Func<T> itemFactory = null, Func<T, T> itemCloner = null)
            : base(source)
        {
            this.itemFactory = itemFactory;
            this.itemCloner = itemCloner;

            this.AddCommand = new RelayCommand(ExecuteAddCommand, CanExecuteAddCommand) { Name = "Add" };
            this.RemoveCommand = new RelayCommand(ExecuteRemoveCommand, CanExecuteRemoveCommand) { Name = "Remove" };
            this.CloneCommand = new RelayCommand(ExecuteCloneCommand, CanExecuteCloneCommand) { Name = "Clone" };
            this.MoveUpCommand = new RelayCommand(ExecuteMoveUpCommand, CanExecuteMoveUpCommand) { Name = "Move Up" };
            this.MoveDownCommand = new RelayCommand(ExecuteMoveDownCommand, CanExecuteMoveDownCommand) { Name = "Move Down" };
            this.ClearCommand = new RelayCommand(ExecuteClearCommand, CanExecuteClearCommand) { Name = "Clear" };
            this.ImportCommand = new RelayCommand(ExecuteImportCommand, CanExecuteImportCommand) { Name = "Import" };
            this.ExportCommand = new RelayCommand(ExecuteExportCommand, CanExecuteExportCommand) { Name = "Export" };
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
            this.ExportCommand.InvalidateCanExecute();
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

        private Func<T> itemFactory;
        public Func<T> ItemFactory
        {
            get { return itemFactory; }
            set
            {
                if (ReferenceEquals(itemFactory, value)) return;
                itemFactory = value;

                RaisePropertyChanged(Properties.ItemFactoryPropertyChanged);
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
            return this.allowAdd && this.items != null && (this.itemFactory != null || hasDefaultCtor);
        }

        private void ExecuteAddCommand(object parameter)
        {
            T newItem;
            if (this.itemFactory != null)
                newItem = this.itemFactory();
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
            var mem = Math.Min(this.selectedIndex, this.items.Count - 2);
            
            this.items.RemoveAt(this.selectedIndex);
            
            this.selectedIndex = -1;
            this.SelectedIndex = mem;
        }

        #endregion

        #region Clone

        private static readonly bool implementsICloneable = typeof(T).GetInterfaces().Any(p => p == typeof(ICloneable));

        private Func<T, T> itemCloner;
        public Func<T, T> ItemCloner
        {
            get { return itemCloner; }
            set
            {
                if (ReferenceEquals(this.itemCloner, value)) return;
                this.itemCloner = value;

                this.RaisePropertyChanged(Properties.ItemClonerPropertyChanged);
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
            return this.allowClone && (this.itemCloner != null || implementsICloneable) && this.items != null && this.selectedIndex >= 0 && this.selectedIndex < this.items.Count;
        }

        private void ExecuteCloneCommand(object parameter)
        {
            var cloned = this.itemCloner != null 
                ? this.itemCloner(this.items[this.selectedIndex]) : 
                (T)((ICloneable)this.items[this.selectedIndex]).Clone();
            
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
            this.items.Move(this.selectedIndex, this.selectedIndex - 1);
            this.SelectedIndex = this.selectedIndex - 1;
        }

        private bool CanExecuteMoveDownCommand(object parameter)
        {
            return this.allowMove && this.items != null && this.selectedIndex >= 0 && this.selectedIndex < this.items.Count - 1;
        }

        private void ExecuteMoveDownCommand(object parameter)
        {
            this.items.Move(this.selectedIndex, this.selectedIndex + 1);
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

        #region Import

        private Func<IEnumerable<T>> importer;
        public Func<IEnumerable<T>> Importer
        {
            get { return this.importer; }
            set
            {
                if (this.importer == value) return;
                this.importer = value;

                this.RaisePropertyChanged(Properties.ImporterPropertyChanged);
                this.ImportCommand.InvalidateCanExecute();
            }
        }

        private bool allowImport = true;
        public bool AllowImport
        {
            get { return this.allowImport; }
            set
            {
                if (this.allowImport == value) return;
                this.allowImport = value;

                this.RaisePropertyChanged(Properties.AllowImportPropertyChanged);
                this.ImportCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand ImportCommand { get; private set; }

        private bool CanExecuteImportCommand(object parameter)
        {
            return this.allowImport && this.importer != null;
        }

        private void ExecuteImportCommand(object parameter)
        {
            var imported = this.importer();
            if (imported == null) return;

            if (this.items == null)
                this.items = new ObservableCollection<T>(imported);
            else
            {
                foreach (var item in imported)
                    this.items.Add(item);   
            }
        }

        #endregion

        #region Import

        private Action<IEnumerable<T>> exporter;
        public Action<IEnumerable<T>> Exporter
        {
            get { return this.exporter; }
            set
            {
                if (this.exporter == value) return;
                this.exporter = value;

                this.RaisePropertyChanged(Properties.ExporterPropertyChanged);
                this.ExportCommand.InvalidateCanExecute();
            }
        }

        private bool allowExport = true;
        public bool AllowExport
        {
            get { return this.allowExport; }
            set
            {
                if (this.allowExport == value) return;
                this.allowExport = value;

                this.RaisePropertyChanged(Properties.AllowExportPropertyChanged);
                this.ExportCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand ExportCommand { get; private set; }

        private bool CanExecuteExportCommand(object parameter)
        {
            return this.allowExport && this.items != null && this.exporter != null;
        }

        private void ExecuteExportCommand(object parameter)
        {
            this.exporter(this.items);
        }

        #endregion
    }
}