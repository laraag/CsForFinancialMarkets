// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// TestLinq.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class TestLinq
{
    public static void Main()
    {
        //DeferredExecution();
        //DeferredExecution2();
        //Combination();
        //DateArray();
        //PresentValue();
        //Scenario();
        //TestFuncDelegate();
        //TestGenericActionDelegate();
        //CashFlowsAggregator();
        //OrderBy();
        //OrderBy2();
        //FwdEonia();    
    }

    public static void DeferredExecution()
    {
        // show example of deferred execution
        // 1) I initialise a collection of rate(defined as KeyValuePair)
        // 2) I perform a query (using and without using ToList()) 
        // 3) I add an element to my collection and note deferred execution 

        // 1) my collection of rate
        var rate = new List<KeyValuePair<string, double>>
        {
          new KeyValuePair<string,double> ("1y",0.012),
          new KeyValuePair<string,double> ("2y",0.025),
          new KeyValuePair<string,double> ("3y",0.03)
        };

        // 2) perform a query: rates > 0.015
        var v = from c in rate
                where c.Value > 0.015
                select c;
        var v2 = (from c in rate
                  where c.Value > 0.015
                  select c).ToList();

        // print results
        v.Print("Rates > 0.015 before adding a rate");
        v2.Print("\nRates > 0.015 before adding a rate, using ToList()");

        // 3) add an element to my collection
        rate.Add(new KeyValuePair<string, double>("5y", 0.04));
        // print results
        v.Print("\nRates > 0.015 after adding a rate");
        v2.Print("\nRates > 0.015 after adding a rate, using ToList()");
    }

    public static void DeferredExecution2()
    {
        Console.WriteLine("\n*** Deferred Execution 2 ***");
        // Create sentence in IEnumerable<> so it can be used by LINQ.
        IEnumerable<char> str =
        "The quick brown fox jumps over the lazy dog.";
        // Build query to remove a, e, i, o and u.
        // Query not yet executed.
        var query = str.Where(c => c != 'a');
        query = query.Where(c => c != 'e');
        query = query.Where(c => c != 'i');
        query = query.Where(c => c != 'o');
        query = query.Where(c => c != 'u');
        // Query now executed.
        foreach (char c in query) Console.Write(c);
        Console.WriteLine();
        query = str;
        // Letter in query is an outer variable.
        foreach (char letter in "aeiou")
            query = query.Where(c => c != letter);
        // Query now executed. This will only remove 'u' because
        // that is the value of the outer variable at time of execution.
        foreach (char c in query) Console.Write(c);
        Console.WriteLine();
        // Building query in loop (corrected version). Outer variable
        // is different in each iteration.
        query = str;
        foreach (char letter in "aeiou")
        { char tmp = letter; query = query.Where(c => c != tmp); }
        // Query now executed. Correct output.
        foreach (char c in query) Console.Write(c);
        Console.WriteLine();
    }

    // Build a matrix for label of forward start instruments
    public static void Combination()
    {
        string[] StartIn = new string[] { "1m", "2m", "3m" };
        string[] Tenor = new string[] { "2y", "5y", "10y" };

        string[] matrix = (from s in StartIn
                           from t in Tenor
                           select s + "X" + t).ToArray();
        foreach (string s in matrix) { Console.WriteLine(s); }
    }

    public static void DateArray()
    {
        /* 1) Create an array of equi-spaced dates. Starting from "refDate", I add one month
         * 2) I define a function "CalcDF" to calculate discount factor using continuous compounding
         * 3) Using "CalcDF" I calculate DF for all my dates
         * 4) I select only quaterly discount factor (subset)
         */

        DateTime refDate = new DateTime(2011, 7, 17); // My ref date
        double r = 0.02;  // Rate used in "CalcDF", continuous rate
        Func<double, double> CalcDF = nDays => Math.Exp(-r * nDays / 365.0);  // Define my discount function: standard continuous compounding

        var Dates = Enumerable.Range(0, 13).Select(nMonths => refDate.AddMonths(nMonths)); // Collection of my month spaced dates
        Dates.Print("My Dates: ");  // Printing my dates

        // My collection of Dates-DiscountFactor
        var DateDf = from d in Dates
                     select new { date = d, df = CalcDF(d.ToOADate() - refDate.ToOADate()) };
        DateDf.Print("All Dates and DF ");

        // Collection in collections: I get only quoterly discount factor
        var QuarterlyDateDF = from d in DateDf
                              where d.date.Month == 1 || d.date.Month == 3 || d.date.Month == 6 || d.date.Month == 12
                              select new { date = d.date, df = d.df };
        QuarterlyDateDF.Print("Quaterly Dates and DF "); // print query

        // Coing the same with more syntetic syntax
        var QuarterlyDateDF2 = DateDf.Where(d => d.date.Month == 1 || d.date.Month == 3 || d.date.Month == 6 || d.date.Month == 12);
        QuarterlyDateDF.Print("Quaterly Dates and DF "); // print query
    }

    public static void PresentValue()
    {
        /*Using anonymous type
         * 1) Create a Bond defined as a collection of Time,CashFlows;
         * 2) Define a function to calculate discount factors;
         * 3) Calculated discounted cash flows (i.e present value of each cash flows);
         * 4) Calculate the sum of discounted cash flows;
         */

        // My bond defined as collection of pair (time, cash flows)
        var Bond1 = new[]
        {
                new { Time = 1.0 , CashFlow = 5.0 },
                new { Time = 1.5 , CashFlow = 5.0 },
                new { Time = 2.0 , CashFlow = 105.0 }
        };
        Bond1.Print("Bond Cash Flows:");  // Print bond cash flows

        // Define my discount (lambda) function: standard continuous compounding
        Func<double, double, double> CalcDF = (t, r) => Math.Exp(-r * t);

        // Continuous rate, 5% as example
        double rate = 0.05;

        // Present value of each cash flow
        var PvCashFlows = from p in Bond1
                          select new { Time = p.Time, CashFlow = p.CashFlow * CalcDF(p.Time, rate) };
        PvCashFlows.Print("\nPresent Value of each cash flow: ");

        Console.WriteLine("\nSum of PV of cash flows = {0}", PvCashFlows.Sum(p => p.CashFlow));
    }

    public static void Scenario()
    {
        /*
         * 1) Define some scenarios. We consider as a scenario simply a pair of label/rate
         * 2) Create a Bond defined as a collection of Time,CashFlows;
         * 3) Define funtion to calculate bond price, given a continuous rate r
         * 4) Calculate bond prices in different scenarios
         * 5) Calculate the differences in bond prices in different scenarios, with respect to "flat" scenario
         */

        double rate = 0.05;  // My starting rate

        // 1) Defining scenarios
        var Scenario = new[]
        {
            new {Name = "Flat", RateValue = rate},
            new {Name = "+1bp", RateValue = rate+0.0001},
            new {Name = "+100bp", RateValue = rate+0.01},
            new {Name = "-1bp", RateValue = rate-0.0001},
            new {Name = "-100bp", RateValue = rate-0.01},
        };
        Scenario.Print("Scenarios");

        // 2) My bond defined as collection of pair (time, cash flows)
        var Bond = new[]
        {
                new { Time = 1.0 , CashFlow = 2.0 },
                new { Time = 2.0 , CashFlow = 2.0 },
                new { Time = 3.0 , CashFlow = 102.0 }
        };

        // 3) Define my discount function: standard continuous compounding
        Func<double, double, double> CalcDF = (t, r) => Math.Exp(-r * t);

        // Define my bond pricer calculator: it calculate the sum of present values of cash flows according to "CalcDf"
        Func<double, double> CalcBondPx = r => (from p in Bond
                                                select p.CashFlow * CalcDF(p.Time, r)).Sum();

        // 4) Calculate bond prices in different scenarios
        var BondValues = from s in Scenario
                         select new { Name = s.Name, BondPx = CalcBondPx(s.RateValue) };

        BondValues.Print("\nBond Prices in different scenarios");

        // 5) Calculate the differences in bond prices in different scenarios, with respect to "flat" scenario
        var PricesDelta = from s in BondValues
                          select new
                          {
                              Name = s.Name,
                              Delta = s.BondPx - BondValues.Single(p => p.Name == "Flat").BondPx
                          };
        PricesDelta.Print("\nDeltas in bond prices with respect 'Flat' scenario");
    }

    public static void TestFuncDelegate()
    {
        Func<double, double, double, double>
        Distance = (x, y, z) => Math.Sqrt(x * x + y * y + z * z);

        double myX = 3.0;
        double myY = 4.0;
        double myZ = 0.0;

        Console.WriteLine("Distance: {0}", Distance(myX, myY, myZ));  // 5! 
    }

    public static void TestGenericActionDelegate()
    {
        Func<double, double, double, double>
          Distance = (x, y, z) => Math.Sqrt(x * x + y * y + z * z);

        double myX = 3.0;
        double myY = 4.0;
        double myZ = 0.0;

        Action<double, double, double> Display = (x, y, z) =>
            Console.Write("{0}", Distance(myX, myY, myZ));

        myX = 1.0;
        myY = 1.0;
        myZ = 1.0;

        Display(myX, myY, myZ);
    }

    public static void CashFlowsAggregator()
    {
        /*
         * The goal is to aggregate cash flows of different bonds
         * grouping for time when they occurs
         * 1) Inizialize collection of bond (a bond is defined as a collection of Time,CashFlows);
         * 2) using LINQ syntax I aggregate cash flows per time, and ordered them by time;
         */

        // Collection of bonds
        var Bonds = new[] 
        {
            new[]  // First Bond
            {
                new { Time = 1.0 , CashFlow = 1.0 },
                new { Time = 2.0 , CashFlow = 1.0 },
                new { Time = 3.0 , CashFlow = 101.0 }
            },
            new[]
            {  // Second Bond
                    new { Time = 1.0 , CashFlow = 2.0 },
                    new { Time = 1.5 , CashFlow = 2.0 },
                    new { Time = 3.0 , CashFlow = 102.0 }
            }
             // Add more bonds..
        };

        // Aggregated Cash Flows
        var OutPut = from SingleBond in Bonds
                     from TimeCashFlows in SingleBond
                     orderby TimeCashFlows.Time
                     group TimeCashFlows by TimeCashFlows.Time into grp
                     select new { Time = grp.Key, CashFlows = grp.Sum(t => t.CashFlow) };
        // Print the output
        OutPut.Print("Aggregated Cash Flows:");
    }

    public static void OrderBy()
    {
        // Show an example of sorting a collection.
        // We use a set of rates. Each rate is defined as TenorInYear, RateValue and RateType

        // Collection of rates
        var RatesSet = new[]
            {
                new { TenorInYear = 1, RateValue = 0.02, RateType = "SwapVs6M"},
                new { TenorInYear = 4, RateValue = 0.03, RateType = "SwapVs3M"},
                new { TenorInYear = 3, RateValue = 0.01, RateType = "SwapVs6M"},
                new { TenorInYear = 2, RateValue = 0.05, RateType = "SwapVs3M"}
        };

        // Order by TenorInYears: 
        var query1 = from r in RatesSet
                     orderby r.TenorInYear
                     select r;
        query1.Print("Order by TenorInYears: ");  // Print results

        // Order by RateType and then for TenorInYear: 
        var query2 = from r in RatesSet
                     orderby r.RateType, r.TenorInYear
                     select r;
        query2.Print("\nOrder by RateType and then for TenorInYear: ");  // Print results
    }

    public static void OrderBy2()
    {
        // Show an example of sorting a collection.
        // We use a set of rates. Each rate is deffined as a pair of Tenor and value
        // 1) We initialise our collection
        // 2) We create a function "nMonth", that given a string like "2y" return number of months (for example 2y -> 24, 3m->3..)
        // 3) We use LINQ syntax to find the end date of each rate, and the type of rate (if maturity is >12m it is Swap, if < 12m it is a depo)

        // 1) We initialise our collection
        var RatesSet = new[]
        {
            new {Tenor = "1m", Value = 0.01},
            new {Tenor = "2m", Value = 0.0125},
            new {Tenor = "6m", Value = 0.0135},
            new {Tenor = "1y", Value = 0.0199},
            new {Tenor = "2y", Value = 0.0215},
            new {Tenor = "9m", Value = 0.0140},
            new {Tenor = "10y", Value = 0.05}            
        };

        DateTime refDate = new DateTime(2011, 7, 20);  // Reference date for our calcolus

        // 2) We create a function "nMonth", that given a string like "2y" return number of months (for example 2y -> 24, 3m->3..)
        Func<string, int> nMonth = x =>
        {
            if (x[x.Length - 1].ToString() == "y") { return 12 * Convert.ToInt32(x.Replace("y", "")); }
            else if (x[x.Length - 1].ToString() == "m") { return 1 * Convert.ToInt32(x.Replace("m", "")); }
            else return 0;
        };

        // 3) we use LINQ syntax to find the end date of each rate, and the type of rate (if maturity is >12m it is Swap, if < 12m it is a depo)
        var query = from r in RatesSet
                    orderby refDate.AddMonths(nMonth(r.Tenor)) ascending
                    select new
                    {
                        EndDate = refDate.AddMonths(nMonth(r.Tenor)),
                        RateVAlue = r.Value,
                        RateType = (nMonth(r.Tenor) >= 12 ? "Swap" : "Depo")
                    };

        query.Print("Rates Set: ");  // Print output
    }

    public static void FwdEonia()
    {
        /* We try to replicate the calculation of forward eonia rate, using data  
         * page 12 www.aciforex.org/docs/misc/20091112_EoniaSwapIndexbrochure.pdf
         * 1) Inizialize starting input
         * 2) query preparing data to be used in fwd calculation
         * 3) "CalcFwd" function for standard forward rate
         * 4) Calculate fwd eonia swap and print results
         */

        Date refDate = new Date(2008, 4, 2);  // Ref date

        // 1) Inizialize starting input. These are not real data
        List<KeyValuePair<string, double>> MonthRate = new List<KeyValuePair<string, double>>()
        {
            new KeyValuePair<string,double>("1m",3.992e-2),
            new KeyValuePair<string,double>("2m",3.995e-2),
            new KeyValuePair<string,double>("3m",3.994e-2),
            new KeyValuePair<string,double>("4m",3.992e-2),
            new KeyValuePair<string,double>("5m",3.991e-2),
            new KeyValuePair<string,double>("6m",3.976e-2),
            new KeyValuePair<string,double>("7m",3.95e-2),
            new KeyValuePair<string,double>("8m",3.927e-2),
            new KeyValuePair<string,double>("9m",3.901e-2),
            new KeyValuePair<string,double>("10m",3.877e-2),
            new KeyValuePair<string,double>("11m",3.857e-2),
            new KeyValuePair<string,double>("12m",3.838e-2)
        };
        MonthRate.Print("Starting Input");  // Print input
        Console.WriteLine();

        // 2) Query preparing data to be used in fwd calculation
        var Data = from kv in MonthRate
                   let ed = refDate.add_period(kv.Key, true)
                   select new
                   {
                       StartDate = refDate,
                       EndDate = ed,
                       Months = kv.Key,
                       Days = ed.SerialValue - refDate.SerialValue,
                       Fixing = kv.Value
                   };
        // Print data
        Console.WriteLine("Complete starting data");
        foreach (var d in Data)
        {
            Console.WriteLine("{0:d}  {1:d}  {2}  {3}  {4:p3}",
                d.StartDate.DateValue, d.EndDate.DateValue, d.Months, d.Days, d.Fixing);
        }
        Console.WriteLine();

        var ToBeCalculated = new[]
        {
            new { StartIn = "1m", EndIn ="2m"},
            new { StartIn = "2m", EndIn ="3m"},
            new { StartIn = "3m", EndIn ="4m"},
            new { StartIn = "4m", EndIn ="5m"},
            new { StartIn = "5m", EndIn ="6m"},
            new { StartIn = "6m", EndIn ="7m"},
            new { StartIn = "7m", EndIn ="8m"},
            new { StartIn = "8m", EndIn ="9m"},
            new { StartIn = "9m", EndIn ="10m"},
            new { StartIn = "10m", EndIn ="11m"},
            new { StartIn = "11m", EndIn ="12m"},
        };

        // 3) "CalcFwd" function for standard forward rate (http: // en.wikipedia.org/wiki/Forward_rate)
        Func<double, double, double, double, double> CalcFwd = (r1, d1, r2, d2) =>
            (((1 + r2 * d2 / 360.0) / (1 + r1 * d1 / 360.0)) - 1) * (360.0 / (d2 - d1));

        // 4) Calculate fwd eonia swap and print results
        var query = from r in ToBeCalculated
                    let s = Data.Single(x => x.Months == r.StartIn)
                    let e = Data.Single(x => x.Months == r.EndIn)
                    select new
                    {
                        Label = r.StartIn + "x" + r.EndIn,
                        StartDate = s.EndDate,
                        EndDate = e.EndDate,
                        Days = e.Days - s.Days,
                        Fwd = CalcFwd(s.Fixing, s.Days, e.Fixing, e.Days)
                    };

        // Print results
        Console.WriteLine("Complete calculated data");
        foreach (var d in query)
        {
            Console.WriteLine("{0}  {1:d}  {2:d}  {3}  {4:p3}",
                    d.Label, d.StartDate.DateValue, d.EndDate.DateValue, d.Days, d.Fwd);
        }
    }

    // Extension method to print a collection.    
    public static void Print<T>(this IEnumerable<T> collection, string msg)
    {
        Console.Write(msg);
        foreach (T value in collection) Console.Write("\n{0}, ", value);
        Console.WriteLine();
    }
}

