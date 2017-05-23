// BSImp.cs
//
// Class that implements the Black Scholes method.
// Notice that the functions in the IBVP uses constant Properties 
// from an Option object.
//
// Note: in BCL() and BCR() you comment out code depending on whether
// you use Call or Put. Diriclet boundary conditions only.
//
// 2012-11-15 DD clean-up
//
// (C) Datasim Education BV 2013
//

using System;


public class BSIBVPImp : IIBVPImp
{
    private Option m_option;

    public BSIBVPImp( Option option )
    {
        m_option = new Option( option );
    }


    // Coefficient of second derivative
    public double diffusion( double x, double t )
    {
        double v = m_option.Volatility;
        return 0.5 * v * v * x * x;
    }

    // Coefficient of first derivative
    public double convection( double x, double t )
    {
        return m_option.InterestRate * x;
    }

    // Coefficient of zero derivative
    public double zeroterm( double x, double t )
    {
        return -m_option.InterestRate;
    }

    // Inhomogeneous forcing term
    public double RHS( double x, double t )
    {
          return 0.0;
    }

    // Left hand boundary condition
    public double BCL( double t )
    {
        if( m_option.Type == 'C' )
        {
            return 0.0;	//C
        }
        else
        {
            return (m_option.StrikePrice * Math.Exp(-m_option.InterestRate * (t))); // P
        }
    }

    // Right hand boundary condition		
    public double BCR( double t )
    {
        if( m_option.Type == 'C' )
        {
            return m_option.FarFieldCondition - m_option.StrikePrice; // Magic number
        }
        else
        {
            return 0.0; //P
        }
    }

    // Initial condition			
    public double IC( double x )
    {
        return m_option.PayOff( x );
 
    }

    public double Constraint(double x)
    { // Test in American put option

        return m_option.StrikePrice - x;
    }

       
}
