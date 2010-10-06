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

        public abstract bool CanEnter();
        public abstract bool CanLeave();
        public abstract void Setup();
        public abstract void LeaveCanceled();
        public abstract void Leaving();
        public abstract UserControl View { get; set; }
        public abstract object Subject { get; set; }

        #endregion
    }
}