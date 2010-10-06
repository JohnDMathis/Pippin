using Pippin.UI.Regions;

namespace Pippin.UI.Screens
{
    public class ScreenEventArgs
    {
        public ScreenEventArgs()
        {
            // set defaults
            Event = ScreenEventType.Activate;
            RegionName = RegionName.Body;
            UseAnimation = true;
        }
        public object ScreenSubject { get; set; }
        public string ScreenName { get; set; }
        public RegionName RegionName { get; set; }
        public ScreenEventType Event { get; set; }
        public bool UseAnimation { get; set; }
       
    }
}