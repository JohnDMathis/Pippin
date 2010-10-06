using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Pippin.UI.Regions;
using Pippin.UI.Screens;
using Pippin.UI.VisibilityServices;

namespace Pippin.UI.Conductors
{
    public class FooterConductor : ScreenConductor
    {
        public FooterConductor(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry, IEventAggregator eventAggregator, IRegionManager regionManager, IVisibilityService visibilityService)
            : base(container, screenFactoryRegistry, eventAggregator, regionManager, visibilityService)
        {
            MyRegionName = RegionName.Footer;
            Region = this.RegionManager.Regions[RegionConstants.REGION_FOOTER];
        }
    }
}