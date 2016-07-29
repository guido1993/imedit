using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Imedit.Converters
{
    public class BooleanToVisibilityConverter : DependencyObject, IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                var isVisible = (bool)value;
                return isVisible ? Visibility.Visible : Visibility.Collapsed;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}