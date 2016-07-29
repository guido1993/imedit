using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Media.Capture;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Imedit.Models;

namespace Imedit.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IPhotoProvider _photoProvider;

        private ObservableCollection<Photo> _photos;
        public ObservableCollection<Photo> Photos
        {
            get { return _photos; }
            set { this.Set(nameof(Photos), ref _photos, value); }
        }

        private bool _isCameraPresent = false;
        public bool IsCameraPresent
        {
            get { return _isCameraPresent; }
            set { this.Set(nameof(IsCameraPresent), ref _isCameraPresent, value); }
        }

        public RelayCommand TakePhotoCommand { get; set; }

        private RelayCommand<Photo> _goToEditPageCommand;
        public RelayCommand<Photo> GoToEditPageCommand
        {
            get
            {
                if (_goToEditPageCommand == null)
                    _goToEditPageCommand = new RelayCommand<Photo>(photo => _navigationService.NavigateTo(typeof(EditPageViewModel).FullName, photo));

                return _goToEditPageCommand;
            }
        }

        #endregion


        public MainPageViewModel()
        {
            if (!IsInDesignModeStatic)
                throw new System.Exception("You can't call this at runtime");
        }

        [PreferredConstructor]
        public MainPageViewModel(INavigationService navigationService, IPhotoProvider photoProvider) : base (navigationService)
        {
            if (navigationService == null)
                throw new NullReferenceException("navigationService is null inside MainPageViewModel ctor");
            if (photoProvider == null)
                throw new NullReferenceException("photoProvider is null inside MainPageViewModel ctor");

            _navigationService = navigationService;
            _photoProvider = photoProvider;
           
            TakePhotoCommand = new RelayCommand(async () => await TakePhoto());
        }

        private ObservableCollection<PhotoGroup> _photoGroups;
        public ObservableCollection<PhotoGroup> PhotoGroups
        {
            get { return _photoGroups; }
            set { this.Set(nameof(PhotoGroups), ref _photoGroups, value); }
        }

        public async Task Init()
        {
            IsLoading = true;

            try
            {
                PhotoGroups = await _photoProvider.LoadPhotos();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task TakePhoto()
        {
            if (!IsCameraPresent)
                return;

            CameraCaptureUI cameraUI = new CameraCaptureUI();

            cameraUI.PhotoSettings.AllowCropping = false;
            cameraUI.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;
            var file = await cameraUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
        }   
    }
}