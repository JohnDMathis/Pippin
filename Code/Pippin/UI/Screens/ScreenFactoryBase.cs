using Microsoft.Practices.Unity;

namespace Pippin.UI.Screens
{
    public abstract class ScreenFactoryBase : IScreenFactory
    {
        protected IUnityContainer Container { get; set; }
        protected IScreen Screen { get; set; }

        public abstract IScreen CreateScreen(object screenSubject);

        protected ScreenFactoryBase(IUnityContainer container)
        {
            this.Container = container;
        }
    }
}