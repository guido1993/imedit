using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Imedit.Views;

namespace Imedit
{
    sealed partial class App : Application
    {
        FrameContainer _rootFrame;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }
        
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            _rootFrame = Window.Current.Content as FrameContainer;

            if (_rootFrame == null)
            {
                _rootFrame = new FrameContainer();
                _rootFrame.InnerFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = _rootFrame;
            }

            if (string.IsNullOrEmpty(e.Arguments))
                _rootFrame.InnerFrame.Navigate(typeof(MainPage), e.Arguments);

            Window.Current.Activate();
        }
        
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
