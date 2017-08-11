using System.ComponentModel;

namespace XShell.Core
{
    public static class Properties
    {
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
    }
}
