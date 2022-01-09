using System;

namespace cs_finance
{
    class Program
    {
        public struct Point
        {
            public double x;
            public double y;
            public Point(double xVal, double yVal)
            {
                // constructor
                x = xVal;
                y = yVal;

            }
            public override string ToString()
            {
                return string.Format("Point ({0}, {1}", x, y);
            }
        }

        static void Main(string[] args)
        {
            // use struct (different ways of calling it)
            Point p1;
            p1.x = 10; p1.y = 20;

            Point p2 = new Point(10, 20);
            Point p3 = p2;

            // print points
            Console.WriteLine("p1: {0}", p1);
            Console.WriteLine("p2: {0}", p2);
            Console.WriteLine("p3=p2: {0}", p3);

            // test; try to change p3, p2 shouldn't change
            p3.x = 1; p3.y = 2;
            Console.WriteLine("p2 see if changed: {0}", p2);
            Console.WriteLine("p3: {0}", p3);


        }
    }
}
