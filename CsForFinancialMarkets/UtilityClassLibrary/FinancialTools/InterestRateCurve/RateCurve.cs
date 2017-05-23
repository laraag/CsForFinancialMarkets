// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// RateCurve.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IRateCurve
{
     // return reference date
    Date RefDate();

     // return discount factor for target date
    double Df(Date TargetDate);

     // return forward rate starting on StartDate for a tenor defined directly in the class
    double Fwd(Date StartDate);
   
     // return forward start swap as same building block used in building curve recalculated starting on custom StartDate, Tenor is the tenor of swap
    double SwapFwd(Date StartDate, string Tenor);

     // return SwapStyle used for bootstrapping, it is swap type used as inputs (i.e. EurSwapVs6m, EurSwapVs3m, ...)
    SwapStyle GetSwapStyle();
}

public interface ISingleRateCurve : IRateCurve 
{
     // return array of curves, initialised after shifting  mktRateSet elements
    ISingleRateCurve[] ShiftedCurveArray(double shift);

     // return one only curve, initialised after shifting all mktRateSet elements up of 'shift' quantity, once at the same time
    ISingleRateCurve ParallelShift(double shift);

    double[] GetInputRates();
}

public interface IMultiRateCurve : IRateCurve
{
     // return array of curves, initialised after shifting  mktRateSet elements of discounting curve
    IMultiRateCurve[] ShiftedCurvesArrayDCurve(double shift);

     // return one only curve, initialised after shifting all mktRateSet elements, of discounting curve, up of 'shift' quantity, once at the same time
    IMultiRateCurve ParallelShiftDCurve(double shift);

     // return array of curves, initialised after shifting  mktRateSet elements of forwarding curve
    IMultiRateCurve[] ShiftedCurvesArrayFwdCurve(double shift);

     // return one only curve, initialised after shifting all mktRateSet elements of forwarding curve, up of 'shift' quantity, once at the same time
    IMultiRateCurve ParallelShiftFwdCurve(double shift);
}

