using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Odin.UI.Infrastructure.Converters
{
    public class Bool2RedBlack : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Red);
            if ((bool)value)
                brush = new SolidColorBrush(Colors.Black);
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = (SolidColorBrush)value;
            if (brush.Color == Colors.Black)
                return true;
            return false;
        }
    }
}
