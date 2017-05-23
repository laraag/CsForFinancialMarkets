// Mesher.cs
// 31 January 2006
// Robbert van der Plas
//
// A simple mesher on a 1d domain. We divide
// an interval into J+1 mesh points, J-1 of which
// are internal mesh points.
//
// (C) Datasim Education BV 2006
//

using System;
using System.Text;
using System.Collections.Generic;

public class Mesher
{
    private double a, b, LT, HT;

    public Mesher()
    {
        a = 0.0;
        b = 1.0;
        LT = 0.0;
        HT = 1.0;
    }

    public Mesher( double A, double B, double t, double T )
    { // Describe the domain of integration
        a = A;
        b = B;
        LT = t;
        HT = T;
    }

    public Mesher( Range<double> rX, Range<double> rT )
    { // Describe the domain of integration
        a = rX.low;
        b = rX.high;
        LT = rT.low;
        HT = rT.high;
    }

    public Vector<double> xarr( int J )
    {
        // NB Full array (includes end points)
        double h = ( b - a ) / ( double )J;

        int size = J + 1;
        int start = 1;

        Vector<double> result = new Vector<double>( size, start );
        result[ result.MinIndex ] = a;

        for( int j = result.MinIndex + 1; j <= result.MaxIndex; j++ )
        {
            result[ j ] = result[ j - 1 ] + h;
        }
        return result;
    }

    public Vector<double> yarr( int N )
    {
        // NB Full array (includes end points)
        double k = ( HT - LT ) / ( double )N;

        int size = N + 1;
        int start = 1;

        Vector<double> result = new Vector<double>( size, start );
        result[ result.MinIndex ] = LT;

        for( int j = result.MinIndex + 1; j <= result.MaxIndex; j++ )
        {
            result[ j ] = result[ j - 1 ] + k;
        }
        return result;
    }

    public double timeStep( int N )
    {
        return ( HT - LT ) / ( double )N;
    }
}