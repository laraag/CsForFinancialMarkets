// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// Date.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
// Web references
 // http: // www.isda.org/c_and_a/pdf/mktc1198.pdf
 // http: // en.wikipedia.org/wiki/Accrued_interest
 // http: // en.wikipedia.org/wiki/Day_count_convention
 // http: // www.swx.com/download/trading/products/bonds/accrued_interest_en.pdf
 // http: // www.isda.org/c_and_a/docs/30-360-2006ISDADefs.xls
 // http: // www.euronext.com/editorial/wide/editorial-4304-EN.html
 // see also Quantlib.org for Target calendar

 // Exercise:
 // 1) add method to complete all convention you find to following address:
 // http: // en.wikipedia.org/wiki/Day_count_convention
 // 2) test the following spreadsheet:
 // http: // www.isda.org/c_and_a/docs/30-360-2006ISDADefs.xls

using System;

 [Serializable]
public class Date
{
     // the reference date
	DateTime my_date;

	 // Constructor
	public Date(){}

     // Constructor for i.e. for 15 Sep 10 use Date(2010,9,15)
	public Date (int Year, int Month, int Day)
	{
		my_date = new DateTime(Year,Month,Day);	
	}

     // Constructor using Class DateTime
	public Date (DateTime MyDateTime)
	{
		my_date = MyDateTime;
	}

     // Constructor using Excel serial number i.e for 15 Sep 10  use Date(40436)
	public Date (double ExcelSerial)
	{
		DateTime Starting = new DateTime(1899,12,30);
		my_date = Starting.AddDays(ExcelSerial);
	}

	 // Copy constrictor
	public Date (Date myDate): this(myDate.my_date){}
	
     // Property: get or set DateValue using DateTime Class, that is reference Date
	public DateTime DateValue
	{
		get { return my_date;}
		set { my_date = value;}
	}

     // Property: get or set DateValue using Excel format
	public double SerialValue
	{
		get
		{
			DateTime Starting = new DateTime(1899,12,30);
			TimeSpan nd = my_date.Subtract(Starting);
			return (double)nd.Days;
		}
		set 
		{
			DateTime Starting = new DateTime(1899,12,30);
			my_date = Starting.AddDays(value);
		}
	}

	 // Methods Member
     // Method Member: Calculate YearFraction between two dates using Act/360 convection
	public double YF_MM (Date Date2)
	{
		return (Date2.SerialValue-this.SerialValue)/360;
	}

     // Method Member: Calculate YearFraction between two dates using Act/365 convection
	public double YF_365 (Date Date2)
	{
		return (Date2.SerialValue-this.SerialValue)/365;
	}
    
     // Method Member: Calculate YearFraction between two dates using Act/365.25 convection
    public double YF_365_25(Date Date2)
    {
        return (Date2.SerialValue - this.SerialValue) / 365.25;
    }

     // Method Member: Calculate YearFraction between two dates using 30/360 convection
     // Please see "Pricing and hedging swaps" by Paul Miron page 12
	public double YF_BB(Date Date2)
	{
		DateTime my_date2 = Date2.DateValue; 
		if(	my_date2.Day ==31 && (my_date.Day !=30 && my_date.Day !=31))
		{
			return ((double)(my_date2.Day-my_date.Day)+ 30*(my_date2.Month-my_date.Month)+
				360*(my_date2.Year-my_date.Year))/360;
		};

		if (my_date.Month==2 && (my_date2.AddDays(1).Month==3))
		{
			return (double) (
				(my_date2.Day+ DateTime.DaysInMonth(my_date.Year,my_date.Month)-my_date.Day)+
				360*(my_date2.Year-my_date.Year)+
				30*(my_date2.Month-my_date.Month-1))/360;
		};
		double a = (30 - my_date.Day > 0) ? 30 - my_date.Day :0;
		double b = (my_date2.Day < 30) ? my_date2.Day :30;
		return (double) (a+b+
			360*(my_date2.Year-my_date.Year)+
			30*(my_date2.Month-my_date.Month-1))/360;		
	}
    
     // 30-360 Bond Basis, see http: // www.isda.org/c_and_a/docs/30-360-2006ISDADefs.xls
    public double YF_30_360(Date Date2)
    { 
        DateTime my_date2 = Date2.DateValue; // calculate YearFraction from my_date to my_date2
        int D1 = my_date.Day;  // Day of my_date (i.e. for 14 Sep 19 is 14)
        int D2 = my_date2.Day ;
        int M1 = my_date.Month; // Month of my_date (i.e. for 14 Sep 19 is 9)
        int M2 = my_date2.Month;
        int Y1 = my_date.Year; // Year of my_date (i.e. for 14 Sep 19 is 2019)
        int Y2 = my_date2.Year;

         // Date adjustment rules for 30E/360
        if (D1 == 31) D1 = 30;
        if ((D2 == 31) && (D1 > 29)) D2 = 30;        
        
         // generic formulas for 30/360
        return (double)(360 * (Y2-Y1) +30 * (M2-M1) + (D2-D1) ) / 360;
    }

     // 30E/360 ISDA Bond Basis, see http: // www.isda.org/c_and_a/docs/30-360-2006ISDADefs.xls
    public double YF_30_360E_ISDA(Date Date2)
    { 
        DateTime my_date2 = Date2.DateValue; // calculate YearFraction from my_date to my_date2
        int D1 = my_date.Day;  // Day of my_date (i.e. for 14 Sep 19 is 14)
        int D2 = my_date2.Day ;
        int M1 = my_date.Month; // Month of my_date (i.e. for 14 Sep 19 is 9)
        int M2 = my_date2.Month;
        int Y1 = my_date.Year; // Year of my_date (i.e. for 14 Sep 19 is 2019)
        int Y2 = my_date2.Year;

         // Date adjustment rules for 30E/360
        if ((M1 == 2 && (my_date.AddDays(1).Month == 3)) || (D1 == 31)) D1 = 30;
        if ((M2==2 && (my_date2.AddDays(1).Month==3)) || (D2 == 31)) D2 = 30;
           
         // generic formulas for 30/360
        return (double)(360 * (Y2-Y1) +30 * (M2-M1) + (D2-D1) ) / 360;
    }

     // 30E/360 ISDA Bond Basis, see http: // www.isda.org/c_and_a/docs/30-360-2006ISDADefs.xls
     // If we have more than a period should be also specified the Termination Date
    public double YF_30_360E_ISDA(Date Date2, Date TerminationDate)
    {
        DateTime my_date2 = Date2.DateValue; // calculate YearFraction from my_date to my_date2
        int D1 = my_date.Day;  // Day of my_date (i.e. for 14 Sep 19 is 14)
        int D2 = my_date2.Day;
        int M1 = my_date.Month; // Month of my_date (i.e. for 14 Sep 19 is 9)
        int M2 = my_date2.Month;
        int Y1 = my_date.Year; // Year of my_date (i.e. for 14 Sep 19 is 2019)
        int Y2 = my_date2.Year;

         // Date adjustment rules for 30E/360
        if ((M1 == 2 && (my_date.AddDays(1).Month == 3)) || (D1 == 31)) D1 = 30;
        if ((M2 == 2 && (my_date2.AddDays(1).Month == 3) && (my_date2.Equals(TerminationDate.DateValue)==false)) || (D2 == 31)) D2 = 30;
        
         // generic formulas for 30/360
        return (double)(360 * (Y2 - Y1) + 30 * (M2 - M1) + (D2 - D1)) / 360;
    }

     // Method Member: Calculate YearFraction between two dates using 30E/360 convection
     // http: // en.wikipedia.org/wiki/Day_count_convention see 30E/360
     // see http: // www.isda.org/c_and_a/docs/30-360-2006ISDADefs.xls
    public double YF_30_360E(Date Date2)
    {   
        DateTime my_date2 = Date2.DateValue; // calculate YearFraction from my_date to my_date2
        int D1 = my_date.Day;  // Day of my_date (i.e. for 14 Sep 19 is 14)
        int D2 = my_date2.Day;
        int M1 = my_date.Month; // Month of my_date (i.e. for 14 Sep 19 is 9)
        int M2 = my_date2.Month;
        int Y1 = my_date.Year; // Year of my_date (i.e. for 14 Sep 19 is 2019)
        int Y2 = my_date2.Year;

         // Date adjustment rules for 30E/360
        if (D1 == 31) D1 = 30;
        if (D2 == 31) D2 = 30;
        
         // generic formulas for 30/360
        return (double)(360 * (Y2-Y1) +30 * (M2-M1) + (D2-D1) ) / 360;
    }

     // see "the Actual/actual Day count fraction -
     // paper for USE with the ISDA Market Convection Survey -3rd June, 1999
    public double YF_ACT_ACT_ISMA(Date Date2, Date TerminationDate) 
    {
         // it does not consider exceptions in relations to irregular coupon period
         // estimate roughly the length in months of a period
        double startDate = this.SerialValue;
        double endDate = TerminationDate.SerialValue;

        int months = (int)(0.5 + 12 * (endDate- startDate) / 365.0);
        double Nperiod = 12.0 / months;

        return (Date2.SerialValue - startDate) / ((endDate - startDate) * Nperiod);
    }

    public double YF_ACT_ACT_ISMA(Date Date2, Date TerminationDate, bool IsRegular)
    {
         // it does not consider exceptions in relations to irregular coupon period
         // estimate roughly the length in months of a period
        double startDate = this.SerialValue;
        double endDate = TerminationDate.SerialValue;

        int months = (int)(0.5 + 12 * (endDate - startDate) / 365.0);
        double Nperiod = 12.0 / months;

        return (Date2.SerialValue - startDate) / ((endDate - startDate) * Nperiod);
    }

     // Method Member: Calculate YearFraction between two dates using Act/Act convection
    public double YF_AA (Date Date2)
	{
	DateTime  my_date2 = Date2.DateValue;
	double d1=365, d2=365;
	int Y1 =my_date.Year,Y2 =my_date2.Year;
	if (DateTime.IsLeapYear(Y1)) d1=366; 
	if (DateTime.IsLeapYear(Y2)) d2=366;	
	int diff =Y2-Y1;
		if (diff==0)
		{
			return (Date2.SerialValue-this.SerialValue)/d1;			
		}
		else
		{
		Date End1 = new Date(Y1,12,31);
		Date End2 = new Date(Y2-1,12,31);
		return (End1.SerialValue-this.SerialValue)/d1 +
			(diff-1)
			+(Date2.SerialValue-End2.SerialValue)/d2;
		}		
	}

     // Method Member: Calculate YearFraction between two dates using Act/Act convection considering leap years
    public double YF_AAL(Date Date2)
    { 
        DateTime my_date2 = Date2.DateValue;
        double d1 = 365;
        int Y1 = my_date.Year, Y2 = my_date2.Year;
        if (DateTime.IsLeapYear(Y1)) d1 = 366;
        int diff = Y2 - Y1;
        if (diff == 0)
        {
            return (Date2.SerialValue - this.SerialValue) / d1;
        }
        else
        {
            Date End1 = new Date(Y1, my_date2.Month, my_date2.Day);
            if (End1.SerialValue - this.SerialValue > 0)
            {
               
                 return (End1.SerialValue - this.SerialValue) / d1 + diff;
            }
            else
            {

                Date End2 = new Date(Y1+1, my_date2.Month, my_date2.Day);
                return (End2.SerialValue - this.SerialValue) / d1 + diff-1;                
            }
        }
    }
     
     // Method Member: calculate numbers of business days between two dates, given an upper bound of days
    public double NumberOfBD(double firstDate, double secondDate, int maxDays) 
    {
        double dummyDate = firstDate;
        int k = 0;
        int clock = 0;
        while (dummyDate != secondDate) 
        {
        Date md = new Date(dummyDate);
        dummyDate= md.add_workdays(1).SerialValue;
        k++;
        clock++;
        if (k == maxDays) { return 0; }
        }
        return clock;
    }    
   
     // Method Member: calculate numbers of days between two dates according YF_MM
	public double D_MM(Date Date2)
	{
		return YF_MM(Date2)*360;
	}

     // Method Member: calculate numbers of days between two dates according YF_365
	public double D_365(Date Date2)
	{
		return YF_365(Date2)*365;
	}

     // Method Member: calculate numbers of days between two dates according YF_BB
	public double D_BB(Date Date2)
	{
		return YF_BB(Date2)*360;
	}

      // Method Member: calculate numbers of days between two dates according YF_30_360
	public double D_30_360(Date Date2)
	{
		return YF_30_360(Date2)*360;
	}

     // Method Member: calculate numbers of days between two dates according YF_30_360E_ISDA
	public double D_30_360E_ISDA(Date Date2)
	{
		return YF_30_360E_ISDA(Date2)*360;
	}

     // Method Member: calculate numbers of days between two dates according YF_30_360E_ISDA
	public double D_30_360E_ISDA(Date Date2,Date TerminationDate)
	{
		return YF_30_360E_ISDA(Date2,TerminationDate)*360;
	}
    
     // Method Member: calculate numbers of days between two dates according YF_BBE
	public double D_30_360E(Date Date2)
	{
		return YF_30_360E(Date2)*360;
	}

     // Method Member: calculate numbers of days between two dates 
    public double D_EFF(Date Date2) 
    {
        return Date2.SerialValue - this.SerialValue;
    }

     // Method Member: adjust the date according to modified following convection: the date
     // is rolled to the next business day, unless doing so you will find a date in the next calendar month,
     // in which case the date is rolled on the previous business day. (see http: // en.wikipedia.org/wiki/Accrued_interest
	public Date mod_foll()
	{
		Date OutPut = new Date();
		DayOfWeek dayOfWeek = my_date.DayOfWeek;
		if (dayOfWeek == DayOfWeek.Sunday)
		{
			if (my_date.Month ==  my_date.AddDays(1).Month)
			{
					OutPut.DateValue = my_date.AddDays(1);
				return OutPut;}
			else
			{
					OutPut.DateValue = my_date.AddDays(-2);
				return OutPut;}
		}

		if (dayOfWeek == DayOfWeek.Saturday)
		{
			if (my_date.Month ==  my_date.AddDays(2).Month)
			{
				OutPut.DateValue = my_date.AddDays(2);
				return OutPut;}
			else
			{
				OutPut.DateValue = my_date.AddDays(-1);
				return OutPut;}
		}
		else 
			return this;
	}
    
     // GetFollowing, GetModFollowing(same of mod_foll()), GetPreceding, GetUnadjusted are used for enum BusinessDayAdjustment
    public Date GetFollowing() 
    {
        Date OutPut = new Date(this);
        DayOfWeek dayOfWeek = my_date.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Sunday)
        {
           
                OutPut.DateValue = my_date.AddDays(1);
                return OutPut;
        }
        if (dayOfWeek == DayOfWeek.Saturday)
        {
                OutPut.DateValue = my_date.AddDays(2);
                return OutPut;            
        }
        else
            return OutPut;
    }

    public Date GetModFollowing()
    {
        Date OutPut = new Date(this);
        DayOfWeek dayOfWeek = my_date.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Sunday)
        {
            if (my_date.Month == my_date.AddDays(1).Month)
            {
                OutPut.DateValue = my_date.AddDays(1);
                return OutPut;
            }
            else
            {
                OutPut.DateValue = my_date.AddDays(-2);
                return OutPut;
            }
        }

        if (dayOfWeek == DayOfWeek.Saturday)
        {
            if (my_date.Month == my_date.AddDays(2).Month)
            {
                OutPut.DateValue = my_date.AddDays(2);
                return OutPut;
            }
            else
            {
                OutPut.DateValue = my_date.AddDays(-1);
                return OutPut;
            }
        }
        else

            return OutPut;        
    }

    public Date GetPreceding() 
    {
        Date OutPut = new Date(this);
        DayOfWeek dayOfWeek = my_date.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Sunday)
        {

            OutPut.DateValue = my_date.AddDays(-2);
            return OutPut;
        }

        if (dayOfWeek == DayOfWeek.Saturday)
        {
            OutPut.DateValue = my_date.AddDays(-1);
            return OutPut;
        }
        else
            return OutPut;    
    }

    public Date GetUnadjusted() 
    {
        return new Date(this);
    }

    public Date GetBusDayAdjust(BusinessDayAdjustment Conv) 
    {
        switch (Conv)
        {
            case BusinessDayAdjustment.Following :
                return GetFollowing();
                
            case BusinessDayAdjustment.ModifiedFollowing:
                return GetModFollowing();
                
            case BusinessDayAdjustment.Preceding:
                return GetPreceding();
                
            case BusinessDayAdjustment.Unadjusted:
                return GetUnadjusted();
                
            default: 
                break;
        }
        return this;
    }
    
    public static Date[] GetBusDayAdjust(Date[] d, BusinessDayAdjustment Conv) 
    {
        int n = d.GetLength(0);
        Date[] outPut = new Date[n];
        for (int i = 0; i < n; i++) 
        {
            outPut[i] = d[i].GetBusDayAdjust(Conv);
        }
        return outPut;
    }

    public static Date[] GetPeriodShifted(Date[] d, string periodString) 
    {
        int n = d.GetLength(0);
        Date[] outPut = new Date[n];
        for (int i = 0; i < n; i++)
        {
            outPut[i] = d[i].add_period(periodString,false);
        }
        return outPut;
    }

    public static Date[] GetBusinessDayShifted(Date[] d, int BusinessDays)
    {
        int n = d.GetLength(0);
        Date[] outPut = new Date[n];
        for (int i = 0; i < n; i++)
        {
            outPut[i] = d[i].add_workdays(BusinessDays);
        }
        return outPut;    
    }

    public static double[] GetSerialValue(Date[] d)     
    {
        int n = d.GetLength(0);
        double[] outPut = new double[n];
        for (int i = 0; i < n; i++)
        {
            outPut[i] = d[i].SerialValue;
        }
        return outPut;    
    }
    
     // Method Member: add n_day to my date
    public Date add_workdays(int n_days)
	{
		int i; 
		DateTime OutDate = new DateTime();
		OutDate = my_date;
		int iterations = Math.Abs(n_days);
		int increment = Math.Sign(n_days);
		for (i=0; i < iterations;i++)
		{
			OutDate = OutDate.AddDays(increment);
			if( OutDate.DayOfWeek == DayOfWeek.Saturday ||
				OutDate.DayOfWeek == DayOfWeek.Sunday)
				i-=1;
		}	
		
		Date output = new Date (OutDate);
		return output;	
	}

     // Method Member: add "period" to my date ("nd" will add "n" days, "nm" will add "n" months, "ny" will add "n" years) 
	public Date add_period(string period)
	{
        string p = period.ToLower();
		char maturity = p[p.Length-1];
		int n_periods = int.Parse(period.Remove(period.Length-1,1));
		DateTime my_date;
		Date outPut = new Date();
		switch (maturity)
		{
			case 'd':
				outPut= this.add_workdays(n_periods);
				break;
			case 'm':
				my_date = this.DateValue;
				outPut = new Date(my_date.AddMonths(n_periods));
				outPut =outPut.mod_foll();
				
				break;
			case 'y':
				my_date = this.DateValue;
				outPut = new Date(my_date.AddYears(n_periods));
				outPut =outPut.mod_foll();
				break;
			default:
				break;
		}
		return outPut;
	}

     // Method Member: add "period" to my date ("nd" will add "n" days, "nm" will add "n" months, "ny" will add "n" years), modified following can be considered or not
    public Date add_period_string(string period, int modFoll)
	{

        string p = period.ToLower();
		 // char maturity =  period[period.Length-1];
        char maturity = p[p.Length - 1];
		int n_periods = int.Parse(period.Remove(period.Length-1,1));
		DateTime my_date;
		Date outPut = new Date();
		switch (maturity)
		{
			case 'd':
				my_date = this.DateValue;
			if (modFoll == 1)
				outPut= this.add_workdays(n_periods);
			else				
				outPut= new Date(my_date.AddDays(n_periods));
			break;
			case 'm':
				my_date = this.DateValue;
				outPut = new Date(my_date.AddMonths(n_periods));				
				if (modFoll==1) outPut =outPut.mod_foll();
				break;
			case 'y':
				my_date = this.DateValue;
				outPut = new Date(my_date.AddYears(n_periods));
				if (modFoll==1) outPut =outPut.mod_foll();
				break;
			default:
				break;
		}
		return outPut;
	}

     // Method Member: add "period" to my date ("nd" will add "n" days, "nm" will add "n" months, "ny" will add "n" years), modified following can be considered or not
     // new implementation
    public Date add_period(string period,bool modFoll)
    {
        Period p = new Period(period);
        TenorType T = p.tenorType;
        DateTime my_date = this.DateValue;
        Date outPut = new Date();
        switch (T)
        {
            case TenorType.D:
                outPut = new Date(this.DateValue.AddDays(p.tenor));
                break;
            case TenorType.W:
                outPut = new Date(this.DateValue.AddDays(p.tenor * 7));
                break;
            case TenorType.M:
                outPut = new Date(this.DateValue.AddMonths(p.tenor));
                break;
            case TenorType.Y:
                outPut = new Date(this.DateValue.AddYears(p.tenor));
                break;
            default:
                break;
        }

        if (modFoll) { outPut = outPut.mod_foll(); }
        return outPut;    
    }

     // Method Member: subtraction "period" to my date ("nd" will add "n" days, "nm" will add "n" months, "ny" will add "n" years), modified following can be considered or not
     // new implementation
    public Date sub_period(string period, bool modFoll)
    {
        Period p = new Period(period);
        TenorType T = p.tenorType;
        DateTime my_date = this.DateValue;
        Date outPut = new Date();
        switch (T)
        {
            case TenorType.D:
                outPut = new Date(this.DateValue.AddDays(-p.tenor));
                break;
            case TenorType.W:
                outPut = new Date(this.DateValue.AddDays(-p.tenor * 7));
                break;
            case TenorType.M:
                outPut = new Date(this.DateValue.AddMonths(-p.tenor));
                break;
            case TenorType.Y:
                outPut = new Date(this.DateValue.AddYears(-p.tenor));
                break;
            default:
                break;
        }

        if (modFoll) { outPut = outPut.mod_foll(); }
        return outPut;
    }

     // Method Member: calculate numbers of date between the date and a period ("nd" means "n" days, "nm" means "n" months, "ny" means "n" years)
	public double actual_days_in_period(string period)
	{ 
		return  this.DateValue.Subtract(this.add_period(period).DateValue).Days;
		 // alternative solution:
		 // return  this.add_period(period).SerialValue - this.SerialValue;
	}

     // Method Member: a list to use YF_30_360, YF_MM, YF_365 methods
	public double YF (string period,int dayCount)
	{
		double outPut = 0;
		switch(dayCount)
		{
			case 1:
				outPut = this.YF_30_360(this.add_period(period));
			break;
			case 2:
				outPut = this.YF_MM(this.add_period(period));
				break;
			case 3:
				outPut = this.YF_365(this.add_period(period));
				break;
			default: break;
		}
	return outPut;	
	}

     // Method Member: a list to use YF_30_360, YF_MM, YF_365 methods using enum .. to be completed
    public double YF(Date d2, Dc dayCount) 
    {
        double outPut = 0;
        switch (dayCount)
        {
            case Dc._30_360:
                outPut = this.YF_30_360(d2);
                break;
            case Dc._Act_360:
                outPut = this.YF_MM(d2);
                break;
            case Dc._Act_365:
                outPut = this.YF_365(d2); ;
                break;
          
            default: break;
        }

        return outPut;	  
    }

     public static double AccruedInterest(Date settDt, Date LastCpnDt, Date NextCpnDt, double CpnRate, double FaceValue, Dc dayCount) 
    {
        double outPut = 0;
        switch (dayCount)
        {
            case Dc._30_360:
                outPut = LastCpnDt.YF_30_360(settDt) * CpnRate * FaceValue;
                break;
            case Dc._Act_360:
                outPut = LastCpnDt.YF_MM(settDt) * CpnRate * FaceValue;
                break;
            case Dc._Act_365:
                outPut = LastCpnDt.YF_365(settDt) * CpnRate * FaceValue;
                break;
            case Dc._Act_Act_ISMA:
                 // it used ACT_ACT_ISMA
                 // see "the Actual/actual Day count fraction -
                 // paper for USE with the ISDA Market Convection Survey -3rd June, 1999
                outPut = LastCpnDt.YF_ACT_ACT_ISMA(settDt, NextCpnDt) * CpnRate * FaceValue; // yf*Coupon*Nominal
                break;
            case Dc._ItalianBTP:
                 // Calculate accrued interest: it is a specific calculation method for this bond.
                 // see VALID CALCULATION TYPES Bloomberg 14 Apr 10 523: Italian BTPS
                double daysHeld = LastCpnDt.D_EFF(settDt);  // days you held the bond from last coupon date
                double daysPeriod = LastCpnDt.D_EFF(NextCpnDt);  // days in current coupon period
                outPut = Math.Round((CpnRate / 2) * (daysHeld / daysPeriod), 7) * FaceValue; // 6 digit for 1K euro  
                break;
            default: break;
        }
        return outPut;
    }

    public static double[] GetFullCouponArray(Date[] FromCpnDt, Date[] ToCpnDt, double CpnRate, double FaceValue, Dc dayCount)
    {
         // FromCpnDt,ToCpnDt should have same dimension
        int n = FromCpnDt.GetLength(0);
        double[] outPut = new double[n];
        for (int i = 0; i < n; i++)
        {
             // To get the full coupon the settlement date should be equal to nextCpnDate of AccruedInterestMethod
            outPut[i] = AccruedInterest(ToCpnDt[i],FromCpnDt[i],ToCpnDt[i],CpnRate,FaceValue,dayCount);
        }
        return outPut;
    }

     // Method Member: return an array of yearFraction given two array of date  // 18sep10
    public static double[] GetYfArray(Date[] d1, Date[] d2, Dc dayCount) 
    {
         // d1,d2 should have same dimension
        int n = d1.GetLength(0);
        double[] outPut = new double[n];
        for (int i = 0; i < n; i++) 
        {
            outPut[i] = d1[i].YF(d2[i], dayCount);        
        }
        return outPut;
    }

     // Method Member: add "days", where "days" is an int
    public Date AddDays(int days)
    {
        Date outPut = new Date(my_date.AddDays(days));
        return outPut;
    }
     // Method Member: add "months", where "months" is an int
    public Date AddMonths(int months)
    {
        Date outPut = new Date(my_date.AddMonths(months));
        return outPut;
    }

     // Method Member: add "years", where "years" is an int
    public Date AddYears(int years)
    {
        Date outPut = new Date(my_date.AddYears(years));
        return outPut;
    }

     // Method Member: add "months", where months is an int, modified following on
    public Date AddMonthsModFoll(int months)
    {
        Date outPut = new Date(my_date.AddMonths(months));
        return outPut.mod_foll();
    }
    
     // Method Member: It gets the third Wednesday of the month, given the month and the year
    public static Date IMMDate(int Month, int Year)
    {
         // Please see "Function IMM_Date(TheYear As Integer, TheMonth As Integer) As Date" from VBE code
         // http: // www.euronext.com/editorial/wide/editorial-4304-EN.html 106420.xls

        DateTime outPut = new DateTime(Year, Month, 1).AddDays(-1);  // I start with last day of previous month
        
        int NofWednesday =0; // Wednesday counter
        
         // loop to find third Wednesday
        while (NofWednesday < 3) 
        {
            outPut = outPut.AddDays(1);
            if (outPut.DayOfWeek == DayOfWeek.Wednesday) 
            {
                NofWednesday++;
            }
            
        }
        return new Date(outPut);
    }

     // Method Member: It gets the third Wednesday of the month of this object
    public Date IMMDate()
    {
        return IMMDate(this.DateValue.Month, this.DateValue.Year);

    }

     // Method Member: It calculates the Nth nearest IMM Date of a quarterly futures (Mar,Jun,Sep,Dec), given Today and NthStir
    public static Date IMM_Date_Nth(Date Today, int NthStir)
    {
         // Please see "Function EffectiveDate(TodaysDate, NthFuture As Integer)" from VBE code
         // http: // www.euronext.com/editorial/wide/editorial-4304-EN.html 106420.xls

        DateTime Today_ = Today.DateValue; // casting to DateTime
        int Month = Today_.Month; // month int
        int Year = Today_.Year; // year int
        int Rem;  // the remainder
        int result = Math.DivRem(Month, 3,  out Rem);
        int Quarter = Month + (3 - Rem) * Math.Sign(Rem);
            if (Today.SerialValue > (IMMDate(Quarter, Year).SerialValue - 2))
            {
                Rem = Quarter + 3 * NthStir;
            }
            else 
            {
                Rem = Quarter + 3 * (NthStir-1);
            }
        return IMMDate(Rem - 12 * (int)((Rem - 0.5) / 12), Year + (int)((Rem - 0.5) / 12));     
    }

     // Method Member: It returns the Nth nearest Futures starting from this.Date using Date IMM_Date_Nth(Date Today, int NthStir)
    public Date IMM_Date_Nth(int NthStir)
    {
        return IMM_Date_Nth(this, NthStir);        
    }

	 // Member modifiers
	public void GetNewDate(int Year,int Month,int Day)
	{my_date = new DateTime(Year,Month,Day);	}


     // return the first day of a month
    public static Date FirstDayOfMonth(Date myDate) 
    {
        return new Date(myDate.DateValue.Year, myDate.DateValue.Month, 1);
    }
     // return the last day of month
    public static Date LastDayOfMonth(Date myDate) 
    {
        Date FD = FirstDayOfMonth(myDate);
        return FD.AddMonths(1).AddDays(-1);
    }

     // Calculate effective day in a month
    public static double EffDaysInMonth(Date myDate) 
    {
        return LastDayOfMonth(myDate).SerialValue - FirstDayOfMonth(myDate).SerialValue + 1;
    }

     // Binary Operators
     // Binary operator: add n_days 
	public static Date operator + (Date my_date2,int n_days)
	{
		DateTime OutDate = new DateTime( );
		OutDate = my_date2.DateValue.AddDays(n_days);
		Date OutPut =new Date(OutDate);
		return OutPut;
	}
     // Binary operator: subtract n_days 
	public static Date operator - (Date my_date2,int n_days)
	{
		DateTime OutDate = new DateTime( );
		OutDate = my_date2.DateValue.AddDays(-n_days);
		Date OutPut =new Date(OutDate);
		return OutPut;
	}

     // Binary operator: is equal? 
	public static bool operator == (Date my_date1, Date my_date2)
	{
		if (Object.Equals(my_date1,null))
			if (Object.Equals(my_date2,null))
				return true;
			else
				return false;
		else
			return my_date1.Equals(my_date2);	
	}

     // Binary operator: is not equal?
	public static bool operator != (Date my_date1, Date my_date2)
	{
		return !(my_date1 == my_date2);
	}

     // Binary operator: is >?
	public static bool operator > (Date my_date1,Date my_date2)
	{
	return my_date1.DateValue > my_date2.DateValue;
	}

     // Binary operator: is <?
	public static bool operator < (Date my_date1,Date my_date2)
	{
		return my_date1.DateValue < my_date2.DateValue;
	}

     // Binary operator: is >?
	public override bool Equals (Object obj)
	{
		if (obj == null) return false;
		Date my_date2 = (Date) obj;
		if (this.my_date == my_date2.my_date) return true;
		else return false;
	}
    
     // GetHashCode
	public override int GetHashCode()
	{
		string hashString = this.my_date.ToString();
		return hashString.GetHashCode();
	}

    public static void PrintDateVector(Date[] v) 
    {
        foreach (Date d in v) 
        {
            Console.WriteLine("{0:D}", d.DateValue);
        }    
    }
}