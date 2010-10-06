using System;
using System.Windows.Controls;

namespace Pippin.UI.VisibilityServices
{
    public interface IVisibilityService
    {
        void EnterViewAnimation(UserControl view);
        void LeaveViewAnimation(UserControl view, Action onLeaveComplete);
    }
}