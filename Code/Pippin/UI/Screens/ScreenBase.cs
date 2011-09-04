using System.Windows.Controls;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Pippin.UI.ViewModel;

namespace Pippin.UI.Screens
{
    public abstract class ScreenBase : IScreen
    {
        protected IUnityContainer Container { get; set; }

        protected ScreenBase(IUnityContainer container)
        {
            this.Container = container;
        }

        #region Implementation of IScreen

        public virtual bool CanEnter()
        {
            return ViewModel.CanEnter();
        }

        public virtual bool CanLeave()
        {
            return ViewModel.CanLeave();
        }

        public abstract void Setup();
        public abstract void LeaveCanceled();
        public abstract void Leaving();
        public virtual UserControl View { get; set; }
        public virtual object Subject { get; set; }
        public virtual string Name { get; set; }
        public virtual ViewModelBase ViewModel { get; set; }
        public virtual IScreenConductor Conductor { get; set; }
        public virtual void Activated(IScreenConductor controller, string name)
        {
            this.Conductor = controller;
            this.Name = name;
        }
        public virtual void Deactivate()
        {
            if (Conductor != null)
                Conductor.DeactivateScreen(this);
        }
        #endregion
    }
}