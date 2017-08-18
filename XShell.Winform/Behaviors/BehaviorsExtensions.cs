using System.Drawing;
using System.Windows.Forms;

namespace XShell.Winform.Behaviors
{
    public static class BehaviorsExtensions
    {
        public static void ApplyFlatStyle(this Button button, Image image = null)
        {
            if (button == null) return;

            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = true;
            button.FlatAppearance.MouseDownBackColor = SystemColors.HotTrack;
            button.FlatAppearance.MouseOverBackColor = SystemColors.ControlLight;

            if (image != null)
            {
                button.Text = null;
                button.BackgroundImage = image;
                button.BackgroundImageLayout = ImageLayout.Stretch;
                button.FlatAppearance.BorderSize = 0;
            }
        }
    }
}
