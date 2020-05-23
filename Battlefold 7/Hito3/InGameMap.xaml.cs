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
    public sealed partial class InGameMap : Page, INotifyPropertyChanged
    {
        //Vehículo activo
        int activeIndex = -1;
        int bombIndex = -1;
        DispatcherTimer clockTimer;
        int totalTime = 200;
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
                    VMitem.CCImg.GotFocus += VehicleGotFocus;
                    MapCanvas.Children.Add(VMitem.CCImg);
                    MapCanvas.Children.Last().SetValue(Canvas.LeftProperty, VMitem.X);
                    MapCanvas.Children.Last().SetValue(Canvas.TopProperty, VMitem.Y);

                    HealthCanvas.Children.Add(new Border());
                    (HealthCanvas.Children.Last() as Border).Height = VMitem.healthBar / 3;
                    (HealthCanvas.Children.Last() as Border).Width = 6;
                    (HealthCanvas.Children.Last() as Border).Background = new SolidColorBrush(Windows.UI.Colors.YellowGreen);
                    (HealthCanvas.Children.Last() as Border).CornerRadius = new Windows.UI.Xaml.CornerRadius(5);
                    HealthCanvas.Children.Last().SetValue(Canvas.LeftProperty, VMitem.X - 5);
                    HealthCanvas.Children.Last().SetValue(Canvas.TopProperty, VMitem.Y + 10);

                    if (VMitem.team == InGameVehicle.aligment.yours)
                    {
                        MapCanvas.Children.Last().PointerPressed += ClickedVeh;
                        MapCanvas.Children.Last().XYFocusKeyboardNavigation = XYFocusKeyboardNavigationMode.Enabled;
                        (MapCanvas.Children.Last() as ContentControl).IsTabStop = true;

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
                    else
                    {
                        MapCanvas.Children.Last().XYFocusKeyboardNavigation = XYFocusKeyboardNavigationMode.Disabled;
                        (MapCanvas.Children.Last() as ContentControl).IsTabStop = false;
                    }

                    i++;
                }
            }

            Image img = new Image();
            img.Visibility = Visibility.Collapsed;
            string s = System.IO.Directory.GetCurrentDirectory() + "\\" + "Assets\\bomb.png";
            img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(s));
            img.Width = 50;
            img.Height = 50;
            MapCanvas.Children.Add(img);

            timeLeft = (e.Parameter as BetweenPageParameter).Time;

            Gamepad.GamepadAdded += (object sender, Gamepad e3) =>
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
                mainGamepad = myGamepads[0];
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
            };

            DispatcherTimerSetup();
        }

        public double PanelSize
        {
            get => _panelSize;
            set
            {
                if (_panelSize != value)
                {
                    _panelSize = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PanelSize)));
                }
            }
        }
        private double _panelSize = 20;

        private void ClickedVeh(object sender, PointerRoutedEventArgs e)
        {
            int newIndex = MapCanvas.Children.IndexOf((e.OriginalSource as Image).Parent as ContentControl);
            if (newIndex < ListaYourVeh.Count() && newIndex > -1)
            {
                activeIndex = newIndex;
                ListViewVehicles.SelectedIndex = newIndex;
                updateAmmo();
            }
        }

        private void ClickedMap(object sender, PointerRoutedEventArgs e)
        {
            if (activeIndex != -1)
            {
                FlagsCanvas.Children[activeIndex].SetValue(Canvas.TopProperty, e.GetCurrentPoint(this).Position.Y - 17);
                FlagsCanvas.Children[activeIndex].SetValue(Canvas.LeftProperty, e.GetCurrentPoint(this).Position.X - 17);
                FlagsCanvas.Children[activeIndex].Visibility = Visibility.Visible;
                ListaVeh[activeIndex].direction[0] = (e.GetCurrentPoint(this).Position.X - 17) - ListaVeh[activeIndex].X;
                ListaVeh[activeIndex].direction[1] = (e.GetCurrentPoint(this).Position.Y - 17) - ListaVeh[activeIndex].Y;
                ListaVeh[activeIndex].objective[0] = e.GetCurrentPoint(this).Position.X;
                ListaVeh[activeIndex].objective[1] = e.GetCurrentPoint(this).Position.Y;
            }
        }

        private void ControlVehicleKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(activeIndex > -1)
            {
                switch (e.Key)
                {
                    case Windows.System.VirtualKey.B:
                    case Windows.System.VirtualKey.GamepadRightShoulder:
                        ListaVeh[activeIndex].bomb = true;
                        bombIndex = activeIndex;
                        MapCanvas.Children.Last().Visibility = Visibility.Visible;
                        break;
                    case Windows.System.VirtualKey.V:
                    case Windows.System.VirtualKey.GamepadLeftShoulder:
                        deactivateAllBombs();   //So no more than 1 bomb carrier
                        bombIndex = -1;
                        MapCanvas.Children.Last().Visibility = Visibility.Collapsed;
                        break;
                    case Windows.System.VirtualKey.W:
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
                        break;
                    case Windows.System.VirtualKey.F:
                    case Windows.System.VirtualKey.GamepadX:
                        BetweenPageParameter param = new BetweenPageParameter();
                        param.Time = timeLeft;
                        this.Frame.Navigate(typeof(FirstPerson), param);
                        break;
                }
                //reposition();
            }
        }

        private void ListViewVehicles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activeIndex = ListViewVehicles.SelectedIndex;
            updateAmmo();
        }

        private readonly object myLock = new object();
        private List<Gamepad> myGamepads = new List<Gamepad>();
        private Gamepad mainGamepad;

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
        }

        bool checkIfBombInBase()
        {
            bool result = false;
            if (ListaVeh[bombIndex].team == InGameVehicle.aligment.ally || ListaVeh[bombIndex].team == InGameVehicle.aligment.yours) 
            {
                if(ListaVeh[bombIndex].X < 200 && ListaVeh[bombIndex].X > 100 && ListaVeh[bombIndex].Y > 100 && ListaVeh[bombIndex].Y < 200)
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
            }
            else
            {
                if (ListaVeh[bombIndex].X < 900 && ListaVeh[bombIndex].X > 800 && ListaVeh[bombIndex].Y > 600 && ListaVeh[bombIndex].Y < 700)
                {
                    this.Frame.Navigate(typeof(MainPage));
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
            bombIndex = -1;
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
            updateTimer.Interval = new TimeSpan(0, 0, 0, 0, 33);
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
                this.Frame.Navigate(typeof(MainPage));
                updateTimer.Stop();
            }
            else
            {
                if(timeLeft <= totalTime - 5)
                {
                    BeginText.Visibility = Visibility.Collapsed;
                }

                for(int i = 0; i < ListaYourVeh.Count(); ++i)
                {
                    if(ListaVeh[i].direction[0] != 0 && ListaVeh[i].direction[1] != 0)
                    {
                        ListaVeh[i].X += ListaVeh[i].direction[0] * 0.05;
                        ListaVeh[i].Y += ListaVeh[i].direction[1] * 0.05;
                    }
                    reposition(i);
                    if(ListaVeh[i].X + 20 < ListaVeh[i].objective[0] + 17 && ListaVeh[i].X + 20 > ListaVeh[i].objective[0] - 17 &&
                        ListaVeh[i].Y + 20 < ListaVeh[i].objective[1] + 17 && ListaVeh[i].Y + 20 > ListaVeh[i].objective[1] - 17)
                    {
                        ListaVeh[i].direction[0] = ListaVeh[i].direction[1] = 0;
                        ListaVeh[i].objective[0] = ListaVeh[i].objective[1] = 0;
                        FlagsCanvas.Children[i].Visibility = Visibility.Collapsed;
                    }
                }
                if (bombIndex > -1)
                {
                    MapCanvas.Children.Last().SetValue(Canvas.LeftProperty, ListaVeh[bombIndex].X + 40);
                    MapCanvas.Children.Last().SetValue(Canvas.TopProperty, ListaVeh[bombIndex].Y);
                    checkIfBombInBase();
                }
            }
            if (mainGamepad != null && activeIndex != -1)
            {
                if (mainGamepad.GetCurrentReading().RightThumbstickY > 0.5)
                {
                    ListaVeh[activeIndex].Y -= 5;
                }
                else if (mainGamepad.GetCurrentReading().RightThumbstickY < -0.5)
                {
                    ListaVeh[activeIndex].Y += 5;
                }
                if (mainGamepad.GetCurrentReading().RightThumbstickX > 0.5)
                {
                    ListaVeh[activeIndex].X += 5;
                }
                else if (mainGamepad.GetCurrentReading().RightThumbstickX < -0.5)
                {
                    ListaVeh[activeIndex].X -= 5;
                }
            }
        }

        void updateAmmo()
        {
            for (int i = 0; i < ListaYourVeh.Count(); ++i)
            {
                if (i != activeIndex) AmmoCanvas.Children[i].Visibility = Visibility.Collapsed;
                else AmmoCanvas.Children[i].Visibility = Visibility.Visible;
            }
        }

        private void page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PanelSize = e.NewSize.Width / ListaYourVeh.Count();
        }

        private void VehicleGotFocus(object sender, RoutedEventArgs e)
        {
            activeIndex = MapCanvas.Children.IndexOf(sender as ContentControl);
            ListViewVehicles.SelectedIndex = activeIndex;
        }
    }
}
