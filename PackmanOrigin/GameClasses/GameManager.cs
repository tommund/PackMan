using PackmanOrigin.Classes;
using PackmanOrigin.GameClasses;
using PackmanOrigin.GameClasses.Enemies;
using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PackmanOrigin
{

    class GameManager
    {
        private readonly List<string> _keys;
        private readonly DispatcherTimer _timer;
        private readonly BoardGame _board;
        private readonly Canvas _canvas;
        private readonly TileObject[,] _tilesArray;
        private readonly List<Coin> _coins;
        private readonly List<Enemy> _enemies;
        private PackManCharacter _pc;
        private Cherry _cherry;
        private double _timeCounter;

        public GameManager(Canvas can)
        {
            _timeCounter = 0;
           
            _board = new BoardGame();
            _canvas = can;
            _tilesArray = new TileObject[_board.RowsLength, _board.ColumnsLength];
            _coins = new List<Coin>();
            _enemies = new List<Enemy>();
            _keys = new List<string>();
            InitializeCanvasBorders();

            _timer = new DispatcherTimer();
            Window.Current.CoreWindow.KeyDown += UserKeyDown;
            Window.Current.CoreWindow.KeyUp += UserKeyUp;
            _timer.Tick += GameLoop;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 10); // game itiration every 1/100 seconds

            
        }

        private void GameLoop(object sender, object e)
        {
            HandlePlayerMovment();
            EnemiesMovment();
            CreateCherry();
        }

        //initialize Game according to Game Board,update Ui and Fill Collections
        public void IinitializeGame()
        {
            int Leftlocation = TileObject.WIDTH;
            int topLocation = TileObject.Height;
            EnemiesImagesPath enemiesImagesPath = new EnemiesImagesPath();
            int Enemiesindex = 0;

            for (int i = 0; i < _board.RowsLength; i++)
            {
                for (int j = 0; j < _board.ColumnsLength; j++)
                {
                    if (_board[i, j] == 0)
                    {
                        BrickTile brick = new BrickTile(Leftlocation * j, topLocation * i, j, i);
                        _tilesArray[i, j] = brick;
                        brick.AddObjectToCanvas(_canvas);
                    }
                    else
                    {
                        BlackTile blackTile = new BlackTile(Leftlocation * j, topLocation * i, j, i);
                        if (!(_board[i, j] == 1)) blackTile.IsAJunction = true;
                        _tilesArray[i, j] = blackTile;
                        blackTile.AddObjectToCanvas(_canvas);

                        Coin coin = new Coin(Leftlocation * j, topLocation * i);
                        _coins.Add(coin);
                        coin.AddObjectToCanvas(_canvas);
                    }
                    if (_board[i, j] == 3) _enemies.Add(new Enemy(Leftlocation * j, topLocation * i, enemiesImagesPath[Enemiesindex++]));
                    if (_board[i, j] == 4) _enemies.Add(new SmartEnemy(Leftlocation * j, topLocation * i, i, j));
                    if (_board[i, j] == 5) _pc = new PackManCharacter(Leftlocation * j, topLocation * i, 3,i,j);
                }
            }
            for (int i = 0; i < _enemies.Count; i++) _enemies[i].AddObjectToCanvas(_canvas);
            _pc.AddObjectToCanvas(_canvas);

            Configuration.TotalPoints = 0;
            
        }
        private void RemoveMovingOFromCanvas()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].RemoveObjectFromCanvas(_canvas);
            }
            _pc.RemoveObjectFromCanvas(_canvas);
        }
        private void CreateCherry()
        {
            _timeCounter += _timer.Interval.TotalMilliseconds;
            if (_timeCounter % 1000 == 0 && (_cherry == null || _cherry.IsEaten))
            {
               _cherry =  new Cherry(_tilesArray);
                _cherry.AddObjectToCanvas(_canvas);
            }
        }
        private void HandleCherriesCollisoin()
        {
            Configuration.TotalPoints += Cherry.POINTS;
            MainPage.ChangeScoreText(Configuration.TotalPoints);
            _cherry.IsEaten = true;
            _cherry.RemoveObjectFromCanvas(_canvas);
        }
        private void EnemiesMovment()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].EnemyMovmentLogic(_tilesArray, _canvas);
            }
            
        }
        private void UserKeyUp(CoreWindow sender, KeyEventArgs args)
        {
            _keys.RemoveAll(key => key == args.VirtualKey.ToString());
        }
        private void UserKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            _keys.Add(args.VirtualKey.ToString());
        }
        private void HandlePlayerMovment()
        {
            //get top and left loacation of pacmanCharacter on canvas

            double left = Canvas.GetLeft(_pc.Image);
            double top = Canvas.GetTop(_pc.Image);

            _pc.IscollidingWithEnemies(left, top, _enemies, HandleEnemiesCollision);

            HandleRightMovment(left, top);
            HandleLeftMovment(left, top);
            HandleUpMovment(left, top);
            HandleDowmMovment(left, top);
        }
        private async void GameWon()
        {
            _timer.Stop();
            MainPage.Audio(true);
            string message = " YOU ARE THE WINNER, GOOD GAME!!!";

            // Initialize a new MessageDialog instance
            MessageDialog messageDialog = new MessageDialog(message);

            // Display the message dialog
            await messageDialog.ShowAsync();
        }
        private void HandleDowmMovment(double left, double top)
        {
            if (_keys.Contains("Down"))
            {
                if (!_pc.IscollidingWithBricksAndUpdateLocation(left, top + PackManCharacter.MOVMENT_SPEED, _tilesArray))
                {
                    Canvas.SetTop(_pc.Image, top + PackManCharacter.MOVMENT_SPEED);
                    _pc.IsCollidingWithCoins(left, top + PackManCharacter.MOVMENT_SPEED, _coins, _canvas, GameWon);
                    _pc.IscollidingWithEnemies(left, top + PackManCharacter.MOVMENT_SPEED, _enemies, HandleEnemiesCollision);
                    _pc.IsCollidingWithCherry(_cherry, left, top + PackManCharacter.MOVMENT_SPEED, HandleCherriesCollisoin);
                }
            }
        }
        private void HandleUpMovment(double left, double top)
        {
            if (_keys.Contains("Up"))
            {
                if (!_pc.IscollidingWithBricksAndUpdateLocation(left, top - PackManCharacter.MOVMENT_SPEED, _tilesArray))
                {
                    Canvas.SetTop(_pc.Image, top - PackManCharacter.MOVMENT_SPEED);
                    _pc.IsCollidingWithCoins(left, top - PackManCharacter.MOVMENT_SPEED, _coins, _canvas, GameWon);
                    _pc.IscollidingWithEnemies(left, top - PackManCharacter.MOVMENT_SPEED, _enemies, HandleEnemiesCollision);
                    _pc.IsCollidingWithCherry(_cherry, left, top - PackManCharacter.MOVMENT_SPEED, HandleCherriesCollisoin);
                }
            }
        }
        private void HandleLeftMovment(double left, double top)
        {
            if (_keys.Contains("Left"))
            {
                // check if in secret path
                if ( left - PackManCharacter.MOVMENT_SPEED < 0)
                {
                    _pc.SetToRSecretPath(_canvas);
                    return;
                }
                if (!_pc.IscollidingWithBricksAndUpdateLocation(left - PackManCharacter.MOVMENT_SPEED, top, _tilesArray))
                {
                    Canvas.SetLeft(_pc.Image, left - PackManCharacter.MOVMENT_SPEED);
                    _pc.IsCollidingWithCoins(left - PackManCharacter.MOVMENT_SPEED, top, _coins, _canvas, GameWon);
                    _pc.IscollidingWithEnemies(left - PackManCharacter.MOVMENT_SPEED, top, _enemies, HandleEnemiesCollision);
                    _pc.IsCollidingWithCherry(_cherry, left - PackManCharacter.MOVMENT_SPEED, top, HandleCherriesCollisoin);
                }
            }
        }
        private void HandleRightMovment(double left, double top)
        {
            if (_keys.Contains("Right"))
            {
                //secret path
                if (top >= TileObject.Height * SecretPath.RIGHT_ROW_IND && left + _pc.Image.ActualWidth + PackManCharacter.MOVMENT_SPEED > _canvas.ActualWidth)
                {
                    _pc.SetToLSecretPath();
                    return;
                }
                if (!_pc.IscollidingWithBricksAndUpdateLocation(left + PackManCharacter.MOVMENT_SPEED, top, _tilesArray))
                {
                    Canvas.SetLeft(_pc.Image, left + PackManCharacter.MOVMENT_SPEED);
                    _pc.IsCollidingWithCoins(left + PackManCharacter.MOVMENT_SPEED, top, _coins, _canvas, GameWon);
                    _pc.IscollidingWithEnemies(left + PackManCharacter.MOVMENT_SPEED, top, _enemies, HandleEnemiesCollision);
                    _pc.IsCollidingWithCherry(_cherry, left + PackManCharacter.MOVMENT_SPEED, top, HandleCherriesCollisoin);
                }
            }
        }
        public void StartGame() => _timer.Start();
        public void StopTimer() => _timer.Stop();
        private void InitializeCanvasBorders()
        {
            _canvas.Width = TileObject.WIDTH * _board.ColumnsLength;
            _canvas.Height = TileObject.WIDTH * _board.RowsLength;
        }
        public void StartNewGame()
        {
            _timeCounter = 0;
            Configuration.TotalPoints = 0;
            _pc.Lives = _pc.StartLives;
            InitCoinsList();
            RemoveMovingOFromCanvas();
            AddMovingObjectsToCanvas();
            SetToStartPosition();
            MainPage.ChangeScoreText(0);
            MainPage.ChangeLivesText(_pc.StartLives);
            MainPage.Audio(false);
            StartGame();
        }
        private void InitCoinsList()
        {
           Coin.TotalCoins = 0;
            for (int i = 0; i < _coins.Count; i++)
            {
                if (_coins[i].IsEaten)
                {
                    _coins[i].IsEaten = false;
                    _coins[i].AddObjectToCanvas(_canvas);
                }
                Coin.TotalCoins++;
            }
        }
        private void AddMovingObjectsToCanvas()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].AddObjectToCanvas(_canvas);
            }
            _pc.AddObjectToCanvas(_canvas);
        }
        private async void HandleEnemiesCollision()
        {
            _timer.Stop();
            _keys.Clear();
            _pc.Lives--;
            MainPage.ChangeLivesText(_pc.Lives);
            SetToStartPosition();
            if (_pc.Lives == 0)
            {
                _timer.Stop();
                // Initialize a new text for message dialog
                string message = "GAME  OVER!!!";

                // Initialize a new MessageDialog instance
                MessageDialog messageDialog = new MessageDialog(message);
               

                // Display the message dialog
                 await messageDialog.ShowAsync();
            }
                
            else _timer.Start();
        }
        private void SetToStartPosition()
        {
            for (int j = 0; j < _enemies.Count; j++)
            {
                _enemies[j].SetToStartLocation();
            }
            _pc.SetToStartLocation();
        }
    }
}
