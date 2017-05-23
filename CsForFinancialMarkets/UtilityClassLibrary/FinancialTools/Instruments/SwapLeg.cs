// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// SwapLeg.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

 // SwapLeg is a abstract base class containing 'descriptive' data of a swap leg
public abstract class BaseSwapLeg 
{      
     // Data member
    Rule swapScheduleGeneratorRule;  // rule to generate swap schedule
    string payFreq; // pay frequency of leg
    BusinessDayAdjustment swapBusDayRollsAdj;  // business day conventions for rolls
    string swapLagPayment; // lag between final day and payment date
    BusinessDayAdjustment swapBusDayPayAdj; // business day conventions for payment
    Dc dayCount; // day count
    FixFloat fixedFloating;  // Fix or Floating

     // constructor
    public BaseSwapLeg(Rule SwapScheduleGeneratorRule, string PayFreq, BusinessDayAdjustment SwapBusDayRollsAdj,
        string SwapLagPayment, BusinessDayAdjustment SwapBusDayPayAdj, Dc DayCount, FixFloat FixedFloating) 
    {
         // assign to data member
        this.swapScheduleGeneratorRule = SwapScheduleGeneratorRule;
        this.payFreq = PayFreq;
        this.swapBusDayRollsAdj = SwapBusDayRollsAdj;
        this.swapLagPayment = SwapLagPayment;
        this.swapBusDayPayAdj = SwapBusDayPayAdj;
        this.dayCount = DayCount;
        this.fixedFloating = FixedFloating; 
    }

     // public properties
    public Rule SwapScheduleGeneratorRule 
    {
        get { return swapScheduleGeneratorRule; }
        set { swapScheduleGeneratorRule = value; }
    }
   
    public string PayFreq
    {
        get { return payFreq; }
        set { payFreq = value; }
    }
    
    public BusinessDayAdjustment SwapBusDayRollsAdj 
    {
        get { return swapBusDayRollsAdj; }
        set { swapBusDayRollsAdj = value; }   
    }

    public string SwapLagPayment
    {
        get { return swapLagPayment; }
        set { swapLagPayment = value; }
    }

    public BusinessDayAdjustment SwapBusDayPayAdj
    {
        get { return swapBusDayPayAdj; }
        set { swapBusDayPayAdj = value; }
    }

    public Dc DayCount
    {
        get { return dayCount; }
        set { dayCount = value; }
    }
    
    public FixFloat FixedFloating
    {
        get { return fixedFloating; }
        set { fixedFloating = value; }
    }
}

 // Derived class for fixed Swap leg
public class SwapLegFixed: BaseSwapLeg
{
     // constructor
    public SwapLegFixed(Rule SwapScheduleGeneratorRule, string PayFreq, BusinessDayAdjustment SwapBusDayRollsAdj,
        string SwapLagPayment, BusinessDayAdjustment SwapBusDayPayAdj, Dc DayCount): base(SwapScheduleGeneratorRule,PayFreq, SwapBusDayRollsAdj, SwapLagPayment, SwapBusDayPayAdj,DayCount, FixFloat.Fixed)
    {
    }
}

 // Derived class for floating Swap leg
public class SwapLegFloat : BaseSwapLeg
{
     // Data member
    string underlyingRateTenor;  // Underlying rate tenor
  
     // Constructor
    public SwapLegFloat(Rule SwapScheduleGeneratorRule, string PayFreq, BusinessDayAdjustment SwapBusDayRollsAdj,
        string SwapLagPayment, BusinessDayAdjustment SwapBusDayPayAdj, Dc DayCount, string UnderlyingRateTenor)
        : base(SwapScheduleGeneratorRule, PayFreq, SwapBusDayRollsAdj, SwapLagPayment, SwapBusDayPayAdj, DayCount, FixFloat.Floating)
    {
        this.underlyingRateTenor = UnderlyingRateTenor;
    }

     // Get/set underlying rate tenor
    public string UnderlyingRateTenor
    {
        get { return underlyingRateTenor; }
        set { underlyingRateTenor = value; }
    }
}

public class SwapLeg
{
     // Data member
    Rule swapScheduleGeneratorRule;  // rule to generate swap schedule
    string payFreq; // pay frequency of leg
    BusinessDayAdjustment swapBusDayRollsAdj;  // business day conventions for rolls
    string swapLagPayment; // lag between final day and payment date
    BusinessDayAdjustment swapBusDayPayAdj; // business day conventions for payment
    Dc dayCount; // day count
    FixFloat fixedFloating;  // Fix or Floating
    string underlyingRateTenor;  // underlying rate tenor

     // Constructor
    public SwapLeg(Rule SwapScheduleGeneratorRule, string PayFreq, BusinessDayAdjustment SwapBusDayRollsAdj,
        string SwapLagPayment, BusinessDayAdjustment SwapBusDayPayAdj, Dc DayCount, FixFloat FixedFloating, string UnderlyingRateTenor)
    {
         // If leg is fixed the underlying floating rate tenor should be blank
        if ((FixedFloating == FixFloat.Fixed) && (UnderlyingRateTenor != "")) 
        {
            throw new ArgumentException("error UnderlyingRateTenor must be blank for fixed leg");
        }

         // Assign to data member
        this.swapScheduleGeneratorRule = SwapScheduleGeneratorRule;
        this.payFreq = PayFreq;
        this.swapBusDayRollsAdj = SwapBusDayRollsAdj;
        this.swapLagPayment = SwapLagPayment;
        this.swapBusDayPayAdj = SwapBusDayPayAdj;
        this.dayCount = DayCount;
        this.fixedFloating = FixedFloating;
        this.underlyingRateTenor = UnderlyingRateTenor;
    }

     // Public properties
    public Rule SwapScheduleGeneratorRule
    {
        get { return swapScheduleGeneratorRule; }
        set { swapScheduleGeneratorRule = value; }
    }

    public string PayFreq
    {
        get { return payFreq; }
        set { payFreq = value; }
    }

    public BusinessDayAdjustment SwapBusDayRollsAdj
    {
        get { return swapBusDayRollsAdj; }
        set { swapBusDayRollsAdj = value; }
    }

    public string SwapLagPayment
    {
        get { return swapLagPayment; }
        set { swapLagPayment = value; }
    }

    public BusinessDayAdjustment SwapBusDayPayAdj
    {
        get { return swapBusDayPayAdj; }
        set { swapBusDayPayAdj = value; }
    }

    public Dc DayCount
    {
        get { return dayCount; }
        set { dayCount = value; }
    }

    public FixFloat FixedFloating
    {
        get { return fixedFloating; }
        set { fixedFloating = value; }
    }

     // Get/set underlying rate tenor
    public string UnderlyingRateTenor
    {
        get { return underlyingRateTenor; }
        set { underlyingRateTenor = value; }
    }
}
