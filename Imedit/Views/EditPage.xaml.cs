using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Imedit.Helpers;
using Imedit.Models;
using Imedit.ViewModels;

namespace Imedit.Views
{
    public sealed partial class EditPage : Page
    {
        private Rectangle _rectangle;
        private Point _originalPosition;
        private ObservableCollection<CustomEditBox> _textBox;
        private bool _isAdded = false;
        private bool _rectangleCreationFailed;

        public double XTopLeft;
        public double YTopLeft;
        public double XBottomRight;
        public double YBottomRight;

        public double XTopLeftMin;
        public double YTopLeftMin;
        public double XBottomRightMax;
        public double YBottomRightMax;

        public EditPageViewModel ViewModel { get { return this.DataContext as EditPageViewModel; } }

        public EditPage()
        {
            this.InitializeComponent();

            this.inkCanvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            this.inkCanvas.InkPresenter.StrokesErased += InkPresenter_StrokesErased;

            ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;

            if (_textBox != null) return;
            _textBox = new ObservableCollection<CustomEditBox>();
            _textBox.CollectionChanged += TextBoxOnCollectionChanged;
        }

        private void TextBoxOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            Debug.WriteLine("Last operation: " + args.Action.ToString("G"));

            if (args.Action != NotifyCollectionChangedAction.Remove) return;
            foreach (CustomEditBox item in args.OldItems)
            {
                Panel.Children.Remove(item);
            }
        }

        private void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
            ViewModel.CanSave = inkCanvas.InkPresenter.StrokeContainer.GetStrokes().Count > 0;
        }

        private void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            ViewModel.CanSave = inkCanvas.InkPresenter.StrokeContainer.GetStrokes().Count > 0;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var image = new Photo { ImageName = e.Parameter.ToString().Split('$')[0],
                                    FolderName = e.Parameter.ToString().Split('$')[1],
                                    PhotoUri = new Uri(e.Parameter.ToString().Split('$')[2]) };

            await ViewModel.Init(image);
            
            ChangeInkAttributes();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            ViewModel.IsInkCanvasEnabled = false;
            ViewModel.IsTextPanelEnabled = false;
            ViewModel.ShowFilters = false;
            ViewModel.IsColorPickerEnabled = true;
            ViewModel.IsFloodFillEnabled = false;
            ViewModel.IsEyeDropperEnabled = false;
            ViewModel.SelectionEnabled = false;
            ViewModel.FullEditorTitle = ResourceHelper.GetTranslation("ChooseTool");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;

            CursorBorder.SetValue(CustomCursor.CursorTemplateProperty, null);
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
        }

        private async void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!ViewModel.SelectionEnabled && !ViewModel.IsFloodFillEnabled && !ViewModel.IsEyeDropperEnabled && !ViewModel.IsInkCanvasEnabled && !ViewModel.IsTextPanelEnabled && !ViewModel.ShowFilters)
            {
                ViewModel.IsColorPickerEnabled = false;
                ViewModel.FullEditorTitle = ResourceHelper.GetTranslation("ChooseTool");
                CursorBorder.SetValue(CustomCursor.CursorTemplateProperty, null);
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
                ViewModel.IsLogoVisible = true;
            }
            
            if (e.PropertyName == "IsFloodFillEnabled" && ViewModel.IsFloodFillEnabled)
            {
                CursorBorder.SetValue(CustomCursor.CursorTemplateProperty, Resources["PaintBucketCursor"] as DataTemplate);
            }
            else if (e.PropertyName == "IsFirstText" && ViewModel.IsTextPanelEnabled && ViewModel.IsFirstText)
            {
                CursorBorder.SetValue(CustomCursor.CursorTemplateProperty, Resources["TextCursor"] as DataTemplate);
            }
            else if (e.PropertyName == "IsFirstText" && ViewModel.IsTextPanelEnabled)
            {
                CursorBorder.SetValue(CustomCursor.CursorTemplateProperty, null);
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            }
            else if (e.PropertyName == "IsTextPanelEnabled" && ViewModel.IsTextPanelEnabled)
            {
                CursorBorder.SetValue(CustomCursor.CursorTemplateProperty, null);
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            }
            else if (e.PropertyName == "SelectionEnabled" && ViewModel.SelectionEnabled)
            {
                CursorBorder.SetValue(CustomCursor.CursorTemplateProperty, null);
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Cross, 1);
            }
            else if (e.PropertyName == "IsInkCanvasEnabled" && ViewModel.IsInkCanvasEnabled)
            {
                CursorBorder.SetValue(CustomCursor.CursorTemplateProperty, null);
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            }
            else if (e.PropertyName == "IsEyeDropperEnabled" && ViewModel.IsEyeDropperEnabled)
            {
                CursorBorder.SetValue(CustomCursor.CursorTemplateProperty, Resources["EyeDropperCursor"] as DataTemplate);
            }
            else if (e.PropertyName == "ShowFilters")
            {
                CursorBorder.SetValue(CustomCursor.CursorTemplateProperty, null);
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            }

            if (e.PropertyName == "Color" || e.PropertyName == "IsInkCanvasEnabled" || e.PropertyName == "StrokeSize")
                ChangeInkAttributes();

            if (e.PropertyName == "IsSaving" && ViewModel.IsSaving)
                await RenderAndSave();

            if (e.PropertyName == "SelectionEnabled" && !ViewModel.SelectionEnabled)
            {
                if (_rectangle != null)
                {
                    _rectangle.Width = 0;
                    _rectangle.Height = 0;
                }
            }
        }

        public void ChangeInkAttributes()
        {
            // set the input type for the ink
            if (ViewModel.IsInkCanvasEnabled)
                inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse |
                                                          CoreInputDeviceTypes.Touch |
                                                          CoreInputDeviceTypes.Pen;
            else
                inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.None;

            var inkDrawingAttributes = new InkDrawingAttributes();
            inkDrawingAttributes.Color = ViewModel.Color.Color;
            if (ViewModel.StrokeSize != 0)
            {
                inkDrawingAttributes.Size = new Size(ViewModel.StrokeSize, ViewModel.StrokeSize);
            }
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(inkDrawingAttributes);
        }

        private async Task RenderAndSave()
        {
            ViewModel.IsLoading = true;

            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(FullCanvas);
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();

            var file = await KnownFolders.PicturesLibrary.CreateFileAsync("Imedit-1.png", CreationCollisionOption.ReplaceExisting);

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    (uint)renderTargetBitmap.PixelWidth,
                    (uint)renderTargetBitmap.PixelHeight, 96d, 96d,
                    pixelBuffer.ToArray());

                await encoder.FlushAsync();
            }

            ViewModel.IsSaving = false;
            ViewModel.IsLoading = false;
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.IsEyeDropperEnabled)
            {
                var pixel = e.GetPosition(SourceImage);
                ViewModel.SetPixelColor(pixel, SourceImage.ActualWidth, SourceImage.ActualHeight);
            }
            else if (ViewModel.IsFloodFillEnabled)
            {
                var pixel = e.GetPosition(SourceImage);
                ViewModel.FloodFillSelection(pixel, SourceImage.ActualWidth, SourceImage.ActualHeight);
            }
        }


        protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
            YTopLeftMin = SourceImage.TransformToVisual(MasterEditGrid).TransformPoint(new Point()).Y;
            XTopLeftMin = SourceImage.TransformToVisual(MasterEditGrid).TransformPoint(new Point()).X;
            XBottomRightMax = XTopLeftMin + SourceImage.ActualWidth;
            YBottomRightMax = YTopLeftMin + SourceImage.ActualHeight;

            if (ViewModel.SelectionEnabled)
            {
                if (e.Position.X > XTopLeftMin && e.Position.X < XBottomRightMax && e.Position.Y > YTopLeftMin &&
                    e.Position.Y < YBottomRightMax)
                {
                    if (_rectangle == null)
                    {
                        _rectangle = new Rectangle();
                        _rectangle.StrokeThickness = 2;
                        _rectangle.Stroke = new SolidColorBrush(Colors.Gray);
                        _rectangle.StrokeDashArray = new DoubleCollection {1, 2};
                    }
                    if (ViewModel.ShapeSelection == "Ellipse")
                    {
                        _rectangle.RadiusX = 500;
                        _rectangle.RadiusY = 500;
                    }
                    else if (ViewModel.ShapeSelection == "Rectangle")
                    {
                        _rectangle.RadiusX = 0;
                        _rectangle.RadiusY = 0;
                    }
                    Canvas.SetLeft(_rectangle, e.Position.X);
                    Canvas.SetTop(_rectangle, e.Position.Y);
                    _rectangle.Width = 0;
                    _rectangle.Height = 0;
                    _originalPosition = e.Position;
                    _rectangleCreationFailed = false;

                    if (!_isAdded)
                    {
                        Panel.Children.Add(_rectangle);
                        _isAdded = true;
                    }
                }
                else
                {
                    _rectangleCreationFailed = true;
                }
            }
            else if (ViewModel.IsTextPanelEnabled && ViewModel.IsFirstText)
            {
                if (e.Position.X > XTopLeftMin && e.Position.X < XBottomRightMax && e.Position.Y > YTopLeftMin &&
                    e.Position.Y < YBottomRightMax)
                {
                    var index = _textBox.Count;

                    if (_textBox.Any(box => box.CreationIndex == index)) //Mi assicuro che sia unico nonostante precedenti eliminazioni di elementi in _textBox
                    {
                        while (_textBox.Any(box => box.CreationIndex == index))
                        {
                            index = index + new Random(Environment.TickCount).Next();
                        }
                    }

                    var extEditBox = new CustomEditBox(index, () => { _textBox.Remove(_textBox.FirstOrDefault(box => box.CreationIndex == index)); })
                    {
                        FontSize = ViewModel.TextSize,
                        PlaceholderText = "Type your text here",
                        Foreground = new SolidColorBrush(ViewModel.Color.Color),
                        BorderBrush = new SolidColorBrush(Colors.Transparent),
                        Background = new SolidColorBrush(Colors.Transparent),
                        FontWeight = ViewModel.TextIsBold ? FontWeights.Bold : FontWeights.Normal,
                        FontStyle = ViewModel.TextIsItalic ? FontStyle.Italic : FontStyle.Normal,
                        FontFamily = new FontFamily(ViewModel.TextFontType)
                };

                    Canvas.SetLeft(extEditBox, e.Position.X);
                    Canvas.SetTop(extEditBox, e.Position.Y);
                    extEditBox.Tag = index;
                    extEditBox.Width = 300;

                    _textBox.Add(extEditBox);

                    Panel.Children.Add(extEditBox);
                    
                    ViewModel.IsFirstText = false;
                }
            }

            base.OnManipulationStarted(e);
        }

        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            if (!ViewModel.SelectionEnabled || _rectangleCreationFailed)
                return;

            XTopLeft = Canvas.GetLeft(_rectangle);
            YTopLeft = Canvas.GetTop(_rectangle);

            if (_originalPosition.X < e.Position.X)
            {
                if (e.Position.X - _originalPosition.X + XTopLeft < XBottomRightMax)
                {
                    Canvas.SetLeft(_rectangle, _originalPosition.X);
                    XBottomRight = _rectangle.Width + XTopLeft;
                    _rectangle.Width = e.Position.X - _originalPosition.X;
                }
            }
            else
            {
                if (XTopLeft > XTopLeftMin)
                {
                    Canvas.SetLeft(_rectangle, e.Position.X);
                    XBottomRight = _rectangle.Width + XTopLeft;
                    _rectangle.Width = _originalPosition.X - e.Position.X;
                }
            }

            if (_originalPosition.Y < e.Position.Y)
            {
                if (e.Position.Y - _originalPosition.Y + YTopLeft < YBottomRightMax)
                {
                    Canvas.SetTop(_rectangle, _originalPosition.Y);
                    YBottomRight = _rectangle.Height + YTopLeft;
                    _rectangle.Height = e.Position.Y - _originalPosition.Y;
                }
            }
            else
            {
                if (YTopLeft > YTopLeftMin)
                {
                    Canvas.SetTop(_rectangle, e.Position.Y);
                    _rectangle.Height = _originalPosition.Y - e.Position.Y;
                    YBottomRight = _rectangle.Height + YTopLeft;
                }
            }

            base.OnManipulationDelta(e);
        }

        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            if (_rectangle != null)
            {
                ViewModel.X1 =
                    Convert.ToInt32(Canvas.GetLeft(_rectangle) -
                                    SourceImage.TransformToVisual(MasterEditGrid).TransformPoint(new Point()).X);
                ViewModel.Y1 =
                    Convert.ToInt32(Canvas.GetTop(_rectangle) -
                                    SourceImage.TransformToVisual(MasterEditGrid).TransformPoint(new Point()).Y);

                ViewModel.X2 = ViewModel.X1 + Convert.ToInt32(_rectangle.Width);
                ViewModel.Y2 = ViewModel.Y1 + Convert.ToInt32(_rectangle.Height);

                ViewModel.CurrentImageWidth = SourceImage.ActualWidth;
                ViewModel.CurrentImageHeight = SourceImage.ActualHeight;
            }
        }
    }
}