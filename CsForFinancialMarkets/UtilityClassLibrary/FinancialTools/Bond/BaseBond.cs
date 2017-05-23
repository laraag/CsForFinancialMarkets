// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// BaseBond.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

 // To Do: we don't manage bond with irregular schedule

 // Base Class: abstract
[Serializable]
public abstract class BaseBond 
{
     // See Terminology and Convention The PRM Handbook – I.B.1 General Characteristics of Bonds
     // Member data
    protected Date today;  // trading date (today)
    protected Date settlementDate;  // settlement date of the bond
    protected int settlementDaysLag; // lag in business day between today (trade date of the bond) and the settlement date
    protected Schedule schdl;  // date schedule of the bond   
    protected Date startDateBond;  // start date of the bond
    protected Date endDateBond;  // end date of the bond
    protected double faceValue;  // face value of the bond (i.e 100 or 100% or..)
    protected Dc dayCount;  // day count of coupon
    protected int iNextCoupon;  // index of next coupon date
    protected double currentCoupon;  // current coupon as rate (x% or 0.0x)
    protected double[] cashFlows;  // coupon cash flows amount, it doesn't contain face value at the end
    protected Date[] cashFlowsDates;  // dates in which cash flows occur
    protected Date lastCouponDate;  // record date
    protected Date nextCouponDate;  // record date
    protected double accrued;  // accrued interest amount

     // Default Constructor
    public BaseBond() { }

     // Constructor
    public BaseBond(Date Today, Date StartDateBond, Date EndDateBond, string CouponTenor,
        Rule RuleGenerator, int SettlementDaysLag, BusinessDayAdjustment RollAdj, BusinessDayAdjustment PayAdj,
       Dc DayCount, string LagPayFromRecordDate, double FaceValue) 
    {
         // see comments on data member
        this.today = Today;
        this.settlementDaysLag = SettlementDaysLag;
         // the schedule of bond here some arguments:
         // RuleGenerator is the rule to create the schedule
         // RollAdj is BusinessDayAdjustment for rolls
         // PayAdj is BusinessDayAdjustment for payment
         // LagPayFromRecordDate is a string for lag between Roll date of coupon and pay date (as "0d")
        this.schdl = new Schedule(StartDateBond, EndDateBond, CouponTenor, RuleGenerator, RollAdj, LagPayFromRecordDate, PayAdj);        
        this.startDateBond = StartDateBond; 
        this.endDateBond = EndDateBond;
        this.dayCount = DayCount;
        this.faceValue = FaceValue;
    }
    
     // Get Last Coupon Date
    public Date GetLastCouponDate() { return this.lastCouponDate; }

     // Get Next Coupon Date
    public Date GetNextCouponDate() { return this.nextCouponDate; }

     // Get Settlement Date
    public Date GetSettlementDate() { return this.settlementDate; }

     // Get Current Coupon
    public double GetCurrentCoupon() { return this.currentCoupon; }

     // Get schedule of all Dates in which coupons will be paid
    public Date[] GetCouponDates() 
    {
        return schdl.toDates;
    }

     // Get Today
    public Date GetToday() 
    {
        return today;
    }
    
     // Calculate Yield according to customized convection (freq,DayCount and compounding), given a clean price
    public double Yield(double cleanPrice, int freq, Dc DayCount, Compounding compounding)
    {
        double fullPrice = DirtyPrice(cleanPrice); // it calculates full price
        double guess = 0.05;  // initial guess: arbitrary value
        return Formula.YieldFromFullPrice(fullPrice, guess, this.cashFlowsDates, this.cashFlows, freq, DayCount,
            settlementDate, compounding, faceValue); // it calculates yield from full price
    }

     // Calculate Clean price from Yield according to customized convection (freq,DayCount and compounding), given a clean price
    public double CleanPriceFromYield(double yield, int freq, Dc DayCount, Compounding compounding)
    {
        double fullPrice = Formula.FullPriceFromYield(yield, this.cashFlowsDates, this.cashFlows, freq, DayCount,
            settlementDate, compounding, faceValue);
        return  fullPrice - accrued; // clean price
    }

     // Calculate DirtyPrice, see page 27, Clean and Dirty Bond Price, Fixed Income Securities And Derivatives Handbook, Moorad Choudhry 
    public double DirtyPrice(double CleanPrice)
    {
        return CleanPrice + accrued;  // dirty price = clean + accrued
    }

     // Calculate Accrued Interest as amount (not as rate),see page 27, Accrued Interest, Price Fixed Income Securities And Derivatives Handbook, Moorad Choudhry 
    public double AccruedInterest()
    {
        return lastCouponDate.YF(settlementDate, dayCount) * currentCoupon * faceValue; // yf*Coupon*Nominal
    }

     // Derived classes must implement this method: will fill double[] cashFlows and Date[] cashFlowsDates
    abstract public void CashFlowsAndDates();
    
     // To update today, used in constructor
    public void SetNewToDay(Date td)
    {
        this.today = td;  // update today

         // settlement date
         this.settlementDate = td.add_workdays(settlementDaysLag);  // This is simplified, but you should use TARGET calendar (see Quantlib)        
        
         // assign lastCouponDate and nextCouponDate
        Date[] Roll = schdl.toDates;  // rolling of coupon
        int k = Array.FindIndex(Roll, n => n > this.settlementDate); // look for index for next coupon

        this.nextCouponDate = Roll[k];  // nextCouponDate

        if (k == 0) { this.lastCouponDate = startDateBond; }
        else { this.lastCouponDate = Roll[k - 1]; }  // lastCouponDate   

         // index of next coupon date
        this.iNextCoupon = k;
               
         // calculate Accrued: should be done ofter calculating settlement!
        this.accrued = AccruedInterest();
    }
}    

 // BondFixedCoupon is derived from BaseBond. It is used for fixed coupon bond
[Serializable]
public class BondFixedCoupon: BaseBond
{
     // constructor
    public BondFixedCoupon(Date Today, Date StartDateBond, Date EndDateBond, double Coupon, string CouponTenor,
        Rule RuleGenerator, int SettlementDaysLag, BusinessDayAdjustment RollAdj, BusinessDayAdjustment PayAdj,
        Dc DayCount, string LagPayFromRecordDate, double FaceValue):base
        (Today, StartDateBond, EndDateBond, CouponTenor,RuleGenerator,SettlementDaysLag,RollAdj,PayAdj,
        DayCount,LagPayFromRecordDate,FaceValue)
    {
         // Current coupon
        this.currentCoupon = Coupon;
    
         // calculate cash flows and pay dates
        CashFlowsAndDates();

         // Following member data will change changing today: lastCouponDate, nextCouponDate, accrued, settlement date
        SetNewToDay(Today);
    }
    
     // calculate cash flows and pay dates,used in constructor 
    public override void CashFlowsAndDates()
    {
         // calculate cash flows and pay dates
        Func<double, double> CalculateEachCash = x => x * currentCoupon * faceValue;  // classic formula
        this.cashFlows = schdl.GetYFVect(dayCount).Select(CalculateEachCash).ToArray();  // cast from IEnumerable<double> 
        this.cashFlowsDates = schdl.payDates;  // pay dates
    }
}

 // Derived class BondBTP is a specific case of BondFixedCoupon. Used as an example
 // It has a specific AccruedInterest() method and some default in constructor
[Serializable]
public class BondBTP : BondFixedCoupon
{
     // Constructor:it use base constructor and add something...
    public BondBTP(Date Today, Date StartDateBond, Date EndDateBond, double Coupon) :
        base(Today, StartDateBond, EndDateBond, Coupon, "6m", Rule.Backward, 3, BusinessDayAdjustment.Unadjusted, BusinessDayAdjustment.Following, Dc._30_360,
          "0d", 100)
    {
         // need as is different from base class
        this.accrued = AccruedInterest();
    }
    
     // Calculate accrued interest: it is a specific calculation method for this bond.
     // see VALID CALCULATION TYPES Bloomberg 14 Apr 10 523: Italian BTPS
    public new double AccruedInterest()
    {       
        double daysHeld = lastCouponDate.D_EFF(settlementDate);  // days you held the bond from last coupon date
        double daysPeriod = lastCouponDate.D_EFF(nextCouponDate);  // days in current coupon period
        return Math.Round((currentCoupon / 2) * (daysHeld / daysPeriod), 7)*faceValue; // 6 digit for 1K Eur  
    }  
}

 // Derived class BondDBR is a specific case of BondFixedCoupon. Used as an example 
 // It has a specific AccruedInterest() method and some default in constructor
[Serializable]
public class BondDBR : BondFixedCoupon
{
     // Constructor:it use base constructor
    public BondDBR(Date Today, Date StartDateBond, Date EndDateBond, double Coupon) :
        base(Today, StartDateBond, EndDateBond, Coupon, "1y", Rule.Backward, 3, BusinessDayAdjustment.Unadjusted, BusinessDayAdjustment.Following, Dc._30_360,
          "0d", 100)
    {
    }

     // Calculate accrued interest
    public new double AccruedInterest()
    {
         // it used ACT_ACT_ISMA
         // see "the Actual/actual Day count fraction -
         // paper for USE with the ISDA Market Convection Survey -3rd June, 1999
        return lastCouponDate.YF_ACT_ACT_ISMA(settlementDate, nextCouponDate) * currentCoupon * faceValue; // yf*Coupon*Nominal
    }
}

 // Derived class BondBTAN is a specific case of BondFixedCoupon. Used as an example
 // It has a specific AccruedInterest() method and some default in constructor
[Serializable]
public class BondBTAN : BondFixedCoupon
{
     // Constructor:it use base constructor
    public BondBTAN(Date Today, Date StartDateBond, Date EndDateBond, double Coupon) :
        base(Today, StartDateBond, EndDateBond, Coupon,
        "1y", Rule.Backward, 1, BusinessDayAdjustment.Unadjusted, BusinessDayAdjustment.Following, Dc._30_360,
          "0d", 100)
    {
    }

     // Calculate accrued interest
    public new double AccruedInterest()
    {   
         // it used ACT_ACT_ISMA
         // see "the Actual/actual Day count fraction -
         // paper for USE with the ISDA Market Convection Survey -3rd June, 1999
        return lastCouponDate.YF_ACT_ACT_ISMA(settlementDate, nextCouponDate) * currentCoupon * faceValue; // yf*Coupon*Nominal
    }
}

 // BondMultiCoupon Derived Class from BaseBond, it is used for bond with multi fixed coupon (step up, step down..)
[Serializable]
public class BondMultiCoupon : BaseBond
{
     // Member data
    protected double[] coupons;  // array with multi coupon rate
    
     // constructor: use base constructor with Coupons[0] as coupon only to give a value.In constructor then update coupons
    public BondMultiCoupon(Date Today, Date StartDateBond, Date EndDateBond, double[] Coupons, string CouponTenor,
        Rule RuleGenerator, int SettlementDaysLag, BusinessDayAdjustment RollAdj, BusinessDayAdjustment PayAdj,
        Dc DayCount, string LagPayFromRecordDate, double FaceValue) :
        base
        (Today, StartDateBond, EndDateBond, CouponTenor, RuleGenerator, SettlementDaysLag, RollAdj, PayAdj,
        DayCount, LagPayFromRecordDate, FaceValue)
    {
         // Update coupon array
        this.coupons = Coupons;
         // calculate cash flows and pay dates
        CashFlowsAndDates();

         // Following member data will change changing today: lastCouponDate, nextCouponDate, accrued, settlement date
        SetNewToDay(Today);
    }

     // To update today
    public new  void SetNewToDay(Date td)
    {
         // update value, today given
        base.SetNewToDay(td);

         // get current coupon from array, iNextCoupon is updated by base.SetNewToDay(td)
        this.currentCoupon = this.coupons[iNextCoupon];

         // refresh accrued with current coupon
        this.accrued = AccruedInterest();
    }
    
     // calculate cash flows and pay dates,used in constructor 
    public override void CashFlowsAndDates()
    {
         // calculate cash flows
        double[] yf = schdl.GetYFVect(dayCount);  // array of year fractions
        int iMax = yf.GetLength(0);  // number of elements
        this.cashFlows = new double[iMax];  // initialise cash flows array
        if (iMax != coupons.GetLength(0)) throw new ArgumentException("error in coupons number"); // number of pay dates and coupons should be equal 
         // iterate to get cashFlows array
        for (int i = 0; i < iMax; i++)
        {
            cashFlows[i] = yf[i] * coupons[i] * faceValue;
        }

         // pay dates
        cashFlowsDates = schdl.payDates;
    }
}

 // BondZeroCoupon Derived class for zero coupon
[Serializable]
public class BondZeroCoupon : BaseBond 
{
     // Constructor: it use a different constructor for schedule schdl
    public BondZeroCoupon(Date Today, Date StartDateBond, Date EndDateBond, int SettlementDaysLag, BusinessDayAdjustment RollAdj, BusinessDayAdjustment PayAdj,
        string LagPayFromRecordDate, double FaceValue) 
    {
         // more comment on data member of base class
        this.settlementDaysLag = SettlementDaysLag;
        this.schdl = new Schedule(StartDateBond, EndDateBond, RollAdj, LagPayFromRecordDate, PayAdj); // schedule for one payment
        this.startDateBond = StartDateBond;
        this.endDateBond = EndDateBond;
        this.dayCount = Dc._30_360;  // not needed, assign as default
        this.faceValue = FaceValue;

        this.currentCoupon = 0.0;  // it is a zero coupon
        this.lastCouponDate = StartDateBond;
        this.nextCouponDate = schdl.toDates[0];
        
         // calculate cash flows and pay dates
        CashFlowsAndDates();

         // updating today
        SetNewToDay(Today);
    }

     // To update today, used in constructor
    public new void SetNewToDay(Date td)
    {
         // update today
        this.today = td; 

         // settlement date
        this.settlementDate = td.add_workdays(settlementDaysLag);        
    }

     // calculate cash flows and pay dates,used in constructor : very simple for a zero coupon
    public override void CashFlowsAndDates() 
    {
        this.cashFlows = new double[] { 0.0 };  // initialise cash flows array: it is a zero coupon !
        this.cashFlowsDates = new Date[] {schdl.payDates[0]}; // the final: the only one
    }
}

 // Derived class BondBOT is a specific case of BondZeroCoupon
[Serializable]
public class BondBOT : BondZeroCoupon 
{
     // Constructor:it use base constructor and add something...
    public BondBOT(Date Today, Date StartDateBond, Date EndDateBond): 
        base (Today, StartDateBond, EndDateBond, 2, BusinessDayAdjustment.Unadjusted, BusinessDayAdjustment.Unadjusted,
        "0d", 100) 
    {}
}

 // Derived class BondCTZ is a specific case of BondZeroCoupon
[Serializable]
public class BondCTZ : BondZeroCoupon
{
     // Constructor:it use base constructor and add something...
    public BondCTZ(Date Today, Date StartDateBond, Date EndDateBond) :
        base(Today, StartDateBond, EndDateBond, 3, BusinessDayAdjustment.Unadjusted, BusinessDayAdjustment.Unadjusted,
         "0d", 100)
    { }
}

