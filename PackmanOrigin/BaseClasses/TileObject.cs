using PackmanOrigin.Interfaces;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace PackmanOrigin.Classes
{
    public class TileObject : IGenricActions
    {
        public Image Image { get; }
        public double LeftLocation { get; set; }
        public double TopLocation { get; set; }
        public bool IsAJunction { get; set; }
        public int ColumnInd { get; set; }
        public int RowInd { get; set; }
        public static readonly int Height = Configuration.TileSize;
        public static readonly int WIDTH = Configuration.TileSize;

    public TileObject( double leftLocation, double topLocation, int columnInd, int rowInd, string imagePath, bool isAJunction = false)
        {
            Image = new Image();
            LeftLocation = leftLocation;
            TopLocation = topLocation;
            Image.Height = Height;
            Image.Width = WIDTH;
            Image.Source = new BitmapImage(new Uri($"ms-appx://{imagePath}"));
            IsAJunction = isAJunction;
            ColumnInd = columnInd;
            RowInd = rowInd;
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
