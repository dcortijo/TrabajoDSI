using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace Hito3
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer;
        int timeLeft = 10;
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            SearchPanel.Visibility = Visibility.Collapsed;
        }

        private void Options_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Options), e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            SearchPanel.Visibility = Visibility.Visible;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += TimerTick;
            timer.Start();

        }
        void TimerTick(object sender, object e)
        {
            if (timeLeft > 0)
                timeLeft--;
            else if (timeLeft < 0)
                timeLeft = 0;

            if (timeLeft == 0)
            {
                this.Frame.Navigate(typeof(PlanningView), e);
                timer.Stop();
            }
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }
    }
}
