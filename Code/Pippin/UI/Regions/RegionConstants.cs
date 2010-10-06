using System.Collections.Generic;

namespace Pippin.UI.Regions
{
    public static class RegionConstants
    {
        public const string REGION_BODY = "Body";
        public const string REGION_OVERLAY = "Overlay";
        public const string REGION_FULL_OVERLAY = "FullOverlay";
        public const string REGION_MODAL_OVERLAY = "ModalOverlay";
        public const string REGION_HEADER = "Header";
        public const string REGION_FOOTER = "Footer";
        public static string Lookup(RegionName regionName)
        {
            if (regionName == RegionName.Body) return REGION_BODY;
            if (regionName == RegionName.Header) return REGION_HEADER;
            if (regionName == RegionName.Footer) return REGION_FOOTER;
            if (regionName == RegionName.Overlay) return REGION_OVERLAY;
            if (regionName == RegionName.FullOverlay) return REGION_FULL_OVERLAY;
            if (regionName == RegionName.ModalOverlay) return REGION_MODAL_OVERLAY;
            return REGION_BODY;
        }
    }
    public enum RegionName
    {
        Body, Header, Footer, Overlay, FullOverlay, ModalOverlay
    }
}