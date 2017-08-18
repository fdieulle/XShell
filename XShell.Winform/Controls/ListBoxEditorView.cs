using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using XShell.Core;
using XShell.Winform.Behaviors;
using XShell.Winform.Properties;

namespace XShell.Winform.Controls
{
    public partial class ListBoxEditorView : UserControl
    {
        private readonly List<IDisposable> suscriptions = new List<IDisposable>();
        private readonly ToolTip toolTip = new ToolTip();
        private ICollectionEditor internalEditor;
        private IDisposable itemsBinding;

        public ListBoxEditorView()
        {
            InitializeComponent();

            this.addButton.ApplyFlatStyle(Resources.add);
            this.removeButton.ApplyFlatStyle(Resources.remove);
            this.cloneButton.ApplyFlatStyle(Resources.clone);
            this.moveUpButton.ApplyFlatStyle(Resources.move_up);
            this.moveDownButton.ApplyFlatStyle(Resources.move_down);
            this.clearButton.ApplyFlatStyle(Resources.clear);
            this.importButton.ApplyFlatStyle(Resources.import);
            this.exportButton.ApplyFlatStyle(Resources.export);

            this.listBox.SelectedIndexChanged += OnListBoxSelectedIndexChanged;

            Disposed += (s, e) => this.InternalDispose();
        }

        private void OnListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.internalEditor != null)
                this.internalEditor.SelectedIndex = listBox.SelectedIndex;
        }

        public void Bind(ICollectionEditor editor)
        {
            this.InternalDispose();

            this.internalEditor = editor;
            if (editor == null) return;

            this.suscriptions.Add(this.addButton.Bind(editor.AddCommand, bindName: false, tooltip: toolTip));
            this.suscriptions.Add(this.removeButton.Bind(editor.RemoveCommand, bindName: false, tooltip: toolTip));
            this.suscriptions.Add(this.cloneButton.Bind(editor.CloneCommand, bindName: false, tooltip: toolTip));
            this.suscriptions.Add(this.moveUpButton.Bind(editor.MoveUpCommand, bindName: false, tooltip: toolTip));
            this.suscriptions.Add(this.moveDownButton.Bind(editor.MoveDownCommand, bindName: false, tooltip: toolTip));
            this.suscriptions.Add(this.clearButton.Bind(editor.ClearCommand, bindName: false, tooltip: toolTip));
            this.suscriptions.Add(this.importButton.Bind(editor.ImportCommand, bindName: false, tooltip: toolTip));
            this.suscriptions.Add(this.exportButton.Bind(editor.ExportCommand, bindName: false, tooltip: toolTip));
            
            OnEditorPropertyChanged(this.internalEditor, Core.Properties.NullPropertyChanged);
            editor.PropertyChanged += OnEditorPropertyChanged;
        }

        private void OnEditorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case Core.Properties.ALLOW_ADD:
                    this.addButton.Visible = this.internalEditor.AllowAdd;
                    break;
                case Core.Properties.ALLOW_REMOVE:
                    this.removeButton.Visible = this.internalEditor.AllowRemove;
                    break;
                case Core.Properties.ALLOW_CLONE:
                    this.cloneButton.Visible = this.internalEditor.AllowClone;
                    break;
                case Core.Properties.ALLOW_MOVE:
                    this.moveUpButton.Visible = this.moveDownButton.Visible = this.internalEditor.AllowMove;
                    break;
                case Core.Properties.ALLOW_CLEAR:
                    this.clearButton.Visible = this.internalEditor.AllowClear;
                    break;
                case Core.Properties.ALLOW_IMPORT:
                    this.importButton.Visible = this.internalEditor.AllowImport;
                    break;
                case Core.Properties.ALLOW_EXPORT:
                    this.exportButton.Visible = this.internalEditor.AllowExport;
                    break;
                case Core.Properties.ITEMS:
                    this.itemsBinding = this.listBox.Items.Bind(this.internalEditor.Items);
                    break;
                case Core.Properties.SELECTED_INDEX:
                    this.listBox.SelectedIndex = this.internalEditor.SelectedIndex;
                    break;
                case Core.Properties.NULL:
                    this.addButton.Visible = this.internalEditor.AllowAdd;
                    this.removeButton.Visible = this.internalEditor.AllowRemove;
                    this.cloneButton.Visible = this.internalEditor.AllowClone;
                    this.moveUpButton.Visible = this.moveDownButton.Visible = this.internalEditor.AllowMove;
                    this.clearButton.Visible = this.internalEditor.AllowClear;
                    this.importButton.Visible = this.internalEditor.AllowImport;
                    this.exportButton.Visible = this.internalEditor.AllowExport;
                    this.itemsBinding = this.listBox.Items.Bind(this.internalEditor.Items);
                    this.listBox.SelectedIndex = this.internalEditor.SelectedIndex;
                    break;
            }
        }

        private void InternalDispose()
        {            
            if (this.internalEditor != null)
                this.internalEditor.PropertyChanged -= OnEditorPropertyChanged;
            if(this.itemsBinding != null)
                this.itemsBinding.Dispose();

            this.suscriptions.ForEach(p => p.Dispose());
            this.suscriptions.Clear();
        }
    }
}
