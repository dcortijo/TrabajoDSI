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
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Input;
using Windows.Gaming.Input;
using System.Drawing;
using Windows.UI;
using System.Data;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace Hito3
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class InGameMap : Page
    {
        //Vehículo activo
        int activeIndex = -1;
        int bombIndex = -1;
        DispatcherTimer clockTimer;
        int timeLeft = 200;
        DispatcherTimer updateTimer;

        public InGameMap()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<InGameVehicle> ListaVeh { get; } = new ObservableCollection<InGameVehicle>();
        public ObservableCollection<InGameVehicle> ListaYourVeh { get; } = new ObservableCollection<InGameVehicle>();
        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Carga la lista de ModelView a partir de la lista de Modelo              
            if (ListaVeh != null)
            {
                int i = 0;
                foreach (InGameVehicle dron in Model.GetAllVehicles())
                {
                    InGameVehicleViewModel VMitem = new InGameVehicleViewModel(dron);
                    ListaVeh.Add(VMitem);
                    if (i < 4) ListaYourVeh.Add(VMitem);

                    //Se añaden al canvas las imágenes que ya tienen los drones
                    //VMitem.CCImg.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    VMitem.CCImg.ManipulationMode = ManipulationModes.All;
                    (VMitem.CCImg.Content as Image).RenderTransform = VMitem.Rotacion;
                    //VMitem.CCImg.GotFocus += VehicleGotFocus;
                    //VMitem.CCImg.LostFocus += VehicleLostFocus;
                    //VMitem.CCImg.KeyDown += VehicleKeyDown;
                    MapCanvas.Children.Add(VMitem.CCImg);
                    MapCanvas.Children.Last().SetValue(Canvas.LeftProperty, VMitem.X);
                    MapCanvas.Children.Last().SetValue(Canvas.TopProperty, VMitem.Y);

                    HealthCanvas.Children.Add(new Border());
                    (HealthCanvas.Children.Last() as Border).Height = VMitem.healthBar / 3;
                    (HealthCanvas.Children.Last() as Border).Width = 6;
                    (HealthCanvas.Children.Last() as Border).Background = new SolidColorBrush(Windows.UI.Colors.Green);
                    (HealthCanvas.Children.Last() as Border).CornerRadius = new Windows.UI.Xaml.CornerRadius(5);
                    HealthCanvas.Children.Last().SetValue(Canvas.LeftProperty, VMitem.X - 5);
                    HealthCanvas.Children.Last().SetValue(Canvas.TopProperty, VMitem.Y + 10);

                    if (VMitem.team == InGameVehicle.aligment.yours)
                    {
                        MapCanvas.Children.Last().PointerPressed += ClickedVeh;

                        AmmoCanvas.Children.Add(new Border());
                        (AmmoCanvas.Children.Last() as Border).Height = VMitem.overheatBar / 3;
                        (AmmoCanvas.Children.Last() as Border).Width = 6;
                        (AmmoCanvas.Children.Last() as Border).Background = new SolidColorBrush(Windows.UI.Colors.Red);
                        (AmmoCanvas.Children.Last() as Border).Visibility = Visibility.Collapsed;
                        AmmoCanvas.Children.Last().SetValue(Canvas.LeftProperty, VMitem.X - 15);
                        AmmoCanvas.Children.Last().SetValue(Canvas.TopProperty, VMitem.Y + 10);

                        FlagsCanvas.Children.Add(new Image());
                        string aa = System.IO.Directory.GetCurrentDirectory() + "\\" + "Assets\\Flag.png";
                        (FlagsCanvas.Children.Last() as Image).Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(aa));
                        (FlagsCanvas.Children.Last() as Image).Visibility = Visibility.Collapsed;
                        (FlagsCanvas.Children.Last() as Image).Width = 35;
                        (FlagsCanvas.Children.Last() as Image).Height = 35;
                        FlagsCanvas.Children.Last().SetValue(Canvas.LeftProperty, VMitem.X);
                        FlagsCanvas.Children.Last().SetValue(Canvas.TopProperty, VMitem.Y);
                    }

                    i++;
                }
            }

            Image img = new Image();
            img.Visibility = Visibility.Collapsed;
            string s = System.IO.Directory.GetCurrentDirectory() + "\\" + "Assets\\bomb.jpg";
            img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(s));
            img.Width = 50;
            img.Height = 50;
            MapCanvas.Children.Add(img);


            /*Gamepad.GamepadAdded += (object sender, Gamepad e3) =>
            {
                // Check if the just-added gamepad is already in myGamepads; if it isn't, add
                // it.
                lock (myLock)
                {
                    bool gamepadInList = myGamepads.Contains(e3);
                    if (!gamepadInList)
                    {
                        myGamepads.Add(e3);
                    }
                }
            };

            Gamepad.GamepadRemoved += (object sender, Gamepad e32) =>
            {
                lock (myLock)
                {
                    int indexRemoved = myGamepads.IndexOf(e32);
                    if (indexRemoved > -1)
                    {
                        if (mainGamepad == myGamepads[indexRemoved])
                        {
                            mainGamepad = null;
                        }
                        myGamepads.RemoveAt(indexRemoved);
                    }
                }
            };*/

            DispatcherTimerSetup();
        }

        private void ClickedVeh(object sender, PointerRoutedEventArgs e)
        {
            int newIndex = MapCanvas.Children.IndexOf((e.OriginalSource as Image).Parent as ContentControl);
            if (newIndex < ListaYourVeh.Count() && newIndex > -1)
            {
                activeIndex = newIndex;
                ListViewVehicles.SelectedIndex = newIndex;
                for (int i = 0; i < ListaYourVeh.Count(); ++i)
                {
                    if (i != activeIndex) AmmoCanvas.Children[i].Visibility = Visibility.Collapsed;
                    else AmmoCanvas.Children[i].Visibility = Visibility.Visible;
                }
            }
        }

        private void ClickedMap(object sender, PointerRoutedEventArgs e)
        {
            if (activeIndex != -1)
            {
                FlagsCanvas.Children[activeIndex].SetValue(Canvas.TopProperty, e.GetCurrentPoint(this).Position.Y - 17);
                FlagsCanvas.Children[activeIndex].SetValue(Canvas.LeftProperty, e.GetCurrentPoint(this).Position.X - 17);
                FlagsCanvas.Children[activeIndex].Visibility = Visibility.Visible;
                ListaVeh[activeIndex].direction[0] = (int)(e.GetCurrentPoint(this).Position.X - 17) - ListaVeh[activeIndex].X;
                ListaVeh[activeIndex].direction[1] = (int)(e.GetCurrentPoint(this).Position.Y - 17) - ListaVeh[activeIndex].Y;
            }
        }

        private void ControlVehicleKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(activeIndex > -1)
            {
                switch (e.Key)
                {
                    case Windows.System.VirtualKey.B:
                        deactivateAllBombs();   //So no more than 1 bomb carrier
                        ListaVeh[activeIndex].bomb = !ListaVeh[activeIndex].bomb;
                        if (ListaVeh[activeIndex].bomb) {
                            bombIndex = activeIndex;
                            MapCanvas.Children.Last().Visibility = Visibility.Visible; 
                        }
                        else {
                            bombIndex = -1;
                            MapCanvas.Children.Last().Visibility = Visibility.Collapsed; 
                        }
                        break;
                    /*case Windows.System.VirtualKey.W:
                        ListaVeh[activeIndex].Y -= 5;
                        break;
                    case Windows.System.VirtualKey.A:
                        ListaVeh[activeIndex].X -= 5;
                        break;
                    case Windows.System.VirtualKey.S:
                        ListaVeh[activeIndex].Y += 5;
                        break;
                    case Windows.System.VirtualKey.D:
                        ListaVeh[activeIndex].X += 5;
                        break;*/
                    case Windows.System.VirtualKey.F:
                        this.Frame.Navigate(typeof(FirstPerson));
                        break;
                }
                //reposition();
            }
        }

        private void ListViewVehicles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activeIndex = ListViewVehicles.SelectedIndex;
        }

        /*private readonly object myLock = new object();
        private List<Gamepad> myGamepads = new List<Gamepad>();
        private Gamepad mainGamepad;*/

        void reposition(int i)
        {
            (MapCanvas.Children[i] as ContentControl).SetValue(Canvas.LeftProperty, ListaVeh[i].X);
            (MapCanvas.Children[i] as ContentControl).SetValue(Canvas.TopProperty, ListaVeh[i].Y);
            HealthCanvas.Children[i].SetValue(Canvas.LeftProperty, ListaVeh[i].X - 5);
            HealthCanvas.Children[i].SetValue(Canvas.TopProperty, ListaVeh[i].Y + 10);
            if(ListaVeh[i].team == InGameVehicle.aligment.yours)
            {
                AmmoCanvas.Children[i].SetValue(Canvas.LeftProperty, ListaVeh[i].X - 15);
                AmmoCanvas.Children[i].SetValue(Canvas.TopProperty, ListaVeh[i].Y + 10);
            }
            if(bombIndex > -1)
            {
                MapCanvas.Children.Last().SetValue(Canvas.LeftProperty, ListaVeh[i].X + 40);
                MapCanvas.Children.Last().SetValue(Canvas.TopProperty, ListaVeh[i].Y);
                checkIfBombInBase();
            }
        }

        bool checkIfBombInBase()
        {
            bool result = false;
            if (ListaVeh[bombIndex].team == InGameVehicle.aligment.ally || ListaVeh[bombIndex].team == InGameVehicle.aligment.yours) 
            {
                if(ListaVeh[bombIndex].X < 200 && ListaVeh[bombIndex].X > 100 && ListaVeh[bombIndex].Y > 100 && ListaVeh[bombIndex].Y < 200)
                {
                    int i = ListaVeh[-1].X; //Crash
                }
            }
            else
            {
                if (ListaVeh[bombIndex].X < 900 && ListaVeh[bombIndex].X > 800 && ListaVeh[bombIndex].Y > 600 && ListaVeh[bombIndex].Y < 700)
                {
                    int i = ListaVeh[-1].X; //Crash
                }
            }
            return result;
        }

        void deactivateAllBombs()
        {
            for (int i = 0; i < ListaVeh.Count(); ++i)
            {
                ListaVeh[i].bomb = false;
            }
        }

        private void DispatcherTimerSetup()
        {
            clockTimer = new DispatcherTimer();
            clockTimer.Interval = new TimeSpan(0, 0, 1);
            clockTimer.Tick += TimerTick;
            clockTimer.Start();
            Timer.Text = ((timeLeft / 60) <= 9 ? "0" : "") + (timeLeft / 60).ToString() + ":"
                + ((timeLeft % 60) <= 9 ? "0" : "") + (timeLeft % 60).ToString();

            updateTimer = new DispatcherTimer();
            updateTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            updateTimer.Tick += UpdateTick;
            updateTimer.Start();
        }

        void TimerTick(object sender, object e)
        {
            if (timeLeft > 0)
                timeLeft--;
            else if (timeLeft < 0)
                timeLeft = 0;
            Timer.Text = ((timeLeft / 60) <= 9 ? "0" : "") + (timeLeft / 60).ToString() + ":"
                + ((timeLeft % 60) <= 9 ? "0" : "") + (timeLeft % 60).ToString();
            if (timeLeft == 0)
            {
                clockTimer.Stop();
            }
        }

        void UpdateTick(object sender, object e)
        {
            if (timeLeft == 0)
            {
                updateTimer.Stop();
            }
            else
            {
                for(int i = 0; i < ListaYourVeh.Count(); ++i)
                {
                    if(ListaVeh[i].direction[0] != 0 && ListaVeh[i].direction[1] != 0)
                    {
                        ListaVeh[i].X += ListaVeh[i].direction[0];
                        ListaVeh[i].Y += ListaVeh[i].direction[1];
                    }
                    reposition(i);
                }
            }
        }
    }
}
