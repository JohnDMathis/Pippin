using System;
using System.Globalization;
using System.Windows.Data;

namespace Odin.UI.Infrastructure.Converters
{
    public class Bool2HyperlinkButtonStyleColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "#FF204586" : "#FFFF0000";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
