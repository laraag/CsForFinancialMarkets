// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// Definitions.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

    public enum Freq
    {
        Once = 0,
        Annual = 1,           
        SemiAnnual = 2,
        Quarterly = 4,
        Monthly = 12,         
        Weekly = 52,          
        Daily = 365         
    };

     // DayCount
    public enum Dc 
    {
         // many can be added
        _30_360 =1,
        _Act_360 =2, 
        _Act_Act_ISMA,
        _ItalianBTP,
        _Act_365,
        _30_Act,
        _30_365
    }    

     // Rule used in schedule generation
    public enum Rule 
    {
        Backward,
        Forward
    }
    
     // Pay and receive
    public enum PayRec 
    {
        Pay,
        Rec
    }

     // Business Day Convection  // From ISDA and Quantlib
    public enum BusinessDayAdjustment
    {
         // ISDA
        Following,         
        ModifiedFollowing,  
        Preceding,         
        Unadjusted
    };

     // Type of tenor
    public enum TenorType
    {        
        D = 365,        
        W = 52,       
        M = 12,       
        Y =1
    };   

     // Fix Float
    public enum FixFloat 
    { 
        Fixed,
        Floating
    }
    
     // Compounding
    public enum Compounding 
    {
        Simple,
        Continuous,
        Compounded    
    }
    
     // Roll Convention
    public enum RollConvention 
    {
        Forward =1,
        Backward=2,  // more can be added
    }

     // Eligible BuildingBlock
    public enum BuildingBlockType 
    {
        EURZERORATE,  // zero rate
        EURDEPO ,     // euro deposit
        EURSWAP6M ,   // eurswap vs 6m    
        EURSWAP3M,   // eurswap vs 3m 
        EURBASIS6M3M,  // basis swap 6m vs 3m
        EONIASWAP     // eonia swap
    }   

     // type of linear interpolation
    public enum OneDimensionInterpolation 
    {        
        Linear,
        LogLinear,
        SimpleCubic
    }

     // Struc for period
    [Serializable]
    public struct Period 
    {
         // Data member 
        public int tenor;  // tenor as int (i.e. 1, 2....)
        public TenorType tenorType;  // tenor type

         // Constructors
        public Period(int tenor, TenorType tenorType)
        {
            this.tenor = tenor;
            this.tenorType = tenorType;
        }
        public Period(string period) 
        {
            char maturity = period[period.Length - 1];
            int n_periods = int.Parse(period.Remove(period.Length - 1, 1));
            tenor = n_periods;
             // C# 3.0 Cookbook, par 20.10
            tenorType = (TenorType)Enum.Parse(typeof(TenorType), Convert.ToString(maturity).ToUpper());            
        }

         // Method get string format
        public string GetPeriodStringFormat()
        {
            return tenor + (tenorType.ToString()).ToLower();
        }

         // Interval in time 1Y = 1, 6m = 0.5, ...18m =1.5
        public double TimeInterval() 
        { 
            return ((double)this.tenor / (double)this.tenorType) ;    
        }
    }

    