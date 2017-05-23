// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// VanillaSwap.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
  
// Interface for vanilla swap valued using Multi Curve approach
    public interface MultiCurveRateCalculus 
    {
         // Net Present Value of my swap
        double NPV();

         // return array of curves, initialised after shifting  mktRateSet elements of discounting curve
        double[] BPVShiftedDCurve(double shift);

         // return one only curve, initialised after shifting all mktRateSet elements, of discounting curve, up of 'shift' quantity, once at the same time
        double BPVParallelShiftDCurve(double shift);

         // return array of curves, initialised after shifting  mktRateSet elements of forwarding curve
        double[] BPVShiftedFwdCurve(double shift);

         // return one only curve, initialised after shifting all mktRateSet elements of forwarding curve, up of 'shift' quantity, once at the same time
        double BPVParallelShiftFwdCurve(double shift);  
    }

     // Vanilla Swap defined vs. a floating rate tenor equal to the one of Multi Curve building blocks
    public class VanillaSwap : MultiCurveRateCalculus 
    {
        SwapStyle mySwap;
        bool payOrRec;
        IMultiRateCurve multiCurve;
        double nominal;

        public VanillaSwap(IMultiRateCurve MultiCurve, double Rate, string SwapTenor,bool PayOrRec, double Nominal) 
        {            
             // Standard swap
            Type SwapType =  MultiCurve.GetSwapStyle().GetType();
            Date myRefDate = MultiCurve.RefDate();
             // using reflection
            this.mySwap = (SwapStyle)Activator.CreateInstance(SwapType, myRefDate, Rate, SwapTenor);
            this.payOrRec = PayOrRec;
            this.multiCurve = MultiCurve;
            this.nominal = Nominal;
        }

        #region implementing MultiCurveRateCalculus
        public double NPV() 
        {
            return Formula.NPV(mySwap,multiCurve,payOrRec) * nominal;
        }

         // return array of curves, initialised after shifting  mktRateSet elements of discounting curve
        public double[] BPVShiftedDCurve(double shift)
        {
            IRateCurve[] DCrvs = multiCurve.ShiftedCurvesArrayDCurve(shift); // collection of ShiftedCurvesArrayDCurve curves
            int nOfRate = DCrvs.Count();  // iterate for market input for discounting curve
            double [] bpv = new double [nOfRate];  // collect bpv for output
            double iniMTM =  this.NPV();
            Parallel.For(0, nOfRate, i=>
            {
                        bpv[i] = Formula.NPV(mySwap, DCrvs[i],this.payOrRec)*nominal  - iniMTM;
            });
            
            return bpv;
        }

         // return array of curves, initialised after shifting  mktRateSet elements of forwarding curve
        public double[] BPVShiftedFwdCurve(double shift)
        {
            IRateCurve[] FwdCrvs = multiCurve.ShiftedCurvesArrayFwdCurve(shift);  // collection of ShiftedCurvesArrayDCurve curves
            int nOfRate = FwdCrvs.Count();  // iterate for market input for discounting curve
            double[] bpv = new double[nOfRate];  // collect bpvs for output data
            double iniMTM = this.NPV();
            Parallel.For(0, nOfRate, i =>
            {
                bpv[i] = Formula.NPV(mySwap, FwdCrvs[i], this.payOrRec) * nominal - iniMTM;
            });

            return bpv;
        }

         // return one only curve, initialised after shifting all mktRateSet elements, of discounting curve, up of 'shift' quantity, once at the same time
        public double BPVParallelShiftDCurve(double shift) 
        {
            IMultiRateCurve sCurve = multiCurve.ParallelShiftDCurve(shift);
            return Formula.NPV(mySwap, sCurve, payOrRec) * nominal - this.NPV();
        }

         // return one only curve, initialised after shifting all mktRateSet elements of forwarding curve, up of 'shift' quantity, once at the same time
        public double BPVParallelShiftFwdCurve(double shift) 
        {
            IMultiRateCurve sCurve = multiCurve.ParallelShiftFwdCurve(shift);
            return Formula.NPV(mySwap, sCurve, payOrRec) * nominal - this.NPV();
        }

         // return array of curves, initialised after shifting  mktRateSet elements of discounting curve
        public double[] BPVShiftedDCurveSeq(double shift)
        {
            IRateCurve[] DCrvs = multiCurve.ShiftedCurvesArrayDCurve(shift);  // collection of ShiftedCurvesArrayDCurve curves
            int nOfRate = DCrvs.Count();  // iterate for market input for discounting curve
            double[] bpv = new double[nOfRate];  // collect bpv for output
            double iniMTM = this.NPV();
            for(int i =0; i<nOfRate; i++)
            {
                bpv[i] = Formula.NPV(mySwap, DCrvs[i], this.payOrRec) * nominal - iniMTM;
            }

            return bpv;
        }
        #endregion
    }

