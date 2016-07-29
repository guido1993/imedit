using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Imedit.Helpers
{
    public class CustomCursor
	{
		private FrameworkElement _element;
		Popup _cursorContainer;
        private bool _withDragStyle;

        public static DataTemplate GetDragCursorTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(DragCursorTemplateProperty);
        }

        public static void SetDragCursorTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(DragCursorTemplateProperty, value);
        }

        public static readonly DependencyProperty DragCursorTemplateProperty =
            DependencyProperty.RegisterAttached("DragCursorTemplate", typeof(DataTemplate), typeof(CustomCursor), new PropertyMetadata(null, OnDragCursorTemplatePropertyChanged));

        private static void OnDragCursorTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement))
                throw new ArgumentOutOfRangeException("Property can only be attached to FrameworkElements");
            var element = (d as FrameworkElement);
            if (e.NewValue is DataTemplate)
            {
                (element.GetValue(CustomCursorProperty) as CustomCursor).AttachDragStyle(e.NewValue as DataTemplate);
            }
        }

		private static readonly DependencyProperty CustomCursorProperty =
			DependencyProperty.RegisterAttached("CustomCursor", typeof(CustomCursor), typeof(CustomCursor), null);

        public static DataTemplate GetCursorTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(CursorTemplateProperty);
        }

        public static void SetCursorTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(CursorTemplateProperty, value);
        }

        public static readonly DependencyProperty CursorTemplateProperty =
            DependencyProperty.RegisterAttached("CursorTemplate", typeof(DataTemplate), typeof(CustomCursor), new PropertyMetadata(null, OnCursorTemplatePropertyChanged));

        private static void OnCursorTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement))
                throw new ArgumentOutOfRangeException("Property can only be attached to FrameworkElements");
            var element = (d as FrameworkElement);
            if (e.OldValue is DataTemplate)
            {
                (element.GetValue(CustomCursorProperty) as CustomCursor).Dispose();
            }
            if (e.NewValue is DataTemplate)
                new CustomCursor(element, e.NewValue as DataTemplate);
        }

        #region DefaultCursor
        private static CoreCursor _defaultCursor;
        private static CoreCursor DefaultCursor
        {
            get { return null; }
        }
        #endregion

		private CustomCursor(FrameworkElement element, DataTemplate template)
		{
			_element = element;
            _element.SetValue(CustomCursorProperty, this);
            _defaultCursor = Window.Current.CoreWindow.PointerCursor ?? _defaultCursor;
            Window.Current.CoreWindow.PointerCursor = null;

            // normal style
            _element.PointerEntered += OnPointerEntered;
            _element.PointerMoved += OnPointerMoved;
            _element.PointerExited += OnPointerExited;

            _element.Unloaded += OnControlUnloaded;

			_cursorContainer = new Popup
			{
				IsOpen = false,
				Child = new ContentControl
				{
					ContentTemplate = template,
					IsHitTestVisible = false,
					RenderTransform = new TranslateTransform()
				}
			};
			_cursorContainer.IsHitTestVisible = false;
		}

        public void AttachDragStyle(DataTemplate template)
        {
            _withDragStyle = true;
            _element.SetValue(DragCursorTemplateProperty, template);

            _element.PointerPressed += OnPointerPressed;
            _element.Holding += OnHolding;
            _element.PointerReleased += OnPointerReleased;
        }

        void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (_withDragStyle)
            {
                (_cursorContainer.Child as ContentControl).ContentTemplate = (DataTemplate)_element.GetValue(DragCursorTemplateProperty);
                Window.Current.CoreWindow.PointerCursor = null;
                UpdateCursor(e);
            }
        }

        void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (_withDragStyle)
            {
                (_cursorContainer.Child as ContentControl).ContentTemplate = (DataTemplate)_element.GetValue(CursorTemplateProperty);
                Window.Current.CoreWindow.PointerCursor = null;
                UpdateCursor(e);
            }
        }

        void OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            if (_withDragStyle)
            {
                (_cursorContainer.Child as ContentControl).ContentTemplate = (DataTemplate)_element.GetValue(DragCursorTemplateProperty);
                Window.Current.CoreWindow.PointerCursor = null;
                UpdateCursor(e);
            }
        }

        private void UpdateCursor(HoldingRoutedEventArgs e)
        {
            _cursorContainer.IsOpen = true;
            var p = e.GetPosition(null);
            var t = (_cursorContainer.Child.RenderTransform as TranslateTransform);
            t.X = p.X;
            t.Y = p.Y;
        }

        void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            UpdateCursor(e);
        }

        private void OnControlUnloaded(object sender, RoutedEventArgs e)
        {
            Detach();
        }

        public void Detach()
        {
            _element.PointerEntered -= OnPointerEntered;
            _element.PointerExited -= OnPointerExited;
            _element.Unloaded -= OnControlUnloaded;
            _element.PointerMoved -= OnPointerMoved;

            if (_withDragStyle)
            {
                _element.PointerPressed -= OnPointerPressed;
                _element.Holding -= OnHolding;
                _element.PointerReleased -= OnPointerReleased;
            }

            Window.Current.CoreWindow.PointerCursor = DefaultCursor;
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (_withDragStyle)
            {
                (_cursorContainer.Child as ContentControl).ContentTemplate = (DataTemplate)_element.GetValue(CursorTemplateProperty);
            }

            Window.Current.CoreWindow.PointerCursor = null;
            UpdateCursor(e);
        }

        private void UpdateCursor(PointerRoutedEventArgs e)
        {
            _cursorContainer.IsOpen = true;
            var p = e.GetCurrentPoint(null).Position;
            var t = (_cursorContainer.Child.RenderTransform as TranslateTransform);
            t.X = p.X;
            t.Y = p.Y;
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            _cursorContainer.IsOpen = false;
        }

		private void Dispose()
		{
            Detach();
            _element.ClearValue(CustomCursorProperty);
            _cursorContainer.IsOpen = false;
            (_cursorContainer.Child as ContentControl).ContentTemplate = null;
            _cursorContainer.Child = null;
            _cursorContainer = null;
		}
	}
}
