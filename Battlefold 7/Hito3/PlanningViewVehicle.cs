using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Hito3
{
    public class PlanningViewVehicle : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        string _vehicleImageSource;
        public string vehicleImageSource
        {
            get
            {
                return _vehicleImageSource;
            }
            set
            {
                if(_vehicleImageSource != value)
                {
                    _vehicleImageSource = value;
                    //OnPropertyChanged();
                }
            }
        }

        string _description;
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        int _totalHealth = 0;
        public int totalHealth
        {
            get
            {
                return _totalHealth;
            }
            set
            {
                if (_totalHealth != value)
                {
                    _totalHealth = value;
                    OnPropertyChanged();
                }
            }
        }
        int _maxSpeed = 0;
        public int maxSpeed
        {
            get
            {
                return _maxSpeed;
            }
            set
            {
                if (_maxSpeed != value)
                {
                    _maxSpeed = value;
                    OnPropertyChanged();
                }
            }
        }
        public PlanningViewWeapon weapon;
        public static List<PlanningViewVehicle> vehicleTemplates = new List<PlanningViewVehicle>()
        {
            new PlanningViewVehicle()
            {
                vehicleImageSource = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0,
                weapon = PlanningViewWeapon.GetWeapon(0)
            },
            new PlanningViewVehicle()
            {
                vehicleImageSource = "Assets\\shipYou.png",
                description = "Barco lento pero duro.",
                totalHealth = 200,
                maxSpeed = 60,
                weapon = PlanningViewWeapon.GetWeapon(0)
            },
            new PlanningViewVehicle()
            {
                vehicleImageSource = "Assets\\carYou.png",
                description = "Coche rápido.",
                totalHealth = 150,
                maxSpeed = 50,
                weapon = PlanningViewWeapon.GetWeapon(0)
            },
            new PlanningViewVehicle()
            {
                vehicleImageSource = "Assets\\jetYou.png",
                description = "Jet rápido pero frágil.",
                totalHealth = 50,
                maxSpeed = 90,
                weapon = PlanningViewWeapon.GetWeapon(0)
            }
        };
        public static List<PlanningViewVehicle> vehicles = new List<PlanningViewVehicle>() {
            new PlanningViewVehicle()
            {
                vehicleImageSource = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0,
                weapon = PlanningViewWeapon.GetWeapon(0)
            },
            new PlanningViewVehicle()
            {
                vehicleImageSource = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0,
                weapon = PlanningViewWeapon.GetWeapon(0)
            },
            new PlanningViewVehicle()
            {
                vehicleImageSource = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0,
                weapon = PlanningViewWeapon.GetWeapon(0)
            },
            new PlanningViewVehicle()
            {
                vehicleImageSource = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0,
                weapon = PlanningViewWeapon.GetWeapon(0)
    }
};

        public static IList<PlanningViewVehicle> getAllVehicles()
        {
            return vehicles;
        }

        public static PlanningViewVehicle getVehicleTemplate(int index)
        {
            return vehicleTemplates[index];
        }
    }
}
