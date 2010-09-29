using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Odin.UI.Infrastructure.Converters
{
    public class Bool2SolidColorBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Red);
            if ((bool)value)
                brush = new SolidColorBrush(Colors.Green);
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
