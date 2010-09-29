using Microsoft.Practices.Unity;

namespace Odin.UI.Infrastructure.ScreenFramework
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