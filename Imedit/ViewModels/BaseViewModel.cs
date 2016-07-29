using Windows.System.Profile;
using Windows.UI.ViewManagement;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace Imedit.ViewModels
{
    public class BaseViewModel : ViewModelBase
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { this.Set(nameof(IsLoading), ref _isLoading, value); }
        }
        
        public bool IsContinuum { get { return ProjectionManager.ProjectionDisplayAvailable || AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop"; } }

        public BaseViewModel()
        {
            if (!IsInDesignModeStatic)
                throw new System.Exception("You can't call this at runtime");
        }

        [PreferredConstructor]
        public BaseViewModel(INavigationService navigationService)
        {

        }
    }
}