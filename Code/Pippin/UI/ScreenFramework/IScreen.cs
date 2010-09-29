using System.Windows.Controls;

namespace Odin.UI.Infrastructure.ScreenFramework
{
    public interface IScreen
    {
        bool CanEnter();
        bool CanLeave();
        void Setup();
        void LeaveCanceled();
        void Leaving();
        UserControl View { get; set; }
        object Subject { get; set; }
    }
}