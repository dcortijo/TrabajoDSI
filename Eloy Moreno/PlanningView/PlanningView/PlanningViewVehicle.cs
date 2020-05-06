using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningView
{
    public class PlanningViewVehicle
    {
        public string vehicleImage;
        public string weaponImage;
        public string description;
        public int totalHealth;
        public int maxSpeed;
        public static List<PlanningViewVehicle> vehicleTemplates = new List<PlanningViewVehicle>()
        {
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\plus.png",
                weaponImage = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0
            },
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\helicopter_purple.png",
                weaponImage = "Assets\\helicopter_purple.png", // CHANGE
                description = "Helicóptero lento pero duro.",
                totalHealth = 200,
                maxSpeed = 60
            },
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\truck_purple.png",
                weaponImage = "Assets\\truck_purple.png", // CHANGE
                description = "Camión lento pero duro.",
                totalHealth = 250,
                maxSpeed = 55
            },
        };
        public static List<PlanningViewVehicle> vehicles = new List<PlanningViewVehicle>() {
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\plus.png",
                weaponImage = "Assets\\plus.png",
                description = "ADASD",
                totalHealth = 0,
                maxSpeed = 0
            },
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\plus.png",
                weaponImage = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0
            },
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\plus.png",
                weaponImage = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0
            },
            new PlanningViewVehicle()
            {
                vehicleImage = "Assets\\plus.png",
                weaponImage = "Assets\\plus.png",
                description = "",
                totalHealth = 0,
                maxSpeed = 0
            }
        };

        public static IList<PlanningViewVehicle> getAllVehicles()
        {
            return vehicles;
        }
    }
}
