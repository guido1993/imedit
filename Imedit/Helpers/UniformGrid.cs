using System;
using Windows.Foundation;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Imedit.Helpers
{
    public class UniformGrid : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            double finalWidth, finalHeight;

            if (this.Orientation == Orientation.Horizontal)
            {
                finalWidth = availableSize.Width;
                var itemWidth = Math.Floor(availableSize.Width / Columns);
                var actualRows = Math.Ceiling((double)Children.Count / Columns);
                var actualHeight = ProjectionManager.ProjectionDisplayAvailable || AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop" ? 350 : 200; //Math.Floor((double)availableSize.Height / actualRows);
                var itemHeight = Math.Min(actualHeight, itemWidth);

                foreach (var child in Children)
                {
                    child.Measure(new Size(itemWidth, itemHeight));
                }

                finalHeight = itemHeight * actualRows;
            }
            else
            {

                finalHeight = availableSize.Height;
                var itemHeight = Math.Floor(availableSize.Height / Rows);
                var actualColumns = Math.Ceiling((double)Children.Count / Rows);
                var actualWidth = Math.Floor((double)availableSize.Width / actualColumns);
                var itemWidth = Math.Min(actualWidth, itemHeight);
                finalWidth = itemWidth * actualColumns;

                foreach (var child in Children)
                {
                    child.Measure(new Size(itemWidth, itemHeight));
                }
            }

            return new Size(finalWidth, finalHeight);

        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.Orientation == Orientation.Horizontal)
            {
                var actualRows = Math.Ceiling((double)Children.Count / Columns);
                var cellWidth = Math.Floor(finalSize.Width / Columns);
                var cellHeight = Math.Floor(finalSize.Height / actualRows);
                Size cellSize = new Size(cellWidth, cellHeight);
                int row = 0, col = 0;
                foreach (UIElement child in Children)
                {
                    child.Arrange(new Rect(new Point(cellSize.Width * col, cellSize.Height * row), cellSize));
                    var element = child as FrameworkElement;
                    if (element != null)
                    {
                        element.Height = cellSize.Height;
                        element.Width = cellSize.Width;
                    }

                    if (++col == Columns)
                    {
                        row++;
                        col = 0;
                    }
                }
            }
            else
            {
                var actualColumns = Math.Ceiling((double)Children.Count / Rows);
                var cellWidth = Math.Floor(finalSize.Width / actualColumns);
                var cellHeight = Math.Floor(finalSize.Height / Rows);
                Size cellSize = new Size(cellWidth, cellHeight);
                int row = 0, col = 0;
                foreach (UIElement child in Children)
                {
                    child.Arrange(new Rect(new Point(cellSize.Width * col, cellSize.Height * row), cellSize));
                    var element = child as FrameworkElement;
                    if (element != null)
                    {
                        element.Height = cellSize.Height;
                        element.Width = cellSize.Width;
                    }

                    if (++row == Rows)
                    {
                        col++;
                        row = 0;
                    }
                }
            }
            return finalSize;
        }

        public int Columns
        {
            get
            {
                if (ProjectionManager.ProjectionDisplayAvailable || 
                    AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop")
                    return 3;

                return 1;
            }
        }

        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty ColumnsProperty =
        DependencyProperty.Register("Columns", typeof(int), typeof(UniformGrid), new PropertyMetadata(1, OnColumnsChanged));


        public static readonly DependencyProperty RowsProperty =
        DependencyProperty.Register("Rows", typeof(int), typeof(UniformGrid), new PropertyMetadata(1, OnRowsChanged));

        public static readonly DependencyProperty OrientationProperty =
DependencyProperty.Register("Orientation", typeof(Orientation), typeof(UniformGrid), new PropertyMetadata(1, OnOrientationChanged));

        static void OnColumnsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            //int cols = (int)e.NewValue;
            //if (cols < 1)
            //    ((UniformGrid)obj).Columns = 1;
        }

        static void OnRowsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            int rows = (int)e.NewValue;
            if (rows < 1)
                ((UniformGrid)obj).Rows = 1;
        }

        static void OnOrientationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
