using System;

namespace Pippin.UI.Screens
{
    public interface IScreenFactoryRegistry
    {
        IScreenFactory Get(string screenName);
        void Register(string screenName, Type screenFactory);
        bool HasFactory(string screenName);
    }
}