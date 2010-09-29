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
using Pippin.UI.Regions;
using Pippin.UI.Converters;
using System.ComponentModel;

namespace Pippin.UI.Regions
{
    public static class RegionDef
    {
        private static readonly Dictionary<ViewRegion, Panel> _regions = new Dictionary<ViewRegion, Panel>();

        public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached(
            "Name",
            typeof(ViewRegion),
            typeof(RegionDef),
            new PropertyMetadata(ViewRegion.Main, null));

        public static ViewRegion GetName(DependencyObject obj)
        {
            return (ViewRegion)obj.GetValue(NameProperty);
        }

        [TypeConverter(typeof(ViewRegionConverter))]
        public static void SetName(DependencyObject obj, ViewRegion value)
        {
            obj.SetValue(NameProperty, value);
            var panel = obj as Panel;
            if (panel != null)
            {
                _regions.Add(value, panel);
            }
        }

        public static Panel GetPanelForRegion(ViewRegion region)
        {
            return _regions[region];
        }

    }
}
