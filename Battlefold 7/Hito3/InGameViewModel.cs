using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Hito3
{
    public class InGameVehicleViewModel : InGameVehicle
    {
        public Image Img;
        public ContentControl CCImg;
        public RotateTransform Rotacion;
        public int Angulo = 0;

        public int healthBar;
        public int overheatBar;

        public InGameVehicleViewModel(InGameVehicle vehicle)
        {
            Id = vehicle.Id;
            Imagen = vehicle.Imagen;

            type = vehicle.type;
            weapon = vehicle.weapon;
            team = vehicle.team;

            maxHealth = vehicle.maxHealth;
            health = vehicle.health;
            healthBar = (int)(100 * (health / maxHealth));

            maxOverheat = vehicle.maxOverheat;
            overheat = vehicle.overheat;
            overheatBar = (int)(100 * (overheat / maxOverheat));

            X = vehicle.X;
            Y = vehicle.Y;

            direction = vehicle.direction;

            Img = new Image();
            string s = System.IO.Directory.GetCurrentDirectory() + "\\" + vehicle.Imagen;
            Img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(s));
            Img.Width = 50;
            Img.Height = 50;

            CCImg = new ContentControl();
            CCImg.Content = Img;
            CCImg.UseSystemFocusVisuals = true;

            Rotacion = new RotateTransform();
            Rotacion.Angle = Angulo;
            Rotacion.CenterX = 25;
            Rotacion.CenterY = 25;
        }
    }
}
