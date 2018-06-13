using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using XShell.Core;
using XShell.Winform.Behaviors;
using XShell.Winform.Binders;
using XShell.Winform.Properties;

namespace XShell.Winform.Controls
{
    public partial class ListBoxEditorView : UserControl
    {
        private ICollectionEditor _internalEditor;
        private IDisposable _itemsBinding;

        public ListBoxEditorView()
        {
            InitializeComponent();

            addButton.ApplyFlatStyle(Resources.add);
            removeButton.ApplyFlatStyle(Resources.remove);
            cloneButton.ApplyFlatStyle(Resources.clone);
            moveUpButton.ApplyFlatStyle(Resources.move_up);
            moveDownButton.ApplyFlatStyle(Resources.move_down);
            clearButton.ApplyFlatStyle(Resources.clear);
            importButton.ApplyFlatStyle(Resources.import);
            exportButton.ApplyFlatStyle(Resources.export);

            listBox.SelectedIndexChanged += OnListBoxSelectedIndexChanged;

            Disposed += (s, e) => InternalDispose();
        }

        private void OnListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_internalEditor != null)
                _internalEditor.SelectedIndex = listBox.SelectedIndex;
        }

        public void Bind(ICollectionEditor editor)
        {
            InternalDispose();

            _internalEditor = editor;
            if (editor == null) return;

            addButton.Bind(editor.AddCommand);
            removeButton.Bind(editor.RemoveCommand);
            cloneButton.Bind(editor.CloneCommand);
            moveUpButton.Bind(editor.MoveUpCommand);
            moveDownButton.Bind(editor.MoveDownCommand);
            clearButton.Bind(editor.ClearCommand);
            importButton.Bind(editor.ImportCommand);
            exportButton.Bind(editor.ExportCommand);
            
            OnEditorPropertyChanged(_internalEditor, Core.Properties.NullPropertyChanged);
            editor.PropertyChanged += OnEditorPropertyChanged;
        }

        private void OnEditorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case Core.Properties.AllowAdd:
                    addButton.Visible = _internalEditor.AllowAdd;
                    break;
                case Core.Properties.AllowRemove:
                    removeButton.Visible = _internalEditor.AllowRemove;
                    break;
                case Core.Properties.AllowClone:
                    cloneButton.Visible = _internalEditor.AllowClone;
                    break;
                case Core.Properties.AllowMove:
                    moveUpButton.Visible = moveDownButton.Visible = _internalEditor.AllowMove;
                    break;
                case Core.Properties.AllowClear:
                    clearButton.Visible = _internalEditor.AllowClear;
                    break;
                case Core.Properties.AllowImport:
                    importButton.Visible = _internalEditor.AllowImport;
                    break;
                case Core.Properties.AllowExport:
                    exportButton.Visible = _internalEditor.AllowExport;
                    break;
                case Core.Properties.Items:
                    _itemsBinding = listBox.Items.Bind(_internalEditor.Items);
                    break;
                case Core.Properties.SelectedIndex:
                    listBox.SelectedIndex = _internalEditor.SelectedIndex;
                    break;
                case Core.Properties.Null:
                    addButton.Visible = _internalEditor.AllowAdd;
                    removeButton.Visible = _internalEditor.AllowRemove;
                    cloneButton.Visible = _internalEditor.AllowClone;
                    moveUpButton.Visible = moveDownButton.Visible = _internalEditor.AllowMove;
                    clearButton.Visible = _internalEditor.AllowClear;
                    importButton.Visible = _internalEditor.AllowImport;
                    exportButton.Visible = _internalEditor.AllowExport;
                    _itemsBinding = listBox.Items.Bind(_internalEditor.Items);
                    listBox.SelectedIndex = _internalEditor.SelectedIndex;
                    break;
            }
        }

        private void InternalDispose()
        {            
            if (_internalEditor != null)
                _internalEditor.PropertyChanged -= OnEditorPropertyChanged;
            _itemsBinding?.Dispose();
        }
    }
}
