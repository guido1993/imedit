using System;
using Windows.UI.Xaml.Data;
using Imedit.Helpers;

namespace Imedit.Converters
{
    public class TranslatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var s = parameter.ToString();
            var converted = ResourceHelper.GetTranslation(s);

            return converted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}