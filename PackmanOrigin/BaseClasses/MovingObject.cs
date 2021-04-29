using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace PackmanOrigin.Interfaces.BaseClasses
{
    class MovingObject : IGenricActions
    {

        public Image Image { get; set; }
        public double LeftLocation { get; set; }
        public double TopLocation { get; set; }
        public double StartLLocation { get;}
        public double StartTLocation { get;}
        public MovingObject(double leftLocation, double topLocation, string imagePath)
        {
            Image = new Image();
            LeftLocation = leftLocation;
            TopLocation = topLocation;
            StartLLocation = leftLocation;
            StartTLocation = topLocation;
            Image.Source = new BitmapImage(new Uri($"ms-appx://{imagePath}"));
        }

        public void AddObjectToCanvas(Canvas can)
        {
            Canvas.SetLeft(Image, LeftLocation);
            Canvas.SetTop(Image, TopLocation);
            can.Children.Add(Image);
        }

        public void RemoveObjectFromCanvas(Canvas can)
        {
            can.Children.Remove(Image);
        }

       
    }
}
