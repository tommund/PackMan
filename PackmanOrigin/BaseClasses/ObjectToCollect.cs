using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace PackmanOrigin.Interfaces.BaseClasses
{
    class ObjectToCollect:IObjectToCollect
    {
        public Image Image { get; set; }
        private readonly double  _leftLocation;
        private readonly double _topLocation;
        public const int Height = 24;
        public const int WIDTH = 24;
        public bool IsEaten { get; set; }
        public ObjectToCollect(double leftLocation, double topLocation, string imagePath)
        {
            Image = new Image();
            _leftLocation = leftLocation;
            _topLocation = topLocation;
            Image.Height = Height;
            Image.Width = WIDTH;
            Image.Source = new BitmapImage(new Uri($"ms-appx://{imagePath}"));
        }

        public void AddObjectToCanvas(Canvas can)
        {
            Canvas.SetLeft(Image, _leftLocation);
            Canvas.SetTop(Image, _topLocation);
            can.Children.Add(Image); ;
        }

        public void RemoveObjectFromCanvas(Canvas can)
        {
            if(IsEaten) can.Children.Remove(Image);
        }
    }
}
