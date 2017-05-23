using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Test102
{

    //public minlm_d_fgh example public static void function1_func(double[] x, ref double func, object obj)
    public static void function1_func(double[] x, ref double func, object obj)
    {
        // this callback calculates f(x0,x1) = 100*(x0+3)^4 + (x1-3)^4
        func = 100 * System.Math.Pow(x[0] + 3, 4) + System.Math.Pow(x[1] - 3, 4);
    }
    public static void function1_grad(double[] x, ref double func, double[] grad, object obj)
    {
        // this callback calculates f(x0,x1) = 100*(x0+3)^4 + (x1-3)^4
        // and its derivatives df/d0 and df/dx1
        func = 100 * System.Math.Pow(x[0] + 3, 4) + System.Math.Pow(x[1] - 3, 4);
        grad[0] = 400 * System.Math.Pow(x[0] + 3, 3);
        grad[1] = 4 * System.Math.Pow(x[1] - 3, 3);
    }
    public static void function1_hess(double[] x, ref double func, double[] grad, double[,] hess, object obj)
    {
        // this callback calculates f(x0,x1) = 100*(x0+3)^4 + (x1-3)^4
        // its derivatives df/d0 and df/dx1
        // and its Hessian.
        func = 100 * System.Math.Pow(x[0] + 3, 4) + System.Math.Pow(x[1] - 3, 4);
        grad[0] = 400 * System.Math.Pow(x[0] + 3, 3);
        grad[1] = 4 * System.Math.Pow(x[1] - 3, 3);
        hess[0, 0] = 1200 * System.Math.Pow(x[0] + 3, 2);
        hess[0, 1] = 0;
        hess[1, 0] = 0;
        hess[1, 1] = 12 * System.Math.Pow(x[1] - 3, 2);
    }
    public static int Main(string[] args)
    {
        //
        // This example demonstrates minimization of F(x0,x1) = 100*(x0+3)^4+(x1-3)^4
        // using "FGH" mode of the Levenberg-Marquardt optimizer.
        //
        // F is treated like a monolitic function without internal structure,
        // i.e. we do NOT represent it as a sum of squares.
        //
        // Optimization algorithm uses:
        // * function value F(x0,x1)
        // * gradient G={dF/dxi}
        // * Hessian H={d2F/(dxi*dxj)}
        //
        double[] x = new double[] { 0, 0 };
        double epsg = 0.0000000001;
        double epsf = 0;
        double epsx = 0;
        int maxits = 0;
        alglib.minlmstate state;
        alglib.minlmreport rep;

        alglib.minlmcreatefgh(x, out state);
        alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
        alglib.minlmoptimize(state, function1_func, function1_grad, function1_hess, null, null);
        alglib.minlmresults(state, out x, out rep);

        System.Console.WriteLine("{0}", rep.terminationtype); // EXPECTED: 4
        System.Console.WriteLine("{0}", alglib.ap.format(x, 2)); // EXPECTED: [-3,+3]
        System.Console.ReadLine();
        return 0;
    }


}