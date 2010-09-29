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
using System.ComponentModel.Composition;

namespace Pippin.UI.Regions
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TargetRegionAttribute : ExportAttribute
    {
        public TargetRegionAttribute()
            : base(typeof(UserControl))
        {

        }

        public ViewRegion Region { get; set; }
    }
}
