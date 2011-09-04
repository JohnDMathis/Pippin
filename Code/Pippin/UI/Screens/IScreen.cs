using System.Windows.Controls;
using Pippin.UI.ViewModel;
using Pippin.UI.Regions;
using Microsoft.Practices.Prism.Regions;

namespace Pippin.UI.Screens
{
    public interface IScreen
    {
        bool CanEnter();
        bool CanLeave();
        void Setup();
        void LeaveCanceled();
        void Leaving();
        void Activated(IScreenConductor controller, string name);
        void Deactivate();
        UserControl View { get; set; }
        ViewModelBase ViewModel { get; set; }
        object Subject { get; set; }
        string Name { get; set; }
        IScreenConductor Conductor { get; set; }
    }
}