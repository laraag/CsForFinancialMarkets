// TestODE.cs
//
// Test ODE and finite difference solver.
//
// (C) Datasim Education BV 2010-2013
//

using System;

public class TestIVP
{

    public static void Main()
    {

        double A = 0.0;
        double T = 0.0;

        ODE myODE = new ODE(A, T);

        myODE.InitialCondition = 1.0;
        myODE.Expiry = 1.0;

        // Getting values of coefficients
        double t = 0.5;

        Console.WriteLine("[a(t), f(t)] values: [{0}, {1}]", myODE.a(t), myODE.f(t));

        // Calculate the FD scheme
        int N = 140; // Number of steps

        ExplicitEuler myFDM = new ExplicitEuler(N, myODE);

        myFDM.calculate();
        double val = myFDM.Value;
        Console.WriteLine("fdm value: {0}", val);
    }

}
