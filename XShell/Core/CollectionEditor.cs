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
            _itemFactory = itemFactory;
            _itemCloner = itemCloner;

            AddCommand = new RelayCommand(ExecuteAddCommand, CanExecuteAddCommand) { Name = "Add" };
            RemoveCommand = new RelayCommand(ExecuteRemoveCommand, CanExecuteRemoveCommand) { Name = "Remove" };
            CloneCommand = new RelayCommand(ExecuteCloneCommand, CanExecuteCloneCommand) { Name = "Clone" };
            MoveUpCommand = new RelayCommand(ExecuteMoveUpCommand, CanExecuteMoveUpCommand) { Name = "Move Up" };
            MoveDownCommand = new RelayCommand(ExecuteMoveDownCommand, CanExecuteMoveDownCommand) { Name = "Move Down" };
            ClearCommand = new RelayCommand(ExecuteClearCommand, CanExecuteClearCommand) { Name = "Clear" };
            ImportCommand = new RelayCommand(ExecuteImportCommand, CanExecuteImportCommand) { Name = "Import" };
            ExportCommand = new RelayCommand(ExecuteExportCommand, CanExecuteExportCommand) { Name = "Export" };
        }

        protected override void OnItemsChanged(IList<T> oldValue, IList<T> newValue)
        {
            base.OnItemsChanged(oldValue, newValue);

            AddCommand.InvalidateCanExecute();
            RemoveCommand.InvalidateCanExecute();
            CloneCommand.InvalidateCanExecute();
            MoveUpCommand.InvalidateCanExecute();
            MoveDownCommand.InvalidateCanExecute();
            ClearCommand.InvalidateCanExecute();
            ExportCommand.InvalidateCanExecute();
        }

        protected override void OnSelectedIndexChanged(int oldValue, int newValue)
        {
            base.OnSelectedIndexChanged(oldValue, newValue);

            RemoveCommand.InvalidateCanExecute();
            CloneCommand.InvalidateCanExecute();
            MoveUpCommand.InvalidateCanExecute();
            MoveDownCommand.InvalidateCanExecute();
        }

        #region Add

        private static readonly bool hasDefaultCtor = typeof(T).GetConstructors().Any(p => p.GetParameters().Length == 0);

        private Func<T> _itemFactory;
        public Func<T> ItemFactory
        {
            get => _itemFactory;
            set
            {
                if (ReferenceEquals(_itemFactory, value)) return;
                _itemFactory = value;

                RaisePropertyChanged(Properties.ItemFactoryPropertyChanged);
                AddCommand.InvalidateCanExecute();
            }
        }

        private bool _allowAdd = true;
        public bool AllowAdd
        {
            get => _allowAdd;
            set
            {
                if (_allowAdd == value) return;
                _allowAdd = value;

                RaisePropertyChanged(Properties.AllowAddPropertyChanged);
                AddCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand AddCommand { get; }

        private bool CanExecuteAddCommand(object parameter)
        {
            return _allowAdd && items != null && (_itemFactory != null || hasDefaultCtor);
        }

        private void ExecuteAddCommand(object parameter)
        {
            T newItem;
            if (_itemFactory != null)
                newItem = _itemFactory();
            else if (hasDefaultCtor) 
                newItem = Activator.CreateInstance<T>();
            else return;

            items.Add(newItem);
            SelectedIndex = items.Count - 1;
        }

        #endregion

        #region Remove

        private bool _allowRemove = true;
        public bool AllowRemove
        {
            get => _allowRemove;
            set
            {
                if (_allowRemove == value) return;
                _allowRemove = value;

                RaisePropertyChanged(Properties.AllowRemovePropertyChanged);
                RemoveCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand RemoveCommand { get; }

        private bool CanExecuteRemoveCommand(object parameter)
        {
            return _allowRemove && items != null && _selectedIndex >= 0 && _selectedIndex < items.Count;
        }

        private void ExecuteRemoveCommand(object parameter)
        {
            var mem = Math.Min(_selectedIndex, items.Count - 2);
            
            items.RemoveAt(_selectedIndex);
            
            _selectedIndex = -1;
            SelectedIndex = mem;
        }

        #endregion

        #region Clone

        private static readonly bool implementsICloneable = typeof(T).GetInterfaces().Any(p => p == typeof(ICloneable));

        private Func<T, T> _itemCloner;
        public Func<T, T> ItemCloner
        {
            get => _itemCloner;
            set
            {
                if (ReferenceEquals(_itemCloner, value)) return;
                _itemCloner = value;

                RaisePropertyChanged(Properties.ItemClonerPropertyChanged);
                CloneCommand.InvalidateCanExecute();
            }
        }

        private bool _allowClone = true;
        public bool AllowClone
        {
            get => _allowClone;
            set
            {
                if (_allowClone == value) return;
                _allowClone = value;

                RaisePropertyChanged(Properties.AllowClonePropertyChanged);
                CloneCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand CloneCommand { get; }

        private bool CanExecuteCloneCommand(object parameter)
        {
            return _allowClone && (_itemCloner != null || implementsICloneable) && items != null && _selectedIndex >= 0 && _selectedIndex < items.Count;
        }

        private void ExecuteCloneCommand(object parameter)
        {
            var cloned = _itemCloner != null 
                ? _itemCloner(items[_selectedIndex]) : 
                (T)((ICloneable)items[_selectedIndex]).Clone();
            
            items.Insert(_selectedIndex + 1, cloned);
            SelectedIndex = SelectedIndex + 1;
        }

        #endregion

        #region Move

        private bool _allowMove = true;
        public bool AllowMove
        {
            get => _allowMove;
            set 
            { 
                if (_allowMove == value) return;
                _allowMove = value;

                RaisePropertyChanged(Properties.AllowMovePropertyChanged);
                MoveUpCommand.InvalidateCanExecute();
                MoveDownCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand MoveUpCommand { get; }
        
        public IRelayCommand MoveDownCommand { get; }

        private bool CanExecuteMoveUpCommand(object parameter)
        {
            return _allowMove && items != null && _selectedIndex > 0 && _selectedIndex < items.Count;
        }

        private void ExecuteMoveUpCommand(object parameter)
        {
            items.Move(_selectedIndex, _selectedIndex - 1);
            SelectedIndex = _selectedIndex - 1;
        }

        private bool CanExecuteMoveDownCommand(object parameter)
        {
            return _allowMove && items != null && _selectedIndex >= 0 && _selectedIndex < items.Count - 1;
        }

        private void ExecuteMoveDownCommand(object parameter)
        {
            items.Move(_selectedIndex, _selectedIndex + 1);
            SelectedIndex = _selectedIndex + 1;
        }

        #endregion

        #region Clear

        private bool _allowClear = true;
        public bool AllowClear
        {
            get => _allowClear;
            set
            {
                if (_allowClear == value) return;
                _allowClear = value;
                
                RaisePropertyChanged(Properties.AllowClearPropertyChanged);
                ClearCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand ClearCommand { get; }

        private bool CanExecuteClearCommand(object parameter)
        {
            return _allowClear && items != null;
        }

        private void ExecuteClearCommand(object parameter)
        {
            items.Clear();
            SelectedIndex = -1;
        }

        #endregion

        #region Import

        private Func<IEnumerable<T>> _importer;
        public Func<IEnumerable<T>> Importer
        {
            get => _importer;
            set
            {
                if (_importer == value) return;
                _importer = value;

                RaisePropertyChanged(Properties.ImporterPropertyChanged);
                ImportCommand.InvalidateCanExecute();
            }
        }

        private bool _allowImport = true;
        public bool AllowImport
        {
            get => _allowImport;
            set
            {
                if (_allowImport == value) return;
                _allowImport = value;

                RaisePropertyChanged(Properties.AllowImportPropertyChanged);
                ImportCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand ImportCommand { get; }

        private bool CanExecuteImportCommand(object parameter)
        {
            return _allowImport && _importer != null;
        }

        private void ExecuteImportCommand(object parameter)
        {
            var imported = _importer();
            if (imported == null) return;

            if (items == null)
                items = new ObservableCollection<T>(imported);
            else
            {
                foreach (var item in imported)
                    items.Add(item);   
            }
        }

        #endregion

        #region Import

        private Action<IEnumerable<T>> _exporter;
        public Action<IEnumerable<T>> Exporter
        {
            get => _exporter;
            set
            {
                if (_exporter == value) return;
                _exporter = value;

                RaisePropertyChanged(Properties.ExporterPropertyChanged);
                ExportCommand.InvalidateCanExecute();
            }
        }

        private bool _allowExport = true;
        public bool AllowExport
        {
            get => _allowExport;
            set
            {
                if (_allowExport == value) return;
                _allowExport = value;

                RaisePropertyChanged(Properties.AllowExportPropertyChanged);
                ExportCommand.InvalidateCanExecute();
            }
        }

        public IRelayCommand ExportCommand { get; }

        private bool CanExecuteExportCommand(object parameter)
        {
            return _allowExport && items != null && _exporter != null;
        }

        private void ExecuteExportCommand(object parameter)
        {
            _exporter(items);
        }

        #endregion
    }
}