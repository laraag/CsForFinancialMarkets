using System;

class Test101
{  // We can use this as a template for other problems in computational finance
    // (see exercises in the Germani/Duffy book). Other examples used for curve fitting.

    public static void function1_func(double[] x, ref double func, object obj)
    {  // Function

        func = (x[0] - 1.0) * (x[0] - 1.0) + (x[1] - 0.5) * (x[1] - 0.5);
    }

    public static void function1_grad(double[] x, ref double func, double[] grad, object obj)
    {  // Function and gradient

        func = (x[0] - 1.0) * (x[0] - 1.0) + (x[1] - 0.5) * (x[1] - 0.5);

        grad[0] = 2.0 * (x[0] - 1.0);
        grad[1] = 2.0 * (x[1] - 0.5);
    }
    public static void function1_hess(double[] x, ref double func, double[] grad, double[,] hess, object obj)
    {  // Function and gradient and Hessian

        func = (x[0] - 1.0) * (x[0] - 1.0) + (x[1] - 0.5) * (x[1] - 0.5);

        grad[0] = 2.0 * (x[0] - 1.0);
        grad[1] = 2.0 * (x[1] - 0.5);

        hess[0, 0] = 2.0;
        hess[0, 1] = 0;
        hess[1, 0] = 0;
        hess[1, 1] = 2.0;
    }

    public static void Main()
    {
        // 
        // Optimisation algorithm uses:
        // * function value F(x0,x1)
        // * gradient G={dF/dxi}
        // * Hessian H={d2F/(dxi*dxj)}
        // 

        // Initial guess
        double[] x = new double[] { -110, 10 };

        // Termination condition, stops when norm of gradient is < epsg
        double epsg = 0.0001;

        // Subroutine stops when norm(F(k+1) - F(k)) < epsf
        double epsf = 0;

        // Tolerance values == 0 simultaneously leads to automatic stopping criterion selection 
        // Subroutine stops when norm(scaled step vector) < epsx  
        double epsx = 0;

        // Maximum number of iterations (== 0 ==> unlimited)
        int maxits = 0;

        // Structure that stores algorithm state
        alglib.minlmstate state;

        // Optimisation report structure
        alglib.minlmreport rep;

        // Initialise algorithm state
        alglib.minlmcreatefgh(x, out state);

        // Turn on/off reporting
        alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);

        // The main algorithm; note arguments 2, 3 and 4 are functions wigh a specific format
        alglib.minlmoptimize(state, function1_func, function1_grad, function1_hess, null, null);

        // Output value and optimisation report
        alglib.minlmresults(state, out x, out rep);

        System.Console.WriteLine("{0}", rep.terminationtype);  // EXPECTED: 4
        System.Console.WriteLine("{0}", alglib.ap.format(x, 2));  // EXPECTED: [1.0,0.5]
        System.Console.ReadLine();
    }
}