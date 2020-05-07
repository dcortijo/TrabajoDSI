using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningView
{
    public class PlanningViewWeapon
    {
        public int ammo;
        public int damage;
        public string weaponDescription;
        public string weaponImage;

        public static List<PlanningViewWeapon> WeaponTemplates = new List<PlanningViewWeapon>()
        {            
            new PlanningViewWeapon()
            {
                ammo = 0,
                damage = 0,
                weaponDescription = "",
                weaponImage = "Assets\\plus.png"

            },
            new PlanningViewWeapon()
            {
                ammo = 100,
                damage = 20,
                weaponDescription = "Mucho rango, poco daño.",
                weaponImage = "Assets\\pistol_icon.png"

            }
        };

        public static PlanningViewWeapon GetWeapon(int index)
        {
            return WeaponTemplates[index];
        }
    }
}
