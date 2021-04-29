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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PackmanOrigin
{
    
    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Game : Page
    {
        private int EnemiesSpeed;
        public Game()
        {
            this.InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void Easy_Click(object sender, RoutedEventArgs e)
        {
            EnemiesSpeed = 2;
            Frame.Navigate(typeof(MainPage),EnemiesSpeed);
        }

        private void Medium_Click(object sender, RoutedEventArgs e)
        {
            EnemiesSpeed = 3;
            Frame.Navigate(typeof(MainPage), EnemiesSpeed);
        }

        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            EnemiesSpeed = 4;
            Frame.Navigate(typeof(MainPage), EnemiesSpeed);
        }
    }
}
