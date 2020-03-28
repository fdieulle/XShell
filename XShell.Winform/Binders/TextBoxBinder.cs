using System;
using System.Windows.Forms;

namespace XShell.Winform.Binders
{
    public class TextBoxBinder : AbstractTwoWayBinder<TextBox, string>
    {
        public TextBoxBinder(TextBox control, object dataContext, string propertyName, bool allowDragDrop = false) 
            : base(control, dataContext, propertyName)
        {
            Control.TextChanged += OnControlTextChanged;

            if (allowDragDrop)
            {
                Control.AllowDrop = true;

                Control.DragEnter += OnControlDragEnter;
                Control.DragOver += OnControlDragOver;
                Control.DragDrop += OnControlDragDrop;
            }
        }

        private void OnControlDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void OnControlDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void OnControlDragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (files == null) return;

            var text = string.Join(Environment.NewLine, files);
            Control.Text = string.IsNullOrEmpty(Control.Text) 
                ? text : $@"{Control.Text}{Environment.NewLine}{text}";
        }

        private void OnControlTextChanged(object sender, EventArgs e) => UpdateDataContext(Control.Text);

        #region Overrides of AbstractOneWayBinder<TextBox,string>

        protected override void UpdateControl(string value) => Control.Text = value;

        protected override void Disposing()
        {
            base.Disposing();

            Control.DragEnter -= OnControlDragEnter;
            Control.DragOver -= OnControlDragOver;
            Control.DragDrop -= OnControlDragDrop;
        }

        #endregion
    }
}
