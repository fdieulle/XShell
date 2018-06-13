using System.ComponentModel;

namespace XShell.Core
{
    public static class Properties
    {
        public const string Null = null;
        public static readonly PropertyChangedEventArgs NullPropertyChanged = new PropertyChangedEventArgs(Null);

        public const string Name = "Name";
        public static readonly PropertyChangedEventArgs NamePropertyChanged = new PropertyChangedEventArgs(Name);

        public const string Title = "Title";
        public static readonly PropertyChangedEventArgs TitlePropertyChanged = new PropertyChangedEventArgs(Title);

        public const string Parameter = "Parameter";
        public static readonly PropertyChangedEventArgs ParameterPropertyChanged = new PropertyChangedEventArgs(Parameter);

        public const string IsRunning = "IsRunning";
        public static readonly PropertyChangedEventArgs IsRunningPropertyChanged = new PropertyChangedEventArgs(IsRunning);

        public const string Object = "Object";
        public static readonly PropertyChangedEventArgs ObjectPropertyChanged = new PropertyChangedEventArgs(Object);

        public const string Editable = "Editable";
        public static readonly PropertyChangedEventArgs EditablePropertyChanged = new PropertyChangedEventArgs(Editable);

        public const string Items = "Items";
        public static readonly PropertyChangedEventArgs ItemsPropertyChanged = new PropertyChangedEventArgs(Items);

        public const string SelectedItem = "SelectedItem";
        public static readonly PropertyChangedEventArgs SelectedItemPropertyChanged = new PropertyChangedEventArgs(SelectedItem);

        public const string SelectedIndex = "SelectedIndex";
        public static readonly PropertyChangedEventArgs SelectedIndexPropertyChanged = new PropertyChangedEventArgs(SelectedIndex);

        public const string ItemFactory = "ItemFactory";
        public static readonly PropertyChangedEventArgs ItemFactoryPropertyChanged = new PropertyChangedEventArgs(ItemFactory);

        public const string AllowAdd = "AllowAdd";
        public static readonly PropertyChangedEventArgs AllowAddPropertyChanged = new PropertyChangedEventArgs(AllowAdd);

        public const string AllowRemove = "AllowRemove";
        public static readonly PropertyChangedEventArgs AllowRemovePropertyChanged = new PropertyChangedEventArgs(AllowRemove);

        public const string ItemCloner = "ItemCloner";
        public static readonly PropertyChangedEventArgs ItemClonerPropertyChanged = new PropertyChangedEventArgs(ItemCloner);

        public const string AllowClone = "AllowClone";
        public static readonly PropertyChangedEventArgs AllowClonePropertyChanged = new PropertyChangedEventArgs(AllowClone);

        public const string AllowMove = "AllowMove";
        public static readonly PropertyChangedEventArgs AllowMovePropertyChanged = new PropertyChangedEventArgs(AllowMove);

        public const string AllowClear = "AllowClear";
        public static readonly PropertyChangedEventArgs AllowClearPropertyChanged = new PropertyChangedEventArgs(AllowClear);

        public const string AllowImport = "AllowImport";
        public static readonly PropertyChangedEventArgs AllowImportPropertyChanged = new PropertyChangedEventArgs(AllowImport);

        public const string Importer = "Importer";
        public static readonly PropertyChangedEventArgs ImporterPropertyChanged = new PropertyChangedEventArgs(Importer);

        public const string AllowExport = "AllowExport";
        public static readonly PropertyChangedEventArgs AllowExportPropertyChanged = new PropertyChangedEventArgs(AllowExport);

        public const string Exporter = "Exporter";
        public static readonly PropertyChangedEventArgs ExporterPropertyChanged = new PropertyChangedEventArgs(Exporter);
    }
}
