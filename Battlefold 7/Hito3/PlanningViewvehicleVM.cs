using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Hito3
{
    public class PlanningViewVehicleVM : PlanningViewVehicle
    {
        public ContentControl CCImg;
        public Image img;
        public int X, Y;
        public PlanningViewVehicleVM(PlanningViewVehicle v)
        {
            img = new Image();
            string s = System.IO.Directory.GetCurrentDirectory() + "\\" + v.vehicleImageSource;
            img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(s));
            img.Width = 100;
            img.Height = 100;
            CCImg = new ContentControl();
            CCImg.Content = img;
            CCImg.UseSystemFocusVisuals = true;

            vehicleImageSource = v.vehicleImageSource;
            weapon = v.weapon;
            description = v.description;
            totalHealth = v.totalHealth;
            maxSpeed = v.maxSpeed;

            X = 100;
            Y = 100;
        }
    }
}
