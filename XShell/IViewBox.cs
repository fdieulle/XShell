using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XShell
{
    public enum ViewBoxResult
    {
        None,
        Ok,
        Cancel,
        No,
        Yes,
    }

    public enum ViewboxButtons
    {
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel
    }

    public enum ViewboxImage
    {
        None,
        Asterisk,
        Error,
        Exclamation,
        Warning,
        Information,
        Hand,
        Question,
        Stop
    }

    public interface IViewBox
    {
        ViewBoxResult Show(string text, string caption = null, ViewboxButtons buttons = ViewboxButtons.Ok, ViewboxImage image = ViewboxImage.None);
    }
}
