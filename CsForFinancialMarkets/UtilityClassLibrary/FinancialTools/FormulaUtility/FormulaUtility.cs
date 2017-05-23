// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// FormulaUtility.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

public static class Formula 
    {
        #region Bond
         // Tuckman, B. 2002: page 60, formula 4.15
        public static double FullPriceFromSemiAnnualYield(double yield, double coupon, int numberOfFutureCoupons, double yfToNextCoupon)
        {
            int freq = 2;
            double fullPrice = 0.0;
            for (int i = 0; i <= numberOfFutureCoupons; i++)
            {
                fullPrice += (coupon / freq) * (1 / (Math.Pow(1 + (yield / freq), i + yfToNextCoupon)));
            }
            fullPrice += (1 / (Math.Pow(1 + (yield / freq), numberOfFutureCoupons + yfToNextCoupon)));
            return fullPrice;
        }

         // Haug, E. 2007: 11.2.5
        public static double FullPriceFromYield(double yield, double coupon, double yfToNextCoupon, int numberOfFutureCoupons,
            double FaceValue, int freq)
        {
            return
                Math.Pow(1 + yield, (1 / freq) - yfToNextCoupon) *
                    (
                    (coupon / freq) *
                    ((Math.Pow(1 + yield, -numberOfFutureCoupons / freq) - 1) / (1 - Math.Pow(1 + yield, 1 / freq))) +
                    (FaceValue / (Math.Pow(1 + yield, numberOfFutureCoupons / freq)))
                    );
        }
        
         // Calculate dirty price from yield, adapting 
        public static double FullPriceFromYield(double yield, Date[] CashFlowsDate, double[] CashFlows,
            int freq, Dc dc, Date SettlementDate, Compounding compounding, double FaceValue)
        {
             // initial discount
            double df = 1.0;  // discount
            double yf = 0.0;

             // find next payment date that is greater than settlement date, previous payments will no more need      
            int iStart = Array.FindIndex(CashFlowsDate, n => n > SettlementDate);  // index of next payment
            int iEnd = CashFlowsDate.GetLength(0);  // total payments (past+ future)
            int dim = iEnd - iStart;  // number of payments in the future

             // myCFs vector of future payments(after settlement date)
             // myCFsDates vector of date of each element of myCFs, starting from settlement date        
            Date[] myCFsDates = new Date[dim + 1]; // start from settlement date + future dates of payment
            double[] myCFs = new double[dim]; // future payments
            Array.ConstrainedCopy(CashFlowsDate, iStart, myCFsDates, 1, dim);  // populate myCFsDates 1 to dim index
            myCFsDates[0] = SettlementDate;  // first element of myCFsDates is settlement date
            Array.ConstrainedCopy(CashFlows, iStart, myCFs, 0, dim); // populate myCFs

             // Iterate to add each cash flow discounted, according to given convention
            double fullPrice = 0.0; // starting value
            for (int i = 0; i < dim; i++)
            {
                yf = myCFsDates[i].YF(myCFsDates[i + 1], dc);  // calculate new yf
                df *= CalCDF(yield, yf, freq, compounding); // calculate new df
                fullPrice += myCFs[i] * df;
            }

             // Adding discounted face amount
            fullPrice += FaceValue * df;

             // final result
            return fullPrice;
        }
       
         // Calculate Yield from dirty price
        public static double YieldFromFullPrice(double fullPrice, double yieldGuess, Date[] CashFlowsDate, double[] CashFlows,
            int freq, Dc dc, Date SettlementDate, Compounding compounding, double FaceValue)
        {
            int N = CashFlowsDate.GetLength(0);  // number of Cash flows dates
            int k = Array.FindIndex(CashFlowsDate, n => n > SettlementDate);  // get index of first cash flow after settlement date
            Date[] Dates = new Date[N - k];  // array with pay dates after settlement
            double[] Flows = new double[N - k];  // flows after settlement
            Array.Copy(CashFlowsDate, k, Dates, 0, N - k); // fill array with needed data
            Array.Copy(CashFlows, k, Flows, 0, N - k); // fill array with needed data

             // define my function: we should find yield that match starting price
            NumMethod.myMethodDelegate fname =
                       y_ => fullPrice - FullPriceFromYield(y_, Dates, Flows,
            freq, dc, SettlementDate, compounding, FaceValue);

             // return yield
            return NumMethod.NewRapNum(fname, yieldGuess);
        }

         // Calculate DF with different type of compounding and freq, r is % rate, yf is year fraction, freq is coupon frequency, c is compounding type 
        public static double CalCDF(double r, double yf, int freq, Compounding c)
        {
            double n = (double)freq;
            switch (c)
            {
                case Compounding.Simple:
                    return 1 / (1.0 + r * yf);  // page 10, formula (1.8) Fixed Income Securities And Derivatives Handbook, Moorad Choudhry
                case Compounding.Compounded:
                    return 1 / (System.Math.Pow(1.0 + r / n, n * yf));  // page 11, formula (1.9) Fixed Income Securities And Derivatives Handbook, Moorad Choudhry
                case Compounding.Continuous:
                    return System.Math.Exp(-r * yf);  // page 9m, formula (1.6) Fixed Income Securities And Derivatives Handbook, Moorad Choudhry. Solve for PV, where FV=1
            }
            return 0;
        }
        #endregion

         // Hagan. P.S. and West, G. 2006. Formula 2) page 4 
        public static double ParRate(double[] YearFraction, double[] DF)
        {
             // Arrays must have the same size
            double up = 0.0;              // numerator
            double down = 0.0;            // denominator
            int n = DF.GetLength(0)-1;  // max size 

             // Numerator
            up = 1 - DF[n];

             // Denominator
            for (int i = 0; i <= n; i++) 
            {
                down += YearFraction[i] * DF[i];            
            }

             // Par Rate
            return up / down;
        }

         // for fwd start swap I need starting discount factor
        public static double ParRate(double[] YearFraction, double[] DF, double DFstart)
        {
             // Arrays must have the same size
            double up = 0.0;              // numerator
            double down = 0.0;            // denominator
            int n = DF.GetLength(0) - 1;  // max size 

             // Numerator
            up = DFstart - DF[n];

             // Denominator
            for (int i = 0; i <= n; i++)
            {
                down += YearFraction[i] * DF[i];
            }

             // Par Rate
            return up / down;
        }

         // Hagan. P.S. and West, G. 2006. Formula 3)
        public static double FinalDF(double ParRate, double[] YearFraction, double[] DF) 
        {
             // DF element should be >= YearFractionElement-1
            double up = 0.0;              // numerator
            double down = 0.0;            // denominator
            int n = YearFraction.GetLength(0)-1;  // max size  of YearFraction
                                                  // it uses only n-1 DF, since DF[n] is the one to find

             // numerator
            for (int i = 0; i < n; i++)
            {
                up += YearFraction[i] * DF[i];
            }
            up = 1 - ParRate * up;

             // Denominator
            down = 1 + ParRate * YearFraction[n];
            
            return up/down;            
        }

         // Hagan. P.S. and West, G. 2006. Formula 4)
        public static double ZeroRate(double refTime, double ParRate, double[] YearFraction, double[] DF) 
        {
            return  -Math.Log(FinalDF(ParRate,YearFraction,DF))/refTime;        
        }

         // Hagan. P.S. and West, G. 2006. Formula 2) page 4  using Vector<T>
        public static double ParRate(Vector<double> YearFraction, Vector<double> DF)
        {
             // Arrays must have the same size
            double up = 0.0;              // numerator
            double down = 0.0;            // denominator
            int n = DF.Length - 1;  // max size 

             // Numerator
            up = 1 - DF[n];

             // Denominator
            for (int i = 0; i <= n; i++)
            {
                down += YearFraction[i] * DF[i];
            }

             // Par Rate
            return up / down;
        }

         // Hagan. P.S. and West, G. 2006. Formula 3) using Vector<T>
        public static double FinalDF(double ParRate, Vector<double> YearFraction, Vector<double> DF)
        {
             // DF element should be >= YearFractionElement-1
            double up = 0.0;              // numerator
            double down = 0.0;            // denominator
            int n = YearFraction.Length - 1;  // max size  of YearFraction
             // it uses only n-1 DF, since DF[n] is the one to find

             // numerator
            for (int i = 0; i < n; i++)
            {
                up += YearFraction[i] * DF[i];
            }
            up = 1 - ParRate * up;

             // Denominator
            down = 1 + ParRate * YearFraction[n];
            
            return up / down;
        }

         // Hagan. P.S. and West, G. 2006. Formula 4) using Vector<T>
        public static double ZeroRate(double refTime, double ParRate, Vector<double> YearFraction, Vector<double> DF)
        {
            return -Math.Log(FinalDF(ParRate, YearFraction, DF)) / refTime;
        }

         // Haug, E. 2007.
        public static double DfFromZeroRate(double zeroRate, double dTime) 
        {
            return Math.Exp(-zeroRate * dTime);
        }

         // Haug, E. 2007. Section 6.2.3
        public static double FromMtoContinuous(int m, double r_m) 
        {
            return m * Math.Log(1 + (r_m / m));
        }

         // simple compounding
        public static double DFsimple(double yf, double simpleRate) 
        {            
            return 1.0 / (1.0 + (yf * simpleRate));
        }

         // Calculate array of DFs from vector of yf and simple rates
        public static double[] DFsimpleVect(double[] yf, double[] simpleRate) 
        {
             // yf and simpleRate array should have same size
            int n = yf.Count();
            double[] Df = new double[n];  // array of df
            Df[0] = DFsimple(yf[0], simpleRate[0]);  // first DF
            for (int i = 1; i < n; i++) 
            {
                Df[i] = DFsimple(yf[i], simpleRate[i]) * Df[i - 1];  // df_0_i = df_0_i-1 * df_i-1_i;
            }
            return Df;
        }

         // Mercurio, F. 2009. (8)
        public static double ParRateFormula(double[] yfFloatLeg, double[] dfFloatLeg, double[] fwdFloatLeg,double[] yfFixLeg,double[] dfFixLeg)
        {
             // number of floating leg elements. 
            int nFlt = yfFloatLeg.GetLength(0); // yfFloatLeg,dfFloatLeg,fwdFloatLeg should have same number of elements
             // number of fix leg elements. 
            int nFix = yfFixLeg.GetLength(0); //  yfFixLeg,dfFixLeg should have same number of elements

             // Mercurio, F. 2009: numerator of formula (8)
            double numerator = 0.0;
             // Mercurio, F. 2009: denominator of formula (8)
            double denominator = 0.0;
            
             // Check correct dim
            if ((yfFloatLeg.GetLength(0) != dfFloatLeg.GetLength(0)) || (fwdFloatLeg.GetLength(0) != dfFloatLeg.GetLength(0))
                || (yfFixLeg.GetLength(0) != dfFixLeg.GetLength(0)))
            {
                throw new ArgumentException("error in leg dimension");
            }           
            
             // numerator
            for (int i = 0; i < nFlt; i++ )
            {
                numerator += yfFloatLeg[i] * dfFloatLeg[i] * fwdFloatLeg[i];
            }

             // denominator
            for (int i = 0; i < nFix; i++) 
            {
                denominator += yfFixLeg[i] * dfFixLeg[i];            
            }

             // par rate
            return numerator / denominator;
        }

         // Mercurio, F. 2009. (8), modified it considering two floating leg.
         // One of them has a spread. We solve for spread on Leg2 Vs Leg1
        public static double SpreadBasisFormula(double[] yfFloatLeg1, double[] dfFloatLeg1, double[] fwdFloatLeg1, 
            double[] yfFloatLeg2, double[] dfFloatLeg2, double[] fwdFloatLeg2)
        {
             // number of floating leg #1 elements. 
            int nFlt1 = yfFloatLeg1.GetLength(0); // yfFloatLeg1,dfFloatLeg1,fwdFloatLeg1 should have same number of elements
             // number of floating leg #2 elements. 
            int nFlt2 = yfFloatLeg2.GetLength(0); // yfFloatLeg2,dfFloatLeg2,fwdFloatLeg2 should have same number of elements

             // numerator of formula is [yfDfFwd1-yfDfFwd2]
            double yfDfFwd1 = 0.0;  // sumproduct of year fraction * Df * Fwd rate for Leg 1
            double yfDfFwd2 = 0.0;  // sumproduct of year fraction * Df * Fwd rate for Leg 2
            
             // denominator 
            double yfDf2 = 0.0;  // sumproduct of year fraction * Df for Leg 2

             // Check correct dim
            if ((yfFloatLeg1.GetLength(0) != dfFloatLeg1.GetLength(0)) || (fwdFloatLeg1.GetLength(0) != dfFloatLeg1.GetLength(0))
            || (yfFloatLeg2.GetLength(0) != dfFloatLeg2.GetLength(0)) || (fwdFloatLeg2.GetLength(0) != dfFloatLeg2.GetLength(0)))
            {
                throw new ArgumentException("error in leg dimension");
            }

             // yfDfFwd1 
            for (int i = 0; i < nFlt1; i++)
            {
                yfDfFwd1 += yfFloatLeg1[i] * dfFloatLeg1[i] * fwdFloatLeg1[i];
            }

             // yfDfFwd2 and yfDf2
            for (int i = 0; i < nFlt2; i++)
            {
                yfDfFwd2 += yfFloatLeg2[i] * dfFloatLeg2[i] * fwdFloatLeg2[i];
                yfDf2 += yfFloatLeg2[i] * dfFloatLeg2[i]; // may be less efficient but clearer to read
            }

             // spread 
            return (( yfDfFwd1 - yfDfFwd2) / yfDf2);
        }
                
         // Log of a vector
        public static Vector<double> LogVect(Vector<double> Input) 
        {
            Vector<double> OutPut = new Vector<double>(Input.Length, Input.MinIndex);
            for (int i = Input.MinIndex; i <= Input.MaxIndex; i++) 
            {
                OutPut[i] = Math.Log(Input[i]);
            }
            return OutPut;
        }

         // Exp of vector
        public static Vector<double> ExpVect(Vector<double> Input)
        {
            Vector<double> OutPut = new Vector<double>(Input.Length, Input.MinIndex);
            for (int i = Input.MinIndex; i <= Input.MaxIndex; i++)
            {
                OutPut[i] = Math.Exp(Input[i]);
            }
            return OutPut;
        }

         // Calculate simple rate
        public static double SimpleRate(Date FromDate, Date ToDate, Dc DayCount, double df) 
        {
            double yf = FromDate.YF(ToDate, DayCount);            
            return ((1 / df) - 1) / yf;
        }        
        #region CapFloorSwaption

         // The Core Black Formula 
         // Brigo, D. and Mercurio, F. 2006. XXVI Notation
        public static double Black(double K, double F, double v, double w)
        {
            double d1 = (Math.Log(F / K) + v * v / 2) / v;
            double d2 = (Math.Log(F / K) - v * v / 2) / v;
            return F * w * CND(w * d1) - K * w * CND(w * d2);
        }

         // Brigo, D. and Mercurio, F. 2006. Formula (1.26) page 17
        public static double CapBlack(double[] T, double[] yf, double N, double K, double sigma, double[] df, double[] fwd)
        {
            int n = T.Length; // T,yf,df should have same number of elements
            double v = 0.0;
            double price = 0.0;  // running sum of caplets
            for (int i = 0; i < n; i++)
            {
                v = sigma * Math.Sqrt(T[i]);
                price += df[i] * yf[i] * Black(K, fwd[i], v, 1);
            }
            return N * price;
        }

         // Brigo, D. and Mercurio, F. 2006. Formula (1.26) page 17
        public static double FloorBlack(double[] T, double[] yf, double N, double K, double sigma, double[] df, double[] fwd)
        {
            int n = T.Length; // T,yf,df should have same number of elements
            double v = 0.0;
            double price = 0.0;  // running sum of caplets
            for (int i = 0; i < n; i++)
            {
                v = sigma * Math.Sqrt(T[i]);
                price += df[i] * yf[i] * Black(K, fwd[i], v, -1);
            }
            return N * price;
        }

         // Brigo, D. and Mercurio, F. 2006. Formula (1.26) page 17
        public static double CapBlack(double[] T, double[] yf, double N, double K, double[] sigma, double[] df, double[] fwd)
        {
            int n = T.Length; // T,yf,df should have same number of elements
            double v = 0.0;
            double price = 0.0;  // running sum of caplets
            for (int i = 0; i < n; i++)
            {
                v = sigma[i] * Math.Sqrt(T[i]);
                price += df[i] * yf[i] * Black(K, fwd[i], v, 1);
            }
            return N * price;
        }

         // Brigo, D. and Mercurio, F. 2006. Formula (1.26) page 17
        public static double CapletBlack(double T, double yf, double N, double K, double sigma, double df, double fwd)
        {
            double v = sigma * Math.Sqrt(T);
            return N * df * yf * Black(K, fwd, v, 1);
        }

         // Brigo, D. and Mercurio, F. 2006. Formula (1.26) page 17
        public static double FloorletBlack(double T, double yf, double N, double K, double sigma, double df, double fwd)
        {
            double v = sigma * Math.Sqrt(T);
            return N * df * yf * Black(K, fwd, v, -1);
        }

         // Cap Price using VolCapletMatrix as argument
        public static double CapBlack(double[] T, double[] yf, double N, double K, BilinearInterpolator VolCapletMatrix, double[] df, double[] fwd)
        {
             // LINQ: getting interpolated vol
            double[] sigma = (from t in T
                              select VolCapletMatrix.Solve(t, K)).ToArray();
            return CapBlack(T, yf, N, K, sigma, df, fwd);
        }

        public static double CapBlack(string Tenor, double strike, double N, IRateCurve curve, BilinearInterpolator VolCapletMatrix)
        {
            Date refDate = curve.RefDate();
            SwapStyle y = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(refDate, 0, Tenor, curve.GetSwapStyle().buildingBlockType);
            double[] yf = y.scheduleLeg2.GetYFVect(Dc._Act_360);
            int toRemove = yf.Length - 1;
            yf = yf.Where((val, inx) => inx != toRemove).ToArray();

            List<Date> ToDate = y.scheduleLeg2.toDates.ToList();
            ToDate.RemoveAt(ToDate.Count - 1);  // remove last element
            double[] T = (from c in ToDate
                          select refDate.YF_365(c)).ToArray();

             // df- getting relevant dates
            Date[] dfDate = y.scheduleLeg2.payDates;
            int Ncaplet = yf.Length;  // number of caplets
            double[] df = new double[Ncaplet];
             // fwd rate
            double[] fwd = new double[Ncaplet];
            Date[] fwdDate = y.scheduleLeg2.fromDates;

            for (int i = 0; i < Ncaplet; i++)  // Note the loop start from 1
            {    // first discount factor is on first payment date of caplet (first caplet skipped)
                df[i] = curve.Df(dfDate[i + 1]);
                fwd[i] = curve.Fwd(fwdDate[i + 1]);
            }

            double[] sigma = (from t in T
                              select VolCapletMatrix.Solve(t, strike)).ToArray();
            return CapBlack(T, yf, N, strike, sigma, df, fwd);
        }

         // Caplet volatilities bootstrap
        public static double[] CapletVolBootstrapping(double[] T, double[] df, double[] fwd, double[] yf, double[] capVol, double[] strike)
        {
            double shorterCap = 0.0;
            int N = df.Length;
            double[] capPrice = new double[N];
            double[] capletVol = new double[N];

             // calculate cap price using flat vol
            for (int i = 0; i < N; i++)
            {
                shorterCap = 0.0;

                for (int j = 0; j <= i; j++)
                {
                    capPrice[i] += Formula.CapletBlack(T[j], yf[j], 100, strike[i], capVol[i], df[j], fwd[j]);
                }
                for (int j = 0; j < i; j++)
                {
                    shorterCap += Formula.CapletBlack(T[j], yf[j], 100, strike[i], capletVol[j], df[j], fwd[j]);
                }

                double iniGuess = capVol[i];
                if (i > 0) { iniGuess = capletVol[i - 1]; }
                capletVol[i] = Formula.CapletImpVol(T[i], yf[i], df[i], fwd[i], strike[i], (capPrice[i] - shorterCap) / 100, iniGuess);
            }
            return capletVol;
        }

         // Mono strike input - used in matrix caplet bootstrapping
        public static double[] CapletVolBootstrapping(double[] T, double[] df, double[] fwd, double[] yf, double[] capVol, double monoStrike)
        {
            double[] strike = Enumerable.Repeat(monoStrike, T.Count()).ToArray();
            return CapletVolBootstrapping(T, df, fwd, yf, capVol, strike);
        }
    
         // PieceWiseConstant see formula 2 "Bloomberg volatility cube" page 8
        public static double[] CapletVolBootstrappingPWC(double[] T, double[] df, double[] fwd, double[] yf, double[] Tquoted, double[] capVolQuoted, double strike)
        {
             // Tquoted and CapVolQuoted have same number of elements < T element
             // T,df,fwd,yf have same number of element. They are referred to longer cap
             // 1) calculate Cap price
            int nQuotedCap = Tquoted.Length;
            double[] P = new double[nQuotedCap];
            for (int i = 0; i < nQuotedCap; i++)
            {
                int t = Array.IndexOf(T, Tquoted[i]);
                for (int j = 0; j <= t; j++)
                {
                    P[i] += Formula.CapletBlack(T[j], yf[j], 1, strike, capVolQuoted[i], df[j], fwd[j]);
                }
            }

             // 2) cap stripping
            double[] sgm = new double[nQuotedCap];
            sgm[0] = capVolQuoted[0];
            for (int i = 1; i < nQuotedCap; i++)
            {
                int ini = Array.IndexOf(T, Tquoted[i - 1]);
                int end = Array.IndexOf(T, Tquoted[i]);
                 // for (int j=ini; j<=end; j++)
                 // {
                Func<double[], double[]> GetValue = (source) =>
                {
                    double[] outPut = new double[end - ini];
                    Array.Copy(source, ini + 1, outPut, 0, end - ini);
                    return outPut;
                };

                sgm[i] = Formula.CapImpVol(GetValue(T), GetValue(yf), GetValue(df), GetValue(fwd), strike, P[i] - P[i - 1], capVolQuoted[i]);
                 // }                 
            }
             // 3) fill caplet piecewise vol
            int nT = T.Length;
            double[] results = new double[nT];
            int z = 0;
            for (int i = 0; i < nQuotedCap; i++)
            {

                while (T[z] <= Tquoted[i])
                {
                    results[z] = sgm[i];
                    z++;
                    if (z == T.Length) break;
                }
            }
            return results;
        }
                
        public static double CapletVega(double timeToStart, double timeAccrual, double df_fin, double fwd_r,
        double sigma, double strike)
        {
             // timeToStart is time from spot to expiry, we usually calculate it as Act/Act
             // timeAccrual from caplet start to end of cap, we usually calculate it as MM     
            double d_1 = (Math.Log(fwd_r / strike) + 0.5 * sigma * sigma * timeToStart) /
                (sigma * Math.Sqrt(timeToStart));
            return df_fin * fwd_r * Formula.ND(d_1) * timeAccrual * Math.Sqrt(timeToStart);
        }

        public static double CapletImpVol(double timeToStart, double timeAccrual, double df_fin, double fwd_r,
          double strike, double price, double sigmaGuess)
        {
            double error = 0.000001;
            double sigma = sigmaGuess;  // Simply a first guess
            double priceError;
            double dv = error + 1;
            double price_fwd_calculated = 0.0, vega_fwd_calculated = 0.0;
            while (Math.Abs(dv) > error)
            {
                price_fwd_calculated = CapletBlack(timeToStart, timeAccrual, 1, strike, sigma, df_fin, fwd_r);
                vega_fwd_calculated = CapletVega(timeToStart, timeAccrual, df_fin, fwd_r, sigma, strike);

                priceError = price_fwd_calculated - price;
                dv = priceError / vega_fwd_calculated;
                sigma -= dv;
            };
            return sigma;
        }

         // CapImplVol: getting implied volatility using the Vega information (idea from Wilmott, P. 2006.) 
        public static double CapImpVol(double[] timeToStart, double[] timeAccrual, double[] df_fin, double[] fwd_r,
         double strike, double price, double sigmaGuess)
        {
            double error = 0.000001;
            double sigma = sigmaGuess;  // Simply a first guess
            double priceError;
            double dv = error + 1;
            double price_fwd_calculated = 0.0, vega_fwd_calculated = 0.0;

            int n = fwd_r.Length;
            while (Math.Abs(dv) > error)
            {
                price_fwd_calculated = 0.0;
                vega_fwd_calculated = 0.0;
                for (int i = 0; i < n; i++)
                {
                    price_fwd_calculated +=
                        CapletBlack(timeToStart[i], timeAccrual[i], 1, strike, sigma, df_fin[i], fwd_r[i]);
                    vega_fwd_calculated +=
                         CapletVega(timeToStart[i], timeAccrual[i], df_fin[i], fwd_r[i], sigma, strike);
                };
                priceError = price_fwd_calculated - price;
                dv = priceError / vega_fwd_calculated;
                sigma -= dv;
            };
            return sigma;
        }

         // Brigo, D. and Mercurio, F. 2001. Formula (1.28) (1.29) page 20
        public static double Swaption(double N, double S, double K, double sigma, double T, bool isPayer, double[] yf, double[] df)
        {
            int PoR = -1;  // Payer Or Receiver
            if (isPayer) PoR = 1;
            double A = 0.0;  // Sum of each yf*df
             // assuming yf and df having same length
            for (int i = 0; i < yf.Length; i++)
            {
                A += yf[i] * df[i];
            }
            return N * Black(K, S, sigma * Math.Sqrt(T), PoR) * A;
        }

         // Brigo, D. and Mercurio, F. 2006. Formula (1.28) (1.29) page 20
        public static double Swaption(double N, double S, double K, double sigma, double T, bool isPayer, double A)
        {
            int PoR = -1;  // Payer Or Receiver
            if (isPayer) PoR = 1;
            return N * Black(K, S, sigma * Math.Sqrt(T), PoR) * A;
        }

        public static double Swaption(double N, double K, string Start, string SwapTenor, bool isPayer, BilinearInterpolator VolMatrix, IRateCurve Curve)
        {
            Date refDate = Curve.RefDate(); // curve ref date
             // false/true with fwd swap matrix
            Date startDate = refDate.add_period(Start, false); // swap start 2 business days after the expiry
            Date expDate = startDate.add_workdays(-2);  // expiry of swaption
            Date today = refDate.add_workdays(-2);
            double T = today.YF_365(expDate);
            Period p = new Period(SwapTenor); // should be in year 1Y, 2Y (not 3m,...)
            double sigma = VolMatrix.Solve(T, p.tenor);
            return Swaption(N, K, Start, SwapTenor, isPayer, sigma, Curve);
        }

        public static double Swaption(double N, double K, string Start, string SwapTenor, bool isPayer, double sigma, IRateCurve Curve)
        {
            Date refDate = Curve.RefDate(); // curve ref date
             // false/true with fwd swap matrix
            Date startDate = refDate.add_period(Start, false); // swap start 2 business days after the expiry
            Date expDate = startDate.add_workdays(-2);  // expiry of swaption
            Date today = refDate.add_workdays(-2);
            double T = today.YF_365(expDate);
            Period p = new Period(SwapTenor); // should be in year 1Y, 2Y (not 3m,...)
            SwapStyle y = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(startDate, 0, SwapTenor, Curve.GetSwapStyle().buildingBlockType);
            double[] yf = y.scheduleLeg1.GetYFVect(Dc._30_360);
            double[] df = (from payDay in y.scheduleLeg1.payDates
                           select Curve.Df(payDay)).ToArray();

            return Swaption(N, Curve.SwapFwd(startDate, SwapTenor), K, sigma, T, isPayer, yf, df);
        }

        #endregion
       
         // The approximation to the cumulative normal distribution
        public static double CND(double x)
        {
             // This algorithm is presented in:
             // Abramowitz, M. & Stegun, I. A. (1974), Handbook of Mathematical Functions, With Formulas, Graphs, and Mathematical Tables, Dover.
             // Haug, E. 2007., 
             // Hull, J.C. 2010.
            
             // For accurate algorithms, refers also to: 
             // Acklam, P. J. (2004), `An algorithm for computing the inverse normal cumulative distribution function'. http: // home.online.no/~pjacklam
             // Hart, J. (1968), Computer Approximations, Wiley. Algorithm 5666 for the error function.
             // West, G Better approximations to cumulative normal functions. Wilmott Magazine May 2005

            double s = x;
            if (x > 0) s = x;
            else s = -x;
            double[] a = { 0.31938153, -0.356563782, 1.781477937, -1.821255978, 1.330274429 };
            double k = 1 / (1 + 0.2316419 * s);
            double outPut = 1 - 1 / Math.Sqrt(2 * Math.PI) * Math.Exp(-s * s / 2) 
                * (a[0] * k + a[1] * k * k + a[2] * k * k * k + a[3] * k * k * k * k + a[4] * k * k * k * k * k);

            if (x < 0) outPut = 1 - outPut;

            return outPut;
        }

        static public double CND_1(double x)
        {
             // Byrc 2001B, Journal of Statistical Research 2007, Vol. 41, No. 1, pp. 59–67 Bangladesh
             // APPROXIMATING THE CUMULATIVE DISTRIBUTION FUNCTION OF THE NORMAL DISTRIBUTION
            double a = x * x + 5.575192695 * x + 12.77436324;
            double b = x * x * x * Math.Sqrt(2.0 * Math.PI) + 14.38718147 * x * x + 31.53531977 * x + 25.548726;
            return 1.0 - (a / b) * Math.Exp(-x * x * .5);
        }        

        static public double CND_2(double x)
        {
             // Badhe (1976) with Chebyshev economization 
             // Johnson, Kotz, Balakrishnan, 1994. Continuous Univariate Distributions. Vol.1 , Wiley                
            double Y = x * x / 32;
            double a = 0.3989422784;
            double b = -2.127690079;
            double c = 10.2125662121;
            double d = -38.8830314909;
            double e = 120.2836370787;
            double f = -303.2973153419;
            double g = 575.073131917;
            double h = -603.9068092058;
            return 0.5 + x * (a + Y * (b + Y * (c + Y * (d + Y * (e + Y * (f + Y * (g + h * Y)))))));
        }

        public static double CND_3(double x)
        {
             // Moran (1980), Johnson, Kotz, Balakrishnan, 1994. Continuous Univariate Distributions. Vol.1 , Wiley    
            double y = 0.0;
            for (int n = 0; n <= 12; n++)
            {
                y += Math.Pow(n + 0.5, -1) * Math.Exp(-Math.Pow(n + 0.5, 2) / 9) * Math.Sin((Math.Sqrt(2) / 3) * (n + 0.5) * x);
            }
            return 0.5 + (1.0 / Math.PI) * y;
        }

        static public double CND_4(double x)
        {
             // Marsaglia 2004, Journal of Statistical Software July 2004, Volume 11, Issue 4.
            double s = x;
            double t = 0.0;
            double b = x;
            double q = x * x;
            double i = 1.0;
            while (s != t) s = (t = s) + (b *= q / (i += 2));
            return .5 + s * Math.Exp(-.5 * q - .91893853320467274178);
        }
       
         // Normal Distribution       
        public static double ND(double x)
        {
            return 1.0 / Math.Sqrt(2.0 * Math.PI) * Math.Exp(-x * x * 0.5);  // Excel
        }

         // From Datasim vectors to C# array
        public static double[] FromVect(Vector<double> V)
        {
            double[] output = new double[V.Length];
            int n = 0;
            for (int i = V.MinIndex; i <= V.MaxIndex; i++)
            {
                output[n] = V[i];
                n++;
            }
            return output;
        }

         // NPV of swap
        public static double NPV(SwapStyle BB, IRateCurve c, bool PayOrRec)
        {
            #region FixLeg
             // fixed leg data
            double[] yfFixLeg = BB.scheduleLeg1.GetYFVect(BB.swapLeg1.DayCount); // fixed is leg 1

             // dfs array of fixed lag
            Date[] dfDates = BB.scheduleLeg1.payDates; // serial date of fixed lag (each dates we should find df)

             // # of fixed cash flows
            int n_fix = dfDates.Length;

            double NPV_fix = 0.0;
             // calculate df
            for (int i = 0; i < n_fix; i++)
            {
                NPV_fix += c.Df(dfDates[i]) * yfFixLeg[i] * BB.rateValue;  // df*yf
            }
            #endregion

            #region FloatLeg
             // floating leg data
            double[] yfFloatLeg = BB.scheduleLeg2.GetYFVect(BB.swapLeg2.DayCount); // float is leg 2

             // dfs array of fixed lag
            Date[] dfDatesFloat = BB.scheduleLeg2.payDates; // serial date of float leg (each dates we should find df)

            Date[] FromDateFloat = BB.scheduleLeg2.fromDates;

             // # of floating cash flows
            int n_float = dfDatesFloat.Length;

            double[] fwd = new double[n_float]; // fwd rates container

             // getting fwd rates
            for (int i = 0; i < n_float; i++)
            {
                fwd[i] = c.Fwd(FromDateFloat[i]);
            }

            double NPV_float = 0.0;
             // calculate df
            for (int i = 0; i < n_float; i++)
            {
                NPV_float += c.Df(dfDatesFloat[i]) * yfFloatLeg[i] * fwd[i];  // df*yf
            }

            #endregion
            if (!PayOrRec) { return -NPV_fix + NPV_float; } // Receiver Swap
            return NPV_fix - NPV_float;  // NPV Payer Swap
        }
       
        public static double FwdBasis(Date StartDate, string SwapTenor, IRateCurve Curve1, IRateCurve Curve2)
        {                  
            SwapStyle S1 = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(StartDate, 0.0, SwapTenor, Curve1.GetSwapStyle().buildingBlockType);
            SwapStyle S2 = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(StartDate, 0.0, SwapTenor, Curve2.GetSwapStyle().buildingBlockType);
         
            double[] yfFloatLeg1 = S1.scheduleLeg2.GetYFVect(S1.swapLeg2.DayCount);

            // using LINQ syntax
            double[] dfFloatLeg1 = (from d in S1.scheduleLeg2.payDates
                                    select Curve1.Df(d)).ToArray<double>();
            double[] fwdFloatLeg1 = (from d in S1.scheduleLeg2.fromDates
                                     select Curve1.Fwd(d)).ToArray<double>();

            double[] yfFloatLeg2 = S2.scheduleLeg2.GetYFVect(S2.swapLeg2.DayCount);
            // using LINQ syntax
            double[] dfFloatLeg2 = (from d in S2.scheduleLeg2.payDates
                                    select Curve2.Df(d)).ToArray<double>();

            double[] fwdFloatLeg2 = (from d in S2.scheduleLeg2.fromDates
                                     select Curve2.Fwd(d)).ToArray<double>();

            return SpreadBasisFormula(yfFloatLeg1, dfFloatLeg1, fwdFloatLeg1,
            yfFloatLeg2, dfFloatLeg2, fwdFloatLeg2);
        }
    }


