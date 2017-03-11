using System;

namespace XShell
{
    public interface IScreenContainer
    {
        void Register(Type idType, Type viewType, Type logicType);
    }
}