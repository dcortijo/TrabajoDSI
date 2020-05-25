using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input;
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
    public sealed partial class FirstPerson : Page
    {
        static bool firstPerson;
        bool open = false;

        int red = 205;
        int diffRed = 50;
        int green = 154;
        int diffGreen = 154;

        int maxHealth = 100;
        int health;
        double healthWidth;

        int seconds = 600;
        DispatcherTimer dispatcherTimer;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            seconds = (e.Parameter as BetweenPageParameter).Time;
        }
            public FirstPerson()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            StartTimers();

            healthWidth = healthBar.Width;
            health = maxHealth;
            firstPerson = true;
        }

        void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            switch (e.VirtualKey)
            {
                case Windows.System.VirtualKey.Escape:
                case Windows.System.VirtualKey.GamepadB:
                    //Se cambia de página

                    if (firstPerson)
                    {
                        firstPerson = false;
                        BetweenPageParameter param = new BetweenPageParameter();
                        param.Time = seconds;
                        this.Frame.Navigate(typeof(InGameMap), param);
                    }
                    break;
                case Windows.System.VirtualKey.Space:
                    //Daño demostrativo de la barra de vida
                    UpdateHealth(10);
                    break;
                case Windows.System.VirtualKey.Tab:
                    //Se expande/se minimiza el mapa
                    UpdateMap();
                    break;
                default:
                    break;
            }
        }

        public void StartTimers()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            UpdateClock();
            dispatcherTimer.Start();
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            if (seconds > 0)
            {
                seconds--;
                UpdateClock();
            }
            else
                ;//this.Frame.Navigate(typeof(Page2));
        }

        private void UpdateClock()
        {
            string text = "";
            int minutes = (seconds / 60);
            if (minutes < 10) text += "0"; text += minutes + ":";
            int secs = (seconds % 60);
            if (secs < 10) text += "0"; text += secs;
            clock.Text = text;
        }

        private void UpdateMap()
        {
            if (open)
            {
                expandedMap.Opacity = 0.0;
                expandedJet.Opacity = 0.0;
            }
            else
            {
                expandedMap.Opacity = 1.0;
                expandedJet.Opacity = 1.0;
            }
            open = !open;
        }

        private void UpdateHealth(double dmg)
        {
            if (health > 0)
            {
                if (health - (int)dmg < 0) health = 0;
                else health -= (int)dmg;
                healthText.Text = health + "%";

                //Verde #FF000800 a Rojo #FF800000
                string color = "#ff";
                int diff;

                //Valor rojo
                diff = (int)(diffRed * (dmg / maxHealth));
                red += diff; if (red > 255) red = 255;
                color += Convert.ToString(red, 16);

                //Valor verde
                diff = (int)(diffGreen * (dmg / maxHealth));
                green -= diff; if (green < 0) green = 0;
                if (green < 16) color += "0";
                color += Convert.ToString(green, 16);

                color += "00";

                healthText.Foreground = GetSolidColorBrush(color);
                if ((healthBar.Width - healthWidth * (dmg / maxHealth)) < 0) healthBar.Width = 0;
                else healthBar.Width -= healthWidth * (dmg / maxHealth);
                healthBar.Background = GetSolidColorBrush(color);
            }
        }

        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }
    }
}
