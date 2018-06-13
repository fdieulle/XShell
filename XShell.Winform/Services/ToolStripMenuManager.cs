using System.Windows.Forms;
using XShell.Services;
using XShell.Winform.Controls;

namespace XShell.Winform.Services
{
    public class ToolStripMenuManager : AbstractMenuManager<XToolStripMenuItem>
    {
        private readonly MenuStrip _menu;

        public ToolStripMenuManager(MenuStrip menu)
        {
            _menu = menu;
        }

        #region Overrides of AbstractMenuManager<LightToolStripMenuItem>

        protected override XToolStripMenuItem CreateMenuItem(XToolStripMenuItem parent)
        {
            var item = new XToolStripMenuItem();
            
            if (parent == null)
                _menu.Items.Add(item);
            else parent.DropDownItems.Add(item);

            return item;
        }

        protected override void DeleteMenuItem(XToolStripMenuItem parent, XToolStripMenuItem item)
        {
            if(parent == null)
                _menu.Items.Remove(item);
            else parent.DropDownItems.Remove(item);
        }

        #endregion
    }
}
