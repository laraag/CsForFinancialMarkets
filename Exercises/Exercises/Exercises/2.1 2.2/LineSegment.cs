using System;
using System.Collections.Generic;
using System.Text;

namespace Exercise2
{
    public struct LineSegment
    {
        private Point startPoint;
        private Point endPoint;

        //Constructors
        public LineSegment (Point p1, Point p2)
        {
            startPoint = p1;
            endPoint = p2;
        }
        public LineSegment (LineSegment l)
        {
            startPoint = l.startPoint;
            endPoint = l.endPoint;
        }

        //Accessing functions
        public Point start()
        {
            return startPoint;
        }
        public Point end()
        {
            return endPoint;
        }

        //Modifiers
        public void start(Point pt)
        {
            startPoint = pt;
        }
        public void end(Point pt)
        {
            endPoint = pt;
        }

        //Arithmetic
        public double length()
        {
            return startPoint.Distance(endPoint);
        }

        //interactions with points
        public Point MidPoint()
        {
            Point p = new Point(startPoint.x + ((endPoint.x - startPoint.x) / 2), startPoint.y + ((endPoint.y - startPoint.y) / 2));
            return p;
        }
        
    }
}
