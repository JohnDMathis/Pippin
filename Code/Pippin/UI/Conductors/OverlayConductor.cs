using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Pippin.UI.Regions;
using Pippin.UI.Screens;
using Pippin.UI.VisibilityServices;
using Pippin.UI.Events;

namespace Pippin.UI.Conductors
{
    public class OverlayConductor:ScreenConductor
    {
        public OverlayConductor(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry, IEventAggregator eventAggregator, IRegionManager regionManager, IVisibilityService visibilityService) : base(container, screenFactoryRegistry, eventAggregator, regionManager, visibilityService)
        {
            MyRegionName = RegionName.Overlay;
            Region = RegionManager.Regions[RegionConstants.REGION_OVERLAY];
        }
        //protected override void HandleScreenEvent(ScreenEventArgs args)
        //{
        //    if (args.RegionName != MyRegionName) return;
        //    base.HandleScreenEvent(args);
        //    if (args.Event == ScreenEventType.Activate && args.ScreenName == this._activeScreenName)
        //    {
        //        // activate was successful; now need to enable the controls in the overlay.
        //        _eventManager.SendOverlayStateChanged(true);
        //    }
        //    else if (_activeScreenName == null) { }
        //    else if (args.Event == ScreenEventType.Deactivate)
        //        _eventManager.SendOverlayStateChanged(false);
               
        //}
        protected override void ScreenActivated(string screenName)
        {
            _eventManager.SendOverlayStateChanged(true);
        }
        protected override void ScreenDeactivated(string screenName)
        {
            _eventManager.SendOverlayStateChanged(false);
        }
    }
}