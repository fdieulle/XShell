using System.Windows.Controls;
using XShell.Services;
using XShell.Wpf.Controls;

namespace XShell.Wpf.Services.Shell
{
    public class DefaultMenuManager : AbstractMenuManager<XMenuItem>
    {
        private readonly Menu menu;

        public DefaultMenuManager(Menu menu)
        {
            this.menu = menu;
        }

        #region Overrides of AbstractMenuManager<XMenuItem>

        protected override XMenuItem CreateMenuItem(XMenuItem parent)
        {
            var item = new XMenuItem();
            
            if (parent != null)
                parent.Items.Add(item);
            else menu.Items.Add(item);

            return item;
        }

        protected override void DeleteMenuItem(XMenuItem parent, XMenuItem item)
        {
            if (parent != null)
                parent.Items.Remove(item);
            else menu.Items.Remove(item);
        }

        #endregion
    }
}
