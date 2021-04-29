using PackmanOrigin.Classes;
using PackmanOrigin.GameClasses.Enemies;
using PackmanOrigin.Interfaces.BaseClasses;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace PackmanOrigin.GameClasses
{
    class PackManCharacter : MovingObject
    {
        
        private const int gapSizeFromInanimateObject = 4;
        public readonly int Height = TileObject.Height - gapSizeFromInanimateObject;
        public readonly int WIDTH = TileObject.WIDTH - gapSizeFromInanimateObject;
        public const int MOVMENT_SPEED = 4;
        static public int CurrentRowInd { get;private set; }
         static public int CurrentColInd { get; private set; }
        public int StartRowInd { get; set; }
        public int StartColInd { get; set; }

        public int StartLives { get; set; }
        public int Lives { get; set; }

        public PackManCharacter(double leftLocation, double topLocation,int lives, int currentRowInd, int currentColInd,string imagePath = "/Assets/Images/pacman-eating.gif")
            : base(leftLocation, topLocation, imagePath)
        {

            StartRowInd = currentRowInd;
            StartColInd = currentColInd;
            CurrentRowInd = currentRowInd;
            CurrentColInd = currentColInd;
            Image.Height = Height;
            Image.Width = WIDTH;
            Lives = lives;
            StartLives = lives;
        }

        public bool IscollidingWithBricksAndUpdateLocation(double newleft, double newtop,TileObject[,] tilesArray)
        {
            for (int i = 0; i < tilesArray.GetLength(0); i++)
            {
                for (int j = 0; j < tilesArray.GetLength(1); j++)
                {
                    if (tilesArray[i, j].GetType() == typeof(BrickTile))
                    {
                        if (HelperFunctions.IsImageCollidingWithOtherImage(Image, newleft, newtop, tilesArray[i, j].Image)) return true;
                    }
                    else
                    {
                        if (HelperFunctions.IsImageCollidingWithOtherImage(Image, newleft, newtop, tilesArray[i, j].Image))
                        {
                            CurrentRowInd = i;
                            CurrentColInd = j;
                        }
                    }    
                }
            }
            return false;
        }

        public bool IsCollidingWithCoins(double newleft, double newtop,List<Coin> coins,Canvas can,Action GameWon) // coins colliding, update coins + points in map  
        {
            for (int i = 0; i < coins.Count; i++)
            {
                if (coins[i].IsEaten == false)
                {
                    if (HelperFunctions.IsImageCollidingWithOtherImage(Image, newleft, newtop, coins[i].Image))
                    {
                        coins[i].IsEaten = true;
                        coins[i].RemoveObjectFromCanvas(can);
                        Coin.TotalCoins--;
                        Configuration.TotalPoints += Coin.point;
                        MainPage.ChangeScoreText(Configuration.TotalPoints);
                        if (Coin.TotalCoins == 0) GameWon();
                        return true ;
                    }
                }

            }
            return false;
        }

        public bool IscollidingWithEnemies(double newleft, double newtop,List<Enemy> enemies,Action collisonLogic)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (HelperFunctions.IsImageCollidingWithOtherImage(Image, newleft, newtop, enemies[i].Image)) collisonLogic();
            }
            return false;
        }

        public bool IsCollidingWithCherry(Cherry cherry, double newleft, double newtop, Action collisonLogic)
        {

            if (cherry == null) return false;
            if (cherry.IsEaten == false)
            {
               if( HelperFunctions.IsImageCollidingWithOtherImage(Image, newleft, newtop, cherry.Image))
                {
                    collisonLogic();
                    return true;
                }
            }
            return false;
        }
        public void SetToStartLocation()
        {
            CurrentRowInd = StartRowInd;
            CurrentColInd = StartColInd;
            Canvas.SetTop(Image, StartTLocation);
            Canvas.SetLeft(Image, StartLLocation);
        }

        public void SetToLSecretPath()
        {
            CurrentRowInd = SecretPath.LEFT_ROW_IND;
            CurrentColInd = SecretPath.LEFT_COL_IND;
            Canvas.SetLeft(Image, 0);
        }

        public void SetToRSecretPath(Canvas can)
        {
            CurrentRowInd = SecretPath.RIGHT_ROW_IND;
            CurrentColInd = SecretPath.RIGHT_COL_IND;
            Canvas.SetLeft(Image, can.ActualWidth - Image.ActualWidth);

        }
    }
}

