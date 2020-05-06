using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace PlanningView
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<PlanningViewVehicleVM> ListaVehiculos { get; } = new ObservableCollection<PlanningViewVehicleVM>();
        DispatcherTimer timer;
        int timeLeft = 120;

        public MainPage()
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
                }
            }
            DispatcherTimerSetup();
        }

        private void DispatcherTimerSetup()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += TimerTick;
            timer.Start();
            Timer.Text = ((timeLeft / 60) <= 9 ? "0" : "") + (timeLeft / 60).ToString() + ":"
                + ((timeLeft % 60) <= 9 ? "0" : "") + (timeLeft % 60).ToString();
        }

        void TimerTick(object sender, object e)
        {
            timeLeft--;
            Timer.Text = ((timeLeft / 60) <= 9 ? "0" : "") + (timeLeft / 60).ToString() + ":"
                + ((timeLeft % 60) <= 9 ? "0" : "") + (timeLeft % 60).ToString();
        }
    }
}
