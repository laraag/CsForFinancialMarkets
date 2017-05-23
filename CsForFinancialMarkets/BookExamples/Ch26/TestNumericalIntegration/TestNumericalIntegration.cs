// TestNumericalIntegration.cs
//
// Multithreaded numerical integration.
//
// (C) Datasim Education BV 2009-2012 
//

using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections;

public class Integrator
{
    static double myFunction1(double x)
    { // Integral on (0,1) == pi^2/4 ~ 2.4672

         return Math.Log((1.0+x)/(1.0-x))/x;
    }
    
    static double myFunction2(double x)
    { // Integral on (0,1) == -pi^2/6 ~ - 1.644

         return Math.Log(x)/(1.0-x);
    }

    static double myFunction3(double x)
    { // Integral on (0,1) == -pi^2/8 ~ - 1.2336

        return Math.Log(x) / (1.0 - x*x);
    }

    static void Main(string[] args)
    {
        // Build Integrator function
	    Range<double> range = new Range<double> (0.0, 1.0);	// Region of integration
	    int N = 200;						        	    // Number of subdivisions
	
        //FunctionIntegrator fi1 = new FunctionIntegrator(myFunction1, range, N);
        //FunctionIntegrator fi2 = new FunctionIntegrator(myFunction2, range, N);
        FunctionIntegrator fi3 = new FunctionIntegrator(myFunction3, range, N);

        // V1: no return type; it is printed to to Console
        Thread t1 = new Thread(new ThreadStart(fi3.MidPoint));
        Thread t2 = new Thread(new ThreadStart(fi3.Tanh));
       
        t1.Start();
        t2.Start();
      
        t1.Join();
        t2.Join();
 
        Console.WriteLine("Done.");
    }
 
}

