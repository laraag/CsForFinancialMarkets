// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// VanillaCap.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class VanillaCapFloor 
{
    public double[] yf;  // year fraction
    public double[] df;  // df
    public double[] T;  // expiry
    public double[] fwd;  // fwd rates
    public double N;  // nominal
    public bool IsCap;  // true= cap, false = floor
    public BilinearInterpolator volMatrix;  // caplet floret vol matrix
    public IRateCurve rateCurve;  // rate curve
    string stringTenor;
    public Schedule capSchedule;

    public VanillaCapFloor(string tenor, IRateCurve curve, BilinearInterpolator capletVolMatrix, double nominal)
    {
        Ini(tenor, curve, capletVolMatrix, nominal);
    }

     // used in constructor
    private void Ini (string tenor, IRateCurve curve, BilinearInterpolator capletVolMatrix,double nominal)
    {
        stringTenor = tenor;
        rateCurve = curve;
        volMatrix = capletVolMatrix;
        N = nominal;
        
         // yf of longer cap            
        SwapStyle y = (SwapStyle)new BuildingBlockFactory().CreateBuildingBlock(curve.RefDate(), 0, tenor, curve.GetSwapStyle().buildingBlockType);
        yf = y.scheduleLeg2.GetYFVect(Dc._Act_360);
        capSchedule = y.scheduleLeg2;
        int toRemove = yf.Length - 1;
        yf = yf.Where((val, inx) => inx != toRemove).ToArray();

         // T all tenor needed. They are more than input 
        Date refDate = curve.RefDate();
        List<Date> ToDate = y.scheduleLeg2.toDates.ToList();
        ToDate.RemoveAt(ToDate.Count - 1);  // remove last element
        T = (from c in ToDate
             select refDate.YF_365(c)).ToArray();
         // df- getting relevant dates
        Date[] dfDate = y.scheduleLeg2.payDates;
        int Ncaplet = yf.Length;  // number of caplets
        df = new double[Ncaplet];
         // fwd rate
        fwd = new double[Ncaplet];
        Date[] fwdDate = y.scheduleLeg2.fromDates;

        for (int i = 0; i < Ncaplet; i++)  // Note the loop start from 1
        {    // first discount factor is on first payment date of caplet (first caplet skipped)
            df[i] = curve.Df(dfDate[i + 1]);
            fwd[i] = curve.Fwd(fwdDate[i + 1]);
        }
    }

     // Update rate curve
    public virtual void SetNewRateCurve(IRateCurve newCurve)
    {
        Ini(stringTenor, newCurve, volMatrix, N);
    }

     // Update VolMatrix
    public virtual void SetNewVolMatrix(BilinearInterpolator newCapletVolMatrix)
    {
        Ini(stringTenor,rateCurve, newCapletVolMatrix, N);
    }

     // Derived classes must implement this method
    abstract public double Price(double strike);  // will use sigma from vol matrix
    abstract public double Price(double sigma, double strike);  // cap sigma
    abstract public double Price(double[] sigma, double strike);  // array of sigma
    abstract public double Price(BilinearInterpolator CapletVolMatrix, double strike);  // you can customize it 
    abstract public double ImplVolFromPrice(double price, double strike, double guess);
}

public class VanillaCap: VanillaCapFloor
{
     // Constructor
    public VanillaCap(string tenor, IRateCurve curve, BilinearInterpolator capletVolMatrix, double nominal)
        : base(tenor, curve, capletVolMatrix,nominal)
    {
        IsCap = true;  // if true is a cap
    }

    public override double Price(double strike)
    {  // will use sigma from vol matrix
        return Formula.CapBlack(T, yf, N, strike, volMatrix, df, fwd);
    }
    public override double Price(double sigma, double strike)
    {  // cap sigma
        return Formula.CapBlack(T,yf,N,strike,sigma,df,fwd);
    }
    
    public override double Price(double[] sigma, double strike)
    {
         // array of sigma
        return Formula.CapBlack(T, yf, N, strike, sigma, df, fwd);
    }

    public override double Price(BilinearInterpolator CapletVolMatrix, double strike)
    {
        return Formula.CapBlack(T, yf, N, strike, CapletVolMatrix,df,fwd);
    }

    public override double ImplVolFromPrice(double price, double strike, double guess) 
    {
        return Formula.CapImpVol(T, yf, df, fwd, strike, price/N, guess);
    }
}