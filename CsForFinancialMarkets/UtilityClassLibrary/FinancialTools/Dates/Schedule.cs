// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// Schedule.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Class Schedule: 
 * according to given rule as defined in constructor, the class will generate array of dates needed in schedule of payment
 * each Schedule will collect multi period:  start of each period (fromDates), end of each period (toDates), payment date
 * of each period (payDates)
 * 
 * Class FloatingSchedule:
 * this class is derived from Schedule. It is like Schedule but it is used when in schedule of payment you need also for
 * each period a fixing date (i.e. floating rate schedule payment: fixing date for euribor...).
 * You can decide days leg from fixing and if the leg is from end of period or from start  (arrears)
 */


 // base class
[Serializable]
public class Schedule 
{
     // Data members
    public Date[] fromDates;  // array of starting date of each period
    public Date[] toDates;   // array of end date of each period
    public Date[] payDates;  // array of payment date
      
    Date startDate;  // starting date of schedule
    Date endDate;    // ending date of schedule
    string stringTenor;  // string tenor of frequency of schedule (for example 3m, 6m, 1y...)
    Rule generatorRule;  // the rule generating the period
    BusinessDayAdjustment busDayAdjRolls;  // Business day adjustment rule for roll of end date of each period
    BusinessDayAdjustment busDayAdjPay;  // Business day adjustment rule for payment date of each period
    string payLeg;  // the leg between the end date of each period and the payment date as string (for example 0d,1d...)
    
     // Constructor standard
    public Schedule(Date StartDate, Date EndDate, string PayFreqString, Rule GeneratorRule, BusinessDayAdjustment BusDayAdjRolls,
        string LagPaymentString, BusinessDayAdjustment BusDayAdjPay) 
    {
        this.startDate = StartDate;  // start date of schedule
        this.endDate = EndDate; // end date of schedule
        this.stringTenor = PayFreqString; // frequency of period
        this.generatorRule = GeneratorRule; // rule generator
        this.busDayAdjRolls = BusDayAdjRolls;  // Business day adjustment rule for roll of end date of each period
        this.busDayAdjPay = BusDayAdjPay;  // Business day adjustment rule for payment date of each period
        this.payLeg = LagPaymentString; // the leg between the end date of each period and the payment date as string (for example 0d,1d...
        BuildSchedule();     // method that build the schedule
    }      

     // Constructor for one period schedule
    public Schedule(Date StartDate, Date EndDate, BusinessDayAdjustment BusDayAdjRolls, string LagPaymentString,BusinessDayAdjustment BusDayAdjPay)
    {
        this.startDate = StartDate;  // start date of schedule
        this.endDate = EndDate; // end date of schedule
        this.stringTenor = "Once"; // To Give a value
        this.generatorRule = Rule.Backward; // to give a rule value generator
        this.busDayAdjRolls = BusDayAdjRolls;  // Business day adjustment rule for roll of end date of each period
        this.busDayAdjPay = BusDayAdjPay;  // Business day adjustment rule for payment date of each period
        this.payLeg = LagPaymentString; // the leg between the end date of each period and the payment date as string (for example 0d,1d...
        
        this.fromDates = new Date[] { StartDate };
        this.toDates = new Date[] { EndDate.GetBusDayAdjust(BusDayAdjRolls) };
        this.payDates = Date.GetPeriodShifted(this.toDates, this.payLeg);
        this.payDates = Date.GetBusDayAdjust(this.payDates, this.busDayAdjPay);
    }

     // private Method used in constructor
    private void BuildSchedule()
    {
         // List of date
        List<Date> L = new List<Date>();

         // Forward and Backward case 
        switch (generatorRule)
        {

            case Rule.Forward:
                L.Add(startDate);  // start from start date
                while (endDate.DateValue > L.Last().DateValue)  // before end date
                {
                    L.Add(L.Last().add_period(this.stringTenor, false)); // build according stringTenor                
                }

                if (L.Last().DateValue > endDate.DateValue)   // after end date
                {
                    L.Remove(L.Last()); // no to go after the end of schedule
                    L.Add(endDate);  // last date should be the end date
                }
                break;

            case Rule.Backward:
                L.Add(endDate);  // starts from the end date

                Period p = new Period(this.stringTenor);
                int i = 1;
                while (startDate.DateValue < L.Last().DateValue) // after start date
                {
                    Period pp = new Period(p.tenor * i, p.tenorType);
                    L.Add(endDate.sub_period(pp.GetPeriodStringFormat(), false));
                    i++;
                }

                if (L.Last().DateValue < startDate.DateValue)  // before start date
                {
                    L.Remove(L.Last());  // remove date before start date
                    L.Add(startDate);   // should add start date              

                }

                L.Reverse();  // reverse to sort it ascending
                break;
        }

         // Fill data member
        FillDataMember(L);
    }
     
    private void FillDataMember(List<Date>L)
    {
         // Get in a Date Vector
        Date[] temp = Date.GetBusDayAdjust(L.ToArray(), this.busDayAdjRolls);

        int n = temp.GetLength(0);   // dim of temp
        this.fromDates = new Date[n - 1];  // array of fromDate
        this.toDates = new Date[n - 1];  // array of toDates

         // populate fromDate and toDate array
        Array.Copy(temp, 0, this.fromDates, 0, n - 1);
        Array.Copy(temp, 1, this.toDates, 0, n - 1);

         // payDates: i add payDaysLeg then I applicate adj rule
        this.payDates = Date.GetPeriodShifted(this.toDates, this.payLeg);
        this.payDates = Date.GetBusDayAdjust(this.payDates, this.busDayAdjPay);

    }

     // GetMethod

     // Method Get Vector of YF
    public double[] GetYFVect(Dc DayCount) 
    {
        return Date.GetYfArray(this.fromDates, this.toDates, DayCount);
    }

     // Method to visualise schedule on console
    public void PrintSchedule()
    {
        int n = fromDates.GetLength(0); // number of element
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine("{0:ddd dd-MMM-yyyy} ___  {1:ddd dd-MMM-yyyy}  ___ {2:ddd dd-MMM-yyyy}",
                fromDates[i].DateValue, toDates[i].DateValue, payDates[i].DateValue);
        }
    }
}

 // Derived class: fort floating rate: it consider also fixing vector dates
public class FloatingSchedule : Schedule 
{
     // Data members
    public Date[] fixingDates;  // date in which we will observe the fixing
    public int numOfBussDays;   // leg of days to observe as numbers of business days
    public bool arrears; // true if I calculate leg from endDate, false if I calculate the leg from start date

     // constructors
    public FloatingSchedule(Date StartDate, Date EndDate, string PayFreqString, Rule GeneratorRule, BusinessDayAdjustment BusDayAdjRolls,
          string LegPaymentString, BusinessDayAdjustment BusDayAdjPayment, bool ArrearFixing, int NumOfBussDays):
        base(StartDate, EndDate, PayFreqString, GeneratorRule, BusDayAdjRolls, LegPaymentString, BusDayAdjPayment)
    {
        this.arrears = ArrearFixing;  // date in which we will observe the fixing
        this.numOfBussDays = NumOfBussDays;  // leg of days to observe as numbers of business days
 
        BuildFloatingSchedule(); // method to build schedule
    }
    
      // private Method used in constructor
    private void BuildFloatingSchedule() 
    {
        if (arrears)
        { // in arrears
            this.fixingDates = Date.GetBusinessDayShifted(this.toDates, numOfBussDays);
        }
        else 
        { // in advance
            this.fixingDates = Date.GetBusinessDayShifted(this.fromDates, numOfBussDays);
        }
    }

     // Method to visualise schedule on console
    public new void PrintSchedule() 
    {
        int n = fromDates.GetLength(0); // number of element
        Console.WriteLine("\nFixing \t\t  From  \t\t  To \t\t  PayDate");
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine("{0:ddd dd-MMM-yyyy}___{1:ddd dd-MMM-yyyy}___{2:ddd dd-MMM-yyyy}___{3:ddd dd-MMM-yyyy}",
                fixingDates[i].DateValue,fromDates[i].DateValue,toDates[i].DateValue,payDates[i].DateValue);
        }
    }
}


