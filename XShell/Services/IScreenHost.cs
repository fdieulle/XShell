﻿using System;

namespace XShell.Services
{
    public interface IScreenHost
    {
        event Action<IScreenHost> ScreenClosed;

        string Title { set; }

        void Close();

        void BringToFront();

        string GetPersistenceId();
    }

    public interface IPopupScreenHost : IScreenHost
    {
        RectangleSettings GetPositionAndSize();
    }
}