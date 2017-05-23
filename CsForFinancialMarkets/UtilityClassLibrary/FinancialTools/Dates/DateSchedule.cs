// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// DateSchedule.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
 // 
 // The goal of this class is to crate a sort of scheduler of dates that can be used for a stream of cash flows
 // a standard scheduler will be like a matrix of n rows and 4 column.
 // 1° column will contain fixing date of i-row
 // 2° column will contain the start date of i-row
 // 3° column will contain the end date of i-row
 // 4° column will contain the payment date of i-row

public class DateSchedule
{
     // Data members
	private bool adjusted; // true = adjust for business days
	private bool arrears; // true = fixingDays will be counted from at the end of period
    private bool firstShortPeriod; // true = the short period is at beginning: rolling from the end (backward) 
    private int fixingDays; // number of days before for taking the fixing
	private Array<Date> my_DateArray; // my Date array
    private Array<Date> my_DateArrayAdjusted; // my Date array adjusted
	
     // Constructors
    /*Constructor using:
    StartDate starting date of scheduler, EndDate: end of scheduler
    ShortPeriod (1=yes) if the starting period is short
    Adjusted (1=yes) if adjusted according modified following
    Arrears (1=yes) if Fixing days (1°column) start to be calculate from the end of period, otherwise from the start
    FixingDay: number of days of for fixing procedure i.e. -2 will fix two days before
	*/

     // Base constructor
    public DateSchedule(DateTime StartDate, DateTime EndDate, int PaymentPerYear, bool FirstShortPeriod,
        bool Adjusted, bool Arrears, int FixingDays)
    {
         // Updating Data Member
        this.adjusted = Adjusted;
        this.arrears = Arrears;
        this.fixingDays = FixingDays;
        this.my_DateArray = DateArrayCreator(StartDate, EndDate, PaymentPerYear, FirstShortPeriod);
        this.my_DateArrayAdjusted = this.Adj();
    }

	public DateSchedule(Date StartDate,Date EndDate, int PaymentPerYear,bool ShortPeriod):
		this( StartDate.DateValue,EndDate.DateValue,PaymentPerYear,ShortPeriod,true , false , -2)
	{} // std convention	
	
	 // Copy Constructor
    public DateSchedule(DateSchedule my_dateListo)
	{
	    this.adjusted = my_dateListo.adjusted;
	    this.arrears= my_dateListo.arrears;
	    this.fixingDays = my_dateListo.fixingDays;
        this.firstShortPeriod = my_dateListo.firstShortPeriod;	   
        this.my_DateArray = my_dateListo.my_DateArray;
        this.my_DateArrayAdjusted = my_dateListo.my_DateArrayAdjusted;
	}
	
	 // Member Function
     // DateListCreator: private method to crate Date vector, a base Array of date according inputs
    private Array<Date> DateArrayCreator(DateTime StartDate,DateTime EndDate, int PaymentPerYear,bool FirstShortPeriod)
    {        
		int MonthsNumber =(EndDate.Year-StartDate.Year)*12+
			(EndDate.Month-StartDate.Month);
		int rem = 0, i ; 
		int Freq = (int) (12/PaymentPerYear);
		if((MonthsNumber % Freq) != 0) rem = 1;
		MonthsNumber /= Freq;
		int n= rem + MonthsNumber+1;
		Array<Date> my_dateArray = new Array<Date>(n,0);
        my_dateArray[0] = new Date(StartDate);
		if (rem !=0)
		{			
			if (FirstShortPeriod ==true)
			{
				for (i=1;i<n;i++)
                    my_dateArray[i] = new Date(EndDate.AddMonths(Freq * (-n + i + 1)));			
			}
			else 
			{
				for (i=1;i<n;i++)
                my_dateArray[i] = new Date(StartDate.AddMonths(Freq * (i)));
                my_dateArray[n - 1] = new Date(EndDate);			
			}
		}
		else  // rem ==0
		{
            if (FirstShortPeriod == true)
			{
			for (i=1;i<n;i++)
                my_dateArray[i] = new Date(EndDate.AddMonths(Freq * (-n + i + 1)));			
			}
			else
			{
				for ( i=1;i<n;i++)
                    my_dateArray[i] = new Date(StartDate.AddMonths(Freq * i));
                my_dateArray[n - 1] = new Date(EndDate);
			}			
		}
        return my_dateArray;
    }     

    public Array<Date> Adj() 
    {
        int nElement = my_DateArray.Length;        
        int minIndex = my_DateArray.MinIndex;
        int maxIndex = my_DateArray.MaxIndex;

        Array<Date> my_new_DateArray = new Array<Date>(nElement, minIndex);
        for (int i = minIndex; i <= maxIndex; i++)
        {
            my_new_DateArray[i] = my_DateArray[i].mod_foll();
        }
        return my_new_DateArray;
    }
 
     // Calculate Array of fixing 
    public Array<Date> FixingArray()
    {
        int nElement = my_DateArray.Length-1;  //        
        int minIndex = my_DateArray.MinIndex;
         // my_new_DateArray has 1 element less then my_DateArray
        Array<Date> my_new_DateArray = new Array<Date>(nElement, minIndex);
         // checking if it is adj
        Array<Date> my_DateArrayRef = 
            (this.adjusted == true) ? 
            new Array<Date>(this.my_DateArrayAdjusted) : new Array<Date>(this.my_DateArray);

        if (this.arrears == false)
            for (int i = minIndex; i < nElement; i++)
            {
                my_new_DateArray[i] = my_DateArrayRef[i].add_workdays(this.fixingDays);
            }
        else
            for (int i = minIndex; i < nElement; i++)
            {
                my_new_DateArray[i] = my_DateArrayRef[i + 1].add_workdays(this.fixingDays);
            }
        return my_new_DateArray;
    }

     // Calculate Array of FromDate
    public Array<Date> FromDateArray()
    {
        int nElement = my_DateArray.Length - 1;
        int minIndex = my_DateArray.MinIndex;
        Array<Date> my_new_DateArray = new Array<Date>(nElement, minIndex);
         // checking if it is adj
        Array<Date> my_DateArrayRef =
            (this.adjusted == true) ?
            new Array<Date>(this.my_DateArrayAdjusted) : new Array<Date>(this.my_DateArray);

        for (int i = minIndex; i < nElement; i++)
        {
            my_new_DateArray[i] = my_DateArrayRef[i];
        }
        return my_new_DateArray;
    }

     // Calculate Array of ToDate
    public Array<Date> ToDateArray()
    {
        int nElement = my_DateArray.Length - 1;
        int minIndex = my_DateArray.MinIndex;
        Array<Date> my_new_DateArray = new Array<Date>(nElement, minIndex);
         // checking if it is adj
        Array<Date> my_DateArrayRef =
            (this.adjusted == true) ?
            new Array<Date>(this.my_DateArrayAdjusted) : new Array<Date>(this.my_DateArray);
        for (int i = minIndex; i < nElement; i++)
        {
            my_new_DateArray[i] = my_DateArrayRef[i+1];
        }
        return my_new_DateArray;
    }

     // Calculate Array of PaymentDate
    public Array<Date> PaymentDateArray()
    {
        int nElement = my_DateArray.Length - 1;
        int minIndex = my_DateArray.MinIndex;
        Array<Date> my_new_DateArray = new Array<Date>(nElement, minIndex);
       
        for (int i = minIndex; i < nElement; i++)
        {
            my_new_DateArray[i] = my_DateArrayAdjusted[i + 1];
        }
        return my_new_DateArray;
    }

     // the main output a 4column matrix as described
    public NumericMatrix<Date> GetLongScheduleDate()
    {
       
            Array<Date> myFixingArray = this.FixingArray();
            Array<Date> myFromDateArray = this.FromDateArray();
            Array<Date> myToDateArray = this.ToDateArray();
            Array<Date> myPaymentArray = this.PaymentDateArray();

            NumericMatrix<Date> myOutMatrix = new NumericMatrix<Date>(myFixingArray.Length, 4, 0, 0);

            myOutMatrix.Column(0, myFixingArray);
            myOutMatrix.Column(1, myFromDateArray);
            myOutMatrix.Column(2, myToDateArray);
            myOutMatrix.Column(3, myPaymentArray);

            return myOutMatrix;        
    }

     // take only 2° and 3° column  fromDate and toDate of matrix
    public NumericMatrix<Date> GetShortScheduleDate() 
    {        
            Array<Date> myFromDateArray = this.FromDateArray();
            Array<Date> myToDateArray = this.ToDateArray();

            NumericMatrix<Date> myOutMatrix = new NumericMatrix<Date>(myFromDateArray.Length, 2, 0, 0);
        
            myOutMatrix.Column(0, myFromDateArray);
            myOutMatrix.Column(1, myToDateArray);

            return myOutMatrix;        
    }

     // read only property:Matrix<Date> GetLongScheuleDate() but output will be in excel serial number
    public NumericMatrix<double> GetLongScheduleSerial() 
    {        
            int nElement = my_DateArray.Length - 1;
            NumericMatrix<double> myOutMatrix = new NumericMatrix<double>(nElement, 4, 0, 0);
            Matrix<Date> myDateMatrix = this.GetLongScheduleDate();
            
             // I get min and max to iterate more clearly
            int minRowIndex = myDateMatrix.MinRowIndex;
            int maxRowIndex = myDateMatrix.MaxRowIndex;
            int minColumnIndex = myDateMatrix.MinColumnIndex;
            int maxColumnIndex = myDateMatrix.MaxColumnIndex;

            for (int r = minRowIndex; r <= maxRowIndex; r++)
            {
                for (int c = minColumnIndex; c <= maxColumnIndex; c++)
                {
                    myOutMatrix[r, c] = myDateMatrix[r, c].SerialValue;
                }
            }
            return myOutMatrix;        
    }

     // Get length
    public int Length
    {
        get
        {
            return this.my_DateArray.Length;
        }
    }

     // Utility to print a Date Vector
    public void PrintDateArray(Array<Date> myDateArray)
    {
        int minIndex = myDateArray.MinIndex;
        int maxIndex = myDateArray.MaxIndex;

        for (int i = minIndex; i <= maxIndex; i++)
        {
            Console.WriteLine("{0:dd,MMM,yyyy}",myDateArray[i].DateValue);
        }
    }

     // Utility to print a Date Matrix
    public void PrintDateMatrix(Matrix<Date> dateMatrix)
    {
         // I get min and max to iterate more clearly
        int minRowIndex = dateMatrix.MinRowIndex;
        int maxRowIndex = dateMatrix.MaxRowIndex;
        int minColumnIndex = dateMatrix.MinColumnIndex;
        int maxColumnIndex = dateMatrix.MaxColumnIndex;

        for (int j = minRowIndex; j <= maxRowIndex; j++)
        {
            for (int i = minColumnIndex; i <= maxColumnIndex; i++)
            {
                Console.Write("{0:ddd_dd_MMM_yyyy}\t", dateMatrix[j, i].DateValue);
            }
            Console.WriteLine();
        }
    }

     // Utility to print this long schedule date
    public void PrintDateMatrix()
    {
        Matrix<Date> dateMatrix = this.GetLongScheduleDate();
       
         // I get min and max to iterate more clearly
        int minRowIndex = dateMatrix.MinRowIndex;
        int maxRowIndex = dateMatrix.MaxRowIndex;
        int minColumnIndex = dateMatrix.MinColumnIndex;
        int maxColumnIndex = dateMatrix.MaxColumnIndex;

        for (int j = minRowIndex; j <= maxRowIndex; j++)
        {
            for (int i = minColumnIndex; i <= maxColumnIndex; i++)
            {
                Console.Write("{0:ddd_dd_MMM_yyyy}\t", dateMatrix[j, i].DateValue);
            }
            Console.WriteLine();
        }
    }

     // Utility to print a Double Matrix
    public void PrintDoubleMatrix(double[,] doubleMatrix)
    {
        int r = doubleMatrix.GetLength(0);
        int c = doubleMatrix.GetLength(1);
        for (int j = 0; j < r; j++)
        {
            for (int i = 0; i < c; i++)
            {
                Console.Write("{0}\t", doubleMatrix[j, i]);
            }
            Console.WriteLine();
        }
    }   
}