using PackmanOrigin.Classes;
using PackmanOrigin.Interfaces;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace PackmanOrigin.GameClasses
{
    class Cherry : IObjectToCollect
    {
        public const int POINTS = 20;
        public Image Image { get; set; }
        public bool IsEaten { get;  set; }
        public double LeftLocation { get;private set; }
        public double TopLocation { get;private set; }
        public const int Height = 24;
        public const int WIDTH = 24;

        public Cherry(TileObject[,] tileImageArray,string imagePath ="/Assets/Images/cherries.jpg")

        {
           
            TileObject opentile= FindAvialableTile( tileImageArray);
            LeftLocation = opentile.LeftLocation;
            TopLocation = opentile.TopLocation;

            Image = new Image
            {
                Width = WIDTH,
                Height = Height,
                Source = new BitmapImage(new Uri($"ms-appx://{imagePath}"))
            };
        }

        public void AddObjectToCanvas(Canvas can)
        {
            Canvas.SetLeft(Image, LeftLocation);
            Canvas.SetTop(Image,TopLocation);
            can.Children.Add(Image); ;
        }

        public void RemoveObjectFromCanvas(Canvas can)
        {
            if (IsEaten) can.Children.Remove(Image);
        }

        private TileObject FindAvialableTile(TileObject[,] tileImageArray)
        {
            Random  rand= new Random();
            TileObject tileObject;
            while (true)
            {
                int Row = rand.Next(0, tileImageArray.GetLength(0) - 1);
                int column = rand.Next(0, tileImageArray.GetLength(1) - 1);
                tileObject = tileImageArray[Row, column];
                if (IsBlackTile(tileImageArray[Row, column])) break;
            }
            return tileObject;
        }

        private bool IsBlackTile(TileObject tile)
        {
            return tile.GetType() == typeof(BlackTile);
        }
    }
}

    

