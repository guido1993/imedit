using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Imedit.Converters
{
    public class NegateBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                var isVisible = (bool)value;
                return isVisible ? Visibility.Collapsed : Visibility.Visible;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}