// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// CapFloorVols.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
  
    public class CapletMatrixVolBuilder<monoStrikeCapletVolBuilder>
       where monoStrikeCapletVolBuilder : MonoStrikeCapletVolBuilder, new()
    {
         // Data member
        public List<double[]> CapletVolMatrix;  // Caplet Implied volatility, each i-List element is vector relative to i-strike
        public double[] Exp;  // expiry of cap
        public double[] Strike;  // strike used in calibration      
        public BilinearInterpolator BI;

         // constructor
        public CapletMatrixVolBuilder(string[] Tenor, IRateCurve Curve, double[] strike, List<double[]> VolSilos)
        {
            #region preparing data
            
             // yf of longer cap            
            SwapStyle y = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(Curve.RefDate(), 0, Tenor.Last(), Curve.GetSwapStyle().buildingBlockType);
            double[] yf = y.scheduleLeg2.GetYFVect(Dc._Act_360);
            int toRemove = yf.Length -1;
            yf = yf.Where((val,inx) => inx != toRemove).ToArray(); 

             // T all tenor needed. They are more than input 
            Date refDate = Curve.RefDate();
            List<Date> ToDate = y.scheduleLeg2.toDates.ToList();
            ToDate.RemoveAt(ToDate.Count-1);  // remove last element
            double[] T = (from c in ToDate
                           select refDate.YF_365(c)).ToArray();           
          
             // available T from input
            double[] Tquoted = new double[Tenor.Length];
            for (int i = 0; i < Tquoted.Length; i++)
            {    // calculate yf of each available T as it was a swap
                SwapStyle myS = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(Curve.RefDate(), 0, Tenor[i], Curve.GetSwapStyle().buildingBlockType);
                int L = myS.scheduleLeg2.toDates.Count()-1;
                Date[] toDate =myS.scheduleLeg2.toDates;
                Tquoted[i] = refDate.YF_365(toDate[L - 1]);
            }            

             // df- getting relevant dates
            Date[] dfDate = y.scheduleLeg2.payDates;
            int Ncaplet = yf.Length;  // number of caplets
            double[] df = new double[Ncaplet];
             // fwd rate
            double[] fwd = new double[Ncaplet];
            Date[] fwdDate = y.scheduleLeg2.fromDates;

            for (int i = 0; i < Ncaplet; i++)  // Note the loop start from 1
            {    // first discount factor is on first payment date of caplet (first caplet skipped)
                df[i] = Curve.Df(dfDate[i+1]);
                fwd[i] = Curve.Fwd(fwdDate[i+1]);
            }

            #endregion

            Ini(T, df, fwd, yf, Tquoted, VolSilos, strike);
        }


        public CapletMatrixVolBuilder(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, List<double[]> VolSilos, double[] strike) 
        {
            Ini(T, df, fwd, yf, Tquoted, VolSilos, strike);
        }

        private void Ini(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, List<double[]> VolSilos, double[] strike) 
        {
             // data member
            Exp = T; Strike = strike;
            
             // Caplet are telescopic, max number of caplets. Fill data in CapletVolMatrix.
            CapletVolMatrix = new List<double[]>();
            for (int i = 0; i < strike.Length; i++)
            {
                monoStrikeCapletVolBuilder b = new monoStrikeCapletVolBuilder();
                b.LateIni(T, df, fwd, yf, Tquoted, VolSilos[i], strike[i]);
                CapletVolMatrix.Add(b.GetCapletVol());
            }

             // Inizialize the Bilinear Interpolator
            BI = new BilinearInterpolator(T, strike, CapletVolMatrix);
        }
    }

    #region MonoStrikeBuilder
    public abstract class MonoStrikeCapletVolBuilder 
    {
         // Data member
        protected double[] CapPrice;
        protected double[] T_;
        protected double[] df_;
        protected double[] fwd_;
        protected double[] yf_;
        protected double[] Tquoted_;
        protected double strike_;
        protected double[] capVolQuoted_;
        public double[] CapletVol;

        public MonoStrikeCapletVolBuilder() { }

        public MonoStrikeCapletVolBuilder(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike)
        {
                    LateIni(T, df,fwd, yf, Tquoted,capVolQuoted, strike); 
        }

        public virtual void LateIni(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike) 
        {
            // this can be override
            LoadDataMember(T, df, fwd, yf, Tquoted, capVolQuoted, strike);
        }
        protected void LoadDataMember(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike)
        {
             // to data member
            T_ = T;
            df_ = df;
            fwd_ = fwd;
            yf_ = yf;
            Tquoted_ = Tquoted;
            strike_ = strike;
            capVolQuoted_ = capVolQuoted;  
        }

        public double[] GetCapletVol() 
        {
            return CapletVol;
        }
    }

    public class MonoStrikeCapletVolBuilderPWC : MonoStrikeCapletVolBuilder 
    {
        public MonoStrikeCapletVolBuilderPWC() { }
        public MonoStrikeCapletVolBuilderPWC(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike) :
            base(T, df, fwd, yf, Tquoted, capVolQuoted, strike)
        {           
        }
        public override void LateIni(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike)
        {
            base.LateIni(T, df, fwd, yf, Tquoted, capVolQuoted, strike);
            CapletVol = Formula.CapletVolBootstrappingPWC(T, df, fwd, yf, Tquoted, capVolQuoted, strike);
        }
    }

     // base class for input interpolate
    public class MonoStrikeCapletVolBuilderInputInterp<Interpolator> : MonoStrikeCapletVolBuilder
    where Interpolator : BaseOneDimensionalInterpolator, new() 
    {
        private Interpolator interpolator; // generic 1D interpolator

        public MonoStrikeCapletVolBuilderInputInterp(){}

        public MonoStrikeCapletVolBuilderInputInterp(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike) :
            base(T, df, fwd, yf, Tquoted, capVolQuoted, strike)
        {          
        }

        public override void LateIni(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike)
        {
             // this can be override
            LoadDataMember(T, df, fwd, yf, Tquoted, capVolQuoted, strike);
            interpolator = new Interpolator();
            interpolator.Ini(Tquoted_, capVolQuoted_);
            double[] interpolatedCapVol = interpolator.Curve(T_);
            CapletVol = Formula.CapletVolBootstrapping(T, df, fwd, yf, interpolatedCapVol, strike);
        }
    }

    public class MonoStrikeCapletVolBuilderInputInterpLinear : MonoStrikeCapletVolBuilderInputInterp<LinearInterpolator> 
    {
        public MonoStrikeCapletVolBuilderInputInterpLinear() { }
        public MonoStrikeCapletVolBuilderInputInterpLinear(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike) :
            base(T, df, fwd, yf, Tquoted, capVolQuoted, strike){}
    }

    public class MonoStrikeCapletVolBuilderInputInterpCubic : MonoStrikeCapletVolBuilderInputInterp<SimpleCubicInterpolator>
    {
        public MonoStrikeCapletVolBuilderInputInterpCubic() { }
        public MonoStrikeCapletVolBuilderInputInterpCubic(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike) :
            base(T, df, fwd, yf, Tquoted, capVolQuoted, strike) { }
    }

    public abstract class MonoStrikeCapletVolBuilderBestFit : MonoStrikeCapletVolBuilder  
    {
        public MonoStrikeCapletVolBuilderBestFit() { }
        public MonoStrikeCapletVolBuilderBestFit(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike):
            base(T, df, fwd, yf, Tquoted, capVolQuoted, strike){}

        public override void LateIni(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike)
        {
            base.LateIni(T, df, fwd, yf, Tquoted, capVolQuoted, strike);
            CapPrice = CalcCapPrice();
            Ini();
        }

        public virtual void Ini()         
        {
            double[] firstGuess = Formula.CapletVolBootstrappingPWC(T_, df_, fwd_, yf_, Tquoted_, capVolQuoted_, strike_);
             // firstGuess = new double[] { 1, 1, 1, 1 };
            Optimize(CapPrice.Length, firstGuess);
        }

        protected void Optimize(int NofEquation, double[] firstGuess) 
        {
            double epsg = 0.0000000001;  // original setting
            double epsf = 0;
            double epsx = 0;
            int maxits = 0;
            alglib.minlmstate state;
            alglib.minlmreport rep;

             // see alglib documentation
            alglib.minlmcreatev(NofEquation, firstGuess, 1e-18, out state);  // or alglib.minlmcreatev(N, x, 0.0001, out state)       
            alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);

            
            alglib.minlmoptimize(state, myFunction, null, null);
            alglib.minlmresults(state, out firstGuess, out rep);

            LoadFinalOutPut(firstGuess);
        }

        public abstract void myFunction(double[] CapletVol, double[] toBeMin, object obj);

         // Pass to member the output
        public virtual void LoadFinalOutPut(double[] BestFitOut)
        {
            CapletVol = BestFitOut;
        }

         // Given array of all Caplet Vol, re-calculate only the Cap prices of the quoted Cap
        protected double[] RecalcCapPrice(double[] myCapletVol)
        {
             // Cap is telescopic
            double[] RecCap = new double[CapPrice.Length];
            for (int i = 0; i < CapPrice.Length; i++)
            {
                int k = Array.IndexOf(T_, Tquoted_[i]);
                for (int j = 0; j <= k; j++)
                {
                    RecCap[i] += Formula.CapletBlack(T_[j], yf_[j], 1, strike_, myCapletVol[j], df_[j], fwd_[j]);
                }
            }
            return RecCap;
        }

         // Calculate Cap Price using input cap vol
        protected double[] CalcCapPrice()
        {
            int nQuotedCap = Tquoted_.Length;
            double[] P = new double[nQuotedCap];
            for (int i = 0; i < nQuotedCap; i++)
            {
                int t = Array.IndexOf(T_, Tquoted_[i]);
                for (int j = 0; j <= t; j++)
                {
                    P[i] += Formula.CapletBlack(T_[j], yf_[j], 1, strike_, capVolQuoted_[i], df_[j], fwd_[j]);
                }
            }
            return P;
        }
       
    }

    public class MonoStrikeCapletVolBuilderBestFitStd : MonoStrikeCapletVolBuilderBestFit
    {
        public MonoStrikeCapletVolBuilderBestFitStd() { }
        public MonoStrikeCapletVolBuilderBestFitStd(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike) :
            base(T, df, fwd, yf, Tquoted, capVolQuoted, strike){}

        public override void myFunction(double[] CapletVol, double[] toBeMin, object obj)
        {
        
             // recalculate starting cap price using CapletVol            
            double[] RecCap = RecalcCapPrice(CapletVol);
             // best fit if each fi[i]==0
            for (int i = 0; i < CapPrice.Length; i++)
            {
                toBeMin[i] = (RecCap[i] - CapPrice[i]) * 10000;  // best fit if fi[i] ==0!, for each i
            }
        }
    }

    public class MonoStrikeCapletVolBuilderBestFitSmooth : MonoStrikeCapletVolBuilderBestFit 
    {
        public MonoStrikeCapletVolBuilderBestFitSmooth() { }
        public MonoStrikeCapletVolBuilderBestFitSmooth(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike):
            base( T, df, fwd, yf, Tquoted, capVolQuoted, strike)
        {
        }

        public override void Ini()
        {
            double[] firstGuess = Formula.CapletVolBootstrappingPWC(T_, df_, fwd_, yf_, Tquoted_, capVolQuoted_, strike_);
            Optimize(CapPrice.Length+1, firstGuess);
        }

        public override void myFunction(double[] CapletVol, double[] toBeMin, object obj)
        {
             // see Flavell, R. 2002. Swaps and other Derivatives. John Wiley and Sons, Chichester page 278.
             // recalculate starting cap price using CapletVol            
            double[] RecCap = RecalcCapPrice(CapletVol);
             // best fit if each fi[i]==0
            for (int i = 0; i < CapPrice.Length; i++)
            {
                toBeMin[i] = (RecCap[i] - CapPrice[i]) * 10000;  // best fit if fi[i] ==0!, for each i
            }
            double sum = 0.0;
            for (int i = 0; i < CapletVol.Length -1; i++)
            {
                sum += (CapletVol[i] - CapletVol[i + 1]) * (CapletVol[i] - CapletVol[i + 1]);
            }
            toBeMin[CapPrice.Length] = sum;
        }
    }
    
    public class MonoStrikeCapletVolBuilderBestFitInterp<Interpolator> : MonoStrikeCapletVolBuilderBestFit 
        where Interpolator: BaseOneDimensionalInterpolator, new() 
    {
        private Interpolator interpolator;  // generic 1D interpolator

        public MonoStrikeCapletVolBuilderBestFitInterp() { }
        public MonoStrikeCapletVolBuilderBestFitInterp(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike) :
            base(T, df, fwd, yf, Tquoted, capVolQuoted, strike) { }

        public override void Ini()
        {
            interpolator = new Interpolator();  // initialise interpolator
            double[] firstGuess = capVolQuoted_;
            Optimize(CapPrice.Length, firstGuess);
        }

        public override void myFunction(double[] CapletVol, double[] toBeMin, object obj)
        {
             // see Flavell, R. 2002. Swaps and other Derivatives. John Wiley and Sons, Chichester page 278.
            interpolator.Ini(Tquoted_, CapletVol);

            double[] CapletVolInterpolated = interpolator.Curve(T_);
             // recalculate starting cap price using CapletVol            
            double[] RecCap = RecalcCapPrice(CapletVolInterpolated);
             // best fit if each fi[i]==0
            for (int i = 0; i < CapPrice.Length; i++)
            {
                toBeMin[i] = (RecCap[i] - CapPrice[i]) * 1e6;  // best fit if fi[i] ==0!, for each i
            }
        }

        public override void LoadFinalOutPut(double[] BestFitOut)
        {
            interpolator.Ini(Tquoted_, BestFitOut);
            CapletVol = interpolator.Curve(T_);
        }
    }

    public class MonoStrikeCapletVolBuilderBestFitPWL : MonoStrikeCapletVolBuilderBestFitInterp<LinearInterpolator> 
    {
        public MonoStrikeCapletVolBuilderBestFitPWL() { }
        public MonoStrikeCapletVolBuilderBestFitPWL(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike) :
            base(T, df, fwd, yf, Tquoted, capVolQuoted, strike) { }
    }

    public class MonoStrikeCapletVolBuilderBestFitCubic : MonoStrikeCapletVolBuilderBestFitInterp<SimpleCubicInterpolator>
    {
        public MonoStrikeCapletVolBuilderBestFitCubic() { }
        public MonoStrikeCapletVolBuilderBestFitCubic(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike) :
            base(T, df, fwd, yf, Tquoted, capVolQuoted, strike) { }
    }
    
    public class MonoStrikeCapletVolBuilderBestFitFunct : MonoStrikeCapletVolBuilderBestFit
    {
        public MonoStrikeCapletVolBuilderBestFitFunct() { }
        public MonoStrikeCapletVolBuilderBestFitFunct(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike) :
            base(T, df, fwd, yf, Tquoted, capVolQuoted, strike) { }

        public override void Ini()
        {            
            double[] firstGuess = GetFirstGuess();
            Optimize(CapPrice.Length, firstGuess);
        }

        public override void myFunction(double[] parms, double[] toBeMin, object obj)
        {
             // see Flavell, R. 2002. Swaps and other Derivatives. John Wiley and Sons, Chichester page 278.
            double[] newCapletVol = new double[T_.Length];
            double variance = 0.0;
            for (int i = 0; i < T_.Length; i++) 
            {
                variance = CapletVar(T_[i], T_[i], parms) - CapletVar(0, T_[i], parms);
                newCapletVol[i] = Math.Sqrt(Math.Max(0.0, variance) / T_[i]);
            }

             // recalculate starting cap price using CapletVol            
            double[] RecCap = RecalcCapPrice(newCapletVol);
             // best fit if each fi[i]==0
            for (int i = 0; i < CapPrice.Length; i++)
            {
                toBeMin[i] = (RecCap[i] - CapPrice[i]) * (RecCap[i] - CapPrice[i]) * 1e18; // best fit if fi[i] ==0!, for each i
                
            }
        }

        public override void LoadFinalOutPut(double[] FittedPars)
        {
            CapletVol = new double[T_.Length];
            double variance = 0.0;
            for (int i = 0; i < T_.Length; i++)
            {
                variance = CapletVar(T_[i], T_[i], FittedPars) - CapletVar(0, T_[i], FittedPars);
                CapletVol[i] = Math.Sqrt(Math.Max(0.0, variance) / T_[i]);
            }
        }

        private double CapletVar(double t,double T,double[] parameters)
        {
             // Brigo and Mercurio (2001) pg.197, primitive of parametrization formulation six

            double a = parameters[0];
            double b = parameters[1];
            double c = parameters[2];
            double d = parameters[3];
            double dt = t-T;
            double e = Math.Exp(b*dt);
            double eT = Math.Exp(-b * T);
   
            double a2 = a*a;
            double b2 = b*b;
            double b3 = b2*b;
            double c2 = c*c;
            double d2 = d*d;
            double dt2 = dt*dt;
            double e2 = e*e;
            double eT2 = eT*eT;
            double t2 = t*t;
            double t3 = t2*t;
            double t4 = t3*t;
            double t5 = t4*t;
            double T2 = T*T;

            double rval = 0.0;

           if (b< 1.0e-6)
              rval = a2*b2/5.0*t5 - a*b*(a*(2*b*T-1.0)+b*d)/2.0*t4 +
                     (a2*(6*b2*T2-6*b*T+1.0)+2*a*b*(3*b*d*T-c-2*d)+b2*d2)/3.0*t3 -
                     (a*(2*b*T-1.0)+b*d)*((b*T-1.0)*(a*T+d)-c)*t2 +
                     ((b*T-1.0)*(a*T+d)-c)*((b*T-1.0)*(a*T+d)-c)*t;

           else
              rval = ( e2 * (a2 + 2*a2*b2*dt2 - 2*a2*b*dt - 4*a*b2*d*dt + 2*a*b*d + 2*b2*d2) +
                       e * (8*a*b*c - 8*a*b2*c*dt + 8*b2*c*d) + 4*b3*c2*t -
                       eT2 * (a2 + 2*a2*b2*T2 + 2*a2*b*T + 2*a*b*d + 4*a*b2*d*T + 2*b2*d2) -
                       eT * (8*a*b*c + 8*b2*c*d + 8*a*b2*c*T) ) / (4*b3);

           return rval;
        }


        private double[] GetFirstGuess() 
        {
            double[] pwcCapletVol = Formula.CapletVolBootstrappingPWC(T_, df_, fwd_, yf_, Tquoted_, capVolQuoted_, strike_);
            CapletVol = pwcCapletVol;
            double epsg = 0.0000000001;  // original setting
            double epsf = 0;
            double epsx = 0;
            int maxits = 0;
            alglib.minlmstate state;
            alglib.minlmreport rep;
            double[] veryFirstGuess = new double[] {1, 1, 1,1 };
             // see alglib documentation
            alglib.minlmcreatev(pwcCapletVol.Length, veryFirstGuess,1e-10, out state);  // or alglib.minlmcreatev(N, x, 0.0001, out state)       
            alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);


            alglib.minlmoptimize(state, myF, null, null);
            alglib.minlmresults(state, out veryFirstGuess, out rep);

            return veryFirstGuess;
        }


        private void myF(double[] firstPars, double[] toBeMin, object obj)
        {
             // see Flavell, R. 2002. Swaps and other Derivatives. John Wiley and Sons, Chichester page 278.
            double[] newCapletVol = new double[T_.Length];
            double variance = 0.0;
            for (int i = 0; i < T_.Length; i++)
            {
                variance = CapletVar(T_[i], T_[i], firstPars) - CapletVar(0, T_[i], firstPars);
                newCapletVol[i] = Math.Sqrt(Math.Max(0.0, variance) / T_[i]);
            }

             // recalculate starting cap price using CapletVol            
            double[] RecCap = RecalcCapPrice(newCapletVol);
             // best fit if each fi[i]==0
            for (int i = 0; i < T_.Length; i++)
            {
                toBeMin[i] = (CapletVol[i] - newCapletVol[i]) * (CapletVol[i] - newCapletVol[i])*1e10;  // best fit if fi[i] ==0!, for each i
            }
        }
    }
    #endregion
