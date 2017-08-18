using System.Windows.Forms;
using XShell.Winform.Behaviors;

namespace XShell.Winform.Controls
{
    public partial class CollectionEditorView : UserControl
    {
        public CollectionEditorView()
        {
            InitializeComponent();

            this.addButton.ApplyFlatStyle(Properties.Resources.add);
            this.removeButton.ApplyFlatStyle(Properties.Resources.remove);
            this.cloneButton.ApplyFlatStyle(Properties.Resources.clone);
            this.moveDownButton.ApplyFlatStyle(Properties.Resources.move_up);
            this.moveDownButton.ApplyFlatStyle(Properties.Resources.move_down);
            this.clearButton.ApplyFlatStyle(Properties.Resources.clear);
        }
    }
}
