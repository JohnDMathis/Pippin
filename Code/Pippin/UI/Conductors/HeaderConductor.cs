using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Pippin.UI.Regions;
using Pippin.UI.Screens;
using Pippin.UI.VisibilityServices;

namespace Pippin.UI.Conductors
{
    public class HeaderConductor:ScreenConductor
    {
        public HeaderConductor(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry, IEventAggregator eventAggregator, IRegionManager regionManager, IVisibilityService visibilityService)
            : base(container, screenFactoryRegistry, eventAggregator, regionManager, visibilityService)
        {
            MyRegionName = RegionName.Header;
            Region = this.RegionManager.Regions[RegionConstants.REGION_HEADER];
        }
    }
}