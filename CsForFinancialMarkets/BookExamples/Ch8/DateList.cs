// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author for the use of this code in any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// DateList.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Xml;
using System.Xml.Serialization;

//
//The goal of this class is to crate a sort of scheduler of dates that can be used per a stream of cash flows
// a standard scheduler will be like a matrix of n rows and 4 column.
// 1° column will contain fixing date of i-row
// 2° column will contain the start date of i-row
// 3° column will contain the end date of i-row
// 4° column will contain the payment date of i-row

[Serializable] public class DateList
{
    //Data members
	public int adjusted;// 1= Adj, (adjust for business days)
	public int arrears;// 1 = arrears yes (fixingDays will be counted from at the end of period)
    public int fixingDays;// number of days before for taking the fixing
	public Date[] my_dateList; //my Date vector
	
    //Constructors
    /*Constructor using:
    StartDate starting date of scheduler, EndDate: end of scheduler
    ShortPeriod (1=yes) if the starting period is short
    Adjusted (1=yes) if adjusted according modified following
    Arrears (1=yes) if Fixing days (1°column) start to be calculate from the end of period, otherwise from the start
    FixingDay: number of days of for fixing procedure i.e. -2 will fix two days before
	*/
    // parameterless constructor
    public DateList() { }
    
    public DateList(DateTime StartDate,DateTime EndDate, int PaymentPerYear,int ShortPeriod,
		int Adjusted, int Arrears, int FixingDays)
	{
		this.adjusted =Adjusted;
		this.arrears =Arrears;
		this.fixingDays =FixingDays;
		int MonthsNumber =(EndDate.Year-StartDate.Year)*12+
			(EndDate.Month-StartDate.Month);
		int rem = 0, i ; 
		int Freq = (int) (12/PaymentPerYear);
		if((MonthsNumber % Freq) != 0) rem = 1;
		MonthsNumber /= Freq;
		int n= rem + MonthsNumber+1;
		my_dateList = new Date[n];
		my_dateList[0] = new Date(StartDate);
		if (rem !=0)
		{			
			if (ShortPeriod ==1)
			{
				for (i=1;i<n;i++)
					my_dateList[i]=new Date(EndDate.AddMonths(Freq * (-n+i+1)));			
			}
			else 
			{
				for (i=1;i<n;i++)
				my_dateList[i]=new Date(StartDate.AddMonths(Freq * (i)));
				my_dateList[n-1] =new Date(EndDate);			
			}
		}

		else //rem ==0
		{
			if (ShortPeriod == 1 )
			{
			for (i=1;i<n;i++)
				my_dateList[i]=new Date(EndDate.AddMonths(Freq * (-n+i+1)));			
			}
			else
			{
				for ( i=1;i<n;i++)
				my_dateList[i]=new Date(StartDate.AddMonths(Freq*i));
				my_dateList[n-1] =new Date(EndDate);
			}			
		}	
	}

    //short cut for constructor... a very std one
	public DateList(Date StartDate,Date EndDate, int PaymentPerYear,int ShortPeriod):
		this( StartDate.DateValue,EndDate.DateValue,PaymentPerYear,ShortPeriod,1 , 0 , -2)
	{}//std conventions
	
	//Member Function
    //to adjust dates according to mod foll., for payment date
	public Date[] Adj()
	{
	int n = my_dateList.GetLength(0);
	Date[] my_new_dateList = new Date[n];
		for (int i=0; i < n; i++)
		{
			my_new_dateList[i] = my_dateList[i].mod_foll();
		}

		return my_new_dateList;
	}

    // calculate fixing dates vector
	public Date[] FixingList(int n_days)
	{
		int n = my_dateList.GetLength(0);
		Date[] my_new_dateList = new Date[n];
		for (int i=0; i < n; i++)
		{
			my_new_dateList[i] = my_dateList[i].add_workdays(n_days);
		}

		return my_new_dateList;
	}
    
    //read only property
    // the main output a 4column matrix as described 
    public Date[,] GetDateList
    {
        get
        {
            int n = my_dateList.Length - 1;
            Date[,] outList = new Date[n, 4];
            if (this.adjusted == 1)
                for (int i = 0; i < n; i++)
                {

                    outList[i, 1] = this.Adj()[i];
                    outList[i, 2] = this.Adj()[i + 1];
                    outList[i, 3] = this.Adj()[i + 1];
                    if (this.arrears == 1)
                        outList[i, 0] = outList[i, 2].add_workdays(fixingDays);
                    else
                        outList[i, 0] = this.FixingList(fixingDays)[i];
                }
            else
                for (int i = 0; i < n; i++)
                {
                    outList[i, 1] = this.my_dateList[i];
                    outList[i, 2] = this.my_dateList[i + 1];
                    outList[i, 3] = this.Adj()[i + 1];
                    if (this.arrears == 1)
                        outList[i, 0] = outList[i, 2].add_workdays(fixingDays);
                    else
                        outList[i, 0] = this.FixingList(fixingDays)[i];

                }


            return outList;
        }
    }
        
    //Utility to print the main output
    public void PrintVectDateList()
    {
        Date[,] myDL = this.GetDateList;
        int r = myDL.GetLength(0);
        int c = myDL.GetLength(1);
        for (int j = 0; j < r; j++)
        {
            for (int i = 0; i < c; i++)
            {
                Console.Write("{0:ddd_dd_MMM_yyyy}\t", myDL[j, i].DateValue);
            }
            Console.WriteLine();
        }
    }
}