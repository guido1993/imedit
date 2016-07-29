using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Imedit.Models;
using Imedit.Views;
using Microsoft.Practices.ServiceLocation;

namespace Imedit.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigationService>(() => CreateNavigationService());

            SimpleIoc.Default.Register<IPhotoProvider, PhotoProviderReal>();
            SimpleIoc.Default.Register<FrameContainerViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<EditPageViewModel>();
        }

        private static INavigationService CreateNavigationService()
        {
            var navigationService = new Helpers.NavigationService();
            
            navigationService.Configure(typeof(MainPageViewModel).FullName, typeof(MainPage));
            navigationService.Configure(typeof(EditPageViewModel).FullName, typeof(EditPage));

            return navigationService;
        }

        public MainPageViewModel MainPage
        {
            get { return ServiceLocator.Current.GetInstance<MainPageViewModel>(); }
        }

        public EditPageViewModel EditPage
        {
            get { return ServiceLocator.Current.GetInstance<EditPageViewModel>(); }
        }
    }
}