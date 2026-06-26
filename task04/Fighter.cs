using System;

namespace task04
{
    public class Fighter : ISpaceship
    {
        public int Speed { get; } = 100;
        public int FirePower { get; } = 50;

        public int Angle;
        public double X;
        public double Y;
        public int Ammo;

        public Fighter()
        {
            Angle = 0;
            X = 0.0;
            Y = 0.0;
            Ammo = 10;
        }

        public void MoveForward()
        {
            double radians = Angle * Math.PI / 180.0;
            X += Speed * Math.Cos(radians);
            Y += Speed * Math.Sin(radians);
        }

         public void Rotate(int angle)
        {
            Angle = (Angle + angle) % 360;
            if (Angle < 0) 
            {
                Angle += 360;
            }
        }

        public void Fire()
        {
            if (Ammo > 0)
            {
                Ammo -= 1;
            }
        }

    }
}