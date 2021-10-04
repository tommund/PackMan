using PackmanOrigin.Classes;
using PackmanOrigin.Interfaces.BaseClasses;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace PackmanOrigin.GameClasses.Enemies
{
    class Enemy : MovingObject
    {
        protected Random _random = new Random();
        public readonly int Height = TileObject.Height;
        public readonly int WIDTH = TileObject.WIDTH;
        public double BeforeMoveLeft { get; set; }
        public double BeforeMoveTop { get; set; }
        private bool _IsOnAJunction { get; set; }

        private static int _movmentSpeed = 1;
        public static int MovmentSpeed
        {
            get => _movmentSpeed;
            set
            {
                if (Configuration.TileSize % value != 0)
                    throw new ArgumentException("tile size must divided by the speed of enemy with a residue of 0");
                _movmentSpeed = value;
            }
        }
       

        public Enemy(double leftLocation, double topLocation, string imagePath,bool isOnJunction =true)
            : base(leftLocation, topLocation, imagePath)
        {
            Image.Height = Height;
            Image.Width = WIDTH;
            _IsOnAJunction = isOnJunction;
        }
        public virtual void EnemyMovmentLogic(TileObject[,] tileImageArray,Canvas can)
        {
            int row = 0, column = 0;
            if (IsEnemyOnJunction(tileImageArray, ref row, ref column))
            {
                GetFirstMovment(tileImageArray, row, column);
                return;
            }
            LeftLocation = Canvas.GetLeft(Image);
            TopLocation = Canvas.GetTop(Image);

            // according to first movment move until reach to the exact size of a tile
            if (BeforeMoveLeft < LeftLocation ) EnemyMoveRight();
            if (BeforeMoveTop < TopLocation ) EnemyMoveDown();
            if (BeforeMoveLeft > LeftLocation ) EnemyMoveLeft();
            if (BeforeMoveTop > TopLocation) EnemyMoveUp();

            double AfterMovmentWidth = Image.ActualWidth + MovmentSpeed;

            //secret path
            if (LeftLocation - MovmentSpeed < 0)
            {
                BeforeMoveLeft = can.ActualWidth;
                Canvas.SetLeft(Image, can.ActualWidth - AfterMovmentWidth);
                return;
            }
            //secret path
            if (LeftLocation + AfterMovmentWidth > can.ActualWidth)
            {
                BeforeMoveLeft = 0;
                Canvas.SetLeft(Image, MovmentSpeed);
                return;
            }
        }
        private bool IsEnemyOnJunction(TileObject[,] tileImageArray,ref int row, ref int column)
        {
            for (int i = 0; i < tileImageArray.GetLength(0); i++)
            {
                for (int j = 0; j < tileImageArray.GetLength(1); j++)
                {
                    if (IsOnJunctionF(tileImageArray[i,j]))
                    {
                        row = i;
                        column = j;
                        _IsOnAJunction = true;
                        return true;
                    }
                }
            }
            return false;
        }
        protected  void GetFirstMovment(TileObject[,] tileImageArray,int row,int column)
        {
                BeforeMoveLeft = Canvas.GetLeft(Image);
                BeforeMoveTop = Canvas.GetTop(Image);

                EnemiesDirection enemyR = new EnemiesDirection(DirectionE.Right);
                EnemiesDirection enemyL = new EnemiesDirection(DirectionE.Left);
                EnemiesDirection enemyU = new EnemiesDirection(DirectionE.Up);
                EnemiesDirection enemyD = new EnemiesDirection(DirectionE.Down);

                if (tileImageArray[row + 1, column].GetType() == typeof(BlackTile)) enemyD.IsEnemyCanGo = true;
                if (tileImageArray[row, column + 1].GetType() == typeof(BlackTile)) enemyR.IsEnemyCanGo = true;
                if (tileImageArray[row, column - 1].GetType() == typeof(BlackTile)) enemyL.IsEnemyCanGo = true;
                if (tileImageArray[row - 1, column].GetType() == typeof(BlackTile)) enemyU.IsEnemyCanGo = true;
                DirectionE direction = GEtDirection(enemyR, enemyL, enemyU, enemyD);
                MoveEnemy(direction);
        }
        private bool IsOnJunctionF(TileObject tileImageArray)
        {
            return tileImageArray.IsAJunction == true && Canvas.GetTop(Image) == Canvas.GetTop(tileImageArray.Image)
                && Canvas.GetLeft(Image) == Canvas.GetLeft(tileImageArray.Image);
        }
        private  DirectionE GEtDirection(params EnemiesDirection[] enemiesDirections)
        {
            Dictionary<int, DirectionE> directionAndKey = new Dictionary<int, DirectionE>();
            int countPossibilities = 0;
            foreach (var item in enemiesDirections)
            {
                if (item.IsEnemyCanGo)
                {
                    directionAndKey.Add(countPossibilities++, item.Direction);
                }
            }
            int randomOptionIndex = _random.Next(0, countPossibilities);
            return directionAndKey[randomOptionIndex];
        }
        protected void MoveEnemy(DirectionE direction)
        {
            if (direction == DirectionE.Down) EnemyMoveDown();
            if (direction == DirectionE.Up) EnemyMoveUp();
            if (direction == DirectionE.Right) EnemyMoveRight();
            if (direction == DirectionE.Left) EnemyMoveLeft();
        }
        protected void EnemyMoveUp()
        {
            Canvas.SetTop(Image, Canvas.GetTop(Image) - MovmentSpeed);
        }
        protected void EnemyMoveDown()
        {
            Canvas.SetTop(Image, Canvas.GetTop(Image) + MovmentSpeed);
        }
        protected void EnemyMoveLeft()
        {
            Canvas.SetLeft(Image, Canvas.GetLeft(Image) - MovmentSpeed);
        }
        protected void EnemyMoveRight()
        {
            Canvas.SetLeft(Image, Canvas.GetLeft(Image) + MovmentSpeed);
        }
        public virtual void SetToStartLocation()
        {
            Canvas.SetTop(Image,StartTLocation);
            Canvas.SetLeft(Image,StartLLocation);
        }
    }
}
