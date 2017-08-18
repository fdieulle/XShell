using System.ComponentModel;

namespace XShell.Core
{
    public static class Properties
    {
        public const string NULL = null;
        public static readonly PropertyChangedEventArgs NullPropertyChanged = new PropertyChangedEventArgs(NULL);

        public const string NAME = "Name";
        public static readonly PropertyChangedEventArgs NamePropertyChanged = new PropertyChangedEventArgs(NAME);

        public const string TITLE = "Title";
        public static readonly PropertyChangedEventArgs TitlePropertyChanged = new PropertyChangedEventArgs(TITLE);

        public const string PARAMETER = "Parameter";
        public static readonly PropertyChangedEventArgs ParameterPropertyChanged = new PropertyChangedEventArgs(PARAMETER);

        public const string IS_RUNNING = "IsRunning";
        public static readonly PropertyChangedEventArgs IsRunningPropertyChanged = new PropertyChangedEventArgs(IS_RUNNING);

        public const string OBJECT = "Object";
        public static readonly PropertyChangedEventArgs ObjectPropertyChanged = new PropertyChangedEventArgs(OBJECT);

        public const string EDITABLE = "Editable";
        public static readonly PropertyChangedEventArgs EditablePropertyChanged = new PropertyChangedEventArgs(EDITABLE);

        public const string ITEMS = "Items";
        public static readonly PropertyChangedEventArgs ItemsPropertyChanged = new PropertyChangedEventArgs(ITEMS);

        public const string SELECTED_ITEM = "SelectedItem";
        public static readonly PropertyChangedEventArgs SelectedItemPropertyChanged = new PropertyChangedEventArgs(SELECTED_ITEM);

        public const string SELECTED_INDEX = "SelectedIndex";
        public static readonly PropertyChangedEventArgs SelectedIndexPropertyChanged = new PropertyChangedEventArgs(SELECTED_INDEX);

        public const string ITEM_FACTORY = "ItemFactory";
        public static readonly PropertyChangedEventArgs ItemFactoryPropertyChanged = new PropertyChangedEventArgs(ITEM_FACTORY);

        public const string ALLOW_ADD = "AllowAdd";
        public static readonly PropertyChangedEventArgs AllowAddPropertyChanged = new PropertyChangedEventArgs(ALLOW_ADD);

        public const string ALLOW_REMOVE = "AllowRemove";
        public static readonly PropertyChangedEventArgs AllowRemovePropertyChanged = new PropertyChangedEventArgs(ALLOW_REMOVE);

        public const string ITEM_CLONER = "ItemCloner";
        public static readonly PropertyChangedEventArgs ItemClonerPropertyChanged = new PropertyChangedEventArgs(ITEM_CLONER);

        public const string ALLOW_CLONE = "AllowClone";
        public static readonly PropertyChangedEventArgs AllowClonePropertyChanged = new PropertyChangedEventArgs(ALLOW_CLONE);

        public const string ALLOW_MOVE = "AllowMove";
        public static readonly PropertyChangedEventArgs AllowMovePropertyChanged = new PropertyChangedEventArgs(ALLOW_MOVE);

        public const string ALLOW_CLEAR = "AllowClear";
        public static readonly PropertyChangedEventArgs AllowClearPropertyChanged = new PropertyChangedEventArgs(ALLOW_CLEAR);

        public const string ALLOW_IMPORT = "AllowImport";
        public static readonly PropertyChangedEventArgs AllowImportPropertyChanged = new PropertyChangedEventArgs(ALLOW_IMPORT);

        public const string IMPORTER = "Importer";
        public static readonly PropertyChangedEventArgs ImporterPropertyChanged = new PropertyChangedEventArgs(IMPORTER);

        public const string ALLOW_EXPORT = "AllowExport";
        public static readonly PropertyChangedEventArgs AllowExportPropertyChanged = new PropertyChangedEventArgs(ALLOW_EXPORT);

        public const string EXPORTER = "Exporter";
        public static readonly PropertyChangedEventArgs ExporterPropertyChanged = new PropertyChangedEventArgs(EXPORTER);
    }
}
