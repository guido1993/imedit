using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Imedit.Models;

namespace Imedit.Converters
{
    public class FilterToVisibilityConverter : DependencyObject, IValueConverter
    {        
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var filter = (FilterEnum)value;
            var param = (FilterEnum)Enum.Parse(typeof(FilterEnum), parameter.ToString());

            if (filter == FilterEnum.None)
                return Visibility.Collapsed;

            if (filter == param)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
