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
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Options_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Options), e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Friends_Back.Visibility = Visibility.Visible;
            Friends_Tab.Visibility = Visibility.Visible;
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PlanningView), e);
        }
        
        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        private void Exit_Friends_Click(object sender, RoutedEventArgs e)
        {
            Friends_Back.Visibility = Visibility.Collapsed;
            Friends_Tab.Visibility = Visibility.Collapsed;
        }
    }
}
