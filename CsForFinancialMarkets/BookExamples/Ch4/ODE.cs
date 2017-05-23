// ODE.cs
//
// Interface and base class for ordinary differential equations (ODE) and
// the corresponding initial value problem (IVP).
//
// (C) Datasim Education BV 2009-2013
//

using System;

// Interfaces for *Continuous* problem
public interface IODE<V> // V = underlying numeric type
{ // u = u(t); du/dt + a(t)u = f(t), u(0) = A

    V a(V t);       // Coefficient of free term
    V f(V t);       // Inhomogeneous term
}

public interface IIVP<V> : IODE<V>
{ // Specify initial condition and range [0,T] of integation

    V InitialCondition
    {
        get;
        set;
    }

    V Expiry
    {
        get;
        set;
    }
}

// Interfaces for the *Discrete* problem
public interface IFDM<V> // V = underlying numeric type
{ // Computation fron a given time level n to next n+1

    void calculateOneStep(int m);   // One-step method
}


public class ODE: IIVP<double>
{
    private double A;   // Initial condition
    private double T;  // Solve in interval [0, End]

    public ODE(double InitConditon, double Expiry)
    {
        A = InitConditon;
        T = Expiry;
    }

    public double a(double t)
    { // Coefficient of free term

        return 1.0;
    }

    public double f(double t)
    { // Inhomogeneous term

        return 2.0 + t;
    }

    public double InitialCondition
    {
        get
        {
            return A;
        }
        set
        {
            A = value;
        }
    }

    public double Expiry
    {
        get
        {
            return T;
        }
        set
        {
            T = value;
        }
    }
}


public abstract class OneStepFDM : IFDM<double>
{
    protected int NSteps;   // Number of time steps
    protected ODE ode;      // The reference to ODE

    protected double vOld, vNew;   // Values at levels n, n+1
    protected double [] mesh;
    protected double delta_T;      // Step length

    public OneStepFDM(int NSteps, ODE ode)
    {
        this.NSteps = NSteps;
        this.ode = ode;

        vOld = ode.InitialCondition;
        vNew = vOld;

        mesh = new double[NSteps+1];
        mesh[0] = 0.0;
        mesh[NSteps] = ode.Expiry;
        delta_T = (mesh[NSteps] - mesh[0]) / NSteps;

        for (int n = 1; n < NSteps; n++)
        {
            mesh[n] = mesh[n-1] + delta_T;
            Console.Write(", {0}", mesh[n]);
        }
    }

    public abstract void calculateOneStep(int m);   // One-step method

    // The full algorithm computed at the expity t = T
    public double calculate()
    { 
        for (int m = 0; m <= NSteps; m++)
        {
            calculateOneStep(m);
            vOld = vNew;

        }

        return vNew;
    }

    public double Value
    { // Computed value

        get
        {
            return vNew;
        }
        private set { }
    }
}

public class ExplicitEuler: OneStepFDM
{

    public ExplicitEuler(int NSteps, ODE ode) : base(NSteps, ode) { }

    public override void calculateOneStep(int n)
    { // One-step method

        // Create temp vars for readility
        double aVar = ode.a(mesh[n]);
        double fVar = ode.f(mesh[n]);
       // Console.WriteLine("vars[a(t), f(t)] values: [{0}, {1}]", aVar, fVar);

        vNew = (1.0 - delta_T*aVar) * vOld + delta_T * fVar;
        Console.WriteLine("old, new: [{0}, {1}]", vOld, vNew);

     }
}

