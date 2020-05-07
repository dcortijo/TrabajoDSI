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

        public InGameVehicleViewModel(InGameVehicle vehicle)
        {
            Id = vehicle.Id;
            Imagen = vehicle.Imagen;

            type = vehicle.type;
            weapon = vehicle.weapon;
            team = vehicle.team;

            maxHealth = vehicle.maxHealth;
            health = vehicle.maxHealth;

            maxOverheat = vehicle.maxOverheat;
            overheat = vehicle.overheat;

            X = vehicle.X;
            Y = vehicle.Y;

            Img = new Image();
            string s = System.IO.Directory.GetCurrentDirectory() + "\\" + vehicle.Imagen;
            Img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(s));
            Img.Width = 50;
            Img.Height = 50;

            CCImg = new ContentControl();
            CCImg.Content = Img;
            CCImg.UseSystemFocusVisuals = true;
            //CCImg.Visibility = Windows.UI.Xaml.Visibility.Visible;//.Collapsed;
            //Mapa.Children.Add(CCImg);
            //Mapa.Children.Last().SetValue(Canvas.LeftProperty, X - 25);
            //Mapa.Children.Last().SetValue(Canvas.TopProperty, Y - 25);
            Rotacion = new RotateTransform();
            Rotacion.Angle = Angulo;
            Rotacion.CenterX = 25;
            Rotacion.CenterY = 25;
        }
    }
}
