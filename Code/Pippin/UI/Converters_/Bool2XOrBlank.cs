using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Odin.UI.Infrastructure.Converters
{
    public class Bool2XOrBlank : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                if (parameter != null)
                    return parameter;
                return new Ellipse { Width = 9, Height = 9, Fill = new SolidColorBrush(Colors.Black) };
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
