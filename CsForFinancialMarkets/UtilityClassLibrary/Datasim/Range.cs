// Range.cs
// 31 January 2006
// Robbert van der Plas
//
// Range class.
//
// (C) Datasim Education BV 2006-2013
//


public class Range<T>
{
    private double lo;
    private double hi;

    public Range()
    {
        lo = 0.0;
        hi = 0.0;
    }

    public Range( double l, double h )
    {
        if( l < h )
        {
            lo = l;
            hi = h;
        }
        else
        {
            hi = l;
            lo = h;
        }
    }

    public Range( Range<double> r )
    {
        lo = r.lo;
        hi = r.hi;
    }

    public double low
    {
        get
        {
            return lo;
        }
        set
        { // The low value of the range will be set with the value
            lo = value;
        }
    }

    public double high
    {
        get
        {
            return hi;
        }
        set
        { // The high value of the range will be set with the value
            hi = value;
        }
    }

    public double spread
    {
        get
        {
            return hi - lo;
        }
    }

    public bool left( double val )
    {
        if( val < lo )
        {
            return true;
        }
        return false;
    }

    public bool right( double val )
    {
        if( val > hi )
        {
            return true;
        }
        return false;
    }

    public bool contains( double t )
    {
        if( ( lo <= t ) && ( hi >= t ) )
        {
            return true;
        }
        return false;
    }

    public Vector<double> mesh( int nSteps )
    {
        double h = ( hi - lo ) / ( double )nSteps;
        double val = lo;
        Vector<double> result = new Vector<double>( nSteps + 1, 1 );

        for( int i = 1; i <= nSteps + 1; i++ )
        {
            result[ i ] = val;
            val += h;
        }
        return result;
    }
}