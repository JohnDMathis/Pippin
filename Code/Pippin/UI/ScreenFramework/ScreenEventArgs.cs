using Odin.UI.Infrastructure.Constants;

namespace Odin.UI.Infrastructure.ScreenFramework
{
    public class ScreenEventArgs
    {
        public ScreenEventArgs()
        {
            // set defaults
            Event = ScreenEventType.Activate;
            RegionName = RegionName.Main;
            UseAnimation = true;
        }
        public object ScreenSubject { get; set; }
        public ScreenKeyType ScreenKey { get; set; }
        public RegionName RegionName { get; set; }
        public ScreenEventType Event { get; set; }
        public bool UseAnimation { get; set; }
       
    }
}