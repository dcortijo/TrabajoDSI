using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Hito3
{
    public class InGameVehicle
    {
        public enum vehicleType { plane, ship, car, truck, heli };  
        public enum weaponType { gun, harpoon, axe};
        public enum aligment { yours, ally, enemy };

        public int Id { get; set; }
        public string Imagen { get; set; }

        public vehicleType type;
        public weaponType weapon;
        public aligment team;

        public double maxHealth;
        public double health;

        public double maxOverheat;
        public double overheat;

        public double X { get; set; }
        public double Y { get; set; }

        public double[] direction = { 0, 0 };
        public double[] objective = { 0, 0 };

        public bool selected = false;

        public bool bomb;

        public InGameVehicle() { }
    }

    public class Model
    {
        public static List<InGameVehicle> Vehicles = new List<InGameVehicle>()
        {
            new InGameVehicle()
            {
                Id = 0,
                Imagen = "Assets\\jetYou.png",

                type = InGameVehicle.vehicleType.plane,
                weapon = InGameVehicle.weaponType.harpoon,
                team = InGameVehicle.aligment.yours,

                maxHealth = 100,
                health = 50,

                maxOverheat = 15,
                overheat = 7,

                X = 250,
                Y = 150,

                bomb =  true,
             },
            new InGameVehicle()
            {
                Id = 1,
                Imagen = "Assets\\carYou.png",

                type = InGameVehicle.vehicleType.car,
                weapon = InGameVehicle.weaponType.gun,
                team = InGameVehicle.aligment.yours,

                maxHealth = 150,
                health = 150,

                maxOverheat = 30,
                overheat = 30,

                X = 480,
                Y = 160,

                bomb = false,
             },
            new InGameVehicle()
            {
                Id = 2,
                Imagen = "Assets\\shipYou.png",

                type = InGameVehicle.vehicleType.truck,
                weapon = InGameVehicle.weaponType.gun,
                team = InGameVehicle.aligment.yours,

                maxHealth = 200,
                health = 160,

                maxOverheat = 30,
                overheat = 0,

                X = 140,
                Y = 500,

                bomb = false,
             },
            new InGameVehicle()
            {
                Id = 3,
                Imagen = "Assets\\shipYou.png",

                type = InGameVehicle.vehicleType.heli,
                weapon = InGameVehicle.weaponType.axe,
                team = InGameVehicle.aligment.yours,

                maxHealth = 100,
                health = 25,

                maxOverheat = 10,
                overheat = 2,

                X = 610,
                Y = 110,

                bomb = false,
             },
            new InGameVehicle()
            {
                Id = 4,
                Imagen = "Assets\\shipAlly.png",

                type = InGameVehicle.vehicleType.ship,
                weapon = InGameVehicle.weaponType.harpoon,
                team = InGameVehicle.aligment.ally,

                maxHealth = 200,
                health = 80,

                maxOverheat = 1,
                overheat = 0,

                X = 300,
                Y = 500,

                bomb = false,
             },
            new InGameVehicle()
            {
                Id = 5,
                Imagen = "Assets\\carEnemy.png",

                type = InGameVehicle.vehicleType.car,
                weapon = InGameVehicle.weaponType.harpoon,
                team = InGameVehicle.aligment.enemy,

                maxHealth = 100,
                health = 15,

                maxOverheat = 15,
                overheat = 7,

                X = 130,
                Y = 230,

                bomb = false,
             },
            new InGameVehicle()
            {
                Id = 6,
                Imagen = "Assets\\carEnemy.png",

                type = InGameVehicle.vehicleType.car,
                weapon = InGameVehicle.weaponType.harpoon,
                team = InGameVehicle.aligment.enemy,

                maxHealth = 100,
                health = 50,

                maxOverheat = 15,
                overheat = 7,

                X = 110,
                Y = 330,

                bomb = false,
             }

          };


        public static IList<InGameVehicle> GetAllVehicles()
        {
            return Vehicles;
        }

        public static InGameVehicle GetVehicleById(int id)
        {
            return Vehicles[id];
        }
    }
}
