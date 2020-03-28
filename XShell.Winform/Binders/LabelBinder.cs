using System.Windows.Forms;

namespace XShell.Winform.Binders
{
    public class LabelBinder : AbstractOneWayBinder<Label, string>
    {
        public LabelBinder(Label control, object dataContext, string propertyName) 
            : base(control, dataContext, propertyName) { }

        protected override void UpdateControl(string value) => Control.Text = value;
    }
}
