using System;

namespace Odin.UI.Infrastructure.ScreenFramework
{
    public interface IScreenFactoryRegistry
    {
        IScreenFactory Get(ScreenKeyType screenType);
        void Register(ScreenKeyType screenType, Type screenFactory);
        bool HasFactory(ScreenKeyType screenType);
    }
}