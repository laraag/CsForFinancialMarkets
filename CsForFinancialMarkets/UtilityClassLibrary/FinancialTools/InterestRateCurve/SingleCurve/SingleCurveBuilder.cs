// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// SingleCurveBuilder.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

 // abstract class 
public abstract class SingleCurveBuilder<DoInterpOn, Interpolation>: ISingleRateCurve
    where DoInterpOn : InterpAdapter, new()
    where Interpolation : BaseOneDimensionalInterpolator, new()
{
     // Data Member
    public Date refDate;  // reference date of the curve
    public Interpolation PostProcessInterpo;  // the interpolator post process
    public DoInterpOn interpAdapter; // adapter to interpolator
    public IEnumerable<BuildingBlock> BBArray;  // array of building block, sorted ascending maturity
    protected SwapStyle[] OnlyGivenSwap;  // Only Given Swap from BBArray
    public SwapStyle SwapType; // it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
    public RateSet mktRateSet; // market starting data
    
     // constructor: RateSet rateSet are market data inputs 
    public SingleCurveBuilder(RateSet rateSet) 
    {
         // RefDate
        refDate = rateSet.refDate;
        PostProcessInterpo = new Interpolation(); // Post process interpolator
        interpAdapter = new DoInterpOn();
        mktRateSet = rateSet;
        
         // Create Building block
        IEnumerable<BuildingBlock> BB = rateSet.GetArrayOfBB();
        
         // Sort ascending end date
        BBArray =   from c in BB                
                    orderby c.endDate.SerialValue ascending
                    select c;

         // Only Given Swap from BBArray        
        OnlyGivenSwap = (from c in BBArray
                         where c.GetType().BaseType == typeof(SwapStyle)
                         select (SwapStyle)c).ToArray();

         // some data validation: swap should be all of same building block
         // the type of building block
        BuildingBlockType BBT = OnlyGivenSwap[0].buildingBlockType;
        
         // Are all them the same? 
        bool IsSameSwapType = OnlyGivenSwap.All(s => s.buildingBlockType == BBT);

        if (IsSameSwapType)  // if true
        {
             // it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
            SwapType = (SwapStyle) new BuildingBlockFactory().CreateEmptyBuildingBlock(BBT);
        }
        else
        {
            throw new ArgumentException("error in building blocktype"); // if not throw an exception
        }
    }

     // Protected Method used in constructor
    abstract protected void PreProcessInputs();  // Prepare rateSet to be processed from Solve

    abstract protected void Solve();  // Do calculus. Bootstrap

    abstract protected void PostProcessData(); // prepare for interpolations. Create a SortedList<double,double>

     // Public Methods
    public double DF(Date d) 
    {
        double sd = new Date(d).SerialValue; // serial date to calculate discount factor
         // Adapt data to interpolator according to <DoInterpOn>
        return interpAdapter.FromInterpToDf(PostProcessInterpo.Solve(sd),sd);
    }

    #region implementing IRateCurve interface
     // return reference date
    public Date RefDate() { return this.refDate; }
     // Calculate discount factor
    public double Df(Date d)
    {
        double sd = new Date(d).SerialValue; // serial date to calculate discount factor
         // Adapt data to interpolator according to <DoInterpOn>
        return interpAdapter.FromInterpToDf(PostProcessInterpo.Solve(sd), sd);
    }
     // forward rate of tenor Period FloatTenor
    public double Fwd(Date StartDate) 
    {
        Date ed = StartDate.add_period(SwapType.swapLeg2.UnderlyingRateTenor, false).GetBusDayAdjust(BusinessDayAdjustment.ModifiedFollowing);
        double yf = StartDate.YF(ed, Dc._Act_360);
        double df_ini = DF(StartDate);
        double df_end = DF(ed);
        return ((df_ini / df_end) - 1) / yf;
    }

     // calculate forward start swap
    public double SwapFwd(Date StartDate, string Tenor) 
    {
         // 1. Build the swap. Rate is not important I use 0.0
        SwapStyle myS = (SwapStyle) new BuildingBlockFactory().CreateBuildingBlock(StartDate, 0.0, Tenor, SwapType.buildingBlockType);
         // 2. Calculate Par Rate
         // fixed leg data
        double[] yfFixLeg = myS.scheduleLeg1.GetYFVect(myS.swapLeg1.DayCount); // fixed is leg 1

         // dfs array of fixed lag
        double[] dfDates = Date.GetSerialValue(myS.scheduleLeg1.payDates); // serial date of fixed lag (each dates we should find df)
         // Vector<double> k = PostProcessInterpo.Curve(new Vector<double>(dfDates, 0));

        double[] dfFixLeg = PostProcessInterpo.Curve(dfDates); // get interpolated value (i.e. log df or log r or...
         // transform interpolated value back to discount factor
        for (int i = 0; i < yfFixLeg.Length; i
            ++)
        {
            dfFixLeg[i] = interpAdapter.FromInterpToDf(dfFixLeg[i], dfDates[i]);
        }

         // Interpolation Methods for Curve Construction PATRICK S. HAGAN & GRAEME WEST Applied Mathematical Finance,Vol. 13, No. 2, 89–129, June 2006
         // Formula 2) page 4 
        double iniDf = interpAdapter.FromInterpToDf(PostProcessInterpo.Solve(StartDate.SerialValue),StartDate.SerialValue);
        return Formula.ParRate(yfFixLeg, dfFixLeg,iniDf); // Calculate par rate
    }
     // return SwapStyle used for bootstrapping, it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
    public SwapStyle GetSwapStyle() { return this.SwapType; }
    #endregion
    #region risk

     // return array of curves, initialised after shifting  mktRateSet elements
    public ISingleRateCurve[] ShiftedCurveArray(double shift)
    {
         // i-curve is built using its setup (interpolator,..), but shifting i-element of mktRateSet
         // up of 'shift' quantity
        RateSet[] rsArr = mktRateSet.ShiftedRateSetArray(shift); // array of shifted RateSet (my scenario)
        int n = rsArr.Length; // number of elements
        ISingleRateCurve[] curves = new ISingleRateCurve[n];  // array of curves
        for (int i = 0; i < n; i++)
        { // iterate to build all curve needed
            curves[i] = CreateInstance(rsArr[i]); // build the correct curve
        }
        return curves;
    }

     // return one only curve, initialised after shifting all mktRateSet elements up of 'shift' quantity, once at the same time
    public ISingleRateCurve ParallelShift(double shift)
    {
        RateSet rs = mktRateSet.ParallelShiftRateSet(shift); // parallel shift
        return CreateInstance(rs);  // build the correct curve
    }

     // Derived classes must implement this method
    public abstract ISingleRateCurve CreateInstance(RateSet newRateSet);  // create an instance of class using a new RateSet
    #endregion

     // Get input rates used to build the curve
    public double[] GetInputRates() 
    {
        int n =mktRateSet.Count;
        double[] outPut = new double[n];
        for (int i = 0; i < n; i++) 
        {
            outPut[i] = mktRateSet.Item(i).V;
        }
        return outPut;
    }
}

 // Derived class for traditional/standard bootstrapping 
public class SingleCurveBuilderStandard<DoInterpOn, Interpolation> : SingleCurveBuilder<DoInterpOn, Interpolation>
    where DoInterpOn : InterpAdapter, new()
    where Interpolation : BaseOneDimensionalInterpolator, new()
{
     // Data Member    
    private OneDimensionInterpolation MissingRateInterp;  // Interpolation used on missing rate
     // used in the constructor
    private IEnumerable<BuildingBlock> PreProcessedData;
    private SortedList<double, double> DateDf;  // SerialDate as Key, DF as value, used in constructor to collect data coming from PreProcessedData

     // Constructor
    public SingleCurveBuilderStandard(RateSet rateSet, OneDimensionInterpolation missingRateInterp)
        : base(rateSet)
    {   
         // Constructor arguments
         // RateSet rateSet: market data input
         // OneDimensionInterpolation missingRateInterp: interpolation used to find missing data
         //                                             it interpolates directly on markets rate

         // To Data Member
        MissingRateInterp = missingRateInterp;

         // 1)Prepare data to be used in Solve() (output is: PreProcessedData)
        PreProcessInputs();

         // 2)Do Calculus: from PreProcessedData calculate post processed data (output is: DataDF)
        Solve();

         // 3)Inizialize final Interpolator: from post processed DataDF set up final interpolator (output is: PostProcessInterpo)
        PostProcessData();
    }    

     // Methods
    #region Protected Methods used in constructor    
     // Prepare data to be used in Solve()
    protected override void PreProcessInputs()
    {
         // PreProcessInputs()
         // 1) fill missing data
         //      -find missing swap;
         //      -create them; 
         //      -interpolate value of them according to interpolator
         // 2) Create PreProcessedData: merge missing data with starting available data 

         // fill missing
        int MaxSwapTenor = OnlyGivenSwap.Last().Tenor.tenor; // Max tenor of available swap
        List<BuildingBlock> MissingSwapList = new List<BuildingBlock>(); // List of missing swap

         // Add Missing Swap to MissingSwapList
        BuildingBlockFactory f = new BuildingBlockFactory(); // factory to create missing building block
        for (int i = 1; i < MaxSwapTenor; i++)
        {
            Period p = new Period(i, TenorType.Y); // needed period
            if (!OnlyGivenSwap.Any(bb => bb.Tenor.GetPeriodStringFormat() == p.GetPeriodStringFormat()))
            {
                 // Check if in OnlyGivenSwap period "p" exists.
                 // if not, it starts creating missing swap. Rate 0.0 just to initialise
                MissingSwapList.Add(f.CreateBuildingBlock(refDate, 0.0, p.GetPeriodStringFormat(), SwapType.buildingBlockType));
            }
        }

         // interpolate missing rate using given data: it interpolates directly on markets rate
        IEnumerable<double> xGivenDays = from c in OnlyGivenSwap
                                         select c.endDate.SerialValue;

        IEnumerable<double> yGivenSwap = from c in OnlyGivenSwap
                                         select c.rateValue;
         // Set up interpolator
        OneDimInterpFactory iFactory = new OneDimInterpFactory();
        IInterpolate iSwap = iFactory.FactoryMethod(MissingRateInterp, xGivenDays.ToArray(), yGivenSwap.ToArray());


         // Missing swap with interpolated value
        IEnumerable<BuildingBlock> MissingSwap = from c in MissingSwapList
                                                 select f.CreateBuildingBlock(c.refDate, iSwap.Solve(c.endDate.SerialValue), c.Tenor.GetPeriodStringFormat(), c.buildingBlockType);

         // Complete Filled data ready to be bootstrapped 
        PreProcessedData = from c in BBArray.Union(MissingSwap)
                                                      orderby c.endDate.SerialValue ascending
                                                      select c;       
    }

     // Do Calculus : from Preprocessed Data calculate DF (post processed data is a DataDF)
    protected override void Solve()
    {
        DateDf = new SortedList<double, double>(); // Inizialize DateDF

        DateDf.Add(refDate.SerialValue, 1.0); // first discount factor

         // bootstrap: find df from each building block
        foreach (BuildingBlock b in PreProcessedData)
        {
             // if depo
            if (b.GetType().BaseType == typeof(OnePaymentStyle))
            {
                double yf = refDate.YF(b.endDate, b.dayCount); // year fraction
                double df = Formula.DFsimple(yf, b.rateValue); // calculate discount factor
                DateDf.Add(b.endDate.SerialValue, df); // add df to DateDf
            }
            if (b.GetType().BaseType == typeof(SwapStyle))  // if swap
            {
                 // Calculate Df using formula (3) from
                 // Interpolation Methods for Curve Construction PATRICK S. HAGAN & GRAEME WEST Applied Mathematical Finance,Vol. 13, No. 2, 89–129, June 2006
                
                 // Prepare input to be used in formula from fixed leg of swap
                double[] yfFixLeg = ((SwapStyle)b).scheduleLeg1.GetYFVect(((SwapStyle)b).swapLeg1.DayCount); // fixed is leg 1
                double[] dfDates = Date.GetSerialValue(((SwapStyle)b).scheduleLeg1.payDates); // serial date of fixed lag (each dates we should find df)                        
                double[] dfFixLeg = new double[dfDates.Count()]; // array of df of swap fixed leg
                 // get available df in DateDf. 
                for (int i = 0; i < dfDates.Count() - 1; i++)
                {
                    dfFixLeg[i] = DateDf[dfDates[i]];  // note that dfDates.Count() - 1, since the last is the unknown
                }

                 // calculate final df and add to DateDf
                DateDf.Add(b.endDate.SerialValue, Formula.FinalDF(b.rateValue, yfFixLeg, dfFixLeg));
            }
        }
    }
    
     // From the post processed data (DateDf), it sets up the final interpolator to be used in method DF(..) 
    protected override void PostProcessData()
    {
        // adapt DF from DateDf to PostProcessInterpo according to <DoInterpOn>
        IEnumerable<double> adaptedDF = from c in DateDf
                                        select interpAdapter.FromDfToInterp(c.Value, c.Key);

         // Inizialize PostProcesInterpo
        PostProcessInterpo.Ini(DateDf.Keys, adaptedDF.ToArray());
    }
    #endregion

     // create an instance of class using a new RateSet
    public override ISingleRateCurve CreateInstance(RateSet newRateSet) 
    {
        return new SingleCurveBuilderStandard<DoInterpOn, Interpolation>(newRateSet, MissingRateInterp);
    }
}

 // Derived class class Interpolation And Bootstrap not two process (code comes from Single Curve Best fit
public class SingleCurveBuilderInterpBestFit<DoInterpOn, Interpolation> : SingleCurveBuilder<DoInterpOn, Interpolation>
    where DoInterpOn : InterpAdapter, new()
    where Interpolation : BaseOneDimensionalInterpolator, new() 
{
     // Data Member 
    protected SortedList<double, double> PreProcessedData; // serial date as key, df as value coming only from depo
    protected SortedList<double, double> IniGuessData; // serial date as key, df as value coming only from swap. Ini Guess for iterator
     // protected IEnumerable<BuildingBlock> OnlyGivenSwap;  // Only Given Swap from BBArray

     // Constructor
    public SingleCurveBuilderInterpBestFit(RateSet rateSet)
        : base(rateSet)
    {
         // Prepare data to be used in Solve() (output is: PreProcessedData)
        PreProcessInputs();

         // Do Calculus: from PreProcessedData inizialises PostProcessInterpo
        Solve();

         // Not needed for this class since Solve()initialised directly PostProcessInterpo
         // PostProcessData();
    }

    #region Protected Methods used in constructor

     // Prepare data to be used in Solve()
    protected override void PreProcessInputs() 
    {        
         // Create and populate the followings:
         // 1) DateDfFromDepo
         // 2) DateDfIniGuess 
         // 3) OnlyGivenSwap

         // initialise some containers stored as Data member
        PreProcessedData = new SortedList<double, double>();  // SortedList serial date as key, df as value coming only from depo. 
        IniGuessData = new SortedList<double, double>(); // serial date as key, df as value coming only from swap. Ini Guess for iterator

         // Df on ref date is 1
        PreProcessedData.Add(refDate.SerialValue, interpAdapter.FromDfToInterp(1.0, refDate.SerialValue));

         // Populate DateDfFromDepo and DateDfIniGuess (Iterate over BBArray). 
        foreach (BuildingBlock b in BBArray)
        {
            if (b.GetType().BaseType == typeof(OnePaymentStyle))  // if OnePaymentStyle (i.e depo)
            {
                 // calculate serial date and df and populate DepoDateDf
                double endDate = (((EurDepo)b).endDate).SerialValue;               
                double yf = refDate.YF(((EurDepo)b).endDate, ((EurDepo)b).dayCount); // YF
                double df = Formula.DFsimple(yf, ((EurDepo)b).rateValue); // DF
                                
                PreProcessedData.Add(endDate, interpAdapter.FromDfToInterp(df,endDate)); // PrePocessed Data (serial date as Key, value adapted df)
            }
            else if (b.GetType().BaseType == typeof(SwapStyle))  // if a SwapStyle
            {                
                Date last = ((SwapStyle)b).scheduleLeg2.payDates.Last(); // get the expiry of swap
                double yf = refDate.YF_365(last); // yf
                double df =Math.Exp(-yf * ((SwapStyle)b).rateValue);  // as starting guess  I suppose df = exp(-rt) , I use Swap rate as r introducing error
               
                KeyValuePair<double,double> kv =  // (serial date as Key, value adapted df)
                    new KeyValuePair<double,double>(last.SerialValue,interpAdapter.FromDfToInterp(df, last.SerialValue));

                 // Updating containers
                PreProcessedData.Add(kv.Key, kv.Value); // PrePocessed Data: serial date as Key, value adapted df                
                IniGuessData.Add(kv.Key, kv.Value);  // IniGuessData:  serial date as Key, value adapted df                              
            }
            else
            {
                throw new ArgumentException("error in building blocktype"); // if not depo or swap
            }
        }
    }
    
    protected override void Solve()
    {
        double[] x = IniGuessData.Values.ToArray();

        double epsg = 0.0000000001;  // original setting
        double epsf = 0;
        double epsx = 0;
        int maxits = 0;
        alglib.minlmstate state;
        alglib.minlmreport rep;

         // see alglib documentation
        alglib.minlmcreatev(x.Count(), x, 0.0001, out state);    
        alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
        alglib.minlmoptimize(state, function_fvec, null, null);
        alglib.minlmresults(state, out x, out rep);

        int i = rep.iterationscount;
    }

    protected override void PostProcessData() 
    {
         // not needed interpolator is already ok
    } 
    #endregion
    
     // delegate for swap function
    private delegate double SwapRate(SwapStyle S);  // used in function to calculate swap rate

     // used in Solve() function on which best fit works
    private void function_fvec(double[] x, double[] fi, object obj)
    {
         // fi is output should be vector of zeros (i.e. swap calculated - swap value from input)
        int N =x.Count(); // size of x (set of guess)
        
         // I update PreProcessedDate with new set of guess(x[]), for all Key (dates) in IniGuessData,  
        for (int i = 0; i<N; i++) 
        {
            PreProcessedData[IniGuessData.ElementAt(i).Key] = x[i];
        }

         // set up interpolator with updated data
        PostProcessInterpo.Ini(PreProcessedData.Keys.ToArray(), PreProcessedData.Values.ToArray());
              
         // Lambda expression: calculate Par Rate given a SwapStyle building block
        SwapRate SwapCalc = BB =>
        {            
             // fixed leg data
            double[] yfFixLeg = BB.scheduleLeg1.GetYFVect(BB.swapLeg1.DayCount); // fixed is leg 1

             // dfs array of fixed lag
            double[] dfDates = Date.GetSerialValue(BB.scheduleLeg1.payDates); // serial date of fixed lag (each dates we should find df)
             // Vector<double> k = PostProcessInterpo.Curve(new Vector<double>(dfDates, 0));

            double[] dfFixLeg = PostProcessInterpo.Curve(dfDates); // get interpolated value (i.e. log df or log r or...
             // transform interpolated value back to discount factor
            for (int i = 0; i < yfFixLeg.Length; i++)
            {
                dfFixLeg[i] = interpAdapter.FromInterpToDf(dfFixLeg[i],dfDates[i]);
            }
           
             // Interpolation Methods for Curve Construction PATRICK S. HAGAN & GRAEME WEST Applied Mathematical Finance,Vol. 13, No. 2, 89–129, June 2006
             // Formula 2) page 4 
            return Formula.ParRate(yfFixLeg, dfFixLeg); // Calculate par rate
        };
        
         // iterate building block and calculate difference between starting data and recalculated data: best fit if each fi[i]==0
        for (int i = 0; i < N; i++)
        {      
             // SwapCalc((SwapStyle)SwapStyleArray[i]) is recalculated data
             // SwapStyleArray[i].rateValue is starting data to match
            fi[i] = (SwapCalc((SwapStyle)OnlyGivenSwap[i]) - OnlyGivenSwap[i].rateValue) * 10000; // best fit if fi[i] ==0!, for each i
        }
    }

     // create an instance of class using a new RateSet
    public override ISingleRateCurve CreateInstance(RateSet newRateSet)
    {
        return new SingleCurveBuilderInterpBestFit<DoInterpOn, Interpolation>(newRateSet);
    }
}
  
 // Derived class class Interpolation And Bootstrap not two process (code comes from Single Curve Best fit
public class SingleCurveBuilderSmoothingFwd<DoInterpOn, Interpolation> : SingleCurveBuilder<DoInterpOn, Interpolation>
    where DoInterpOn : InterpAdapter, new()

    where Interpolation : BaseOneDimensionalInterpolator, new()
{
     // Data Member    
    protected double fixing;  // the first fixing (actual or expected) 
    private SortedList<double, double> DateDf;  // SerialDate as Key, DF as value, used in constructor to collect data coming from PreProcessedData

     // Used in Solve(). Note here I use simpler containers
     // / <Note>
     // / yfFloatLegLongerSwap and DatesDfLongerSwap could be contained in a SortedList (DatesDfLongerSwap as Key
     // / and yfFloatLegLongerSwap as value). But since the solve() method need many iteration.
     // / Using SortedList in solve() may slow the process. So I use simpler containers
     // / </Note>
    double[] yfFloatLegLongerSwap; // Year Fraction of floating leg of longer swap. It is needed to calculate DF
    double[] DatesDfLongerSwap; // Dates on which I calculate Df
    double[] fwdGuessLongerSwap; // fwd rate of longer swap
    int N; // number of fwd rate to find

     // Constructor, overwrite the fixing If you need a custom one (i.e. different from the one coming from RateSet
    public SingleCurveBuilderSmoothingFwd(RateSet rateSet, double firstFixing)
        : this(rateSet)
    {
        fixing = firstFixing; // overwrite the fixing
    }

     // Constructor
    public SingleCurveBuilderSmoothingFwd(RateSet rateSet)
        : base(rateSet)
    {
         // 1)Prepare data to be used in Solve() 
         // (outputs are: yfFloatLegLongerSwap,DatesDfLongerSwap,fwdGuessLongerSwap,N and partially DateDf )
        PreProcessInputs();

         // 2)Do Calculus: from PreProcessedData calculate post processed data (output is: DataDF)
        Solve();

         // 3)Inizialize final Interpolator: from post processed DataDF set up final interpolator (output is: PostProcessInterpo)
        PostProcessData();
    }

    #region Protected Methods used in constructor

     // Prepare data to be used in Solve()
    protected override void PreProcessInputs()
    {
        /*
         * 1) Check if input data are ok (data validation): from date contained 
         * 2) I choose the Longer Swap (LS)
         * 3) I get array of dates of payment of LS (pass it to data member)
         * 4) I get array of year fraction of LS (pass it to data member)
         */

         // From date of each fwd rate from longer swap(LS)
        List<double> FromDatesSerial = Date.GetSerialValue(OnlyGivenSwap.Last().scheduleLeg2.fromDates).ToList<double>();

         // validating underlying tenor: swap should be all vs the same tenor
        string UnderlyingTenor = ((SwapStyle)OnlyGivenSwap.First()).swapLeg2.UnderlyingRateTenor;
        
        foreach (SwapStyle b in OnlyGivenSwap)
        {
                // Check if contained
                if (! FromDatesSerial.Contains(((SwapStyle)b).scheduleLeg2.fromDates.Last().SerialValue))
                {
                    throw new ArgumentException("From date not contained");
                }      
        }
        
        DateDf = new SortedList<double, double>(); // Inizialize DateDF
        DateDf.Add(refDate.SerialValue, 1.0); // first discount factor
         // IEnumerable<KeyValuePair<double,double>>
        var f = from c in BBArray
                where c.GetType().BaseType == typeof(OnePaymentStyle)
                where c.endDate != refDate.add_period(UnderlyingTenor)
                let  yf = refDate.YF(c.endDate, c.dayCount) // year fraction
                let df = Formula.DFsimple(yf, c.rateValue) // calculate discount factor
                select new KeyValuePair<double, double>(c.endDate.SerialValue,df);

        foreach (KeyValuePair<double, double> kpv in f) 
        {
            DateDf.Add(kpv.Key, kpv.Value);
        }

         // Getting the fixing
        fixing = (from c in BBArray
                     where c.GetType().BaseType == typeof(OnePaymentStyle)
                     where c.endDate == refDate.add_period(UnderlyingTenor)
                     select c.rateValue).Single();


         // Inizialize some data member
        SwapStyle LongerSwap = OnlyGivenSwap.Last(); // Swap with longer maturity
         // Year Fraction of floating leg of longer swap. It is needed to calculate DF
        yfFloatLegLongerSwap = LongerSwap.scheduleLeg2.GetYFVect(LongerSwap.swapLeg2.DayCount); // floating leg is leg 2
        
         // Dates on which I calculate Df        
        DatesDfLongerSwap = Date.GetSerialValue(LongerSwap.scheduleLeg2.payDates);

         // number of fwd rate to find
        N = DatesDfLongerSwap.Length;

         // fwd rate of longer swap
        fwdGuessLongerSwap = new double[N];
        fwdGuessLongerSwap[0] = fixing;  // first is the fixing
    }

    protected override void Solve()
    {
         // this is initial guess x are fwd to be found, number of
         // x[] is array of forward rates, number of elements of x is = IniGuessData.Count-1, (minus 1 since the first is known. i.e the fixing)
        double[] x = Enumerable.Repeat(fixing, N - 1).ToArray();
    
        double epsg = 0.0000000001;  // original setting
        double epsf = 0;
        double epsx = 0;
        int maxits = 0;
        alglib.minlmstate state;
        alglib.minlmreport rep;

         // Number of equation to match: OnlyGivenSwap.Count() (number of swaps rate to match) + 1 (smoothness condition on forward)
        int NConstrains = OnlyGivenSwap.Length + 1;

         // see alglib documentation
        alglib.minlmcreatev(NConstrains, x, 0.0001, out state); 
        alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
        alglib.minlmoptimize(state, function_fvec, null, null);
        alglib.minlmresults(state, out x, out rep);

         // populate list choice of fwd guess
        List<double> optimizedFwd = new List<double>(x);  // add fwd found
        optimizedFwd.Insert(0, fixing); // add starting fixing
         // from forward rate I get df
        double[] Df = Formula.DFsimpleVect(yfFloatLegLongerSwap, optimizedFwd.ToArray());
        
         // populate DateDf using df from optimized fwd. DateDf is sorted ascending keys so no problem if
         // adding new item with a shorted date than the one already contained
        int iMax = DatesDfLongerSwap.Count();
        for (int i = 0; i < iMax; i++) 
        {
            DateDf.Add(DatesDfLongerSwap[i], Df[i]);
        }

        /*  
        int i = rep.iterationscount;
        System.Console.WriteLine("{0}", rep.iterationscount);  // number of iteration
         */         
    }

    protected override void PostProcessData()
    {
         // adapt DF from DateDf to PostProcessInterpo according to <DoInterpOn>
        IEnumerable<double> adaptedDF = from c in DateDf
                                        select interpAdapter.FromDfToInterp(c.Value, c.Key);

         // Inizialize PostProcesInterpo
        PostProcessInterpo.Ini(DateDf.Keys, adaptedDF.ToArray());
    }

    #endregion

     // delegate for swap function
    private delegate double SwapRate(SwapStyle S);  // used in function to calculate swap rate

     // used in Solve() function on which best fit works
    private void function_fvec(double[] x, double[] fi, object obj)
    {
         // This method will be repeated in iteration, to avoid to be too slow be careful with code 
         // fi is output should be vector of zeros (i.e. swap calculated - swap from inputs, and the last is the condition of smoothness)
    
         // I update PreProcessedDate with new set of guess(x[]), I calculate a array of corresponding df
         Array.Copy(x, 0, fwdGuessLongerSwap, 1, N-1);
         // According to fwdGuessLongerSwap I calculate df of floating leg of longer swap.
         // It will contain all df of shorter swap
        double[] dfFloatLegLongerSwap = Formula.DFsimpleVect(yfFloatLegLongerSwap, fwdGuessLongerSwap);
        
         // Lambda expression: calculate par rate        
        SwapRate SwapCalc = BB =>
        {            
             // fixed leg data
             // dfs array of fixed lag of BB specific swap
            double[] yfFixLeg = BB.scheduleLeg1.GetYFVect(BB.swapLeg1.DayCount); // fixed is leg 1
            double[] dfDates = Date.GetSerialValue(BB.scheduleLeg1.payDates); // serial date of fixed lag (each dates we should find df)                        
            double[] dfFixLeg = new double[dfDates.Count()];  // will contain df of fixed leg of BB swap
            
            Interpolation I = new Interpolation();  // Post process interpolator
            I.Ini(DatesDfLongerSwap, dfFloatLegLongerSwap);

             // transform interpolated value back to discount factor
            for (int i = 0; i < dfDates.Count(); i++)
            {
                dfFixLeg[i] = I.Solve(dfDates[i]);
            }           
            return Formula.ParRate(yfFixLeg, dfFixLeg); // Calculate par rate
        };
        
         // iterate building block
        for (int i = 0; i < fi.Count() -1 ; i++)
        {   
             // Calculate difference from calculated rate and input rate. It should be zero at end of optimization 
            fi[i] = (SwapCalc(OnlyGivenSwap[i]) - (OnlyGivenSwap[i]).rateValue) * 10000;
        }
        
         // Last constrain to match: smoothness condition: sum of square of consecutive fwd 
        double sq = 0.0; // sum of square
        for (int i = 0; i < N - 1; i++)
        {
            sq += Math.Pow(fwdGuessLongerSwap[i] - fwdGuessLongerSwap[i + 1], 2);
        }
         // last element
        fi[fi.Count()-1] = sq * 10000.0; // Sum of square should be zero
    }

     // create an instance of class using a new RateSet
    public override ISingleRateCurve CreateInstance(RateSet newRateSet)
    {
        return new SingleCurveBuilderSmoothingFwd<DoInterpOn, Interpolation>(newRateSet);
    }
}
