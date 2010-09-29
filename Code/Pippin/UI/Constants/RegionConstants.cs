using System.Collections.Generic;

namespace Odin.UI.Infrastructure.Constants
{
    public static class RegionConstants
    {
        public const string REGION_MAIN_AREA = "MainRegion";
        public const string REGION_BUSY = "BusyRegion";
        public const string REGION_OVERLAY = "OverlayRegion";
        public const string REGION_HEADER = "HeaderRegion";
        public const string REGION_FOOTER = "FooterRegion";
        public static string Lookup(RegionName regionName)
        {
            if (regionName == RegionName.Overlay) return REGION_OVERLAY;
            if (regionName == RegionName.Header) return REGION_HEADER;
            if (regionName == RegionName.Footer) return REGION_FOOTER;
            if (regionName == RegionName.Busy) return REGION_BUSY;
            return REGION_MAIN_AREA;
        }
    }
    public enum RegionName
    {
        Main, Overlay, Header, Footer, Busy
    }
}
