﻿using System;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Hito3
{
    enum Tab { Graphics, Controls, Sound }
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Options : Page
    {
        Tab activeTab;
        public Options()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            activeTab = Tab.Graphics;
            changeState();
        }

        private void Graphics_Click(object sender, RoutedEventArgs e)
        {
            activeTab = Tab.Graphics;
            changeState();
        }

        private void Sound_Click(object sender, RoutedEventArgs e)
        {
            activeTab = Tab.Sound;
            changeState();
        }

        private void Controls_Click(object sender, RoutedEventArgs e)
        {
            activeTab = Tab.Controls;
            changeState();
        }

        void changeState()
        {
            Control_Tab.Visibility = Visibility.Collapsed;
            Sound_Tab.Visibility = Visibility.Collapsed;
            Graphic_Tab.Visibility = Visibility.Collapsed;
            switch (activeTab)
            {
                case Tab.Graphics:
                    Graphic_Tab.Visibility = Visibility.Visible;
                    break;
                case Tab.Controls:
                    Control_Tab.Visibility = Visibility.Visible;
                    break;
                case Tab.Sound:
                    Sound_Tab.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void Controls_Switch_Click(object sender, RoutedEventArgs e)
        {
            if (Controls_Switch.Content.ToString() == "Mouse and Keyboard")
                Controls_Switch.Content = "Controller";
            else Controls_Switch.Content = "Mouse and Keyboard";

        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), e);
        }
    }
}
