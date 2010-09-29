using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Odin.UI.Infrastructure.Converters
{
    public class Date2Color : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Warning_red = Application.Current.Resources["Warning_red"] as SolidColorBrush;
            var Warning_yellow = Application.Current.Resources["Warning_yellow"] as SolidColorBrush;
            DateTime now = DateTime.Now;
            var brush = new SolidColorBrush(Colors.Transparent);
            if ((now - (DateTime)value).Days > 20)
                brush = Warning_red;
            else if ((now - (DateTime)value).Days > 15)
                brush = Warning_yellow;
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
