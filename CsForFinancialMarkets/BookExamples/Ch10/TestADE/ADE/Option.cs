// Option.cs
// 23 Januari 2006
// Jannes Albrink
//
// Option class, contains a PayOff method.
//
//
// (C) Datasim Component Technology 2006

using System;


/// <summary>
/// Financial option class.
/// </summary>
public class Option
{
    /// <summary>
    /// Delegate for PayOff strategy. Can be used for both, one factor and multi asset payoff.
    /// </summary>
    /// <param name="K_or_S1"></param>
    /// <param name="S_or_S2"></param>
    /// <returns></returns>
    public delegate double PayOffHandler( double K_or_S1, double S_or_S2 );

	private double m_interest;		        // Interest rate
    private double m_volatility;		    // Volatility
    private double m_strikePrice;		    // Strike price
    private double m_expiryDate;		    // Expiry date
    private double m_costOfCarry;		    // Cost of carry	
    public double m_farFieldCondition;     // Far field condition
    private char   m_type;		            // Call or put
    private PayOffHandler m_payOff;
       

    /// <summary>
    /// Default constructor.
    /// </summary>
	public Option()
	{
         m_interest = 0.08;
        m_volatility = 0.3;
        m_strikePrice = 65.0;
        m_expiryDate = 0.25;
        m_costOfCarry = m_interest;
        m_farFieldCondition = 5 * m_strikePrice;
        m_type = 'P';

        // Modify code when using C <-> P; V2 use a dynamic switch
      //  m_payOff = new Option.PayOffHandler(OneFactorPayOff.MyCallPayoffFN);
        m_payOff = new Option.PayOffHandler(OneFactorPayOff.MyPutPayoffFN);
      //  m_payOff = new Option.PayOffHandler(OneFactorPayOff.MyFirstExitTimeFN);
	}

    /// <summary>
    /// Copy constructor, makes a deep copy of the object. It will also copy the payoff strategy.
    /// </summary>
    /// <param name="option"></param>
    public Option( Option source )
    {
        m_interest          = source.m_interest;
        m_volatility        = source.m_volatility;
        m_strikePrice       = source.m_strikePrice;
        m_expiryDate        = source.m_expiryDate;
        m_costOfCarry       = source.m_costOfCarry;
        m_farFieldCondition = source.m_farFieldCondition;
        m_type              = source.m_type;
        m_payOff            = source.m_payOff;
    }


    /// <summary>
    /// Sets the delegate to use for the payoff method.
    /// </summary>
    public PayOffHandler PayOffStrategy
    {
        set
        {
            m_payOff = value;
        }
    }

    /// <summary>
    /// Sets or gets the interest rate.
    /// </summary>
    public double InterestRate
    {
        get
        {
            return m_interest;
        }
        set
        {
            m_interest = value;
        }
    }

    /// <summary>
    /// Sets or gets the volatility.
    /// </summary>
    public double Volatility
    {
        get
        {
            return m_volatility;
        }
        set
        {
            m_volatility = value;
        }
    }

    /// <summary>
    /// Sets or gets the strike price.
    /// </summary>
    public double StrikePrice
    {
        get
        {
            return m_strikePrice;
        }
        set
        {
            m_strikePrice = value;
        }
    }

    /// <summary>
    /// Sets or gets the exspiry date.
    /// </summary>
    public double ExpiryDate
    {
        get
        {
            return m_expiryDate;
        }
        set
        {
            m_expiryDate = value;
        }
    }

    /// <summary>
    /// Sets or gets the cost of carry.
    /// </summary>
    public double CostOfCarry
    {
        get
        {
            return m_costOfCarry;
        }
        set
        {
            m_costOfCarry = value;
        }
    }

    /// <summary>
    /// Sets or gets the far field condition.
    /// </summary>
    public double FarFieldCondition
    {
        get
        {
            return m_farFieldCondition;
        }
        set
        {
            m_farFieldCondition = value;
        }
    }

    /// <summary>
    /// Sets or gets the option type.
    /// </summary>
    public char Otype
    {
        get
        {
            return m_type;
        }
        set
        {
            m_type = value;
        }
    }

    /// <summary>
    /// Method to calculate the one factor payoff, set the PayOffStrategy property first!
    /// </summary>
    /// <param name="S"></param>
    /// <returns></returns>
    public double PayOff( double S )
    {
        return m_payOff( m_strikePrice, S );
    }

    /// <summary>
    /// Method to calculate the multi asset payoff, set the PayOffStrategy property first!
    /// </summary>
    /// <param name="S1"></param>
    /// <param name="S2"></param>
    /// <returns></returns>
    public double PayOff( double S1, double S2 )
    {
        return m_payOff( S1, S2 );
    }
}
