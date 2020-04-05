using System;
using System.IO;
using System.Text;
using XShell.Services;

namespace XShell
{
    public interface IMainWindow<in TScreen> 
        where TScreen : IScreenHost
    {
        event Action Terminating;

        RectangleSettings GetPositionAndSize();
        void SetPositionAndSize(RectangleSettings rectangle);

        void SaveWorkspace(Stream stream, Encoding encoding, bool leaveOpen);

        void LoadWorkspace(Stream stream, Func<string, TScreen> createContent, bool leaveOpen);
    }
}
