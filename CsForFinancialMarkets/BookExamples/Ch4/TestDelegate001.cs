// Delegate001.cs
//
// Showing the use of Delegates and lambda functions.
//
// (C) Datasim Education BV 2010
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public delegate double ComputableFunction(double x);    // The protocol

public class ArrayGenerator
{ // Create an array of values based on a customisable mathematical 
  // function that is implemented using delegates

        private int N;                                            // Size of array
        private ComputableFunction func;

        public ArrayGenerator(int size, ComputableFunction myFunction)
        {
            N = size;
            func = myFunction;
        }

        public double[] ComputeArray()
        {
            double[] result = new double[N];

            for (int j = 0; j < N; j++)
            {
                result[j] = func(Convert.ToDouble(j));
            }
            return result;
        }

        public void ComputeAndPrint()
        {
            double[] arr = ComputeArray();
            Console.WriteLine('\n');
            for (int j = 0; j < N; j++)
            {
                Console.Write(", {0}", arr[j]);
            }
        }
}



    class Delegates 
    {
       // delegate int Transformer(int k);
     //   delegate int NumericSequence();

        // Some delegate instances
        public static double Square(double x)
        {
            return x * x;
        }
        public static double Cube(double x)
        {
            return x * x * x;
        }
        public static double ModifiedExponential(double x)
        {
            return x * Math.Exp(x);
        }

        public void Print(double[] arr, int N)
        {
            Console.WriteLine('\n');
            for (int j = 0; j < N; j++)
            {
                Console.Write(", {0}", arr[j]);
            }
        }


        // Generic lambda expressions
        delegate T FuncOne<T>(T t);
        delegate T Func<T>(T t1, T t2);
        delegate void Action<T>(T t1, T t2);

        static void Main(string[] args)
        {

           
            // Using the class with delegate instances
            int N = 4;
            ArrayGenerator a1 = new ArrayGenerator (N, Square);
            ArrayGenerator a2 = new ArrayGenerator (N, Cube);
            ArrayGenerator a3 = new ArrayGenerator (N, ModifiedExponential);

            a1.ComputeAndPrint();
            a2.ComputeAndPrint();
            a3.ComputeAndPrint();

            // Lambda calculus; write Square, Cube and ModifiedExponential
            // in lambda form
            ComputableFunction SquareII = x => x * x;
            ComputableFunction CubeII = x => x * x * x;
            ComputableFunction ModifiedExponentialII = x => x * Math.Exp(x);

            int M = 3;
            ArrayGenerator A1 = new ArrayGenerator(N, SquareII);
            ArrayGenerator A2 = new ArrayGenerator(N, CubeII);
            ArrayGenerator A3 = new ArrayGenerator(N, ModifiedExponentialII);

            A1.ComputeAndPrint();
            A2.ComputeAndPrint();
            A3.ComputeAndPrint();

            // Using a statement block for lambda expressions
            ComputableFunction TrickyFunc = x => { double y = x * x; return y + 1; };

            M = 1;
            ArrayGenerator A4 = new ArrayGenerator(M, TrickyFunc);
            A4.ComputeAndPrint();

    /*        Transformer square = x => x * x;
            Console.WriteLine(square(3));
            // Anonymous mehod
            Transformer s2 = delegate (int x) {return x + x;};
            Console.WriteLine(s2(34));

            int seed = 0;
            NumericSequence natural = () => seed++;
            Console.WriteLine(natural());
            Console.WriteLine(natural());
            
            Console.WriteLine("///////////////////////Using GENERIC Lambda calculus");
            // Generics 
            // delegate T FuncOne<T>(T t);
            // delegate T Func<T>(T t1, T t2);
            // delegate void Action<T>(T t1, T t2);
            FuncOne<int> f1 = (int x) => x * x;
            Console.WriteLine(f1(3));

            FuncOne<double> f2 = (double x) => x * x;
            Console.WriteLine(f2(3.1415));

            Func<int> f3 = (int x, int y) => x + y;
            Console.WriteLine(f3(3, 4));
               */
        
          }
    }



 