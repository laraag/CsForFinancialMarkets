using System;

namespace Exercise2
{
    class Program
    {
        static void Main(string[] args)
        {

            //RunExercise1();
            //RunExercise2();
            RunExercise3();
        }

        public static void RunExercise1()
        {
            double nan = 0.0f / 0.0f;

            if (double.IsInfinity(nan) || double.IsNaN(nan))
            {
                //It is faster to NOT try to calculate CDN, rather than trying, waiting for an exception to be thrown, and having to handle it. 
                Console.WriteLine("The number supplied to calcualte the CND is not valid.");
            }
            else
            {
                double d = SpecialFunctions.N(nan);
            }
        }

        public static void RunExercise2()
        {
            //Exercise 2
            Point p1 = new Point(1, 2);
            Point p2 = new Point(4, 3);
            double distance = p1.Distance(p2);

            LineSegment l = new LineSegment(p1, p2);
            LineSegment l2 = new LineSegment(l);

            l2.start(new Point(100, 500));
            Point notModified = l.start();
            Point midified = l2.start();


            LineSegment l3 = new LineSegment(new Point(10, 20), new Point(40, 30));

            Point ps = l.start();
            Point pe = l.end();

            l.start(l3.start());
            l.end(l3.end());

            double length = l.length();

            Point pMid = l.MidPoint();
        }

        public static void RunExercise3()
        {
            //Exercise 3
            // Major client delegates to the mediator (aka sub-contractor)
            OptionMediator med = new OptionMediator();
            med.calculate();


        }
    }
}