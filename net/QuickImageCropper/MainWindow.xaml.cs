using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace QuickImageCropper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Point clickPoint;
        private Rectangle newRect;
        private Point drawPoint;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItemClick(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
            if (openDialog.ShowDialog().Value)
            {
                LoadImage(openDialog.FileName);
            }
        }

        private void LoadImage(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            var src = new BitmapImage();
            src.BeginInit();
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.UriSource = new Uri(fileInfo.FullName, UriKind.Relative);
            src.EndInit();

            var image = new Image {Source = src, Stretch = Stretch.UniformToFill};
            canvas.Children.Add(image);

            var zIndex = canvas.Children.Count;
            Panel.SetZIndex(image, zIndex);
            Canvas.SetLeft(image, 20);
            Canvas.SetTop(image, 20);
        }

        private void CanvasPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickPoint = e.GetPosition(canvas);
            newRect = new Rectangle
                          {
                              Stroke = Brushes.Black,
                              Fill = new SolidColorBrush(Colors.Beige){Opacity = 0.3},
                              StrokeLineJoin = PenLineJoin.Bevel,
                              Width = 10,
                              Height = 10,
                              StrokeThickness = 2
                          };

            canvas.Children.Add(newRect);
            Canvas.SetLeft(newRect, clickPoint.X);
            Canvas.SetTop(newRect, clickPoint.Y);

            var zIndex = canvas.Children.Count;
            Panel.SetZIndex(newRect, zIndex);
        }

        private void CanvasPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            newRect = null;
        }

        private void CanvasPreviewMouseMove(object sender, MouseEventArgs e)
        {
            drawPoint = e.GetPosition(canvas);
            if (newRect != null & e.LeftButton == MouseButtonState.Pressed)
            {
                newRect.Width = Math.Abs(clickPoint.X - drawPoint.X);
                newRect.Height = Math.Abs(clickPoint.Y - drawPoint.Y);
            }
        }
    }
}
