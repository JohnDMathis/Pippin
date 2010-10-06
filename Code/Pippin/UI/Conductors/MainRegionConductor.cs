using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Pippin.UI.Regions;
using Pippin.UI.Screens;
using Pippin.UI.VisibilityServices;


namespace Pippin.UI.Conductors
{
    public class MainRegionConductor : ScreenConductor
    {
        public MainRegionConductor(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry, IEventAggregator eventAggregator, IRegionManager regionManager, IVisibilityService visibilityService) : base(container, screenFactoryRegistry, eventAggregator, regionManager, visibilityService)
        {
            MyRegionName = RegionName.Body;
            Region = this.RegionManager.Regions[RegionConstants.REGION_BODY];
        }
    }
}