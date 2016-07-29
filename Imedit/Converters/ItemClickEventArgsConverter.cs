using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Imedit.Models;

namespace Imedit.Converters
{
    public class ItemClickEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = value as ItemClickEventArgs;

            if (args == null)
                throw new ArgumentException("Value is not ItemClickEventArgs");

            if (args.ClickedItem is Photo)
            {
                var selectedItem = args.ClickedItem as Photo;
                return selectedItem;
            }

            if (args.ClickedItem is FilterEnum)
            {
                var selectedItem = args.ClickedItem as FilterEnum?;
                return selectedItem.Value;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
