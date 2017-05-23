// Delegate002.cs
//
// Not: Showing the use of Delegates and lambda functions.
// Showing how to achieve the same effect with interfaces.
//
// (C) Datasim Education BV 2010-2013
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

//public delegate double ComputableFunction(double x);    // The protocol

public interface ICompute
{
    double ComputableFunction(double x);
}

abstract public class ArrayGenerator : ICompute
{ // Create an array of values based on a customisable mathematical 
  // function that is implemented using delegates

        private int N;            // Size of array

        public ArrayGenerator(int size)
        {
            N = size;
        }

        public abstract double ComputableFunction(double x);

        public double[] ComputeArray()
        {
            double[] result = new double[N];

            for (int j = 0; j < N; j++)
            {
                result[j] = ComputableFunction(Convert.ToDouble(j));
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

public class Square : ArrayGenerator
{
    public Square(int N) : base(N) { }

    public override double ComputableFunction(double x)
    {
        return x * x;
    }
}

public class Cube : ArrayGenerator
{
    public Cube(int N) : base(N) { }

    public override double ComputableFunction(double x)
    {
        return x * x * x;
    }
}

  public class Interface
  {
        static void Main(string[] args)
        {

           
            int N = 4;
            ArrayGenerator a1 = new Square(N);
            ArrayGenerator a2 = new Cube(N);

            a1.ComputeAndPrint();
            a2.ComputeAndPrint();

        }
  }



   