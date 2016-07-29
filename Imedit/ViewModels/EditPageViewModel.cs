using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Imedit.Extensions;
using Imedit.Helpers;
using Imedit.Models;
using WinRTXamlToolkit.Imaging;

namespace Imedit.ViewModels
{
    public class EditPageViewModel : BaseViewModel
    {
        private bool _isColorPickerEnabled;
        public bool IsColorPickerEnabled
        {
            get { return _isColorPickerEnabled; }
            set
            {
                this.Set(nameof(IsColorPickerEnabled), ref _isColorPickerEnabled, value);
            }
        }

        private bool _selectionEnabled;
        public bool SelectionEnabled
        {
            get
            {
                return _selectionEnabled;
            }
            set
            {
                this.Set(nameof(SelectionEnabled), ref _selectionEnabled, value);

                if (value)
                {
                    IsInkCanvasEnabled = false;
                    IsTextPanelEnabled = false;
                    FullEditorTitle = ResourceHelper.GetTranslation("PickArea");
                    ShowFilters = false;
                    IsLogoVisible = false;
                    IsFloodFillEnabled = false;
                    IsEyeDropperEnabled = false;
                    IsWelcomeEnabled = false;
                    IsColorPickerEnabled = true;
                }
            }
        }

        private bool _isFloodFillEnabled;
        public bool IsFloodFillEnabled
        {
            get { return _isFloodFillEnabled; }
            set
            {
                this.Set(nameof(IsFloodFillEnabled), ref _isFloodFillEnabled, value);

                if (value)
                {
                    FullEditorTitle = ResourceHelper.GetTranslation("FillArea");
                    ShowFilters = false;
                    SelectionEnabled = false;
                    IsInkCanvasEnabled = false;
                    IsTextPanelEnabled = false;
                    IsEyeDropperEnabled = false;
                    IsLogoVisible = false;
                    IsWelcomeEnabled = false;
                    IsColorPickerEnabled = true;
                }
            }
        }

        private bool _isFirstText;

        public bool IsFirstText
        {
            get { return _isFirstText; }
            set { this.Set(nameof(IsFirstText), ref _isFirstText, value); }
        }

        private bool _isTextPanelEnabled;
        public bool IsTextPanelEnabled
        {
            get { return _isTextPanelEnabled; }
            set
            {
                this.Set(nameof(IsTextPanelEnabled), ref _isTextPanelEnabled, value);

                if (value)
                {
                    ShowFilters = false;
                    IsLogoVisible = false;
                    IsInkCanvasEnabled = false;
                    FullEditorTitle = ResourceHelper.GetTranslation("WriteText");
                    SelectionEnabled = false;
                    IsFirstText = false;
                    IsFloodFillEnabled = false;
                    IsEyeDropperEnabled = false;
                    IsWelcomeEnabled = false;
                    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
                    IsColorPickerEnabled = true;
                }
            }
        }

        private bool _isEyeDropperEnabled;
        public bool IsEyeDropperEnabled
        {
            get { return _isEyeDropperEnabled; }
            set
            {
                this.Set(nameof(IsEyeDropperEnabled), ref _isEyeDropperEnabled, value);

                if (value)
                {
                    IsInkCanvasEnabled = false;
                    IsFloodFillEnabled = false;
                    SelectionEnabled = false;
                    IsTextPanelEnabled = false;
                    ShowFilters = false;
                    FullEditorTitle = ResourceHelper.GetTranslation("PickColor");
                    IsLogoVisible = false;
                    IsWelcomeEnabled = false;
                    IsColorPickerEnabled = true;
                }
            }
        }

        private bool _isInkCanvasEnabled;
        public bool IsInkCanvasEnabled
        {
            get { return _isInkCanvasEnabled; }
            set
            {
                this.Set(nameof(IsInkCanvasEnabled), ref _isInkCanvasEnabled, value);

                if (value)
                {
                    SelectionEnabled = false;
                    IsTextPanelEnabled = false;
                    IsFloodFillEnabled = false;
                    IsEyeDropperEnabled = false;
                    ShowFilters = false;
                    FullEditorTitle = ResourceHelper.GetTranslation("DrawLines");
                    IsLogoVisible = false;
                    IsWelcomeEnabled = false;
                    IsColorPickerEnabled = true;
                }

            }
        }

        private bool _showFilters;
        public bool ShowFilters
        {
            get { return _showFilters; }
            set
            {
                this.Set(nameof(ShowFilters), ref _showFilters, value);

                if (value)
                {
                    SelectionEnabled = false;
                    IsInkCanvasEnabled = false;
                    IsColorPickerEnabled = false;
                    IsTextPanelEnabled = false;
                    FullEditorTitle = ResourceHelper.GetTranslation("ChooseFilter");
                    IsFloodFillEnabled = false;
                    IsEyeDropperEnabled = false;
                    IsLogoVisible = false;
                    IsWelcomeEnabled = false;
                }
            }
        }

        private bool _isLogoVisible = true;
        public bool IsLogoVisible
        {
            get { return _isLogoVisible; }
            set { this.Set(nameof(IsLogoVisible), ref _isLogoVisible, value); }
        }

        private bool _isWelcomeEnabled = true;
        public bool IsWelcomeEnabled
        {
            get { return _isWelcomeEnabled; }
            set { this.Set(nameof(IsWelcomeEnabled), ref _isWelcomeEnabled, value); }
        }


        private SolidColorBrush _color;
        public SolidColorBrush Color
        {
            get { return _color; }
            set { this.Set(nameof(Color), ref _color, value); }
        }

        private double _strokeSize = 8;
        public double StrokeSize
        {
            get { return _strokeSize; }
            set { this.Set(nameof(StrokeSize), ref _strokeSize, value); }
        }
        
        private RelayCommand<double> _selectedStrokeSizeChangedCommand;
        public RelayCommand<double> SelectedStrokeSizeChangedCommand
        {
            get
            {
                if (_selectedStrokeSizeChangedCommand == null)
                    _selectedStrokeSizeChangedCommand = new RelayCommand<double>(value => this.StrokeSize = value);
                return _selectedStrokeSizeChangedCommand;
            }
        }

        private bool _canSave;
        public bool CanSave
        {
            get { return _canSave; }
            set { this.Set(nameof(CanSave), ref _canSave, value); }
        }

        private bool _isSaving;
        public bool IsSaving
        {
            get { return _isSaving; }
            set { this.Set(nameof(IsSaving), ref _isSaving, value); }
        }

        private WriteableBitmap _image;
        public WriteableBitmap Image
        {
            get { return _image; }
            set { this.Set(nameof(Image), ref _image, value); }
        }

        private ObservableCollection<FilterEnum> _filters;
        public ObservableCollection<FilterEnum> Filters
        {
            get { return _filters; }
            set { this.Set(nameof(Filters), ref _filters, value); }
        }

        private FilterEnum _selectedFilter;
        public FilterEnum SelectedFilter
        {
            get { return _selectedFilter; }
            set { this.Set(nameof(SelectedFilter), ref _selectedFilter, value); }
        }
        
        private int _x1;
        public int X1
        {
            get { return _x1; }
            set { this.Set(nameof(X1), ref _x1, value); }
        }

        private int _y1;
        public int Y1
        {
            get { return _y1; }
            set { this.Set(nameof(Y1), ref _y1, value); }
        }

        private double _x2;
        public double X2
        {
            get { return _x2; }
            set { this.Set(nameof(X2), ref _x2, value); }
        }

        private double _y2;
        public double Y2
        {
            get { return _y2; }
            set { this.Set(nameof(Y2), ref _y2, value); }
        }

        private double _currentImageHeight;
        public double CurrentImageHeight
        {
            get { return _currentImageHeight; }
            set { this.Set(nameof(CurrentImageHeight), ref _currentImageHeight, value); }
        }

        private double _currentImageWidth;
        public double CurrentImageWidth
        {
            get { return _currentImageWidth; }
            set { this.Set(nameof(CurrentImageWidth), ref _currentImageWidth, value); }
        }

        private string _fullEditorTitle;
        public string FullEditorTitle
        {
            get { return _fullEditorTitle; }
            set { this.Set(nameof(FullEditorTitle), ref _fullEditorTitle, value); }
        }

        private double _textSize = 15;
        public double TextSize
        {
            get { return _textSize; }
            set { this.Set(nameof(TextSize), ref _textSize, value); }
        }

        private RelayCommand<double> _changedTextSizeCommand;
        public RelayCommand<double> ChangedTextSizeCommand
        {
            get
            {
                if (_changedTextSizeCommand == null)
                    _changedTextSizeCommand = new RelayCommand<double>(value => this.TextSize = value);

                return _changedTextSizeCommand;
            }
        }

        private bool _textIsBold;
        public bool TextIsBold
        {
            get { return _textIsBold; }
            set { this.Set(nameof(TextIsBold), ref _textIsBold, value); }
        }

        private RelayCommand<bool> _textIsBoldCommand;
        public RelayCommand<bool> TextIsBoldCommand
        {
            get
            {
                if (_textIsBoldCommand == null)
                    _textIsBoldCommand = new RelayCommand<bool>(value => this.TextIsBold = value);

                return _textIsBoldCommand;
            }
        }

        private bool _textIsItalic;
        public bool TextIsItalic
        {
            get { return _textIsItalic; }
            set { this.Set(nameof(TextIsItalic), ref _textIsItalic, value); }
        }

        private RelayCommand<bool> _textIsItalicCommand;
        public RelayCommand<bool> TextIsItalicCommand
        {
            get
            {
                if (_textIsItalicCommand == null)
                    _textIsItalicCommand = new RelayCommand<bool>(value => this.TextIsItalic = value);

                return _textIsItalicCommand;
            }
        }

        [PreferredConstructor]
        public EditPageViewModel(INavigationService navigationService) : base (navigationService)
        {
            SelectedFilter = FilterEnum.None;
            FullEditorTitle = "";
            //ShowFilters = true;

            Window.Current.SizeChanged += SizeChanged;
        }
        
        public async Task Init(Photo image)
        {
            Color = new SolidColorBrush(Colors.Black);
            
            StorageFile file = await GetImage(image); //(StorageFile) await KnownFolders.PicturesLibrary.TryGetItemAsync(photoName);
            var stream = await file.OpenReadAsync();
            var decoder = await BitmapDecoder.CreateAsync(stream);
            var pixels = await decoder.GetPixelDataAsync(
                BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight,
                new BitmapTransform(), ExifOrientationMode.RespectExifOrientation,
                ColorManagementMode.DoNotColorManage);

            var bytes = pixels.DetachPixelData();
            Image = BitmapFactory.New((int)decoder.OrientedPixelWidth, (int)decoder.OrientedPixelHeight).FromByteArray(bytes);

            Filters = Enum.GetValues(typeof(FilterEnum)).Cast<FilterEnum>()
                                                        .Where(x => x != FilterEnum.None)
                                                        .ToObservableCollection();

            //LightenScalePreviewImage = Image.Clone();
            //GrayScalePreviewImage = Image.Clone();
            //GammaPreviewImage = Image.Clone();
            //DarkenPreviewImage = Image.Clone();
            //ContrastPreviewImage = Image.Clone();

            FullEditorTitle = ResourceHelper.GetTranslation("ChooseTool");

            SetupShareContract();
        }

        private bool _isMouse;
        public bool IsMouse
        {
            get { return _isMouse; }
            set { this.Set(nameof(IsMouse), ref _isMouse, value); }
        }

        private bool _isTouch;
        public bool IsTouch
        {
            get { return _isTouch; }
            set { this.Set(nameof(IsTouch), ref _isTouch, value); }
        }

        private void SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            switch (UIViewSettings.GetForCurrentView().UserInteractionMode)
            {
                case UserInteractionMode.Mouse:
                    IsMouse = true;
                    IsTouch = false;
                    break;

                case UserInteractionMode.Touch:
                default:
                    IsMouse = false;
                    IsTouch = true;
                    break;
            }
        }

        private async Task<StorageFile> GetImage(Photo image)
        {
            var folder = KnownFolders.PicturesLibrary;
            StorageFile file;

            if (image.HasSubFolder)
                folder = await folder.GetFolderAsync(image.FolderName);

            if (image.PhotoUri.OriginalString.StartsWith("ms-appx"))
            {
                folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                folder = await folder.GetFolderAsync("FakePhotos");
            }

            file = (StorageFile) await folder.TryGetItemAsync(image.ImageName);
            return file;
        }

        public void SetPixelColor(Point pixel, double actualWidth, double actualHeight)
        {
            var pixelRichiesto = Image.GetPixel(
                Convert.ToInt32(pixel.X * (Image.PixelWidth / actualWidth)),
                Convert.ToInt32(pixel.Y * (Image.PixelHeight / actualHeight)));

            Color = new SolidColorBrush(Windows.UI.Color.FromArgb(pixelRichiesto.A, pixelRichiesto.R, pixelRichiesto.G, pixelRichiesto.B));
        }

        public void FloodFillSelection(Point pixel, double actualWidth, double actualHeight)
        {
            var pixelRichiesto = Image.GetPixel(
                Convert.ToInt32(pixel.X * (Image.PixelWidth / actualWidth)),
                Convert.ToInt32(pixel.Y * (Image.PixelHeight / actualHeight)));
            
            var hexString = pixelRichiesto.ToString().Substring(1);

            int colorValue1 = int.Parse(hexString, NumberStyles.HexNumber);
            int colorValue2 = int.Parse(Color.Color.ToString().Remove(0,1), NumberStyles.HexNumber);
            Image.FloodFillScanlineReplace(Convert.ToInt32(pixel.X*(Image.PixelWidth/actualWidth)), Convert.ToInt32(pixel.Y*(Image.PixelHeight/actualHeight)), colorValue1, colorValue2, Convert.ToByte(FloodFillAccuracy));
        }

        private RelayCommand<SolidColorBrush> _selectedColorChangeCommand;
        public RelayCommand<SolidColorBrush> SelectedColorChangeCommand
        {
            get
            {
                if (_selectedColorChangeCommand == null)
                    _selectedColorChangeCommand = new RelayCommand<SolidColorBrush>(color => this.Color = color);

                return _selectedColorChangeCommand;
            }
        }

        private int _floodFileAccuracy = 50;
        public int FloodFillAccuracy
        {
            get { return _floodFileAccuracy; }
            set { this.Set(nameof(FloodFillAccuracy), ref _floodFileAccuracy, value); }
        }

        private double _grayScale;
        public double GrayScale
        {
            get { return _grayScale; }
            set { this.Set(nameof(GrayScale), ref _grayScale, value); }
        }

        private RelayCommand<double> _grayScaleChangeCommand;
        public RelayCommand<double> GrayScaleChangeCommand
        {
            get
            {
                if(_grayScaleChangeCommand == null)
                    _grayScaleChangeCommand = new RelayCommand<double>(value =>
                    {
                        GrayScalePreviewImage = Image.Clone();
                        GrayScalePreviewImage.Grayscale(value);
                        this.GrayScale = value;
                    });

                return _grayScaleChangeCommand;
            }
        }

        private WriteableBitmap _grayScalePreviewImage;
        public WriteableBitmap GrayScalePreviewImage
        {
            get { return _grayScalePreviewImage; }
            set { this.Set(nameof(GrayScalePreviewImage), ref _grayScalePreviewImage, value); }
        }

        private double _rotationFactor;
        public double RotationFactor
        {
            get { return _rotationFactor; }
            set { this.Set(nameof(RotationFactor), ref _rotationFactor, value); }
        }

        private RelayCommand<double> _rotationChangeCommand;
        public RelayCommand<double> RotationChangeCommand
        {
            get
            {
                if (_rotationChangeCommand == null)
                    _rotationChangeCommand = new RelayCommand<double>(value => this.RotationFactor = value);

                return _rotationChangeCommand;
            }
        }

        private RelayCommand<FilterEnum> _filterChangedCommand;
        public RelayCommand<FilterEnum> FilterChangedCommand
        {
            get
            {
                if (_filterChangedCommand == null)
                    _filterChangedCommand = new RelayCommand<FilterEnum>(filter =>
                    {
                        this.SelectedFilter = filter;
                    });

                return _filterChangedCommand;
            }
        }

        private RelayCommand _grayApplyCommand;
        public RelayCommand GrayApplyCommand
        {
            get
            {
                if (_grayApplyCommand == null)
                    _grayApplyCommand = new RelayCommand(() => 
                    {
                        Image.Grayscale(GrayScale);
                        GrayScalePreviewImage = null;
                        CanSave = true;
                    });
                return _grayApplyCommand;
            }
        }

        private RelayCommand _rotationApplyCommand;
        public RelayCommand RotationApplyCommand
        {
            get
            {
                if (_rotationApplyCommand == null)
                    _rotationApplyCommand = new RelayCommand(() =>
                    {
                        Image = Image.RotateFree(RotationFactor);
                        CanSave = true;
                    });
                return _rotationApplyCommand;
            }
        }

        private double _isFreeRotationEnabled;
        public double IsFreeRotationEnabled
        {
            get { return _isFreeRotationEnabled; }
            set { this.Set(nameof(IsFreeRotationEnabled), ref _isFreeRotationEnabled, value); }
        }

        private double _stepFrequencyValue = 90;
        public double StepFrequencyValue
        {
            get { return _stepFrequencyValue; }
            set { this.Set(nameof(StepFrequencyValue), ref _stepFrequencyValue, value); }
        }

        private RelayCommand _freeRotationCommand;
        public RelayCommand FreeRotationCommand
        {
            get
            {
                if (_freeRotationCommand == null)
                    _freeRotationCommand = new RelayCommand(() =>
                    {
                        if (_stepFrequencyValue == 1)
                        {
                            StepFrequencyValue = 90;
                        }
                        else
                        {
                            StepFrequencyValue = 1;
                        }
                    });
                return _freeRotationCommand;
            }
        }

        private RelayCommand _flipHorizontallyCommand;
        public RelayCommand FlipHorizontallyCommand
        {
            get
            {
                if (_flipHorizontallyCommand == null)
                    _flipHorizontallyCommand = new RelayCommand(() =>
                    {
                        Image = Image.Flip(WriteableBitmapExtensions.FlipMode.Horizontal);
                    });
                return _flipHorizontallyCommand;
            }
        }

        private RelayCommand _flipVerticallyCommand;
        public RelayCommand FlipVerticallyCommand
        {
            get
            {
                if (_flipVerticallyCommand == null)
                    _flipVerticallyCommand = new RelayCommand(() =>
                    {
                        Image = Image.Flip(WriteableBitmapExtensions.FlipMode.Vertical);
                    });
                return _flipVerticallyCommand;
            }
        }

        private double _lightenScaleValue = 0.5;
        public double LightenScaleValue
        {
            get { return _lightenScaleValue; }
            set
            {
                this.Set(nameof(LightenScaleValue), ref _lightenScaleValue, value);
                LightenScalePreviewImage = Image.Clone();
                LightenScalePreviewImage.Lighten(LightenScaleValue);
            }
        }

        private WriteableBitmap _lightenPreviewImage;
        public WriteableBitmap LightenScalePreviewImage
        {
            get { return _lightenPreviewImage; }
            set { this.Set(nameof(LightenScalePreviewImage), ref _lightenPreviewImage, value); }
        }

        private RelayCommand _applyLightScaleCommand;
        public RelayCommand ApplyLightScaleCommand
        {
            get
            {
                if (_applyLightScaleCommand == null)
                    _applyLightScaleCommand = new RelayCommand(() =>
                    {
                        Image.Lighten(LightenScaleValue);
                        LightenScalePreviewImage = null;
                        CanSave = true;
                    });
                return _applyLightScaleCommand;
            }
        }

        private RelayCommand _fillSelection;
        public RelayCommand FillSelection
        {
            get
            {
                if(_fillSelection == null)
                    _fillSelection = new RelayCommand(() =>
                    {
                        if (CurrentImageHeight != 0 && CurrentImageHeight != 00)
                        {
                            int x1Def = Convert.ToInt32(X1*(Image.PixelWidth/CurrentImageWidth));
                            int y1Def = Convert.ToInt32(Y1*(Image.PixelHeight/CurrentImageHeight));
                            int x2Def = Convert.ToInt32(X2*(Image.PixelWidth/CurrentImageWidth));
                            int y2Def = Convert.ToInt32(Y2*(Image.PixelHeight/CurrentImageHeight));
                            if (FillChecked)
                            {
                                if (ShapeSelection == ResourceHelper.GetTranslation("SelectionShape1"))
                                    Image.FillRectangle(x1Def, y1Def, x2Def, y2Def, Color.Color);

                                else if (ShapeSelection == ResourceHelper.GetTranslation("SelectionShape2"))
                                    Image.FillEllipse(x1Def, y1Def, x2Def, y2Def, Color.Color);
                            }
                            else
                            {
                                if (ShapeSelection == ResourceHelper.GetTranslation("SelectionShape1"))
                                    Image.DrawRectangle(x1Def, y1Def, x2Def, y2Def, Color.Color);
                                else if (ShapeSelection == ResourceHelper.GetTranslation("SelectionShape2"))
                                    Image.DrawEllipse(x1Def, y1Def, x2Def, y2Def, Color.Color);
                            }
                        }
                    });

                return _fillSelection;;
            }
        }

        private bool _fillChecked;
        public bool FillChecked
        {
            get { return _fillChecked; }
            set { this.Set(nameof(_fillChecked), ref _fillChecked, value); }
        }

        private WriteableBitmap _contrastPreviewImage;
        public WriteableBitmap ContrastPreviewImage
        {
            get { return _contrastPreviewImage; }
            set { this.Set(nameof(ContrastPreviewImage), ref _contrastPreviewImage, value); }
        }

        private double _contrastLevel;
        public double ContrastLevel
        {
            get { return _contrastLevel; }
            set
            {
                this.Set(nameof(_contrastLevel), ref _contrastLevel, value);
                ContrastPreviewImage = Image.Clone();
                ContrastPreviewImage = ContrastPreviewImage.AdjustContrast(ContrastLevel);
            }
        }

        private WriteableBitmap _gammaPreviewImage;
        public WriteableBitmap GammaPreviewImage
        {
            get { return _gammaPreviewImage; }
            set { this.Set(nameof(GammaPreviewImage), ref _gammaPreviewImage, value); }
        }

        private double _gammaLevel;
        public double GammaLevel
        {
            get { return _gammaLevel; }
            set
            {
                this.Set(nameof(_gammaLevel), ref _gammaLevel, value);
                GammaPreviewImage = Image.Clone();
                GammaPreviewImage = GammaPreviewImage.AdjustGamma(GammaLevel);
            }
        }

        private WriteableBitmap _darkenPreviewImage;
        public WriteableBitmap DarkenPreviewImage
        {
            get { return _darkenPreviewImage; }
            set { this.Set(nameof(DarkenPreviewImage), ref _darkenPreviewImage, value); }
        }

        private double _darkenLevel;
        public double DarkenLevel
        {
            get { return _darkenLevel; }
            set
            {
                this.Set(nameof(_darkenLevel), ref _darkenLevel, value);
                DarkenPreviewImage = Image.Clone();
                DarkenPreviewImage.Darken(DarkenLevel);
            }
        }

        private string _textFontType = "Segoe UI Symbol";
        public string TextFontType
        {
            get { return _textFontType; }
            set { this.Set(nameof(TextFontType), ref _textFontType, value); }
        }

        private RelayCommand _contrastApplyCommand;
        public RelayCommand ContrastApplyCommand
        {
            get
            {
                if (_contrastApplyCommand == null)
                    _contrastApplyCommand = new RelayCommand(() =>
                    {
                        Image = Image.AdjustContrast(ContrastLevel);
                        ContrastPreviewImage = null;
                        CanSave = true;
                    });
                return _contrastApplyCommand;
            }
        }

        private RelayCommand _darkenApplyCommand;
        public RelayCommand DarkenApplyCommand
        {
            get
            {
                if (_darkenApplyCommand == null)
                    _darkenApplyCommand = new RelayCommand(() => Image = Image.Darken(DarkenLevel));
                return _darkenApplyCommand;
            }
        }

        private RelayCommand _invertApplyCommand;
        public RelayCommand InvertApplyCommand
        {
            get
            {
                if (_invertApplyCommand == null)
                    _invertApplyCommand = new RelayCommand(() =>
                    {
                        Image = Image.Invert();
                        CanSave = true;
                    });
                return _invertApplyCommand;
            }
        }

        private RelayCommand _gammaApplyCommand;
        public RelayCommand GammaApplyCommand
        {
            get
            {
                if (_gammaApplyCommand == null)
                    _gammaApplyCommand = new RelayCommand(() => 
                    {
                        Image = Image.AdjustGamma(GammaLevel);
                        GammaPreviewImage = null;
                        CanSave = true;
                    });
                return _gammaApplyCommand;
            }
        }

        private RelayCommand<ComboBoxItem> _changedFontCommand;
        public RelayCommand<ComboBoxItem> ChangedFontCommand
        {
            get
            {
                if (_changedFontCommand == null)
                    _changedFontCommand =
                        new RelayCommand<ComboBoxItem>(value => TextFontType = value.Content.ToString());
                return _changedFontCommand; ;
            }
        }


        private string _shapeSelection = "Rectangle";
        public string ShapeSelection
        {
            get { return _shapeSelection; }
            set { this.Set(nameof(ShapeSelection), ref _shapeSelection, value); }
        }

        private RelayCommand<ComboBoxItem> _shapeSelectionCommand;
        public RelayCommand<ComboBoxItem> ShapeSelectionCommand
        {
            get
            {
                if (_shapeSelectionCommand == null)
                    _shapeSelectionCommand =
                        new RelayCommand<ComboBoxItem>(value => _shapeSelection = value.Content.ToString());
                return _shapeSelectionCommand; ;
            }
        }

        private RelayCommand _createNewTextCommand;
        public RelayCommand CreateNewTextCommand
        {
            get
            {
                if (_createNewTextCommand == null)
                    _createNewTextCommand = _createNewTextCommand = new RelayCommand(() => IsFirstText = true);

                return _createNewTextCommand;
            }
        }

        private RelayCommand _shareCommand;
        public RelayCommand ShareCommand
        {
            get
            {
                if (_shareCommand == null)
                    _shareCommand = _shareCommand = new RelayCommand(() => DataTransferManager.ShowShareUI());

                return _shareCommand;
            }
        }

        private RelayCommand _saveImageCommand;
        public RelayCommand SaveImageCommand
        {
            get
            {
                if (_saveImageCommand == null)
                    _saveImageCommand = _saveImageCommand = new RelayCommand(() => this.IsSaving = true);

                return _saveImageCommand;
            }
        }

        private void SetupShareContract()
        {
            DataTransferManager datatransferManager = DataTransferManager.GetForCurrentView();
            datatransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(DataRequested);
        }
        
        private async void DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = "Imedit";
            request.Data.Properties.Description = ResourceHelper.GetTranslation("ShareDescription");

            // Because we are making async calls in the DataRequested event handler,
            //  we need to get the deferral first.
            DataRequestDeferral deferral = request.GetDeferral();

            // Make sure we always call Complete on the deferral.
            try
            {
                IsSaving = true;

                // security because is saving
                await Task.Delay(2000);

                StorageFile file = await KnownFolders.PicturesLibrary.GetFileAsync("Imedit-1.png");
                request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(file);
                request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(file));
            }
            finally
            {
                deferral.Complete();
            }

        }
    }
}