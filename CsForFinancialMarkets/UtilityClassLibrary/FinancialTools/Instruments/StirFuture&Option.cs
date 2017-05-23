// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// StirFuture&Option.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------

 
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;

 // 1.What I have done.

 // I created two base class for considering math-financial generic instrument:class STFut, class STFutOption
 // Then I create their derived class in a more specialized way taking in consideration detail of real listed instrument:
 // they are class ListedSTFut and class ListedSTFutOption.
 // class ListedSTFut and class ListedSTFutOption are inspired from Liffe Euronext instrument.
 // ListeContrSpec, ListedContDB, IListedInstrument allow to manage contract listed.
 // I added some method to Date Class IMMDate(int Month, int Year), IMMDate(), IMM_Date_Nth(Date Today, int NthStir), IMM_Date_Nth(int NthStir)
 // This method are a taken for VBA code from Liffe and ported to C# by me 
 // Finally I create 11 Working examples.
 // All classes, structure and interface are documented in details in .cs files.

 // 2. Short description of Code
 // -IListedInstrument: 
 // It is an interface containing a description of functionality
 // for all typical "ListedIntrument", but it does not provide implementation

 // -Struct ListedContSpec:
 // It is a structure. It is used to store typical data and specifications needed for a listed contract

 // -Class ListedContDB:
 // It ia a class. It is a like a collection of ListedContSpec. To make a ListContSpec available in others
 // classes please enter here new data.

 // -class STFut
 // it is an class for interest rate future. It is a class for math-financial generic instrument. 
 // It doesn't consider real world details of a listed instrument. It is a base class

 // -Class ListedSTFut
 // it is a class derived from class STFut. It consider all real element of a Listed STFut

 // -class STFutOption
 // it is an class for interest rate future option. It is a class for math-financial generic instrument. 
 // It doesn't consider real world details details of a listed instrument. It is a base class

 // -class ListedSTFutOption
 // it is a class derived from class STFutOption. It consider all real element of a Listed STFutOption

 // -class Singleton<T>
 // it is an implementation of singleton pattern taken from  // http: // www.sanity-free.org/132/generic_singleton_pattern_in_csharp.html
 // I think you have your own implementation. So I think it is better to take your implementation for sure.

 // 3. Class Diagram
 // I create a class diagram to show the design of classes


 // 4. References

 // http: // www.euronext.com/tools/documentation/wide/documents-2087-EN.html" [107286.pdf]
 // "Introduction to Trading Stir" Ch 5-6-7-8-9-10 to understand how Short Future and Options work 

 // http: // www.euronext.com/fic/000/047/616/476163.pdf
 // Short Term Interest Rate ("STIR") Options Contracts, code refers to "London Info-Flash No.LO09/08"

 // http: // www.euronext.com/tools/documentation/wide/documents-2087-EN.html" [418361.pdf]
 // "Summary of futures and options contracts" to understand contract specifications
 // page 5-6 Three Month Euribor Future and Option
 // page 8-9 Three Month Eurodollar Future and Option
 // page 11-12 Three Month Sterling  Future and Option
 // page 14-15 Three Month Euro Swiss Franc  Future and Option

 // http: // www.euronext.com/editorial/wide/editorial-4304-EN.html 106420.xls
 // reference for following methods in Class Date: IMMDate(int Month, int Year), IMMDate(), IMM_Date_Nth(Date Today, int NthStir), IMM_Date_Nth(int NthStir) 
 // reference for following methods in Class STFut: CalcNotionalRepayDate()Term()fwDFq()

 // http: // www.sanity-free.org/132/generic_singleton_pattern_in_csharp.html
 // implementation of singleton pattern class Singleton<T>

 // "The Complete Guide To Option Pricing Formulas" Espen Haug 2007 ch 11.3.1 e 11.3.2

 // Further readings and docs:
 // "LIFFE Options a guide to trading strategies" [107279.pdf]
 // "Interest rate portfolio Short Term Interest Rate products" [268469.pdf]
 // http: // www.euronext.com/editorial/wide/editorial-4304-EN.html 106421.xls and 106422.xls

 // 5- In General we show the Use of:

 // Syntax: interface; derive class; singleton pattern, 
 // class/struct: Singleto<T>, ListedContSpec,ListedContDB,IListedInstrument,AssocArray<Key,Value>, Date, Set<t>, Queue<T>.
 // STFut,ListedSTFut,STFutOption,ListedSTFutOption,Date


 // http: // www.sanity-free.org/132/generic_singleton_pattern_in_csharp.html

public static class Singleton<T> where T : new()
{
    static Mutex mutex = new Mutex();
    static T instance;

    public static T Instance
    {
        get
        {
            mutex.WaitOne();
            if(instance == null)
            {
                instance = new T();
            }
            mutex.ReleaseMutex();
            return instance;
        }
    }
}

 // Interface Price
public interface IPrice 
    {
    double Price();  // Calculate the price
    }

 // Interface Listed Instrument
public interface IListedInstrument 
    {
         // Property signatures:
        
         // Getting Label
        string GetLabel
        {
            get;            
        }

         // Getting minimum Tick
        double GetMinTick
        {
            get;            
        }

         // Getting minimum Tick Value
        double GetMinTickValue
        {
            get;            
        }

         // Getting contract size
        double GetContractSize
        {
            get;
        }

         // Getting currency
        string GetCurrency
        {
            get;
        }    
    }

 // Listed Contract Specification
public struct ListedContSpec: IListedInstrument
    {
         // Data Member
        private string _label;           // label to identify the ListedContSpec
        private double _minTick;         // minimum price-movement allowed 
        private double _minTickValue;    // value of _minTick
        private double _contractSize;    // the underl. contract size
        private string _currency;        // currency of contract
        private double _dayBasis;        // base for day count interest rate

         // Constructor
        public ListedContSpec(string label, double minTick, double minTickValue, double contractSize, 
            string currency, double dayBasis)
        {
            this._label = label;
            this._minTick = minTick;
            this._minTickValue = minTickValue;
            this._contractSize = contractSize;
            this._currency = currency;
            this._dayBasis = dayBasis;
        }

         // Get Minimum Tick read-only property
        public string GetLabel
        {
            get
            {
                return _label;
            }
        }

         // Get Minimum Tick read-only property
        public double GetMinTick
        {
            get
            {
                return _minTick;
            }
        }

         // Get Minimum Tick Value read-only property
        public double GetMinTickValue
        {
            get
            {
                return _minTickValue;
            }
        }

         // Get ContractSize read-only property
        public double GetContractSize
        {
            get
            {
                return _contractSize;
            }
        }

         // Get Currency read-only property
        public string GetCurrency
        {
            get
            {
                return _currency;
            }
        }

         // Get DayBasis read-only property
        public double GetDayBasis
        {
            get
            {
                return _dayBasis;
            }
        }
    }

 // DataContrSec: is a "data base" for used ListedConSpec
public class ListedContDB
{
         // Data member
        AssocArray<string,ListedContSpec> CollectOfContrSpec;

         // Constructors  
         // Default Constructor
        public ListedContDB()
        {
             // send std data to data member
            SetDataMember(GetStdListedContSpec());            
        }
 
         // Constructor adding customs ListedContSpec
    public ListedContDB(ListedContSpec[] myCustoms)
    {
         // send std data to data member
        Queue<ListedContSpec> q = new Queue<ListedContSpec>(); // contains  ListedContSpec  
        q = GetStdListedContSpec();
        foreach (ListedContSpec l in myCustoms)
        {
            q.Enqueue(l);
        }
        SetDataMember(q);
    }
    
     // Method
     // assign data to data member
    private void SetDataMember(Queue<ListedContSpec> contractSpec)
        {
             // Data member
            Set<string> s = new Set<string>();  // contains labels

             // Populate the set with label
            foreach (ListedContSpec l in contractSpec)
            {
                s.Insert(l.GetLabel);
            }

             // assigns
            CollectOfContrSpec = new AssocArray<string, ListedContSpec>(s, new Vector<ListedContSpec>(contractSpec.ToArray(), 0));
        }

         // return a queue of known ListedContSpec
        private Queue<ListedContSpec> GetStdListedContSpec()
        {
            Queue<ListedContSpec> q = new Queue<ListedContSpec>(); // contains  ListedContSpec          

             // Adding my contracts: this is the only entry point. Please enter here new specification.
             // For contract specification see "Summary of Futures And Option Contracts" ("418361.pdf")
             // http: // www.euronext.com/editorial/wide/editorial-4304-EN.html
            q.Enqueue(new ListedContSpec("ER", 0.005, 12.5, 1000000, "EUR",360));
            q.Enqueue(new ListedContSpec("FD", 0.005, 12.5, 1000000, "USD",365)); // Liffe Future Settlement 
            q.Enqueue(new ListedContSpec("L", 0.01, 12.5, 500000, "GBP",365));
            q.Enqueue(new ListedContSpec("S", 0.01, 12.5, 1000000, "CHF",360));
             // ....

            return q;
        }

         // Given a label will return a ListedContSpec
        public ListedContSpec GetContrSpec(string label) 
        {
            return CollectOfContrSpec[label];
        }      
    
         // Read only property returns labels
        public Set<string> GetLabels 
        {
            get
            {
                return CollectOfContrSpec.Keys;
            }
        }

         // Read only property returns GetListedContSpecArray
        public Array<ListedContSpec> GetListedContSpecArray 
        {
            get 
            {
                return new Array < ListedContSpec > (GetStdListedContSpec().ToArray());
            }
        }
    }

     // base class
    public class STFut: IPrice
    {
         // data member
        private double mktPrice;
        private double implRate;
        private Date IMMDate;
        private Date NotionalRepayDate;

         // Constructor              
        public STFut(double price, int Month, int Year)
        {
            this.mktPrice = price;
            this.IMMDate = Date.IMMDate(Month, Year);
            CalcRate();
            CalcNotionalRepayDate();
        }
        
         // Methods
         // return price
        public double Price() 
        {
            return mktPrice;
        }

         // Recalculate rate
        private void CalcRate()
        {
            this.implRate = (100.0 - this.mktPrice)/100.0;
        }

         // Recalculate price
        private void CalcPrice()
        {
            this.mktPrice = 100.0 - this.implRate * 100.0;
        }

         // Calculate the final repay date of stir
        private void CalcNotionalRepayDate()
        {
            NotionalRepayDate = IMMDate.AddMonths(3);
        }

         // Calculate number of days between NotionalRepayDate and IMMDate
        public int Term()
        {
            return (int)IMMDate.D_EFF(NotionalRepayDate);
        }

         // Calculate quarterly forward discount factor using future price
        public double fwDFq(double Base)
        {
             // formula: 1/(1+(100-FuturePrice)% * Term/dayBasis)
             // You can chose the base you want. Often 360 for EUR, 365 for GBP, ... 
             // "Introduction to trading STIRs" (107286.pdf)  Ch 5 and Ch 6
            return 1 / (1 + ((100.0 - mktPrice) / 100.0) * Term() / Base);
        }

         // Property
         // Get rate read and write property
        public double GetSetRate
        {
            get
            {
                return implRate;
            }
            set
            {
                this.implRate = value;
                CalcPrice();
            }
        }

         // Get IMM Date read-only property
        public Date GetIMMDate
        {
            get
            {
                return IMMDate;
            }
        }

         // Get price read and write property
        public double GetSetPrice
        {
            get
            {
                return mktPrice;
            }
            set
            {
                this.mktPrice = value;
                CalcRate();
            }
        }

         // Get NotionalRepayDate read-only property
        public Date GetNotionalRepayDate
        {
            get
            {
                return NotionalRepayDate;
            }
        }
    }

     // Derived Class from STFut: Class Generic Listed Short Term Future 
    public class ListedSTFut : STFut, IListedInstrument 
{
         // data member
        ListedContSpec  ContractSpec;

         // Constuctor: can create a custom Listed Short Term Future. "customLabel" can be as you like.
    public ListedSTFut(double price, int Month, int Year, 
        string customLabel, double minTick, double minTickValue, double contractSize, string currency,double dayBasis)
        :base(price, Month, Year)
        {
            ContractSpec = new ListedContSpec(customLabel,minTick, minTickValue, contractSize, currency, dayBasis);
        }

         // Constructor: can only use "ContractLabelFromDB" value that are already in ListedContDB
    public ListedSTFut(double price, int Month, int Year, string ContractLabelFromDB)
        : base(price, Month, Year)
    {
        ContractSpec = Singleton<ListedContDB>.Instance.GetContrSpec(ContractLabelFromDB);
    }

     // Calculate quarterly forward discount factor using future price according to specific  day basis
    public double fwDFq()
    {
         // formula: 1/(1+(100-FuturePrice)% * Term/DayBasis)
        return 1 / (1 + ((100.0 - base.GetSetPrice) / 100.0) * Term() / ContractSpec.GetDayBasis);
    }

            
         // Property implementation:
         // Get Minimum Tick read-only property
        public string GetLabel
        {
            get
            {
                return ContractSpec.GetLabel;
            }
        }

         // Get Minimum Tick read-only property
        public double GetMinTick
        {
            get
            {
                return ContractSpec.GetMinTick;
            }
        }

         // Get Minimum Tick Value read-only property
        public double GetMinTickValue
        {
            get
            {
                return ContractSpec.GetMinTickValue;
            }
        }

         // Get ContractSize read-only property
        public double GetContractSize
        {
            get
            {
                return ContractSpec.GetContractSize;
            }
        }

         // Get Currency read-only property
        public string GetCurrency
        {
            get
            {
                return ContractSpec.GetCurrency;
            }
        }

         // Get GetDayBasis read-only property
        public double GetDayBasis
        {
            get
            {
                return ContractSpec.GetDayBasis;
            }
        }
}

     // base class Short Term Future Option
     // /Short Term Interest Rate ("STIR") Options Contracts, code refers to "London Info-Flash No.LO09/08"
     // /Web: http: // www.euronext.com/fic/000/047/616/476163.pdf
    public class STFutOption: IPrice
    {
        public double COP;       // 1= call, -1 = put
        public double S;         // Underlying Asset Price
        double X;                // Exercise Price
        double PriceVol;         // Volatility
        double D1;               // D1 = (ln(S/X)+(0.5*PriceVol*PriceVol*T)) / (PriceVol*T^(0.5));
        double D2;               // D2 = D1 - (PriceVol*T^(0.5)) ;
        double DaysToExpiry;     // Days to Expiry
        double T;                // Time To Maturity in years
        double DaysPerYear;      // Days Per Years, used to calculate year fraction

         // Constructors
         // Generic constructor
        public STFutOption(double CoP, double underlying, double strike,
                       double daysToExpiry, double priceVol, double daysPerYear)
        {
            this.COP = CoP; this.S = underlying; this.X = strike;
            this.DaysToExpiry = daysToExpiry; this.PriceVol = priceVol; // FromYieldVolToPriceVol(underlying, yieldVol);
            this.DaysPerYear = daysPerYear; // 365 or 365.25 or 252 or ...
            this.T = daysToExpiry / DaysPerYear;
            double SqS = PriceVol * PriceVol;
            double SqT = Math.Sqrt(T);
            D1 = (Math.Log(S / X) + (0.5 * SqS * T)) / (SqT * PriceVol);
            D2 = D1 - (SqT * PriceVol);
        }

         // Constructor using daysPerYear = 365
        public STFutOption(double CoP, double underlying, double strike,
                       double daysToExpiry, double yieldVol)
            : this(CoP, underlying, strike, daysToExpiry, yieldVol, 365.0) { }    

         // Method: calculate the price
        public double Price()
        {
            if (COP == 1)  // For Call
                return (S * Formula.CND(D1) - X * Formula.CND(D2));  // Calculate the premium according "London Info-Flash No.LO09/08"
            if (COP == -1) // For Put
                return (X * Formula.CND(-D2) - S * Formula.CND(-D1));  // Calculate the premium according "London Info-Flash No.LO09/08"
            return 0;
        }

         // Method: switch call/put: if it is a call switch to a put and reverse
        public void SwitchCallPut()
        {
            if (COP == 1)
                COP = -1;
            else
                COP = 1;
        }

         // Method: Calculate the Delta
        public double Delta()
        {
            if (COP == 1)
                return Formula.CND(D1);
            else
                return Formula.CND(D1) - 1;
        }

         // Method: Calculate the Gamma
        public double Gamma()
        {
            return Formula.ND(D1) / (S * PriceVol * Math.Sqrt(T));
        }

         // Method: Calculate the Vega
        public double Vega()
        {
            return S * Formula.ND(D1) * Math.Sqrt(T);
        }

         // Method: Calculate the Theta
        public double Theta()
        {
            return -S * Formula.ND(D1) * PriceVol / (2 * Math.Sqrt(T));
        }

         // Method: Update Time To Maturity, D1 and D2
        void UpDate()
        {
            this.T = this.DaysToExpiry / 365; // or 365.25 or 252
            double SqS = PriceVol * PriceVol;
            double SqT = Math.Sqrt(T);
            D1 = (Math.Log(S / X) + (0.5 * SqS * T)) / (SqT * PriceVol);
            D2 = D1 - (SqT * PriceVol);
        }

         // Method: given Future Price and Price Volatility, will return Yield Volatility
         // Reference "The Complete Guide To Option Pricing Formulas" Espen Haug 2007 ch 11.3.2
        public static double FromPriceVolToYieldVol(double _underlying, double _priceVol) 
        {
             // return Yield Volatility
            return (_priceVol * _underlying) / (100 - _underlying);
        }

         // Method: given Future Price and Yield Volatility, will return Price Volatility
         // Reference "The Complete Guide To Option Pricing Formulas" Espen Haug 2007 ch 11.3.2
        public static double FromYieldVolToPriceVol(double _underlying, double _yieldVol)
        {
             // return Price Volatility
            return _yieldVol * (100 - _underlying) / _underlying;
        }

         // Method: given a volatility guess and a price will return implied vol
        public double ImplVol(double price)
        {
             // We try to translate in c# the idea of Paul Wilmott Introduces Quantitative Finance ch 10
            double error = 1E-10; // precision of calculation        
            double implYieldVol = FromPriceVolToYieldVol(this.S, this.PriceVol); // the starting vol value is the one used in the constructor
        
             // data used in iteration process
            double priceError; // price error
            double dv = error + 1; // vega correction
            double price_calculated = 0.0, vega_calculated = 0.0; // used to iterate

             // my option to iterate
            STFutOption myOpt = new STFutOption(this.COP, this.S, this.X, this.DaysToExpiry,implYieldVol );
             // iteration
            while (Math.Abs(dv) > error)
            {
                 // I iterate according to vega information
                price_calculated = 0; vega_calculated = 0;

                myOpt.new_YieldVol = implYieldVol;
                price_calculated = myOpt.Price();
                vega_calculated = myOpt.Vega();

                priceError = price_calculated - price;
                dv = priceError / vega_calculated;
                implYieldVol -= dv;
            };
            return implYieldVol;
        }

         // Property - write: updating the underlying
        public double new_S
        {
            set
            {
                this.S = value;
                UpDate();

            }
        }

         // Property - write: updating DaysTo Expiry
        public double new_DaysToExpiry
        {
            set
            {

                this.DaysToExpiry = value;
                UpDate();

            }
        }

         // Property - write: updating Sigma
        public double new_YieldVol
        {
            set
            {
                this.PriceVol = FromYieldVolToPriceVol( S, value);
                UpDate();
            }
        }

        public double new_Sigma
        {
            set
            {
                this.PriceVol = value;
                UpDate();
            }
        }
    }

     // Derived Class from STFutOption: Class Generic Listed Short Term Future Option 
    public class ListedSTFutOption : STFutOption, IListedInstrument 
    {
         // data member    
        ListedContSpec myContractSpec;
        Date OptExpiryDate;      

         // Generic Constructor, not used in derived class              
        public ListedSTFutOption(double CoP, double underlying, double strike,
            Date ValuationDate, Date ExpiryDate, double sigma, double daysPerYear, string label, 
            double minTick, double minTickValue, double contractSize, string currency, double dayBasis)
        :base(CoP, underlying, strike, ExpiryDate.SerialValue - ValuationDate.SerialValue ,sigma,daysPerYear)
            {
                myContractSpec = new ListedContSpec(label, minTick, minTickValue, contractSize, currency, dayBasis);            
                OptExpiryDate = ExpiryDate;
            }

             // Constructor using ContractLabelFromDB      
        public ListedSTFutOption(double CoP, double underlying, double strike,
            Date ValuationDate, Date ExpiryDate, double sigma, double daysPerYear,
            string ContractLabelFromDB)
            : base(CoP, underlying, strike, ExpiryDate.SerialValue - ValuationDate.SerialValue, sigma, daysPerYear) 
        {
            myContractSpec = Singleton<ListedContDB>.Instance.GetContrSpec(ContractLabelFromDB);
            OptExpiryDate = ExpiryDate;
        } 
    
         // Constructor using default value of 365 days per years, used in derived class             
        public ListedSTFutOption(double CoP, double underlying, double strike,
            Date ValuationDate, Date ExpiryDate, double sigma, string ContractLabelFromDB)
            : this(CoP, underlying, strike, ValuationDate, ExpiryDate, sigma, 365, ContractLabelFromDB) { }
    
         // Constructor using option ExpiryMonth and ExpiryYear. 
         // Expiry is two business days prior to the third Wednesday of the expiry month for both serial and 
         // quarterly expiry option
        public ListedSTFutOption(double CoP, double underlying, double strike,
            double sigma, double daysPerYear, Date ValuationDate, int ExpiryMonth, int ExpiryYear,
            string ContractLabelFromDB)
            : this(CoP,underlying,strike, ValuationDate,Date.IMMDate(ExpiryMonth, ExpiryYear).add_workdays(-2),
            sigma, daysPerYear, ContractLabelFromDB) { }
    
         // Read only property
        public Date GetExpiryDate 
        {
            get
            {
                return OptExpiryDate;
            }
        }
        
         // Property implementation:
         // Get Minimum Tick read-only property
        public string GetLabel
        {
            get
            {
                return myContractSpec.GetLabel;
            }
        }
   
         // Get Minimum Tick read-only property
        public double GetMinTick
        {
            get
            {
                return myContractSpec.GetMinTick;
            }
        }

         // Get Minimum Tick Value read-only property
        public double GetMinTickValue
        {
            get
            {
                return myContractSpec.GetMinTickValue;
            }
        }

         // Get ContractSize read-only property
        public double GetContractSize
        {
            get
            {
                return myContractSpec.GetContractSize;
            }
        }

         // Get Currency read-only property
        public string GetCurrency
        {
            get
            {
                return myContractSpec.GetCurrency;
            }
        }
    }




