using System;
//For demonstration purposes only, it comes with no warranty and guarantee. No liability is accepted by the Authors for the use of this code in any circumstances.
public class Bond
{
        // Bond delegates to interest rate algorithms
    InterestRateCalculator eng;

        // Use redundant data to save delegating to InterestRateCalculator
    private double r;        // Interest rate
    private int nPeriods;    // Number of periods
    private double c;        // Cash coupon payment

    public Bond(int numberPeriods, double interest, double Coupon, int paymentPerYear)
    {
        nPeriods = numberPeriods;
        r = interest / (double)paymentPerYear;
        c = Coupon;
        eng = new InterestRateCalculator(nPeriods, r);
    }

    public Bond(InterestRateCalculator irCalculator, double Coupon, int paymentPerYear)
    {
        eng = irCalculator;
        c = Coupon;

        nPeriods = eng.NumberOfPeriods;
        r = eng.Interest / (double)paymentPerYear;
    }

        // Price by adding 1) present value of coupon payments 2) PV of par/maturity
        // at maturity date according to payments frequency
    public double price(double redemptionValue)
    {
            // present value of coupon payments
        double pvCoupon = eng.PresentValueConstant(c);

            // present value of redemption value
        double pvPar = eng.PresentValue(redemptionValue);
        return pvCoupon + pvPar;
    }
}
