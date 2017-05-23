// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// Ch14_Examples.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
// Based on http: // www.euronext.com/editorial/wide/editorial-4304-EN.html 106420.xls

public class TestListedST
{
    public static void Main() 
    {
         Example1();
        // Example2();
        // Example3();
        // Example4();
        // Example5();
        // Example6();
        // Example7();
    }

    // Class STFut: Property and Method
    public static void Example1()
    {
        // my Future
        STFut myFut = new STFut(99.00, 12, 2013);

        // Method
        Console.WriteLine("fwdDF(360): {0}", myFut.fwDFq(360));
        Console.WriteLine("IMMDate: {0}", myFut.GetIMMDate);
        Console.WriteLine("Term: {0}", myFut.Term());
        Console.WriteLine("Price: {0}", myFut.Price());

        // Property
        Console.WriteLine("NotionalRepayDate: {0:D}", myFut.GetNotionalRepayDate.DateValue);
        Console.WriteLine();
        // Get/Set
        Console.WriteLine("Price: {0}, Rate: {1:P2}", myFut.GetSetPrice, myFut.GetSetRate);
        // Changing price        
        double newPrice = 99.50;
        myFut.GetSetPrice = newPrice;
        Console.WriteLine("New price {0}", newPrice);
        Console.WriteLine("Price: {0}, Rate: {1:P2}", myFut.GetSetPrice, myFut.GetSetRate);
        Console.WriteLine();
        // Changing rate
        double newRate = 0.015;
        myFut.GetSetRate = newRate;
        Console.WriteLine("New Rate {0:P2}", newRate);
        Console.WriteLine("Price: {0}, Rate: {1:P2}", myFut.GetSetPrice, myFut.GetSetRate);
    }

    // Class STFutOption
    public static void Example2()
    {
        // Option data for opt1 and opt2
        double CoP = -1;
        double underlying = 99.23;
        double strike = 99.25;
        double daysToExpiry = 82;
        double yieldVol = 0.449;  // (_priceVol * _underlying) / (100 - _underlying);

        // option data only for
        double daysPerYear = 360;
        STFutOption myOpt1 = new STFutOption(CoP, underlying, strike, daysToExpiry, yieldVol);
        STFutOption myOpt2 = new STFutOption(CoP, underlying, strike, daysToExpiry, yieldVol, daysPerYear);

        // Showing Data
        Console.WriteLine("CoP = {0}; underlying = {1}; strike = {2}; daysToExpiry = {3}; yieldVol = {4}",
            CoP, underlying, strike, daysToExpiry, yieldVol);

        Console.WriteLine("daysPerYear Opt1 = {0} daysPerYear Opt2 = {1}", 365, daysPerYear);
        Console.WriteLine("CoP\t opt1: {0}\t opt2: {1}", myOpt1.COP, myOpt2.COP);
        Console.WriteLine("Price\t opt1: {0:F4}\t opt2: {1:F4}", myOpt1.Price() / 100, myOpt2.Price() / 100);
        Console.WriteLine("Delta\t opt1: {0:F4}\t opt2: {1:F4}", myOpt1.Delta(), myOpt2.Delta());
        Console.WriteLine("Gamma\t opt1: {0:F4}\t opt2: {1:F4}", myOpt1.Gamma(), myOpt2.Gamma());
        Console.WriteLine("Theta\t opt1: {0:F4}\t opt2: {1:F4}", myOpt1.Theta(), myOpt2.Theta());
        Console.WriteLine("Vega \t opt1: {0:F4}\t opt2: {1:F4}", myOpt1.Vega(), myOpt2.Vega());

        // Changing data:now opt2 become a call, underlying and expiry change
        myOpt2.SwitchCallPut(); // now is a call
        underlying = 99.05; // new price
        daysToExpiry--; // a day pass

        // updating options
        myOpt1.new_DaysToExpiry = daysToExpiry;
        myOpt2.new_DaysToExpiry = daysToExpiry;
        myOpt1.new_S = underlying;
        myOpt2.new_S = underlying;

        // showing new data
        Console.WriteLine("underlying = {0}; daysToExpiry = {1}", underlying, daysToExpiry);
        Console.WriteLine("CoP\t opt1: {0}\t opt2: {1}", myOpt1.COP, myOpt2.COP);
        Console.WriteLine("Price\t opt1: {0:F4}\t opt2: {1:F4}", myOpt1.Price() / 100, myOpt2.Price() / 100);
        Console.WriteLine("Delta\t opt1: {0:F4}\t opt2: {1:F4}", myOpt1.Delta(), myOpt2.Delta());
        Console.WriteLine("Gamma\t opt1: {0:F4}\t opt2: {1:F4}", myOpt1.Gamma(), myOpt2.Gamma());
        Console.WriteLine("Theta\t opt1: {0:F4}\t opt2: {1:F4}", myOpt1.Theta(), myOpt2.Theta());
        Console.WriteLine("Vega \t opt1: {0:F4}\t opt2: {1:F4}", myOpt1.Vega(), myOpt2.Vega());
    }

    // Class Date: generating IMM Dates of Future Strip Type Mar, Jun, Sep, Dec
    public static void Example3()
    {
        Date today = new Date(2011, 6, 16);   // my today        
        int minIndex = 0;                    // min index of myDateVect
        int nOfDates = 6;                   // # element of myDateVect

        Vector<Date> myDateVect = new Vector<Date>(nOfDates, minIndex); // my ref dates vector

        string myDateFormat = "ddd dd MMM yyyy";  // Data format for output

        // Starting from "today", we create a Vector of reference dates adding 3 months 
        // Foreach reference date, we print the Third Wednesday. IMMDate()gets the third Wednesday 
        // of the month given a Date. Since today is Dec, we will have IMM Date of futures strip of type Mar,Jun,Sep,Dec.
        Console.WriteLine("Reference Date          Third Wednesday of Ref Date");
        for (int i = minIndex; i < nOfDates; i++)
        {
            myDateVect[i] = today.AddMonths(3 * i);
            Console.WriteLine("{0}  \t{1}", myDateVect[i].DateValue.ToString(myDateFormat),
                myDateVect[i].IMMDate().DateValue.ToString(myDateFormat));
        }

        Console.WriteLine("\n{0} {1}", "IMM Dates for futures strip [Mar,Jun,Sep,Dec]. Today is",
            today.DateValue.ToString(myDateFormat));
        // we show the IMM Date of futures strip of type Mar,Jun,Sep,Dec
        for (int i = minIndex + 1; i < nOfDates; i++)
        {
            Console.WriteLine("#{0} stir \t{1}", i, today.IMM_Date_Nth(i).DateValue.ToString(myDateFormat));
        }
    }

    // Class ListedSTFutOption and AssocMatrix
    public static void Example4()
    {
        // Stir (Short Term Interest Rate ) Option example. 
        // (Please check details on www.euronext.com) Example is based on Black Formula

        // I create my option
        STFutOption opx1 = new STFutOption(1, 97.935, 98.25, 175, 0.4756);

        // option deltas
        Console.WriteLine("{0}, {1}", opx1.Price(), opx1.Delta());
        opx1.SwitchCallPut();
        Console.WriteLine("{0}, {1}", opx1.Price(), opx1.Delta());

        // Call and put checking deltas and call put parity
        double S = 97.935;
        double K = 98.25;
        double DaysToExpiry = 175;
        STFutOption Call = new STFutOption(1, S, K, 175, 0.4756);
        STFutOption Put = new STFutOption(-1, S, K, 175, 0.4756);

        Console.WriteLine("{0},{1},{2}", Call.Delta(), Put.Delta(), Call.Delta() - Put.Delta()); // Call/Put delta are reversed
        Console.WriteLine("{0}", Call.Price() - Put.Price() - S + K);  // Call/Put parity

        // I will crate a matrix showing deltas of a option, for different underlying price (rows) and passing the time (columns)
        // header Row: different value of underlying. the center is the current one
        // header column: passing the time, less days to maturity

        // Time passing - column
        int d_columns = 20;  // how many days from and including today           
        double shift_c = 1.0; // interval in days between each columns

        // Changing value of underlying
        int d_rows = 15;  // how many values plus or minus the value 
        double shift_h = 0.10; // interval in underlying

        NumericMatrix<double> deltaMatrix = new NumericMatrix<double>(d_rows * 2 + 1, d_columns);

        // Underlying value 
        double d_r = -d_rows;
        Set<double> underlying = new Set<double>();
        for (int i = 0; i < deltaMatrix.Rows; i++)
        {
            underlying.Insert(S + d_r * shift_h);
            d_r++;
        }

        // Days to maturity
        double d_c = 0;
        Set<double> days = new Set<double>();
        for (int i = 0; i < deltaMatrix.Columns; i++)
        {
            days.Insert(DaysToExpiry + d_c * shift_c);
            d_c--;
        }

        // Populate DeltaMatrix
        int my_r = 1;
        int my_c = 1;
        foreach (double valueS in underlying)
        {
            Call.new_S = valueS;

            foreach (double valueDays in days)
            {
                Call.new_DaysToExpiry = valueDays;
                deltaMatrix[my_r, my_c] = Call.Delta();
                my_c++;
            }

            my_r++;
            my_c = 1;
        }

        // Creating AssocMatrix            
        AssocMatrix<double, double, double> OutMatrix = new AssocMatrix<double, double, double>(underlying, days, deltaMatrix);

        // Print associative matrices in Excel, to "StirDeltas" sheet
        ExcelMechanisms exl = new ExcelMechanisms();
        exl.printAssocMatrixInExcel<double, double, double>(OutMatrix, "StirDeltas");
    }

    // Tensor: simulation of values of an option. Row prices, column vols, nThird daysToMaturity
    public static void Example5()
    {
        // Values are hard-coded, can be generalized. Data for rows, columns, nthrd
        Vector<double> Prices = new Vector<double>(new double[] { 99.10, 99.15, 99.20, 99.25, 99.30 }, 0);
        Vector<double> DaysToMaturity = new Vector<double>(new double[] { 82, 81, 80, 79, 78, 76 }, 0);
        Vector<double> Vols = new Vector<double>(new double[] { 0.35, 0.40, 0.45 }, 0);

        // Create my option
        STFutOption opx1 = new STFutOption(1, 99.20, 99.25, 82, 0.40);
        STFutOption opxk = new STFutOption(1, 99.10, 99.25, 82, 0.40);
        double dummy = opxk.Price();

        // Create my tensor
        Tensor<double> MyTensor = new Tensor<double>(Prices.MaxIndex + 1, Vols.MaxIndex + 1, DaysToMaturity.MaxIndex + 1, 0, 0, 0);

        // Populating tensor
        for (int t = DaysToMaturity.MinIndex; t <= DaysToMaturity.MaxIndex; t++)
        {
            for (int r = Prices.MinIndex; r <= Prices.MaxIndex; r++)
            {
                for (int c = Vols.MinIndex; c <= Vols.MaxIndex; c++)
                {
                    opx1.new_DaysToExpiry = DaysToMaturity[t];
                    opx1.new_S = Prices[r];
                    opx1.new_Sigma = Vols[c];
                    MyTensor[r, c, t] = opx1.Price() / 100.0;
                }
            }
        }

        // Printing out       
        for (int t = DaysToMaturity.MinIndex; t <= DaysToMaturity.MaxIndex; t++)
        {
            Console.WriteLine("Now is Day: {0}", t);
            for (int r = Prices.MinIndex; r <= Prices.MaxIndex; r++)
            {
                Console.Write("{0:F2} (Vols{1:F2}/{2:F2}/{3:F2})  :", Prices[r], Vols[0], Vols[1], Vols[2]);
                for (int c = Vols.MinIndex; c <= Vols.MaxIndex; c++)
                {
                    Console.Write("{0:F5} ", MyTensor[r, c, t]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    // Class ListedSTFut: Property and Method, using a predefined "ER" setting
    public static void Example6()
    {
        // Using ListedSTFu Class, I take "ER" setting for euribor future.        
        ListedSTFut ERZ3 = new ListedSTFut(99.18, 12, 2013, "ER");

        // Some property of Stir class
        Console.WriteLine("{0}:\t{1}", "ERZ3.GetMinTick", ERZ3.GetMinTick);
        Console.WriteLine("{0}:\t{1}", "ERZ3.GetMinTickValue", ERZ3.GetMinTickValue);
        Console.WriteLine("{0}:\t{1}", "ERZ3.GetPrice", ERZ3.GetSetPrice);
        Console.WriteLine("{0}:\t{1:P}", "ERZ3.GetRate", ERZ3.GetSetRate);
        Console.WriteLine("{0}:\t{1}", "ERZ3.GetIMMDate", ERZ3.GetIMMDate.DateValue.ToString("ddd dd MMM yyyy"));
        Console.WriteLine("{0}:\t{1}", "ERZ3.GetNotionalRepayDate", ERZ3.GetNotionalRepayDate.DateValue.ToString("ddd dd MMM yyyy"));
        Console.WriteLine("{0}:\t{1}", "ERZ3.GetContractSize", ERZ3.GetContractSize);
        Console.WriteLine("{0}:\t{1}", "ERZ3.GetCurrency", ERZ3.GetCurrency);
        Console.WriteLine("{0}:\t{1}", "ERZ3.GetDayBasis", ERZ3.GetDayBasis);
        Console.WriteLine();

        // Changing price and rate
        ERZ3.GetSetPrice = 99.25;
        Console.WriteLine("New Price: {0}\t Rate: {1:P}", ERZ3.GetSetPrice, ERZ3.GetSetRate);
        ERZ3.GetSetRate = 0.01;
        Console.WriteLine("New Rate: {0:P}\t Price: {1}", ERZ3.GetSetRate, ERZ3.GetSetPrice);
        Console.WriteLine();

        // Public Methods        
        Console.WriteLine("{0}\t {1}", "ERZ3.Term()", ERZ3.Term());
        // Console.WriteLine("{0}\t {1}", "ERZ3.fwDFq(365)", ERZ3.fwDFq(365));
        // Console.WriteLine("{0}\t {1}", "ERZ3.fwDFq(360)", ERZ3.fwDFq(360));
        Console.WriteLine("{0}\t {1}", "ERZ3.fwDFq()", ERZ3.fwDFq()); // Eur default
    }

    // Class ListedSTFutOption: Property and Method, using a predefined "ER" setting
    public static void Example7()
    {
        // Using ListedSTFutOption Class, I take "ER" setting for euribor future.        
        // Put Option Data
        double CoP = -1;
        double underlying = 99.235;
        double strike = 99.25;
        Date ValuationDate = new Date(2009, 12, 23);
        Date ExpiryDate = new Date(2010, 3, 15);
        double sigma = 0.40; // yield Vol
        string ContractLabelFromDB = "ER";
        ListedSTFutOption Put = new ListedSTFutOption(CoP, underlying, strike, ValuationDate, ExpiryDate, sigma, ContractLabelFromDB);

        // Call option using different constructor
        double daysPerYear = 365;
        int ExpiryMonth = 3;
        int ExpiryYear = 2010;
        ListedSTFutOption Call = new ListedSTFutOption(-CoP, underlying, strike, sigma, daysPerYear, ValuationDate, ExpiryMonth, ExpiryYear, ContractLabelFromDB);

        // Calculate the theoretical prices
        Console.WriteLine("Put Theor. Price:  {0:F5}", Put.Price());
        Console.WriteLine("Call Theor. Price: {0:F5}", Call.Price());

        // Call and put checking deltas and call put parity
        Console.WriteLine("CallDelta {0:F5}, PutDelta {1:F5}, CallDelta-PutDelta {2:F5}", Call.Delta(), Put.Delta(), Call.Delta() - Put.Delta()); // Call/Put delta are reversed
        Console.WriteLine("CallPut Parity Check: call- put - undelying + strike = {0:F5}", Call.Price() - Put.Price() - underlying + strike);  // Call/Put parity

        // Some calculation using ListConstrSpec details
        double TradingOffer = 0.08;
        int Quantity = 100;
        Console.WriteLine("I Buy {0} @ {1}, I will pay {2} in {3}", Quantity, TradingOffer,
            TradingOffer * Quantity * Put.GetMinTickValue / Put.GetMinTick, Put.GetCurrency);
        Console.WriteLine("Reference Position is {0}", Put.GetContractSize * Quantity);
    }

    // Class Date: use of IMMDate(int Month, int Year), IMMDate(), IMM_Date_Nth(Date Today, int NthStir), IMM_Date_Nth(int NthStir)
    public static void Example_1()
    {
        string myDateFormat = "ddd dd MMM yyyy";  // Data format for output
        Date today = new Date(2009, 12, 13);

        // using IMMDate(), IMM_Date_Nth(int NthStir)
        Console.WriteLine("{0}:\t{1}", "Date today", today.DateValue.ToString(myDateFormat));
        Console.WriteLine("{0}:\t\t{1}", "today.IMMDate()", today.IMMDate().DateValue.ToString(myDateFormat));
        Console.WriteLine("{0}:\t\t{1}", "today.IMM_Date_Nth(2)", today.IMM_Date_Nth(2).DateValue.ToString(myDateFormat));
        Console.WriteLine();

        // Some output using IMMDate(int Month, int Year) and IMM_Date_Nth(Date Today, int NthStir)
        Console.WriteLine("{0}:\t{1}", "Date today", today.DateValue.ToString(myDateFormat));
        Console.WriteLine("{0}:\t\t{1}", "Date.IMMDate(12, 2009)", Date.IMMDate(12, 2009).DateValue.ToString(myDateFormat));
        Console.WriteLine("{0}:\t{1}", "Date.IMM_Date_Nth(today,2)", Date.IMM_Date_Nth(today, 2).DateValue.ToString(myDateFormat));
    }

    // class ListedContSpec
    public static void Example_3()
    {
        // I create a customized ListedContSpec
        ListedContSpec myListedContSpec = new ListedContSpec("Custom", 0.01, 10, 100000, "EUR", 360);

        Console.WriteLine("customized ListedContSpec:");
        // ListedContSpec properties
        Console.WriteLine("{0}\t{1}", "myListedContSpec.GetLabel", myListedContSpec.GetLabel);
        Console.WriteLine("{0}\t{1}", "myListedContSpec.GetMinTick", myListedContSpec.GetMinTick);
        Console.WriteLine("{0}\t{1}", "myListedContSpec.GetMinTickValue", myListedContSpec.GetMinTickValue);
        Console.WriteLine("{0}\t{1}", "myListedContSpec.GetContractSize", myListedContSpec.GetContractSize);
        Console.WriteLine("{0}\t{1}", "myListedContSpec.GetCurrency", myListedContSpec.GetCurrency);
        Console.WriteLine("{0}\t{1}", "myListedContSpec.GetDayBasis", myListedContSpec.GetDayBasis);
    }

    // class ListedContDB
    public static void Example_4()
    {
        // ListedContDB
        ListedContDB DCS = new ListedContDB();

        // my list contract spec used to iterate
        ListedContSpec lcs;

        // Iteration showing contract listed spec
        foreach (string label in DCS.GetLabels)
        {
            lcs = DCS.GetContrSpec(label);
            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", lcs.GetLabel, lcs.GetContractSize,
                lcs.GetCurrency, lcs.GetMinTick, lcs.GetMinTickValue, lcs.GetDayBasis);
        }
        Console.WriteLine();

        // Different way for Iteration showing contract listed spec
        Array<ListedContSpec> myDB = DCS.GetListedContSpecArray;
        int minIndex = myDB.MinIndex;
        int maxIndex = myDB.MaxIndex;
        for (int i = minIndex; i <= maxIndex; i++)
        {
            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", myDB[i].GetLabel, myDB[i].GetContractSize,
                    myDB[i].GetCurrency, myDB[i].GetMinTick, myDB[i].GetMinTickValue, myDB[i].GetDayBasis);
        }
    }

    // Class ListedSTFut: creating a customized setting
    public static void Example_7()
    {
        // Data for custom contract. In any case it works like a short term future.
        double price = 99.10;
        int Month = 12;
        int Year = 2013;
        string customLabel = "MyFuture";
        double minTick = 0.10;
        double minTickValue = 50;
        double contractSize = 3000000;
        string currency = "EUR";
        double dayBasis = 360;

        // My custom contract
        ListedSTFut FUT = new ListedSTFut(price, Month, Year, customLabel, minTick, minTickValue, contractSize, currency, dayBasis);

        // some property of Stir class
        Console.WriteLine("My custom short term future:");
        Console.WriteLine("{0}:\t{1}", "FUT.GetMinTick", FUT.GetMinTick);
        Console.WriteLine("{0}:\t{1}", "FUT.GetMinTickValue", FUT.GetMinTickValue);
        Console.WriteLine("{0}:\t{1}", "FUT.GetPrice", FUT.GetSetPrice);
        Console.WriteLine("{0}:\t{1:P}", "FUT.GetRate", FUT.GetSetRate);
        Console.WriteLine("{0}:\t{1}", "FUT.GetIMMDate", FUT.GetIMMDate.DateValue.ToString("ddd dd MMM yyyy"));
        Console.WriteLine("{0}:\t{1}", "FUT.GetNotionalRepayDate", FUT.GetNotionalRepayDate.DateValue.ToString("ddd dd MMM yyyy"));
        Console.WriteLine("{0}:\t{1}", "FUT.GetContractSize", FUT.GetContractSize);
        Console.WriteLine("{0}:\t{1}", "FUT.GetCurrency", FUT.GetCurrency);
    }

    // Implied Vol
    public static void Example12()
    {
        // My Option data
        double CoP = 1;
        double underlying = 99.29;
        double strike = 99.375;
        double daysToExpiry = 154;
        double yieldVol = 0.46;
        STFutOption myOpt = new STFutOption(CoP, underlying, strike, daysToExpiry, yieldVol);

        // Printing some data
        Console.WriteLine("Given yieldVol: {0:F6} price is: {1:F6}", yieldVol, myOpt.Price());
        double newPrice = 0.05; // changing price
        double newYieldVol = myOpt.ImplVol(newPrice); // implied vol
        Console.WriteLine("Given price: {0:F6} impliedVol is: {1:F6}", newPrice, newYieldVol);
        // now I use implied vol calculated to check if newPrice return
        myOpt.new_YieldVol = newYieldVol;
        Console.WriteLine("Given yieldVol: {0:F6} price is: {1:F6}", newYieldVol, myOpt.Price());
        Console.WriteLine("Using for 'F6' format for output");

        // Checking YieldVol to PriceVol and vice versa
        Console.WriteLine(STFutOption.FromYieldVolToPriceVol(94.53, 0.2592));
        Console.WriteLine(STFutOption.FromPriceVolToYieldVol(94.53, 0.015));
    }
}