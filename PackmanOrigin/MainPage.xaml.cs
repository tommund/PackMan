using PackmanOrigin.GameClasses.Enemies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PackmanOrigin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GameManager _gameManager;
        private static TextBlock _scoreTextBlock;
        private static TextBlock _LivesBlock;
        private static MediaElement _weAreTheChampion;
        public MainPage()
        {
            this.InitializeComponent();
            _scoreTextBlock = score;
            _LivesBlock = Lives;
            _weAreTheChampion = weAreTheChamphions;
            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            try
            {
                Enemy.MovmentSpeed = (int)e.Parameter;
            }
            catch (Exception exe)
            {
                _ = exe.Message;
            }


            _gameManager = new GameManager(canvas);
            _gameManager.IinitializeGame();
            _gameManager.StartNewGame();
        }

        private void New_Game(object sender, RoutedEventArgs e)
        {
            _gameManager.StartNewGame();
        }

        public static void ChangeScoreText(int result)
        {
            _scoreTextBlock.Text = $"SCORE: {result}";
        }

        public static void ChangeLivesText(int result)
        {
            _LivesBlock.Text = $"Lives: {result}";
        }

        public static void Audio(bool isGameWon)
        {
            if (isGameWon) _weAreTheChampion.Play();
            else _weAreTheChampion.Stop();
        }

        private void Home_PAGE(object sender, RoutedEventArgs e)
        {
            _gameManager.StopTimer();
            Frame.Navigate(typeof(Game));
        }
    }
}
