using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;

namespace Imedit.Extensions
{
    public static class TitleBarManager
    {
        public static void SetColor(string value, bool isColor = false)
        {
            SolidColorBrush background = new SolidColorBrush(value.ToColor());
            SolidColorBrush foreground = new SolidColorBrush(Colors.Black);

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = titleBar.ButtonBackgroundColor = background.Color;
            titleBar.ForegroundColor = titleBar.ButtonForegroundColor = foreground.Color;

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();

                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = background.Color;
                    statusBar.ForegroundColor = foreground.Color;
                }
            }
        }

        public static Color ToColor(this string htmlColor)
        {
            htmlColor = htmlColor.Replace("#", "");
            byte a = 0xff, r = 0, g = 0, b = 0;

            switch (htmlColor.Length)
            {
                case 3:
                    r = byte.Parse(htmlColor.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(htmlColor.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(htmlColor.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                    break;
                case 4:
                    a = byte.Parse(htmlColor.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                    r = byte.Parse(htmlColor.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(htmlColor.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(htmlColor.Substring(3, 1), System.Globalization.NumberStyles.HexNumber);
                    break;
                case 6:
                    r = byte.Parse(htmlColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(htmlColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(htmlColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    break;
                case 8:
                    a = byte.Parse(htmlColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    r = byte.Parse(htmlColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(htmlColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(htmlColor.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                    break;
            }

            return Color.FromArgb(a, r, g, b);
        }
    }
}
