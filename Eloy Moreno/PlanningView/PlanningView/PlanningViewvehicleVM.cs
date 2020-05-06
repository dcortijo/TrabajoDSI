using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PlanningView
{
    public class PlanningViewVehicleVM : PlanningViewVehicle
    {
        public ContentControl CCImg;
        public ContentControl CCWeaponImg;
        public Image img;
        public Image weaponImg;

        public PlanningViewVehicleVM(PlanningViewVehicle v)
        {
            img = new Image();
            string s = System.IO.Directory.GetCurrentDirectory() + "\\" + v.vehicleImage;
            img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(s));
            img.Width = 100;
            img.Height = 100;
            CCImg = new ContentControl();
            CCImg.Content = img;
            CCImg.UseSystemFocusVisuals = true;
            weaponImg = new Image();
            s = System.IO.Directory.GetCurrentDirectory() + "\\" + v.weaponImage;
            weaponImg.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(s));
            weaponImg.Width = 50;
            weaponImg.Height = 50;
            CCWeaponImg = new ContentControl();
            CCWeaponImg.Content = weaponImg;
            CCWeaponImg.UseSystemFocusVisuals = true;

            vehicleImage = v.vehicleImage;
            weaponImage = v.weaponImage;
            description = v.description;
            totalHealth = v.totalHealth;
            maxSpeed = v.maxSpeed;
        }
    }
}
