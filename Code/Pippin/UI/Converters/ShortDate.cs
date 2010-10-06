using System;
using System.Globalization;
using System.Windows.Data;

namespace Pippin.UI.Converters
{
    public class ShortDate:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dt = (DateTime) value;
            return dt.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
  
    }
}