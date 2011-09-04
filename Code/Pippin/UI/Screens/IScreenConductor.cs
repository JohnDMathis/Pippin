using System;
namespace Pippin.UI.Screens
{
    public interface IScreenConductor
    {
        void DeactivateScreen();
        void DeactivateScreen(IScreen screen);
        void DeactivateScreen(string screenName);
    }
}
