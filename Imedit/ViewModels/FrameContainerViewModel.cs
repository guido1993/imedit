using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace Imedit.ViewModels
{
    public class FrameContainerViewModel : BaseViewModel
    {
        [PreferredConstructor]
        public FrameContainerViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}