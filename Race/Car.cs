using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Race
{
    public class Car
    {
        int maxSpeed;

        public int MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }
        int speed;

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        int place;

        public int Place
        {
            get { return place; }
            set { place = value; }
        }
        double distance;

        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public Car(int mSpeed)
        {
            maxSpeed = mSpeed;
            speed = 0;
            distance = 0;
            place = 0;
        }
    }
}
