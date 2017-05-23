// IBVP.cs
//
// A class that models a generic initial boundary value
// problem. It is part of the Bridge pattern and it delegaes
// to its implementations.
//
// This PDE assumes Dirichlet boundary conditions, that is 
// option values known on the boundaries.
//
// (C) Datasim ducation BV 2013
//

using System.Collections;
using System.Collections.Generic;

class IBVP
{
    // The interface to use
    private Range<double> xaxis;
    private Range<double> taxis;
    private IIBVPImp imp;

    public IBVP( IIBVPImp imp, Range<double> Xrange, Range<double> Trange )
    {
        this.imp = imp;
        xaxis = new Range<double>( Xrange );
        taxis = new Range<double>( Trange );
    }

    // The domain in which the PDE is 'played'
    public Range<double> Xaxis
    {
        get
        {
            return xaxis;
        }
        set
        {
            xaxis = value;
        }
    }

    public Range<double> Taxis
    {
        get
        {
            return taxis;
        }
        set
        {
            taxis = value;
        }
    }

    public double diffusion( double x, double t )
    {
        return imp.diffusion( x, t );
    }

    public double convection( double x, double t )
    {
        return imp.convection( x, t );
    }

    public double zeroterm( double x, double t )
    {
        return imp.zeroterm( x, t );
    }

    public double RHS( double x, double t )
    {
        return imp.RHS( x, t );
    }

    public double BCL( double t )
    {
        return imp.BCL( t );
    }

    public double BCR( double t )
    {
        return imp.BCR( t );
    }

    public double IC( double x )
    {
        return imp.IC( x );
    }

    double Constraint(double x)
    { // Test in American put option

	    return imp.Constraint(x);
    }

}