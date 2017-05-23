// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// BuildingBlock.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * BuildingBlock is the basic element per building a curve
 * Using Factory Pattern (interface IBuildingBlockFactory and class BuildingBlockFactory) I'm able to create runtime my
 * building block as needed.
 * Each building block according to its specifications (LoadSpecifications()) knows how to contribute to discount factor calculation
 * void CalculateDF(..) will allow to add dfs to SortedList sl using the information embedded in building block
 * BuildingBlock is an abstract class. The two direct derived abstract class are SwapStyle OnePaymentStyle
 * SwapStyle refers to instrument with a schedule of payments
 * OnePaymentStyle refers to instrument with only one payment
 * The reason for these two main abstract class is that Number of payment is influent on DF calculation 
 * EurSwapVs6m, EoniaSwap  derived from SwapStyle (we can add more: i.e EurSwapVs3M,BasisSwap...)
 * EurDepo, EurZeroRate is derived from OnePaymentStyle (we can add more: i.e. Fra..)
 * Each building block has its own specification. We can also store specification on xml instead of in the class itself, to make it more generic
 */

     // my factory interface
    public interface IBuildingBlockFactory
    {       
        BuildingBlock CreateBuildingBlock(Date RefDate, double rateValue, string TenorString, BuildingBlockType myBuildingBlockType);
        BuildingBlock CreateEmptyBuildingBlock(BuildingBlockType myBuildingBlockType);
    }

     // Factory for building block
    public class BuildingBlockFactory : IBuildingBlockFactory
    {
         // According to BuildingBlockType will crate the right BuildingBlock
        public BuildingBlock CreateBuildingBlock(Date RefDate, double rateValue, string TenorString, BuildingBlockType myBuildingBlockType)       
        {
           if (myBuildingBlockType== BuildingBlockType.EURZERORATE)
           { return new EurZeroRate(RefDate, rateValue, TenorString); } 

           else if (myBuildingBlockType== BuildingBlockType.EURDEPO)
           { return new EurDepo(RefDate, rateValue, TenorString);}
 
           else if (myBuildingBlockType == BuildingBlockType.EURSWAP6M)
           { return new EurSwapVs6m(RefDate, rateValue, TenorString); }

           else if (myBuildingBlockType == BuildingBlockType.EURSWAP3M)
           { return new EurSwapVs3m(RefDate, rateValue, TenorString); }

           else if (myBuildingBlockType == BuildingBlockType.EURBASIS6M3M)
           { return new EurSwap6mVs3m(RefDate, rateValue, TenorString); }

           else if (myBuildingBlockType == BuildingBlockType.EONIASWAP)
           { return new EoniaSwap(RefDate, rateValue, TenorString);  }
           else
           { return null; }
        }

         // According to BuildingBlockType will create an empty building block (so no rate, no tenor, no ref date).
         // Empty building block is used to have info stored in Load Specifications()
        public BuildingBlock CreateEmptyBuildingBlock( BuildingBlockType myBuildingBlockType)
        {
            if (myBuildingBlockType == BuildingBlockType.EURZERORATE)
            { return new EurZeroRate(); }

            else if (myBuildingBlockType == BuildingBlockType.EURDEPO)
            { return new EurDepo(); }

            else if (myBuildingBlockType == BuildingBlockType.EURSWAP6M)
            { return new EurSwapVs6m(); }

            else if (myBuildingBlockType == BuildingBlockType.EURSWAP3M)
            { return new EurSwapVs3m(); }

            else if (myBuildingBlockType == BuildingBlockType.EURBASIS6M3M)
            { return new EurSwap6mVs3m(); }

            else if (myBuildingBlockType == BuildingBlockType.EONIASWAP)
            { return new EoniaSwap(); }
            else
            { return null; }
        }
    }

     // Abstract class for building block  
    public abstract class BuildingBlock // 
    {
         // Data Member
        public Date refDate;     // reference date, will be used in curve building
        public Date endDate;     // end date of building block
        public Period Tenor;     // tenor of building block
        public double rateValue; // rate value of building block    
        public BuildingBlockType buildingBlockType;  // building block type
        public Dc dayCount; // day count
       
         // Paremeterless constructor
        protected BuildingBlock() { LoadSpecifications(); } 

         // Constructor
        public BuildingBlock(Date RefDate, double RateValue, string TenorString)
        {
            this.refDate = RefDate;            
            this.rateValue = RateValue;
            this.Tenor = new Period(TenorString);
             // Load specification of each contract
            LoadSpecifications(); // method that load each specific building block setting
        }

         // Derived class should implement it
        abstract public void LoadSpecifications();
    }

     // Swap style is derived from BuildingBlock: it means instrument with a series of payments 
    public abstract class SwapStyle : BuildingBlock 
    {
         // Data member        
         // Leg 1
        public SwapLeg swapLeg1;  // Leg1 SwapLeg
        public Schedule scheduleLeg1;  // Leg1 Schedule

         // Leg2
        public SwapLeg swapLeg2;  // Leg2 SwapLeg
        public Schedule scheduleLeg2;  // Leg2 Schedule

         // Parameter-less constructor 
        protected SwapStyle():base() { } 

         // Constructor
        public SwapStyle(Date RefDate, double RateValue, string TenorString) : base(RefDate, RateValue, TenorString) 
        {   
             // create schedule of base class
            this.scheduleLeg1 = new Schedule(RefDate, RefDate.add_period(TenorString, false), swapLeg1.PayFreq, swapLeg1.SwapScheduleGeneratorRule, swapLeg1.SwapBusDayRollsAdj,
               swapLeg1.SwapLagPayment, swapLeg1.SwapBusDayPayAdj);

             // create schedule Leg2 of base class 
            this.scheduleLeg2 = new Schedule(RefDate, RefDate.add_period(TenorString, false), swapLeg2.PayFreq, swapLeg1.SwapScheduleGeneratorRule, swapLeg1.SwapBusDayRollsAdj,
               swapLeg2.SwapLagPayment, swapLeg2.SwapBusDayPayAdj);

             // populate end date
            this.endDate = scheduleLeg1.payDates.Last();
        }      
    }

     // EurSwapVs6m derived from SwapStyle
    public class EurSwapVs6m : SwapStyle 
    {
        public EurSwapVs6m(): base() { }
         // Constructor
        public EurSwapVs6m(Date RefDate, double RateValue, string TenorString)
            : base(RefDate, RateValue, TenorString){}
                
         // Specification for this class
        public override void LoadSpecifications() 
        {
             // standard swap 30/360
            this.buildingBlockType = BuildingBlockType.EURSWAP6M;
           
             // Fixed leg details on swapLeg1
            Rule FixSwapRule = Rule.Backward;
            string FixSwapPayFreqString = "1y";
            BusinessDayAdjustment FixSwapBusDayAdjRolls = BusinessDayAdjustment.ModifiedFollowing;
            string FixSwapLegPaymentString = "0d";
            BusinessDayAdjustment FixSwapBusDayAdjPay = BusinessDayAdjustment.ModifiedFollowing;
            Dc FixDayCount = Dc._30_360;  // day count of fixed leg   
            FixFloat FixFixOrFloat = FixFloat.Fixed;

            string FixUnderlyingRateTenor = "";
                                 
             // Floating details on Leg2
            Rule FloatSwapRule = Rule.Backward;
            string FloatSwapPayFreqString = "6m";
            BusinessDayAdjustment FloatSwapBusDayAdjRolls = BusinessDayAdjustment.ModifiedFollowing;
            string FloatSwapLegPaymentString = "0d";
            BusinessDayAdjustment FloatSwapBusDayAdjPay = BusinessDayAdjustment.ModifiedFollowing;
            Dc FloatDayCount = Dc._Act_360;  // day count of Floating leg
            FixFloat FloatFixOrFloat = FixFloat.Floating;
            string FloatUnderlyingRateTenor = "6m";
         
             // Creating swap legs
             // create swapLeg1 
            this.swapLeg1 = new SwapLeg(FixSwapRule, FixSwapPayFreqString, FixSwapBusDayAdjRolls,
                FixSwapLegPaymentString, FixSwapBusDayAdjPay, FixDayCount, FixFixOrFloat, FixUnderlyingRateTenor);
             // create swapLeg2 
            this.swapLeg2 = new SwapLeg(FloatSwapRule, FloatSwapPayFreqString, FloatSwapBusDayAdjRolls,
                FloatSwapLegPaymentString, FloatSwapBusDayAdjPay, FloatDayCount, FloatFixOrFloat, FloatUnderlyingRateTenor);
        }             
    }

     // EurSwapVs3m derived from SwapStyle
    public class EurSwapVs3m : SwapStyle
    {
         // no parameter constructor
        public EurSwapVs3m(): base() { }

         // Constructor
        public EurSwapVs3m(Date RefDate, double RateValue, string TenorString)
            : base(RefDate, RateValue, TenorString) { }

         // Specification for this class
        public override void LoadSpecifications()
        {
             // standard swap 30/360
            this.buildingBlockType = BuildingBlockType.EURSWAP3M;

             // Fixed leg details on swapLeg1
            Rule FixSwapRule = Rule.Backward;
            string FixSwapPayFreqString = "1y";
            BusinessDayAdjustment FixSwapBusDayAdjRolls = BusinessDayAdjustment.ModifiedFollowing;
            string FixSwapLegPaymentString = "0d";
            BusinessDayAdjustment FixSwapBusDayAdjPay = BusinessDayAdjustment.ModifiedFollowing;
            Dc FixDayCount = Dc._30_360;  // day count of fixed leg   
            FixFloat FixFixOrFloat = FixFloat.Fixed;
            string FixUnderlyingRateTenor = "";

             // Floating details on Leg2
            Rule FloatSwapRule = Rule.Backward;
            string FloatSwapPayFreqString = "3m";
            BusinessDayAdjustment FloatSwapBusDayAdjRolls = BusinessDayAdjustment.ModifiedFollowing;
            string FloatSwapLegPaymentString = "0d";
            BusinessDayAdjustment FloatSwapBusDayAdjPay = BusinessDayAdjustment.ModifiedFollowing;
            Dc FloatDayCount = Dc._Act_360;  // day count of Floating leg
            FixFloat FloatFixOrFloat = FixFloat.Floating;
            string FloatUnderlyingRateTenor = "3m";
            
             // Creating swap legs
             // create swapLeg1 
            this.swapLeg1 = new SwapLeg(FixSwapRule, FixSwapPayFreqString, FixSwapBusDayAdjRolls,
                FixSwapLegPaymentString, FixSwapBusDayAdjPay, FixDayCount, FixFixOrFloat, FixUnderlyingRateTenor);
             // create swapLeg2 
            this.swapLeg2 = new SwapLeg(FloatSwapRule, FloatSwapPayFreqString, FloatSwapBusDayAdjRolls,
                FloatSwapLegPaymentString, FloatSwapBusDayAdjPay, FloatDayCount, FloatFixOrFloat, FloatUnderlyingRateTenor);
        }
    }

     // EoniaSwap derived from SwapStyle
    public class EoniaSwap : SwapStyle
    {
         // no parameter constructor
        public EoniaSwap(): base() { }

         // Constructor
        public EoniaSwap(Date RefDate, double RateValue, string TenorString)
            : base(RefDate, RateValue, TenorString) { }

         // Specification for this class
        public override void LoadSpecifications()
        {
             // standard EoniaSwap Data may be stored on xml 
            this.buildingBlockType = BuildingBlockType.EONIASWAP;

             // Fixed leg details on swapLeg1
            Rule FixSwapRule = Rule.Backward;
            string FixSwapPayFreqString = "1y";
            BusinessDayAdjustment FixSwapBusDayAdjRolls = BusinessDayAdjustment.ModifiedFollowing;
            string FixSwapLegPaymentString = "1d";
            BusinessDayAdjustment FixSwapBusDayAdjPay = BusinessDayAdjustment.ModifiedFollowing;
            Dc FixDayCount = Dc._Act_360;  // day count of fixed leg   
            FixFloat FixFixOrFloat = FixFloat.Fixed;
            string FixUnderlyingRateTenor = "";

             // Floating details on Leg2
            Rule FloatSwapRule = Rule.Backward;
            string FloatSwapPayFreqString = "1y";
            BusinessDayAdjustment FloatSwapBusDayAdjRolls = BusinessDayAdjustment.ModifiedFollowing;
            string FloatSwapLegPaymentString = "1d";
            BusinessDayAdjustment FloatSwapBusDayAdjPay = BusinessDayAdjustment.ModifiedFollowing;
            Dc FloatDayCount = Dc._Act_360;  // day count of Floating leg
            FixFloat FloatFixOrFloat = FixFloat.Floating;
            string FloatUnderlyingRateTenor = "1d";
            
             // Creating swap legs
             // create swapLeg1 
            this.swapLeg1 = new SwapLeg(FixSwapRule, FixSwapPayFreqString, FixSwapBusDayAdjRolls,
                FixSwapLegPaymentString, FixSwapBusDayAdjPay, FixDayCount, FixFixOrFloat, FixUnderlyingRateTenor);
             // create swapLeg2 
            this.swapLeg2 = new SwapLeg(FloatSwapRule, FloatSwapPayFreqString, FloatSwapBusDayAdjRolls,
                FloatSwapLegPaymentString, FloatSwapBusDayAdjPay, FloatDayCount, FloatFixOrFloat, FloatUnderlyingRateTenor);            
        }
    }

    public class EurSwap6mVs3m : SwapStyle 
    {
         // Parameter-less constructor
        public EurSwap6mVs3m() : base() { }

         // Constructor
        public EurSwap6mVs3m(Date RefDate, double RateValue, string TenorString)
            : base(RefDate, RateValue, TenorString) { }

         // Specification for this class
        public override void LoadSpecifications()
        {
             // Basis swap 6m vs 3m
            this.buildingBlockType = BuildingBlockType.EURSWAP3M;

             // Fixed leg details on swapLeg1
            Rule FloatSwapRule1 = Rule.Backward;
            string FloatSwapPayFreqString1 = "6m";
            BusinessDayAdjustment FloatSwapBusDayAdjRolls1 = BusinessDayAdjustment.ModifiedFollowing;
            string FloatSwapLegPaymentString1 = "0d";
            BusinessDayAdjustment FloatSwapBusDayAdjPay1 = BusinessDayAdjustment.ModifiedFollowing;
            Dc FloatDayCount1 = Dc._Act_360;  // day count of Floating leg
            FixFloat FloatFixOrFloat1 = FixFloat.Floating;
            string FloatUnderlyingRateTenor1 = "6m";

             // Floating details on Leg2
            Rule FloatSwapRule2 = Rule.Backward;
            string FloatSwapPayFreqString2 = "3m";
            BusinessDayAdjustment FloatSwapBusDayAdjRolls2 = BusinessDayAdjustment.ModifiedFollowing;
            string FloatSwapLegPaymentString2 = "0d";
            BusinessDayAdjustment FloatSwapBusDayAdjPay2 = BusinessDayAdjustment.ModifiedFollowing;
            Dc FloatDayCount2 = Dc._Act_360;  // day count of Floating leg
            FixFloat FloatFixOrFloat2 = FixFloat.Floating;
            string FloatUnderlyingRateTenor2 = "3m";
            
             // Creating swap legs
             // create swapLeg1 
            this.swapLeg1 = new SwapLeg(FloatSwapRule1, FloatSwapPayFreqString1, FloatSwapBusDayAdjRolls1,
               FloatSwapLegPaymentString1, FloatSwapBusDayAdjPay1, FloatDayCount1, FloatFixOrFloat1, FloatUnderlyingRateTenor1);
             // create swapLeg2 
            this.swapLeg2 = new SwapLeg(FloatSwapRule2, FloatSwapPayFreqString2, FloatSwapBusDayAdjRolls2,
                FloatSwapLegPaymentString2, FloatSwapBusDayAdjPay2, FloatDayCount2, FloatFixOrFloat2, FloatUnderlyingRateTenor2);
        }
    }
    
     // One period style is derived from BuildingBlock: it means instrument with only one payment
    public abstract class OnePaymentStyle : BuildingBlock 
    {
         // Data member
        public BusinessDayAdjustment BusDayAdjPay;

         // no parameter constructor
        protected OnePaymentStyle() : base() { }

         // Constructor
        public OnePaymentStyle(Date RefDate, double RateValue, string TenorString)
            : base(RefDate, RateValue, TenorString) 
        {
             // getting last date
            this.endDate = this.refDate.add_period(TenorString, false).GetBusDayAdjust(BusDayAdjPay);
        }       
    }

     // Standard EurDepo is derived from OnePaymentStyle
    public class EurDepo : OnePaymentStyle
    {
         // no parameter constructor
        public EurDepo() : base() { }

         // Constructor
        public EurDepo(Date RefDate, double RateValue, string TenorString)
            : base(RefDate, RateValue, TenorString){}

         // Specification for this class
        public override void LoadSpecifications()
        {
             // standard Eur Deposit Data may be stored on xml 
            this.buildingBlockType = BuildingBlockType.EURDEPO;
            this.BusDayAdjPay = BusinessDayAdjustment.ModifiedFollowing;
            this.dayCount = Dc._Act_360;  // day count of fixed leg                       
        }
    }

     // EurZeroRate is derived from OnePaymentStyle: it is not tradable but used in calculation.
    public class EurZeroRate : OnePaymentStyle
    {
         // no parameter constructor
        public EurZeroRate() : base() { }
        
         // Constructor
        public EurZeroRate(Date RefDate, double RateValue, string TenorString)
            : base(RefDate, RateValue, TenorString) { }

         // Specification for this class
        public override void LoadSpecifications()
        {
             // standard Eur zero rate Data may be stored on xml 
             // they are not quoted as tradable instrument but only used for calculations
            this.buildingBlockType = BuildingBlockType.EURZERORATE;
            this.BusDayAdjPay = BusinessDayAdjustment.ModifiedFollowing;
            this.dayCount = Dc._Act_365;  // day count of fixed leg                       
        }
    }
