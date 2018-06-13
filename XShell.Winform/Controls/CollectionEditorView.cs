using System.Windows.Forms;
using XShell.Winform.Behaviors;

namespace XShell.Winform.Controls
{
    public partial class CollectionEditorView : UserControl
    {
        public CollectionEditorView()
        {
            InitializeComponent();

            addButton.ApplyFlatStyle(Properties.Resources.add);
            removeButton.ApplyFlatStyle(Properties.Resources.remove);
            cloneButton.ApplyFlatStyle(Properties.Resources.clone);
            moveDownButton.ApplyFlatStyle(Properties.Resources.move_up);
            moveDownButton.ApplyFlatStyle(Properties.Resources.move_down);
            clearButton.ApplyFlatStyle(Properties.Resources.clear);
        }
    }
}
