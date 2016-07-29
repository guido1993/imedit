using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Imedit.Extensions;
using Imedit.Helpers;
using Imedit.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace Imedit.Views
{
    public sealed partial class FrameContainer : UserControl
    {
        public Frame InnerFrame { get { return frame; } }
        private long _token;

        public FrameContainer()
        {
            this.InitializeComponent();
            
            Loaded += (s, e) =>
            {
                _token = frame.RegisterPropertyChangedCallback(Frame.CanGoBackProperty, Callback);
                var mainColor = ((SolidColorBrush)Application.Current.Resources["PageHeaderDefaultBrush"]).Color.ToString();

                SystemNavigationManager.GetForCurrentView().BackRequested += (sender, args) =>
                {
                    if (frame.CanGoBack)
                    {
                        args.Handled = true;
                        frame.GoBack();
                        TitleBarManager.SetColor(mainColor, true);
                    }
                };

                TitleBarManager.SetColor(mainColor, true);

                if (Platform.GetCurrent() == Platform.PlatformEnum.Mobile)
                    DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait | DisplayOrientations.PortraitFlipped;
                
                this.DataContext = ServiceLocator.Current.GetInstance<FrameContainerViewModel>();
            };

            Unloaded += (s, e) =>
            {
                frame.UnregisterPropertyChangedCallback(Frame.CanGoBackProperty, _token);
            };
        }

        private void Callback(DependencyObject sender, DependencyProperty dp)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = frame.CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }
    }
}