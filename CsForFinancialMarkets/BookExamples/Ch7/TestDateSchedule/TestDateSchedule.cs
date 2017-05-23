// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// TestDateSchedule.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

public class TestDateSchedule
{
    public static void Main() 
    {
        // Please uncomment to run your choice as stand alone Console Project
        //Example1();
        //Example2();
        //Example3();
        //Example4();
        //Example5();
        //Example6();
        //Example7();
        //Example8();
        //Example9();
        //Example10();
        //Example11();
        //Example12();    
    }

    public static void Example1()
    {
        // Example 1: Constructors:checking different constructors
        Date date1 = new Date();
        Console.WriteLine("Constructor: = new Date(), Output: {0:D}", date1.DateValue);
        Date date2 = new Date(2006, 5, 15);
        Console.WriteLine("Constructor: = new Date(2006,5,15), Output: {0:D}", date2.DateValue);
        Date date3 = new Date(new DateTime());
        Console.WriteLine("Constructor: = new Date(new DateTime()), Output: {0:D}", date3.DateValue);
        Date date4 = new Date(38852);
        Console.WriteLine("Constructor: = new new Date(38852), Output: {0:D}", date4.DateValue);
    }
    // Testing Date and DateSchedule
    public static void Example2()
    {
        // Example 2: year fraction
        // Year Fraction shorter then a year
        Date startDate = new Date(2006, 3, 2);
        Date endDate = new Date(2006, 5, 12);
        Console.WriteLine("StartDate:{0:D},\tEndDate:{1:D}", startDate.DateValue, endDate.DateValue);
        Console.WriteLine("YearFraction Act/Act: " + startDate.YF_AA(endDate));
        Console.WriteLine("YearFraction Act/360: " + startDate.YF_MM(endDate));
        Console.WriteLine("YearFraction 30/360: " + startDate.YF_30_360(endDate));
        Console.WriteLine("YearFraction Act/365: " + startDate.YF_365(endDate));
        Console.WriteLine("YearFraction Act/365.25: " + startDate.YF_365_25(endDate));

        Console.WriteLine("***********");

        // Year Fraction one year tenor
        Date startDate_1 = new Date(2009, 3, 2);
        Date endDate_1 = new Date(2010, 3, 2);
        Console.WriteLine("StartDate:{0:D},\tEndDate:{1:D}", startDate_1.DateValue, endDate_1.DateValue);
        Console.WriteLine("YearFraction Act/Act: " + startDate_1.YF_AA(endDate_1));
        Console.WriteLine("YearFraction Act/360: " + startDate_1.YF_MM(endDate_1));
        Console.WriteLine("YearFraction 30/360: " + startDate_1.YF_30_360(endDate_1));
        Console.WriteLine("YearFraction Act/365: " + startDate_1.YF_365(endDate_1));
        Console.WriteLine("YearFraction Act/365.25: " + startDate_1.YF_365_25(endDate_1));
        Console.WriteLine("***********");

        // 30/360
        Date startDate_3 = new Date(2007, 1, 15);
        Date endDate_3 = new Date(2007, 1, 31);
        Console.WriteLine("StartDate:{0:D},\tEndDate:{1:D}", startDate_3.DateValue, endDate_3.DateValue);
        Console.WriteLine("Days 30/360: " + startDate_3.D_30_360(endDate_3));
        Console.WriteLine("Days 30E/360: " + startDate_3.D_30_360E(endDate_3));
        Console.WriteLine("Days 30E/360 ISDA: " + startDate_3.D_30_360E_ISDA(endDate_3));

        Date startDate_4 = new Date(2007, 2, 14);
        Date endDate_4 = new Date(2007, 2, 28);
        Console.WriteLine("StartDate:{0:D},\tEndDate:{1:D}", startDate_4.DateValue, endDate_4.DateValue);
        Console.WriteLine("Days 30/360: " + startDate_4.D_30_360(endDate_4));
        Console.WriteLine("Days 30E/360: " + startDate_4.D_30_360E(endDate_4));
        Console.WriteLine("Days 30E/360 ISDA: " + startDate_4.D_30_360E_ISDA(endDate_4));
        Console.WriteLine("***********");
    }

    public static void Example3()
    {
        // Calculating difference in days according different conv.
        Date startDate = new Date(2012, 2, 15);
        Date endDate = new Date(2012, 8, 15);
        Console.WriteLine("StartDate:{0:D},\tEndDate:{1:D}", startDate.DateValue, endDate.DateValue);
        Console.WriteLine("Number of days according Act/360: " + startDate.D_MM(endDate));
        Console.WriteLine("Number of days according 30/360: " + startDate.D_30_360(endDate));

        Console.WriteLine();

        Date date1 = new Date(2009, 6, 11);

        // Adding working date, actual days in a period
        Console.WriteLine("Date1 is : {0:D}", date1.DateValue);
        Console.WriteLine("Date1 + 1 working days : {0:D}, actual days between: {1}", date1.add_workdays(1).DateValue,
             date1.D_EFF(date1.add_workdays(1)));
        Console.WriteLine("Date1 + 2 working days : {0:D}, actual days between: {1}", date1.add_workdays(2).DateValue,
            date1.D_EFF(date1.add_workdays(2)));
        Console.WriteLine("Date1 + 3 working days : {0:D}, actual days between: {1}", date1.add_workdays(3).DateValue,
            date1.D_EFF(date1.add_workdays(3)));

        // Adding periods
        Console.WriteLine("Date1 + 2 days : {0:D}", date1.add_period_string("2d", 0).DateValue);
        Console.WriteLine("Date1 + 4 months : {0:D}", date1.add_period_string("4m", 0).DateValue);
        Console.WriteLine("Date1 + 4 months mod foll: {0:D}", date1.add_period_string("4m", 1).DateValue);
        Console.WriteLine("Date1 + 7 years : {0:D}", date1.add_period_string("7y", 0).DateValue);
    }

    public static void Example4()
    {
        DateTime startDate = new Date(2009, 8, 3).DateValue;
        DateTime endDate = new Date(2014, 10, 3).DateValue;

        // Now test with bool as well
        bool adjusted = true;
        int paymentPerYear = 2;
        bool arrears = false;
        int fixingDays = -2;
        bool shortPeriod = true;
        DateSchedule myDL_1 = new DateSchedule(startDate, endDate, paymentPerYear, shortPeriod, adjusted, arrears, fixingDays);
        Console.WriteLine("Marix<Date> GetLongScheduleDate()");
        myDL_1.PrintDateMatrix(myDL_1.GetLongScheduleDate());
        Console.WriteLine();
        Console.WriteLine("Matrix<double> GetLongScheduleSerial()");
        myDL_1.GetLongScheduleSerial().extendedPrint();
        Console.WriteLine();
        Console.WriteLine("Marix<Date> GetShortScheduleDate()");
        myDL_1.PrintDateMatrix(myDL_1.GetShortScheduleDate());
        Console.WriteLine();
        Console.WriteLine("Array<Date>PaymentDateArray()");
        myDL_1.PrintDateArray(myDL_1.PaymentDateArray());
        Console.WriteLine();
        Console.WriteLine("count numbers of rows: public int Length " + myDL_1.Length);
    }

    public static void Example5()
    {
        // 1. Create the date schedule data
        DateTime startDate = new Date(2009, 8, 3).DateValue;
        DateTime endDate = new Date(2014, 10, 3).DateValue;
        bool adjusted = true;
        int paymentPerYear = 2;
        bool arrears = false;
        int fixingDays = -2;
        bool shortPeriod = true;

        // 2. My date scheduled.
        DateSchedule myDL_1 = new DateSchedule(startDate, endDate,
        paymentPerYear, shortPeriod, adjusted, arrears, fixingDays);

        // 3. Init a NumericMatrix<double> Class from my dates.
        NumericMatrix<double> myDates = (NumericMatrix<double>)
        myDL_1.GetLongScheduleSerial();

        // 4. Create an associative matrix AssocMatrix with "header" label
        // for columns and "n_lines" for rows 4A. Label for columns.
        Set<string> header = new Set<string>();
        header.Insert("FixingDate");
        header.Insert("StartDate");
        header.Insert("EndDate");
        header.Insert("PaymentDate");
        // 4B. Label for rows
        Set<string> n_line = new Set<string>();
        for (int i = 0; i < myDates.MaxRowIndex + 1; i++)
        {
            n_line.Insert("# " + (i + 1));
        }
        // 5. Creating AssocMatrix.
        AssocMatrix<string, string, double> OutMatrix =
        new AssocMatrix<string, string, double>(n_line, header, myDates);

        // 6. Print associative matrices in Excel, to "My Date
        // List" sheet, the output in Excel serial number format.
        ExcelMechanisms exl = new ExcelMechanisms();
        exl.printAssocMatrixInExcel<string, string, double>
        (OutMatrix, "My Date List");
    }

    // More examples
    public static void Example6()
    {
        Date date1 = new Date(2006, 5, 15);
        Date date2 = new Date(date1);
        Console.WriteLine("Using CopyConstructor: 'Date date2 = new Date(date1)' ");
        Console.WriteLine("Date1: {0:D}", date1.DateValue);
        Console.WriteLine("Date2: {0:D}", date2.DateValue);
        date2.SerialValue = 38853;
        Console.WriteLine("After changing Date2, the value of Date1 is: {0:D}", date1.DateValue);
        Console.WriteLine("After changing Date2, the value of Date2 is: {0:D}", date2.DateValue);
        Console.WriteLine();
        Date date3 = date1;

        Console.WriteLine("Using =  'Date date3 = date1' ");
        Console.WriteLine("Date1: {0:D}", date1.DateValue);
        Console.WriteLine("Date3: {0:D}", date3.DateValue);
        date1.SerialValue = 38851;
        Console.WriteLine("After changing Date1, the value of Date1 is: {0:D}", date1.DateValue);
        Console.WriteLine("After changing Date1, the value of Date3 is: {0:D}", date3.DateValue);
        Console.WriteLine();
        Console.WriteLine("Using properties: ");
        Console.WriteLine("Date1 get value: {0:D}", date1.DateValue);
        Console.WriteLine("Date1 get serial number: {0}", date1.SerialValue);
        date1.DateValue = date2.DateValue;
        Console.WriteLine("After setting Date1.Value = Date2.Value, Date1.value is: {0:D}", date1.DateValue);
        date1.SerialValue = 38860;
        Console.WriteLine("After setting Date1.SerialValue = 38860, Date1.value is: {0:D}", date3.DateValue);
    }

    public static void Example7()
    {
        // Modified Following
        Date date1 = new Date(2009, 6, 13);
        Console.WriteLine("my date1 is :{0:D} ", date1.DateValue);
        Console.WriteLine("my date1.mod_foll() is : {0:D} ", date1.mod_foll().DateValue);

        Console.WriteLine();

        Date date2 = new Date(2009, 6, 18);
        Console.WriteLine("my date2 is :{0:D} ", date2.DateValue);
        Console.WriteLine("my date2.mod_foll() is : {0:D} ", date2.mod_foll().DateValue);
    }

    public static void Example8()
    {
        // DateSchedule. The output of is a matrix of dates, for scheduled payments period, each rows is a period
        // for columns:
        // 1° column fixing date
        // 2° column startingDate of period
        // 3° column endDate of period
        // 4° column payment date of period
        DateTime startDate = new Date(2009, 8, 3).DateValue;
        DateTime endDate = new Date(2014, 10, 3).DateValue;
        bool adjusted = true;
        int paymentPerYear = 2;
        bool arrears = false;
        int fixingDays = -2;
        bool firstShortPeriod = true;

        DateSchedule myDL_1 = new DateSchedule(startDate, endDate, paymentPerYear, firstShortPeriod, adjusted, arrears, fixingDays);
        Console.WriteLine("Starting DateSchedule");
        myDL_1.PrintDateMatrix();
        Console.WriteLine();

        adjusted = false;
        DateSchedule myDL_2 = new DateSchedule(startDate, endDate, paymentPerYear, firstShortPeriod, adjusted, arrears, fixingDays);
        Console.WriteLine("adjusted = 0, 3° column can be Sat or Sun");
        myDL_2.PrintDateMatrix();
        Console.WriteLine();

        paymentPerYear = 1;
        DateSchedule myDL_3 = new DateSchedule(startDate, endDate, paymentPerYear, firstShortPeriod, adjusted, arrears, fixingDays);
        Console.WriteLine("paymentPerYear = 1, frequency of payment changed");
        myDL_3.PrintDateMatrix();
        Console.WriteLine();

        arrears = true;
        DateSchedule myDL_4 = new DateSchedule(startDate, endDate, paymentPerYear, firstShortPeriod, adjusted, arrears, fixingDays);
        Console.WriteLine("arrears = 1, 1° column count fixing date starting from 3° column and not from 2° column");
        myDL_4.PrintDateMatrix();
        Console.WriteLine();

        fixingDays = 0;
        DateSchedule myDL_5 = new DateSchedule(startDate, endDate, paymentPerYear, firstShortPeriod, adjusted, arrears, fixingDays);
        Console.WriteLine("fixingDays = 0, 1° column has a different leg");
        myDL_5.PrintDateMatrix();
        Console.WriteLine();

        firstShortPeriod = false;
        DateSchedule myDL_6 = new DateSchedule(startDate, endDate, paymentPerYear, firstShortPeriod, adjusted, arrears, fixingDays);
        Console.WriteLine("shortPeriod = 2, changed the short period");
        myDL_6.PrintDateMatrix();
        Console.WriteLine();
    }

    public static void Example9()
    {
        // Testing different version of 30/360 and 30E/360 according ISDA definition 
        // http: // www.isda.org/c_and_a/docs/30-360-2006ISDADefs.xls
        // you can check data in sheet "Comparison"
        Date TerminationDate = new Date(2009, 2, 28);
        Array<Date> StartDates = new Array<Date>(23, 1);
        Array<Date> EndDates = new Array<Date>(23, 1);
        StartDates[1] = new Date(2007, 1, 15); EndDates[1] = new Date(2007, 1, 30);
        StartDates[2] = new Date(2007, 1, 15); EndDates[2] = new Date(2007, 2, 15);
        StartDates[3] = new Date(2007, 1, 15); EndDates[3] = new Date(2007, 7, 15);
        StartDates[4] = new Date(2007, 9, 30); EndDates[4] = new Date(2008, 3, 31);
        StartDates[5] = new Date(2007, 9, 30); EndDates[5] = new Date(2007, 10, 31);
        StartDates[6] = new Date(2007, 9, 30); EndDates[6] = new Date(2008, 9, 30);
        StartDates[7] = new Date(2007, 1, 15); EndDates[7] = new Date(2007, 1, 31);
        StartDates[8] = new Date(2007, 1, 31); EndDates[8] = new Date(2007, 2, 28);
        StartDates[9] = new Date(2007, 2, 28); EndDates[9] = new Date(2007, 3, 31);
        StartDates[10] = new Date(2006, 8, 31); EndDates[10] = new Date(2007, 2, 28);
        StartDates[11] = new Date(2007, 2, 28); EndDates[11] = new Date(2007, 8, 31);
        StartDates[12] = new Date(2007, 2, 14); EndDates[12] = new Date(2007, 2, 28);
        StartDates[13] = new Date(2007, 2, 26); EndDates[13] = new Date(2008, 2, 29);
        StartDates[14] = new Date(2008, 2, 29); EndDates[14] = new Date(2009, 2, 28);
        StartDates[15] = new Date(2008, 2, 29); EndDates[15] = new Date(2008, 3, 30);
        StartDates[16] = new Date(2008, 2, 29); EndDates[16] = new Date(2008, 3, 31);
        StartDates[17] = new Date(2007, 2, 28); EndDates[17] = new Date(2007, 3, 5);
        StartDates[18] = new Date(2007, 10, 31); EndDates[18] = new Date(2007, 11, 28);
        StartDates[19] = new Date(2007, 8, 31); EndDates[19] = new Date(2008, 2, 29);
        StartDates[20] = new Date(2008, 2, 29); EndDates[20] = new Date(2008, 8, 31);
        StartDates[21] = new Date(2008, 8, 31); EndDates[21] = new Date(2009, 2, 28);
        StartDates[22] = new Date(2009, 2, 28); EndDates[22] = new Date(2009, 8, 31);

        Console.WriteLine("Number of days:");
        for (int i = 1; i < 23; i++)
        {
            Console.WriteLine("{0:d},\t{1:d},\t{2},\t{3},\t{4},\t{5}", StartDates[i].DateValue, EndDates[i].DateValue,
                 StartDates[i].D_30_360(EndDates[i]), StartDates[i].D_30_360E(EndDates[i]), StartDates[i].D_30_360E_ISDA(EndDates[i], TerminationDate),
            StartDates[i].D_EFF(EndDates[i]));
        }

        Console.WriteLine("\n{0}", "Year Fraction:");
        for (int i = 1; i < 23; i++)
        {
            Console.WriteLine("{0:d},\t{1:d},\t{2:F4},\t{3:F4},\t{4:F4},\t{5:F4}", StartDates[i].DateValue, EndDates[i].DateValue,
                 StartDates[i].YF_30_360(EndDates[i]), StartDates[i].YF_30_360E(EndDates[i]), StartDates[i].YF_30_360E_ISDA(EndDates[i], TerminationDate),
            StartDates[i].YF_MM(EndDates[i]));
        }
    }

    public static void Example10()
    {
        // I will pay 5% quarterly 30/360 for 5y starting from 5 Oct 09 on a nominal of 1.000.000 USD. I want to see
        // interest rates cash flows
        // input data
        Date startDate = new Date(2009, 10, 5);
        Date endDate = startDate.AddYears(5);
        int paymentPerYear = 4; // Quarterly
        bool firstShortPeriod = true;
        double rate = 0.05;

        // I use standard constructor
        DateSchedule myDS = new DateSchedule(startDate, endDate, paymentPerYear, firstShortPeriod);

        // I get start and end date schedule
        NumericMatrix<Date> myShortSchedule = myDS.GetShortScheduleDate();

        // my nominals
        Array<double> nominals = new Array<double>(myShortSchedule.Rows, 0, 1000000);

        // year fraction from/to according 30/360
        Array<double> yearFractions = new Array<double>(myShortSchedule.Rows, 0);

        // days from/to according 30/360
        Array<double> days = new Array<double>(myShortSchedule.Rows, 0);

        // interest to pay
        Array<double> interest = new Array<double>(myShortSchedule.Rows, 0);

        Console.WriteLine("{0},\t{1},\t{2},\t{3},\t{4}", "days", "yearFrac", "rate",
           "nominals", "interest");

        // running sum
        double totalInterest = 0.0;

        // iterate interest calculation using schedule
        for (int i = 0; i < myShortSchedule.Rows; i++)
        {
            yearFractions[i] = myShortSchedule[i, 0].YF_30_360(myShortSchedule[i, 1]);
            days[i] = myShortSchedule[i, 0].D_30_360(myShortSchedule[i, 1]);
            interest[i] = yearFractions[i] * rate * nominals[i];

            Console.WriteLine("{0},\t{1:F5},\t{2:P},\t{3:C},\t{4:C}", days[i], yearFractions[i], rate,
           nominals[i], interest[i]);
            totalInterest += interest[i];
        }

        Console.WriteLine("{0},\t{1:F2}", "Total interests: ", totalInterest);
    }

    public static void Example11()
    {
        // Date class: reference to same object vs copy constructor

        // reference to object, then I create a new obj 
        Date d1 = new Date(2009, 11, 12);
        Date d2 = d1;  // Reference to same object is made           
        Console.WriteLine("{0},{1:d}", "Before change", d2.DateValue);
        d1 = d1.AddMonths(3); // a new object is created, lost the ref
        // AddMonths returns a new date object with the result that is assigned to d1.
        // AddMonths does not change the current object!
        // So d1 is not pointing to the same object as d2 anymore
        Console.WriteLine("{0},{1:d}", "After change", d2.DateValue);
        Console.WriteLine();

        // reference to object
        Date d3 = new Date(2010, 11, 12);
        Date d4 = d3;  // Reference to same object is made
        Console.WriteLine("{0},{1:d}", "Before change", d3.DateValue);
        d3.DateValue = new DateTime(2010, 11, 13); // changing d3
        Console.WriteLine("{0},{1:d}", "After change", d3.DateValue);
        Console.WriteLine();

        // using cope constructor (like struc)
        Date d5 = new Date(2011, 11, 12);
        Date d6 = new Date(d5);  // Copy is made
        Console.WriteLine("{0},{1:d}", "Before change", d6.DateValue);
        d5.DateValue = new DateTime(2011, 11, 13);
        Console.WriteLine("{0},{1:d}", "After change", d6.DateValue);
    }

    public static void Example12()
    {
        // What is excel serial?
        Date d1 = new Date(2009, 10, 11);

        // DateTime.FromOADate(d);
        Console.WriteLine("{0}\t{1:D}", "My Date d1:", d1.DateValue);

        // serial value
        Console.WriteLine("{0}\t{1:F0}", "d1.SerialValue:", d1.SerialValue);

        // Using DateTime.FromOADate(double myserial) will return the same day! 
        Console.WriteLine("{0}\t{1:D}", "DateTime.FromOADate(d1.SerialValue):", DateTime.FromOADate(d1.SerialValue));

        // Using serial value as constructor will return the same day!
        Console.WriteLine("{0}\t{1:D}", "new Date(d1.SerialValue).DateValue:", new Date(d1.SerialValue).DateValue);
    }
}

