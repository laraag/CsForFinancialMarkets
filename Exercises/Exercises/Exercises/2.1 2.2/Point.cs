using System;
using System.Collections.Generic;
using System.Text;

namespace Exercise2
{
    public struct Point
    {
        public double x;
        public double y;

        //	public Point(){} // Illegal: default constructor automatically generated	

        public Point(double xVal, double yVal)
        { // Normal constructor

            // Constructor must initialize all fields
            x = xVal;
            y = yVal;
        }

        public override string ToString()
        { // Redefine this method from base class 'object'

            return string.Format("Point ({0}, {1})", x, y);
        }

        public double Distance (Point p2)
        {
            double distance = Math.Sqrt(Math.Pow(p2.x - x, 2) + Math.Pow(p2.y - y, 2));
            return distance;
        }
    }
}
