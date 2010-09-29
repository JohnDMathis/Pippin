using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Pippin.UI.Regions
{
    public enum ViewRegion
    {
        Header,
        Main,
        Sub
    }

    public static class ViewRegionExtensions
    {
        private static readonly List<ViewRegion> _regions = new List<ViewRegion>{
            ViewRegion.Header, ViewRegion.Main, ViewRegion.Sub};

        public static ViewRegion AsViewRegion(this string regionName)
        {
            foreach (var region in _regions)
                if (regionName.Equals(region.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    return region;
            return ViewRegion.Main;
        }
    }
}
