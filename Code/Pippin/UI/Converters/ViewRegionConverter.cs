using System;
using System.ComponentModel;
using Pippin.UI.Regions;

namespace Pippin.UI.Converters
{
    public class ViewRegionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType.Equals(typeof(string));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return ((string)value).AsViewRegion();
        }
    }
}
