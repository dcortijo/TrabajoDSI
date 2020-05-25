using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.Input;
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
    public sealed partial class PlanningView : Page
    {
        public ObservableCollection<PlanningViewVehicleVM> ListaVehiculos { get; } = new ObservableCollection<PlanningViewVehicleVM>();
        DispatcherTimer timer;
        DispatcherTimer updateTimer;
        int timeLeft = 30;
        int selectedVehicle = 0;

        private readonly object myLock = new object();
        private List<Gamepad> myGamepads = new List<Gamepad>();
        private Gamepad mainGamepad;

        public PlanningView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (ListaVehiculos != null) // Carga el VM a partir del modelo
            {
                foreach (PlanningViewVehicle vehicle in PlanningViewVehicle.getAllVehicles())
                {
                    PlanningViewVehicleVM vm = new PlanningViewVehicleVM(vehicle);
                    ListaVehiculos.Add(vm);
                    vm.CCImg.Visibility = Visibility.Collapsed;
                    vm.CCImg.ManipulationMode = ManipulationModes.All;
                    vm.CCImg.GotFocus += GotFocus_CCImg;
                    VehicleMap.Children.Add(vm.CCImg);
                    VehicleMap.Children.Last().SetValue(Canvas.LeftProperty, vm.X);
                    VehicleMap.Children.Last().SetValue(Canvas.TopProperty, vm.Y);
                }
            }

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

        private void PointerPressed_CCImg(object sender, PointerRoutedEventArgs e)
        {
            if ((e.OriginalSource as Image) != null && ((e.OriginalSource as Image).Parent as ContentControl) != null)
            {
                if (e.GetCurrentPoint(VehicleMap).Properties.IsLeftButtonPressed)
                {
                    selectedVehicle = VehicleMap.Children.IndexOf((e.OriginalSource as Image).Parent as ContentControl);
                }
            }
        }

        private void PointerReleased_CCImg(object sender, PointerRoutedEventArgs e)
        {
            //selectedVehicle = -1;
        }

        private void PointerMoved_CCImg(object sender, PointerRoutedEventArgs e)
        {
            if (selectedVehicle != -1)
            {
                if (e.GetCurrentPoint(VehicleMap).Properties.IsLeftButtonPressed)
                {
                    ListaVehiculos[selectedVehicle].X = (int)e.GetCurrentPoint(VehicleMap).Position.X - (int)(VehicleMap.Children[selectedVehicle] as ContentControl).ActualWidth / 2;
                    ListaVehiculos[selectedVehicle].Y = (int)e.GetCurrentPoint(VehicleMap).Position.Y - (int)(VehicleMap.Children[selectedVehicle] as ContentControl).ActualHeight / 2;
                    VehicleMap.Children[selectedVehicle].SetValue(Canvas.LeftProperty, ListaVehiculos[selectedVehicle].X);
                    VehicleMap.Children[selectedVehicle].SetValue(Canvas.TopProperty, ListaVehiculos[selectedVehicle].Y);
                }
            }
        }
        private void MainGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (selectedVehicle != -1)
            {
                switch (e.Key)
                {
                    case Windows.System.VirtualKey.W:
                        MoveDronUp(5);
                        break;
                    case Windows.System.VirtualKey.A:
                        MoveDronLeft(5);
                        break;
                    case Windows.System.VirtualKey.S:
                        MoveDronDown(5);
                        break;
                    case Windows.System.VirtualKey.D:
                        MoveDronRight(5);
                        break;
                }
            }
        }

        private void MoveDronRight(int cant)
        {
            ListaVehiculos[selectedVehicle].X += cant;
            if (ListaVehiculos[selectedVehicle].X + (VehicleMap.Children[selectedVehicle] as ContentControl).ActualWidth >= VehicleMap.ActualWidth) ListaVehiculos[selectedVehicle].X = (int)VehicleMap.ActualWidth - (int)(VehicleMap.Children[selectedVehicle] as ContentControl).ActualWidth;
            VehicleMap.Children[selectedVehicle].SetValue(Canvas.LeftProperty, ListaVehiculos[selectedVehicle].X);
        }

        private void MoveDronDown(int cant)
        {
            ListaVehiculos[selectedVehicle].Y += cant;
            if (ListaVehiculos[selectedVehicle].Y + (VehicleMap.Children[selectedVehicle] as ContentControl).ActualHeight >= VehicleMap.ActualHeight) ListaVehiculos[selectedVehicle].Y = (int)VehicleMap.ActualHeight - (int)(VehicleMap.Children[selectedVehicle] as ContentControl).ActualWidth;
            VehicleMap.Children[selectedVehicle].SetValue(Canvas.TopProperty, ListaVehiculos[selectedVehicle].Y);
        }

        private void MoveDronLeft(int cant)
        {
            ListaVehiculos[selectedVehicle].X -= cant;
            if (ListaVehiculos[selectedVehicle].X <= 0) ListaVehiculos[selectedVehicle].X = 0;
            VehicleMap.Children[selectedVehicle].SetValue(Canvas.LeftProperty, ListaVehiculos[selectedVehicle].X);
        }

        private void MoveDronUp(int cant)
        {
            ListaVehiculos[selectedVehicle].Y -= cant;
            if (ListaVehiculos[selectedVehicle].Y <= 0) ListaVehiculos[selectedVehicle].Y = 0;
            VehicleMap.Children[selectedVehicle].SetValue(Canvas.TopProperty, ListaVehiculos[selectedVehicle].Y);
        }

        private void GotFocus_CCImg(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void DispatcherTimerSetup()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += TimerTick;
            timer.Start();
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
                BetweenPageParameter param = new BetweenPageParameter();
                param.Time = 200;
                this.Frame.Navigate(typeof(InGameMap), param);
                timer.Stop();
            }
        }

        void UpdateTick(object sender, object e)
        {
            if (mainGamepad != null && selectedVehicle != -1)
            {
                if (mainGamepad.GetCurrentReading().RightThumbstickY > 0.5)
                {
                    ListaVehiculos[selectedVehicle].Y -= 5;
                }
                else if (mainGamepad.GetCurrentReading().RightThumbstickY < -0.5)
                {
                    ListaVehiculos[selectedVehicle].Y += 5;
                }
                if (mainGamepad.GetCurrentReading().RightThumbstickX > 0.5)
                {
                    ListaVehiculos[selectedVehicle].X += 5;
                }
                else if (mainGamepad.GetCurrentReading().RightThumbstickX < -0.5)
                {
                    ListaVehiculos[selectedVehicle].X -= 5;
                }
            }
        }

        private void Jet_Button_Click(object sender, RoutedEventArgs e)
        {
            PlanningViewVehicle template = PlanningViewVehicle.getVehicleTemplate(3);
            SetVehicle(template);
        }
        private void Car_Button_Click(object sender, RoutedEventArgs e)
        {
            PlanningViewVehicle template = PlanningViewVehicle.getVehicleTemplate(2);
            SetVehicle(template);
        }

        private void SetVehicle(PlanningViewVehicle template)
        {
            ListaVehiculos[selectedVehicle].vehicleImageSource = template.vehicleImageSource;
            //string s = System.IO.Directory.GetCurrentDirectory() + "\\" + template.vehicleImageSource;
            //DataTemplate g = ((ListViewVehiculos.SelectedItem as DataTemplate));
            //Image i = g.FindName("VehicleImage") as Image;
            //i.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(s));
            ListaVehiculos[selectedVehicle].description = template.description;
            ListaVehiculos[selectedVehicle].totalHealth = template.totalHealth;
            ListaVehiculos[selectedVehicle].maxSpeed = template.maxSpeed;
        }

        private void Ship_Button_Click(object sender, RoutedEventArgs e)
        {
            PlanningViewVehicle template = PlanningViewVehicle.getVehicleTemplate(1);
            SetVehicle(template);
        }

        private void VehicleFlyout_Closed(object sender, object e)
        {
        }

        private void VehicleButton_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
            //selectedVehicle = VehicleMap.Children.IndexOf((sender as PlanningViewVehicleVM).CCImg);
        }

        private void WeaponButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
            //selectedVehicle = VehicleMap.Children.IndexOf((sender as PlanningViewVehicleVM).CCImg);
        }

        private void Pistol_Button_Click(object sender, RoutedEventArgs e)
        {
            //PlanningViewWeapon weapon = PlanningViewWeapon.GetWeapon(1);
            //ListaVehiculos[selectedVehicle].weapon = weapon;
        }

        private void Harpoon_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Axe_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WeaponFlyout_Closed(object sender, object e)
        {

        }

        private void ListViewVehiculos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedVehicle = ListViewVehiculos.SelectedIndex;
            foreach (PlanningViewVehicleVM VMitem in e.AddedItems)
                VMitem.CCImg.Visibility = Visibility.Visible;

        }
    }
}
