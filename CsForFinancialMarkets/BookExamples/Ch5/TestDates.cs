// TestDates.cs
//
// 101 example 
//
// (C) Datasim Education BV 2009
//

using System;


public class TestDate
{
    public static void Main()
    {
        // Create some dates
        Console.WriteLine("Basic constructors");
        DateTime dt1 = new DateTime();              // 1/1/1
        DateTime dt2 = DateTime.Now;                // Today             
        DateTime dt3 = new DateTime(1952, 8, 29);   // YYYYY, MM, DD

        print(dt1);
        print(dt2);
        print(dt3);

        // Adding some units to dates
        Console.WriteLine("Some + offsets");
        dt1 = dt1.AddYears(1);
        dt2 = dt2.AddMonths(3);
        dt3 = dt3.AddDays(7);

        print(dt1);
        print(dt2);
        print(dt3);

        // Reset all offsets
        Console.WriteLine("Some - offsets, subtract");
        dt1 = dt1.AddYears(-1);
        dt2 = dt2.AddMonths(-3);
        dt3 = dt3.AddDays(-7);

        print(dt1);
        print(dt2);
        print(dt3);

        // Comparing distances between dates (in ticks)
        Console.WriteLine("Using Timespan in ticks");
        TimeSpan t1 = dt2 - dt1;
        TimeSpan t2 = dt2 - dt3;
        Console.WriteLine(t1);
        Console.WriteLine(t2);

        // Convert t2 from ticks to more meaningful units
        Console.WriteLine("Using Timespan in other units");
        Console.WriteLine(t2.TotalDays);
        Console.WriteLine(t2.TotalHours);
        Console.WriteLine(t2.TotalMilliseconds);


        // Formatting and parsing of dates
        Console.WriteLine("Formatting and parsing of dates");
        DateTime dt4 = new DateTime(1952, 8, 29);               // YYYYY, MM, DD, HH, Min, Sec
        DateTime dt5 = new DateTime(1952, 8, 29, 20, 40, 10);   // YYYYY, MM, DD, HH, Min, Sec
        Console.WriteLine(dt4.ToString());
        Console.WriteLine(dt5.ToString());


        // Using format strings
        CultureSensitivePrint(dt5);
        CultureInSensitivePrint(dt5);

        // Creating arrays of dates
        DateTime startDate = DateTime.Now;  
        int NYears = 10;

        DateTime[] CashFlowDates = new DateTime[NYears + 1];
        CashFlowDates[0] = DateTime.Now;
        DateTime tmp = DateTime.Now;

        for (int n = 0; n <= NYears; ++n)
        {
            CashFlowDates[n] = tmp;
            tmp = tmp.AddYears(1);
        }

        for (int n = 0; n <= NYears; ++n)
        {
            Console.WriteLine(CashFlowDates[n].ToString(), "F");
        }


        // Parsing a date
        string s = DateTime.Now.ToString("U");  // UTC

        // Option 1, strict compliance
        DateTime aDate = DateTime.ParseExact(s, "U", null);
        print(aDate);

        // Option 2, 
        DateTime aDate2 = DateTime.Parse(s);
        print(aDate2);


    }

    static void print(DateTime dt)
    {

        Console.WriteLine("{0},{1},{2}", dt.Year, dt.Month, dt.Day);
    }

    static void CultureSensitivePrint(DateTime dt5)
    {
        // Using format strings
        Console.WriteLine("Culture sensitive formatting");
        Console.WriteLine(dt5.ToString("d"));   // Short date
        Console.WriteLine(dt5.ToString("D"));   // Long date

        Console.WriteLine(dt5.ToString("t"));   // Short time
        Console.WriteLine(dt5.ToString("T"));   // Long time

        Console.WriteLine(dt5.ToString("f"));   // Long date + Short time
        Console.WriteLine(dt5.ToString("F"));   // Long date + Long time

        Console.WriteLine(dt5.ToString("g"));   // Short date + Short time
        Console.WriteLine(dt5.ToString("G"));   // Short date + Long time
    }

    static void CultureInSensitivePrint(DateTime dt5)
    {
        // Using format strings
        Console.WriteLine("Culture *in*sensitive formatting");
        Console.WriteLine(dt5.ToString("o"));   // round-trippable
      
        Console.WriteLine(dt5.ToString("r"));   // RFC 1123 standard
        Console.WriteLine(dt5.ToString("R"));   // ditto

        Console.WriteLine(dt5.ToString("s"));   // Sortable, ISO 8601
        // Console.WriteLine(dt5.ToString("S"));   // GIVES ERROR, UNKNOWN CODE

        Console.WriteLine(dt5.ToString("u"));   // "Universal" Sortable
        Console.WriteLine(dt5.ToString("U"));   // UTC (Coordinated Universal Time) ~ GMT
    }

}
