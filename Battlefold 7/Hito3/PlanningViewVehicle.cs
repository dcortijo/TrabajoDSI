using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hito3
{
    public class PlanningViewVehicle : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string vehicleImage;
        public string description;
        public int totalHealth;
        public int maxSpeed;
        public PlanningViewWeapon weapon;
        public static List<PlanningViewVehicle> vehicleTemplates = new List<PlanningViewVehicle>()
        {
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0,
                weapon = PlanningViewWeapon.GetWeapon(0)
            },
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\shipYou.png",
                description = "Barco lento pero duro.",
                totalHealth = 200,
                maxSpeed = 60,
                weapon = PlanningViewWeapon.GetWeapon(0)

    },
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\carYou.png",
                description = "Coche rápido.",
                totalHealth = 150,
                maxSpeed = 50,
                weapon = PlanningViewWeapon.GetWeapon(0)

    },
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\jetYou.png",
                description = "Jet rápido pero frágil.",
                totalHealth = 50,
                maxSpeed = 90,
                weapon = PlanningViewWeapon.GetWeapon(0)
    },
        };
        public static List<PlanningViewVehicle> vehicles = new List<PlanningViewVehicle>() {
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0,
                weapon = PlanningViewWeapon.GetWeapon(0)
            },
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0,
                weapon = PlanningViewWeapon.GetWeapon(0)
            },
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0,
                weapon = PlanningViewWeapon.GetWeapon(0)
            },
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\plus.png",
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
