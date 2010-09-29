using System;
using System.Windows.Controls;

namespace Odin.UI.Infrastructure.VisibilityService
{
    public interface IVisibilityService
    {
        void EnterViewAnimation(UserControl view);
        void LeaveViewAnimation(UserControl view, Action onLeaveComplete);
    }
}