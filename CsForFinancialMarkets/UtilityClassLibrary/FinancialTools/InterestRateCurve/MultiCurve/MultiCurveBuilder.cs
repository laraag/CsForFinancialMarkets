// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// MultiCurveBuilder.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

 // abstract class 
public class MultiCurveBuilder<Interpolation> : IMultiRateCurve
    where Interpolation : BaseOneDimensionalInterpolator, new()
{
     // Data Member
    public Date refDate;  // reference date of the curve
    public IEnumerable<BuildingBlock> BBArray;  // array of building block, sorted ascending maturity
    protected SwapStyle[] OnlyGivenSwap;  // OnlyGivenSwap from BBArray
    public SwapStyle SwapType;  // it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
    public RateSet mktRateSet;  // market starting data
    protected ISingleRateCurve DCurve;  // curve used in discounting
    Interpolation FWDInterpolator;  // the interpolator in fwd
    protected double fixing;  // the first fixing (actual or expected) 
    double[] FromDatesSerial;
     // Constructor
    public MultiCurveBuilder(RateSet rateSetMC, ISingleRateCurve DiscountingCurve) 
    {
        PrePorcessData(rateSetMC, DiscountingCurve);
        Solve();
    }

    #region UsedInConstructor

    protected void PrePorcessData(RateSet rateSetMC, ISingleRateCurve DiscountingCurve)
    {
        this.refDate = rateSetMC.refDate;
        this.DCurve = DiscountingCurve;  // my curve used in discounting
        FWDInterpolator = new Interpolation();  // Interpolator used in fwd
        this.mktRateSet = rateSetMC;  // pass market rate set

         // Create Building block
        IEnumerable<BuildingBlock> BB = mktRateSet.GetArrayOfBB();

         // Sort ascending end date
        BBArray = from c in BB
                  orderby c.endDate.SerialValue ascending
                  select c;

         // Only Given Swap from BBArray        
        OnlyGivenSwap = (from c in BBArray
                         where c.GetType().BaseType == typeof(SwapStyle)                         
                         select (SwapStyle)c).ToArray();
              
         // Validating underlying tenor: swap should be all vs the same tenor
        string UnderlyingTenor = ((SwapStyle)OnlyGivenSwap.First()).swapLeg2.UnderlyingRateTenor;

         // Getting the fixing
        fixing = (from c in BBArray
                  where c.GetType().BaseType == typeof(OnePaymentStyle)
                  where c.endDate == refDate.add_period(UnderlyingTenor)
                  select c.rateValue).Single();

         // From date of each fwd rate from longer swap(LS)
         // FromDatesSerial = Date.GetSerialValue(OnlyGivenSwap.Last().scheduleLeg2.fromDates);
        List<double> SerialDate = (from c in OnlyGivenSwap
                                   select c.scheduleLeg2.fromDates.Last().SerialValue).ToList<double>();

         // adding reference date at beginning (this is important since I use the fixing)
        SerialDate.Insert(0, refDate.SerialValue);
        FromDatesSerial = SerialDate.ToArray();

         // some data validation: swap should be all of same building block

         // the type of building block
        BuildingBlockType BBT = OnlyGivenSwap[0].buildingBlockType;

         // Are all them the same? 
        bool IsSameSwapType = OnlyGivenSwap.All(s => s.buildingBlockType == BBT);

        if (IsSameSwapType)  // if true
        {
             // it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
            SwapType = (SwapStyle)new BuildingBlockFactory().CreateEmptyBuildingBlock(BBT);
        }
        else
        {
            throw new ArgumentException("error in building blocktype"); // if not throw an exception
        }      
    }

    protected void Solve() 
    {
         // this is initial guess x are fwd to be found, number of
         // x[] is array of fwds rates, number of elements of x is = IniGuessData.Count-1, (minus 1 since the first is known. i.e the fixing)
        double[] x = Enumerable.Repeat(fixing, OnlyGivenSwap.Count()+1).ToArray();
        
        double epsg = 0.0000000001;  // original setting
        double epsf = 0;
        double epsx = 0;
        int maxits = 0;
        alglib.minlmstate state;
        alglib.minlmreport rep;

         // Number of equation to match: OnlyGivenSwap.Count() (number of swaps rate to match) + 1 (fixing)
        int NConstrains = OnlyGivenSwap.Length+1;

         // see alglib documentation
        alglib.minlmcreatev(NConstrains, x, 0.0001, out state); 
        alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
        alglib.minlmoptimize(state, function_fvec, null, null);
        alglib.minlmresults(state, out x, out rep);        
    }
    
     // delegate of swap function
    private delegate double SwapRate(SwapStyle S);  // used in function to calculate swap rate

    private double ParRate(SwapStyle S)
    {    // floating leg data
        double[] yfFloatLeg = S.scheduleLeg2.GetYFVect(S.swapLeg2.DayCount);  // floating leg is leg 2
        double[] dfFloatLeg = (from c in S.scheduleLeg2.payDates
                               select DCurve.Df(c)).ToArray();
        double[] fwdFloatLeg = (from c in S.scheduleLeg2.fromDates
                                select FWDInterpolator.Solve(c.SerialValue)).ToArray();
         // fixed leg data
        double[] yfFixLeg = S.scheduleLeg1.GetYFVect(S.swapLeg1.DayCount);  // fixed is leg 1
        double[] dfFixLeg = (from c in S.scheduleLeg1.payDates
                             select DCurve.Df(c)).ToArray();

         // calculate par swap rate according to given data
        return Formula.ParRateFormula(yfFloatLeg, dfFloatLeg, fwdFloatLeg, yfFixLeg, dfFixLeg);
    }

    public void function_fvec(double[] x, double[] fi, object obj)
    {
         // set up interpolator
        FWDInterpolator.Ini(FromDatesSerial,x);

         // Lambda expression
        SwapRate SwapCalc = BB =>
        {
             // floating leg data
            double[] yfFloatLeg = BB.scheduleLeg2.GetYFVect(BB.swapLeg2.DayCount);  // floating leg is leg 2
            double[] dfFloatLeg = (from c in BB.scheduleLeg2.payDates
                                   select DCurve.Df(c)).ToArray();            
            double[] fwdFloatLeg = (from c in BB.scheduleLeg2.fromDates
                                    select FWDInterpolator.Solve(c.SerialValue)).ToArray();

             // fixed leg data
            double[] yfFixLeg = BB.scheduleLeg1.GetYFVect(BB.swapLeg1.DayCount);  // fixed is leg 1
            double[] dfFixLeg = (from c in BB.scheduleLeg1.payDates
                                   select DCurve.Df(c)).ToArray();
            
             // calculate par swap rate according to given data
            return Formula.ParRateFormula(yfFloatLeg, dfFloatLeg, fwdFloatLeg, yfFixLeg, dfFixLeg);
        };

         // starting fixing should be match
        fi[fi.Length-1] = (fixing - x[0]) * 10000;

         // iterate building block
        for (int i = 0; i < OnlyGivenSwap.Length; i++)
        {
            fi[i] = (SwapCalc(OnlyGivenSwap[i]) - OnlyGivenSwap[i].rateValue) * 10000;
        }      
    }
    #endregion

    #region IRateCurve implementation
    
     // return reference date
    public Date RefDate() { return this.refDate; }
    
     // return discount factor for TargetDate
    public double Df(Date TargetDate) { return DCurve.Df(TargetDate); }

     // return forward rate starting on StartDate for a tenor defined directly in the class
    public double Fwd(Date StartDate) { return FWDInterpolator.Solve(StartDate.SerialValue); }

     // return forward start swap as same building block used in building curve recalculated starting on custom StartDate, Tenor is the tenor of swap
    public double SwapFwd(Date StartDate, string Tenor) 
    {
         // 1. Build the swap. Rate is not important I use 0.0
        SwapStyle myS = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(StartDate, 0.0, Tenor, SwapType.buildingBlockType);
        return ParRate(myS); 
    }

     // return SwapStyle used for bootstrapping, it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
    public SwapStyle GetSwapStyle() { return this.SwapType; }
    #endregion

    #region IMultiRateCurve implementation
     // I use TLP: return array of curves, initialised after shifting  mktRateSet elements of discounting curve
    public IMultiRateCurve[] ShiftedCurvesArrayDCurve(double shift) 
    {
         // i-discount curve is built using its setup (interpolator,..), but shifting i-element of mktRateSet of discounting curve
         // up of 'shift' quantity  
        ISingleRateCurve[] dC =DCurve.ShiftedCurveArray(shift);
        int n = dC.Length;
        IMultiRateCurve[] outPut = new IMultiRateCurve[n];
        Parallel.For(0, n, i =>
         {
             outPut[i] = CreateInstance(mktRateSet, dC[i]);
         });

        return outPut;  // array of curves   
    }

     // return one only curve, initialised after shifting all mktRateSet elements, of discounting curve, up of 'shift' quantity, once at the same time
    public IMultiRateCurve ParallelShiftDCurve(double shift) 
    {       
        return CreateInstance(mktRateSet,DCurve.ParallelShift(shift));  // parallel shift in discount curve 
    }

     // I use TLP: return array of curves, initialised after shifting  mktRateSet elements of forwarding curve
    public IMultiRateCurve[] ShiftedCurvesArrayFwdCurve(double shift)
    {
         // i-curve is built using its setup (interpolator,..), but shifting i-element of mktRateSet
         // up of 'shift' quantity
        RateSet[] rsArr = mktRateSet.ShiftedRateSetArray(shift);  // array of shifted RateSet (my scenario)
        int n = rsArr.Length;  // number of elements
        IMultiRateCurve[] curves = new IMultiRateCurve[n];  // array of curves
        Parallel.For(0, n, i =>
        {
            curves[i] = CreateInstance(rsArr[i], DCurve);  // build the correct curve
        });

        return curves;
    }
   
     // return one only curve, initialised after shifting all mktRateSet elements up of 'shift' quantity, once at the same time
    public IMultiRateCurve ParallelShiftFwdCurve(double shift)
    {
        RateSet rs = mktRateSet.ParallelShiftRateSet(shift);  // parallel shift
        return CreateInstance(rs,DCurve);  // build the correct curve
    }  
    #endregion

    #region SequentialVersion
     // return array of curves, initialised after shifting  mktRateSet elements of discounting curve
    public IMultiRateCurve[] ShiftedCurvesArrayDCurveSeq(double shift)
    {
         // i-discount curve is built using its setup (interpolator,..), but shifting i-element of mktRateSet of discounting curve
         // up of 'shift' quantity  
        ISingleRateCurve[] dC = DCurve.ShiftedCurveArray(shift);
        int n = dC.Length;
        IMultiRateCurve[] outPut = new IMultiRateCurve[n];
        for (int i = 0; i < n; i++)
        {
            outPut[i] = CreateInstance(mktRateSet, dC[i]);
        }

        return outPut;  // array of curves  
    }

     // return array of curves, initialised after shifting  mktRateSet elements of forwarding curve
    public IMultiRateCurve[] ShiftedCurvesArrayFwdCurveSeq(double shift)
    {
         // i-curve is built using its setup (interpolator,..), but shifting i-element of mktRateSet
         // up of 'shift' quantity
        RateSet[] rsArr = mktRateSet.ShiftedRateSetArray(shift);  // array of shifted RateSet (my scenario)
        int n = rsArr.Length;  // number of elements
        IMultiRateCurve[] curves = new IMultiRateCurve[n];  // array of curves
        for (int i = 0; i < n; i++)
        {    // iterate to build all curve needed
            curves[i] = CreateInstance(rsArr[i], DCurve);  // build the correct curve
        }

        return curves;
    }
    #endregion

    public IMultiRateCurve CreateInstance(RateSet newRateSet, ISingleRateCurve newDiscountingCurve) 
    {
        return new MultiCurveBuilder<Interpolation>(newRateSet, newDiscountingCurve);
    }    
}


 // Different implementation with weaker performance than MultiCurveBuilder
public class MultiCurveBuilder2<Interpolation> : IMultiRateCurve  // IRateCurve
    where Interpolation : BaseOneDimensionalInterpolator, new()
{
     // Data Member
    public Date refDate;  // reference date of the curve
    public IEnumerable<BuildingBlock> BBArray;  // array of building block, sorted ascending maturity
    protected SwapStyle[] OnlyGivenSwap;  // Only Given Swap from BBArray
    public SwapStyle SwapType;  // it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
    public RateSet mktRateSet;  // market starting data
    ISingleRateCurve DCurve;  // curve used in discounting
    Interpolation FWDInterpolator;  // the interpolator in fwd  
    protected BuildingBlock fixing;  // the first fixing (actual or expected) 
    double[] ToDatesSerial;
    
     // Constructor
    public MultiCurveBuilder2(RateSet rateSetMC, ISingleRateCurve DiscountingCurve)
    {
        PrePorcessData(rateSetMC, DiscountingCurve);
        Solve();
    }

    #region UsedInConstructor

    protected void PrePorcessData(RateSet rateSetMC, ISingleRateCurve DiscountingCurve)
    {
        this.refDate = rateSetMC.refDate;
        this.DCurve = DiscountingCurve;  // my curve used in discounting
        FWDInterpolator = new Interpolation();  // Interpolator used in fwd
        this.mktRateSet = rateSetMC;  // pass market rate set
    
         // Create Building block
        IEnumerable<BuildingBlock> BB = mktRateSet.GetArrayOfBB();

         // Sort ascending end date
        BBArray = from c in BB
                  orderby c.endDate.SerialValue ascending
                  select c;

         // Only Given Swap from BBArray        
        OnlyGivenSwap = (from c in BBArray
                         where c.GetType().BaseType == typeof(SwapStyle)
                         select (SwapStyle)c).ToArray();

         // validating underlying tenor: swap should be all vs the same tenor
        string UnderlyingTenor = ((SwapStyle)OnlyGivenSwap.First()).swapLeg2.UnderlyingRateTenor;

         // Getting the fixing
        fixing = (from c in BBArray
                  where c.GetType().BaseType == typeof(OnePaymentStyle)
                  where c.endDate == refDate.add_period(UnderlyingTenor)
                  select c).Single();

         // From date of each fwd rate from longer swap(LS)
        List<double> SerialDate = (from c in OnlyGivenSwap
                                   select c.scheduleLeg2.toDates.Last().SerialValue).ToList<double>();

         // adding  date at beginning (this is important since I use the fixing)
        SerialDate.Insert(0, refDate.SerialValue);
        SerialDate.Insert(1, refDate.add_period(UnderlyingTenor).SerialValue);
        ToDatesSerial = SerialDate.ToArray();

         // some data validation: swap should be all of same building block

         // the type of building block
        BuildingBlockType BBT = OnlyGivenSwap[0].buildingBlockType;

         // Are all them the same? 
        bool IsSameSwapType = OnlyGivenSwap.All(s => s.buildingBlockType == BBT);

        if (IsSameSwapType)  // if true
        {
             // it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
            SwapType = (SwapStyle)new BuildingBlockFactory().CreateEmptyBuildingBlock(BBT);
        }
        else
        {
            throw new ArgumentException("error in building blocktype");  // if not throw an exception
        }
    }

    protected void Solve()
    {
         // this is initial guess x are fwd to be found, number of
         // x[] is array of fwds rates, number of elements of x is = IniGuessData.Count-1, (minus 1 since the first is known. i.e the fixing)
        double firstGuess =   1.0 / (1.0 + (0.5 * fixing.rateValue)); // brutal approx
        double[] x = Enumerable.Repeat(Math.Log(firstGuess), OnlyGivenSwap.Count() +2).ToArray();

        double epsg = 0.0000000001;  // original setting
        double epsf = 0;
        double epsx = 0;
        int maxits = 0;
        alglib.minlmstate state;
        alglib.minlmreport rep;

         // Number of equation to match: OnlyGivenSwap.Count() (number of swaps rate to match) + 1 (fixing)
        int NConstrains = OnlyGivenSwap.Length + 2;

         // see alglib documentation
        alglib.minlmcreatev(NConstrains, x, 0.0001, out state); 
        alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
        alglib.minlmoptimize(state, function_fvec, null, null);
        alglib.minlmresults(state, out x, out rep);
    }

     // delegate of swap function
    private delegate double SwapRate(SwapStyle S);  // used in function to calculate swap rate

    private double ParRate(SwapStyle S)
    {   
         // floating leg data
        double[] yfFloatLeg = S.scheduleLeg2.GetYFVect(S.swapLeg2.DayCount);  // floating leg is leg 2
        double[] dfFloatLeg = (from c in S.scheduleLeg2.payDates
                               select DCurve.Df(c)).ToArray();
        double[] fwdFloatLeg = (from c in S.scheduleLeg2.fromDates
                                select FWDInterpolator.Solve(c.SerialValue)).ToArray();

         // fixed leg data
        double[] yfFixLeg = S.scheduleLeg1.GetYFVect(S.swapLeg1.DayCount);  // fixed is leg 1
        double[] dfFixLeg = (from c in S.scheduleLeg1.payDates
                             select DCurve.Df(c)).ToArray();

         // calculate par swap rate according to given data
        return Formula.ParRateFormula(yfFloatLeg, dfFloatLeg, fwdFloatLeg, yfFixLeg, dfFixLeg);
    }

    public void function_fvec(double[] x, double[] fi, object obj)
    {
         // set up interpolator
        FWDInterpolator.Ini(ToDatesSerial, x);

         // Lambda expression
        SwapRate SwapCalc = BB =>
        {
             // floating leg data
            double[] yfFloatLeg = BB.scheduleLeg2.GetYFVect(BB.swapLeg2.DayCount);  // floating leg is leg 2
            double[] dfFloatLeg = (from c in BB.scheduleLeg2.payDates
                                   select DCurve.Df(c)).ToArray();
            double[] df_end = (from c in BB.scheduleLeg2.toDates
                             select Math.Exp(FWDInterpolator.Solve(c.SerialValue))).ToArray();
            double[] df_ini = (from c in BB.scheduleLeg2.fromDates
                               select Math.Exp(FWDInterpolator.Solve(c.SerialValue))).ToArray();

            int n =yfFloatLeg.Length;
            double[] fwdFloatLeg = new double[n];
            for (int i = 0; i < n; i++) 
            {
                fwdFloatLeg[i] = (df_ini[i] / df_end[i] - 1) / yfFloatLeg[i];
            }

             // fixed leg data
            double[] yfFixLeg = BB.scheduleLeg1.GetYFVect(BB.swapLeg1.DayCount);  // fixed is leg 1
            double[] dfFixLeg = (from c in BB.scheduleLeg1.payDates
                                 select DCurve.Df(c)).ToArray();
            
             // calculate par swap rate according to given data
            return Formula.ParRateFormula(yfFloatLeg, dfFloatLeg, fwdFloatLeg, yfFixLeg, dfFixLeg);
        };        

         // iterate building block
        for (int i = 0; i < OnlyGivenSwap.Length; i++)
        {
            fi[i] = (SwapCalc(OnlyGivenSwap[i]) - OnlyGivenSwap[i].rateValue) * 10000;
        }

         // starting fixing should be match
        fi[fi.Length - 1] = (fixing.rateValue - Fwd(refDate)) * 10000;

         // starting fixing should be match
        fi[fi.Length - 2] = Math.Exp(FWDInterpolator.Solve(refDate.SerialValue)) * 10000;  // initial df =0!
    }
    #endregion

    #region IRateCurve implementation
     // return reference date
    public Date RefDate() { return this.refDate; }

     // return discount factor for TargetDate
    public double Df(Date TargetDate) { return DCurve.Df(TargetDate); }

     // return forward rate starting on StartDate for a tenor defined directly in the class
    public double Fwd(Date StartDate) 
    {
        Date end_date =StartDate.add_period("6m", false);
        double df_ini = Math.Exp(FWDInterpolator.Solve(StartDate.SerialValue));
        double df_end = Math.Exp(FWDInterpolator.Solve(end_date.SerialValue));
        double yf = StartDate.YF(end_date, fixing.dayCount);
        return  (df_ini / df_end - 1) / yf;       
    }

     // return forward start swap as same building block used in building curve recalculated starting on custom StartDate, Tenor is the tenor of swap
    public double SwapFwd(Date StartDate, string Tenor)
    {
         // 1. Build the swap. Rate is not important I use 0.0
        SwapStyle myS = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(StartDate, 0.0, Tenor, SwapType.buildingBlockType);
        return ParRate(myS);
    }

     // return SwapStyle used for bootstrapping, it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
    public SwapStyle GetSwapStyle() { return this.SwapType; }
    #endregion

    #region IMultiRateCurve implementation
     // return array of curves, initialised after shifting  mktRateSet elements of discounting curve
    public IMultiRateCurve[] ShiftedCurvesArrayDCurve(double shift)
    {
         // i-discount curve is built using its setup (interpolator,..), but shifting i-element of mktRateSet of discounting curve
         // up of 'shift' quantity  
        ISingleRateCurve[] dC = DCurve.ShiftedCurveArray(shift);
        int n = dC.Length;
        IMultiRateCurve[] outPut = new IMultiRateCurve[n];
        for (int i = 0; i < n; i++)
        {
            outPut[i] = CreateInstance(mktRateSet, dC[i]);
        }

        return outPut;  // array of curves  
    }

     // return one only curve, initialised after shifting all mktRateSet elements, of discounting curve, up of 'shift' quantity, once at the same time
    public IMultiRateCurve ParallelShiftDCurve(double shift)
    {
        return CreateInstance(mktRateSet, DCurve.ParallelShift(shift));  // parallel shift in discount curve 
    }

     // return array of curves, initialised after shifting  mktRateSet elements of forwarding curve
    public IMultiRateCurve[] ShiftedCurvesArrayFwdCurve(double shift)
    {
         // i-curve is built using its setup (interpolator,..), but shifting i-element of mktRateSet
         // up of 'shift' quantity
        RateSet[] rsArr = mktRateSet.ShiftedRateSetArray(shift);  // array of shifted RateSet (my scenario)
        int n = rsArr.Length;  // number of elements
        IMultiRateCurve[] curves = new IMultiRateCurve[n];  // array of curves
        for (int i = 0; i < n; i++)
        {  // iterate to build all curve needed
            curves[i] = CreateInstance(rsArr[i], DCurve); // build the correct curve
        }
        return curves;
    }

     // return one only curve, initialised after shifting all mktRateSet elements up of 'shift' quantity, once at the same time
    public IMultiRateCurve ParallelShiftFwdCurve(double shift)
    {
        RateSet rs = mktRateSet.ParallelShiftRateSet(shift);  // parallel shift
        return CreateInstance(rs, DCurve);  // build the correct curve
    }
    #endregion
    
    public IMultiRateCurve CreateInstance(RateSet newRateSet, ISingleRateCurve newDiscountingCurve)
    {
        return new MultiCurveBuilder<Interpolation>(newRateSet, newDiscountingCurve);
    }
}

public class MultiCurveBuilder3<Interpolation> : IMultiRateCurve   
    where Interpolation : BaseOneDimensionalInterpolator, new()
{
     // Data Member
    public Date refDate;  // reference date of the curve
    public IEnumerable<BuildingBlock> BBArray;  // array of building block, sorted ascending maturity
    protected SwapStyle[] OnlyGivenSwap;  // OnlyGivenSwap from BBArray
    public SwapStyle SwapType; // it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
    public RateSet mktRateSet; // market starting data
    protected ISingleRateCurve DCurve;  // curve used in discounting
    Interpolation FWDInterpolator;  // the interpolator in fwd
    protected double fixing;  // the first fixing (actual or expected) 
    double[] FromDatesSerial;

     // Constructor
    public MultiCurveBuilder3(RateSet rateSetMC, ISingleRateCurve DiscountingCurve)
    {
        PrePorcessData(rateSetMC, DiscountingCurve);
        Solve();
    }

    #region UsedInConstructor

    protected void PrePorcessData(RateSet rateSetMC, ISingleRateCurve DiscountingCurve)
    {
        this.refDate = rateSetMC.refDate;
        this.DCurve = DiscountingCurve;  // my curve used in discounting
        FWDInterpolator = new Interpolation();  // Interpolator used in fwd
        this.mktRateSet = rateSetMC;  // pass market rate set

         // Create Building block
        IEnumerable<BuildingBlock> BB = mktRateSet.GetArrayOfBB();

         // Sort ascending end date
        BBArray = from c in BB
                  orderby c.endDate.SerialValue ascending
                  select c;

         // Only Given Swap from BBArray        
        OnlyGivenSwap = (from c in BBArray
                         where c.GetType().BaseType == typeof(SwapStyle)
                         select (SwapStyle)c).ToArray();


         // validating underlying tenor: swap should be all vs the same tenor
        string UnderlyingTenor = ((SwapStyle)OnlyGivenSwap.First()).swapLeg2.UnderlyingRateTenor;

         // Getting the fixing
        fixing = (from c in BBArray
                  where c.GetType().BaseType == typeof(OnePaymentStyle)
                  where c.endDate == refDate.add_period(UnderlyingTenor)
                  select c.rateValue).Single();

         // From date of each fwd rate from longer swap(LS)
         // FromDatesSerial = Date.GetSerialValue(OnlyGivenSwap.Last().scheduleLeg2.fromDates);
        List<double> SerialDate = (from c in OnlyGivenSwap
                                   select c.scheduleLeg2.fromDates.Last().SerialValue).ToList<double>();

         // adding reference date at beginning (this is important since I use the fixing)
        SerialDate.Insert(0, refDate.SerialValue);
        FromDatesSerial = SerialDate.ToArray();

         // some data validation: swap should be all of same building block

         // the type of building block
        BuildingBlockType BBT = OnlyGivenSwap[0].buildingBlockType;

         // Are all them the same? 
        bool IsSameSwapType = OnlyGivenSwap.All(s => s.buildingBlockType == BBT);

        if (IsSameSwapType)  // if true
        {
             // it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
            SwapType = (SwapStyle)new BuildingBlockFactory().CreateEmptyBuildingBlock(BBT);
        }
        else
        {
            throw new ArgumentException("error in building blocktype"); // if not throw an exception
        }
    }

    protected void Solve()
    {
         // this is initial guess x are fwd to be found, number of
         // x[] is array of fwds rates, number of elements of x is = IniGuessData.Count-1, (minus 1 since the first is known. i.e the fixing)
        double[] x = Enumerable.Repeat(fixing, OnlyGivenSwap.Count() + 1).ToArray();
        
        double epsg = 0.0000000001;  // original setting
        double epsf = 0;
        double epsx = 0;
        int maxits = 0;
        alglib.minlmstate state;
        alglib.minlmreport rep;

         // Number of equation to match: OnlyGivenSwap.Count() (number of swaps rate to match) + 1 (fixing)
        int NConstrains = OnlyGivenSwap.Length + 1;

         // see alglib documentation
        alglib.minlmcreatev(NConstrains, x, 0.0001, out state);
        alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
        alglib.minlmoptimize(state, function_fvec, null, null);
        alglib.minlmresults(state, out x, out rep);
    }

     // delegate of swap function
    private delegate double SwapRate(SwapStyle S);  // used in function to calculate swap rate

    private double ParRate(SwapStyle S)
    {    // floating leg data
        double[] yfFloatLeg = S.scheduleLeg2.GetYFVect(S.swapLeg2.DayCount);  // floating leg is leg 2
        double[] dfFloatLeg = (from c in S.scheduleLeg2.payDates
                               select DCurve.Df(c)).ToArray();
        double[] fwdFloatLeg = (from c in S.scheduleLeg2.fromDates
                                select FWDInterpolator.Solve(c.SerialValue)).ToArray();

         // fixed leg data
        double[] yfFixLeg = S.scheduleLeg1.GetYFVect(S.swapLeg1.DayCount);  // fixed is leg 1
        double[] dfFixLeg = (from c in S.scheduleLeg1.payDates
                             select DCurve.Df(c)).ToArray();
        
         // calculate par swap rate according to given data
        return Formula.ParRateFormula(yfFloatLeg, dfFloatLeg, fwdFloatLeg, yfFixLeg, dfFixLeg);
    }

    public void function_fvec(double[] x, double[] fi, object obj)
    {
         // set up interpolator
        FWDInterpolator.Ini(FromDatesSerial, x);

         // Lambda expression
        SwapRate SwapCalc = BB =>
        {
             // floating leg data
            double[] yfFloatLeg = BB.scheduleLeg2.GetYFVect(BB.swapLeg2.DayCount);  // floating leg is leg 2
            double[] dfFloatLeg = (from c in BB.scheduleLeg2.payDates
                                   select DCurve.Df(c)).ToArray();
            double[] fwdFloatLeg = (from c in BB.scheduleLeg2.fromDates
                                    select FWDInterpolator.Solve(c.SerialValue)).ToArray();

             // fixed leg data
            double[] yfFixLeg = BB.scheduleLeg1.GetYFVect(BB.swapLeg1.DayCount);  // fixed is leg 1
            double[] dfFixLeg = (from c in BB.scheduleLeg1.payDates
                                 select DCurve.Df(c)).ToArray();

             // calculate par swap rate according to given data
            return Formula.ParRateFormula(yfFloatLeg, dfFloatLeg, fwdFloatLeg, yfFixLeg, dfFixLeg);
        };

         // starting fixing should be match
        fi[fi.Length - 1] = (fixing - x[0]) * 10000;

         // iterate building block
        for (int i = 0; i < OnlyGivenSwap.Length; i++)
        {
            fi[i] = (SwapCalc(OnlyGivenSwap[i]) - OnlyGivenSwap[i].rateValue) * 10000;
        }
    }    
    #endregion

    #region IRateCurve implementation

     // return reference date
    public Date RefDate() { return this.refDate; }

     // return discount factor for TargetDate
    public double Df(Date TargetDate) { return DCurve.Df(TargetDate); }

     // return forward rate starting on StartDate for a tenor defined directly in the class
    public double Fwd(Date StartDate) { return FWDInterpolator.Solve(StartDate.SerialValue); }

     // return forward start swap as same building block used in building curve recalculated starting on custom StartDate, Tenor is the tenor of swap
    public double SwapFwd(Date StartDate, string Tenor)
    {
         // 1. Build the swap. Rate is not important I use 0.0
        SwapStyle myS = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(StartDate, 0.0, Tenor, SwapType.buildingBlockType);
        return ParRate(myS);
    }

     // return SwapStyle used for bootstrapping, it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
    public SwapStyle GetSwapStyle() { return this.SwapType; }
    #endregion

    #region IMultiRateCurve implementation
     // I use TLP: return array of curves, initialised after shifting  mktRateSet elements of discounting curve
    public IMultiRateCurve[] ShiftedCurvesArrayDCurve(double shift)
    {
         // i-discount curve is built using its setup (interpolator,..), but shifting i-element of mktRateSet of discounting curve
         // up of 'shift' quantity  
        ISingleRateCurve[] dC = DCurve.ShiftedCurveArray(shift);
        int n = dC.Length;
        IMultiRateCurve[] outPut = new IMultiRateCurve[n];
        Parallel.For(0, n, i =>
        {
            outPut[i] = CreateInstance(mktRateSet, dC[i]);
        });

        return outPut;  // array of curves  
    }

     // return one only curve, initialised after shifting all mktRateSet elements, of discounting curve, up of 'shift' quantity, once at the same time
    public IMultiRateCurve ParallelShiftDCurve(double shift)
    {
        return CreateInstance(mktRateSet, DCurve.ParallelShift(shift));  // parallel shift in discount curve 
    }

     // I use TLP: return array of curves, initialised after shifting  mktRateSet elements of forwarding curve
    public IMultiRateCurve[] ShiftedCurvesArrayFwdCurve(double shift)
    {
         // i-curve is built using its setup (interpolator,..), but shifting i-element of mktRateSet
         // up of 'shift' quantity
        RateSet[] rsArr = mktRateSet.ShiftedRateSetArray(shift);  // array of shifted RateSet (my scenario)
        int n = rsArr.Length;  // number of elements
        IMultiRateCurve[] curves = new IMultiRateCurve[n];  // array of curves
        Parallel.For(0, n, i =>
        {
            curves[i] = CreateInstance(rsArr[i], DCurve);  // build the correct curve
        });

        return curves;
    }

     // return one only curve, initialised after shifting all mktRateSet elements up of 'shift' quantity, once at the same time
    public IMultiRateCurve ParallelShiftFwdCurve(double shift)
    {
        RateSet rs = mktRateSet.ParallelShiftRateSet(shift); // parallel shift
        return CreateInstance(rs, DCurve);  // build the correct curve
    }
    #endregion

    #region SequentialVersion

     // return array of curves, initialised after shifting  mktRateSet elements of discounting curve
    public IMultiRateCurve[] ShiftedCurvesArrayDCurveSeq(double shift)
    {
         // i-discount curve is built using its setup (interpolator,..), but shifting i-element of mktRateSet of discounting curve
         // up of 'shift' quantity  
        ISingleRateCurve[] dC = DCurve.ShiftedCurveArray(shift);
        int n = dC.Length;
        IMultiRateCurve[] outPut = new IMultiRateCurve[n];
        for (int i = 0; i < n; i++)
        {
            outPut[i] = CreateInstance(mktRateSet, dC[i]);
        }

        return outPut;  // array of curves  
    }

     // return array of curves, initialised after shifting  mktRateSet elements of forwarding curve
    public IMultiRateCurve[] ShiftedCurvesArrayFwdCurveSeq(double shift)
    {
         // i-curve is built using its setup (interpolator,..), but shifting i-element of mktRateSet
         // up of 'shift' quantity
        RateSet[] rsArr = mktRateSet.ShiftedRateSetArray(shift);  // array of shifted RateSet (my scenario)
        int n = rsArr.Length;  // number of elements
        IMultiRateCurve[] curves = new IMultiRateCurve[n];  // array of curves
        for (int i = 0; i < n; i++)
        { // iterate to build all curve needed
            curves[i] = CreateInstance(rsArr[i], DCurve);  // build the correct curve
        }
        return curves;
    }

    #endregion

    public IMultiRateCurve CreateInstance(RateSet newRateSet, ISingleRateCurve newDiscountingCurve)
    {
        return new MultiCurveBuilder<Interpolation>(newRateSet, newDiscountingCurve);
    }
}