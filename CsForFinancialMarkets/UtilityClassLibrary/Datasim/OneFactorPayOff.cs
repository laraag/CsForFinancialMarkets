// OneFactorPayOff.cs
// 23 Januari 2006
// Jannes Albrink
//
// One factor payoff class. Contains several static methods for the payoff calculation
//
//
// (C) Datasim Component Technology 2006-2011

using System;


public static class OneFactorPayOff
{
    public static double MyCallPayoffFN( double K, double S )
    {
        if( S > K )
        {
            return ( S - K );
        }

        return 0.0;
    }

    public static double MyPutPayoffFN( double K, double S )
    {
        if( K > S )
        {
            return ( K - S );
        }

        return 0.0;
    }

    public static double MyFirstExitTimeFN(double K, double S)
    {
        return 1.0;
    }
}