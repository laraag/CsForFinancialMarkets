// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// ICalculator.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace XLFormulas
{

    /// <summary>
    /// ICalculator interface.
    /// By default a dual interface is created which can be used for both early binding and late binding.
    /// </summary>
    [ComVisible(true)]                                  // Makes the interface visible in COM regardless of the assembly COM visible attribute.
    [Guid("879B339D-196D-450f-A6C2-1370106ACCB6")]      // Explicit GUID for the interface. 
    public interface ICalculator
    {
        // more details on each function in XLBond class        

        #region BondBasic.
        // will add a period starting from a date (as a serial number)
        double AddPeriod(double StartDate, string period, bool adjust);

        // will return some descriptive data on BondBTP as example
        double[,] BondBTPData(double Today, double StartDateBond, double EndDateBond, double Coupon, double CleanPrice);

        // will return yield on BondBTP according to given Freq, DayCount, compounding
        double BondBTPYield(double Today, double StartDateBond, double EndDateBond, double Coupon, double CleanPrice, int Freq, string DayCount, string compounding);

        // will return yield on BondZeroCoupon according to given Freq, DayCount, compounding
        double BondZeroCouponYield(double Today, double StartDateBond, double EndDateBond, int SettlementDaysLag, string RollAdj, string PayAdj, string LagPayFromRecordDate, double FaceValue, double CleanPrice, int Freq, string DayCountYield, string Compounding);

        // will return yield on BondFixedCoupon according to given Freq, DayCount, compounding
        double BondFixedCouponYield(double Today, double StartDateBond, double EndDateBond, double Coupon, string CouponTenor,
        string RuleGenerator, int SettlementDaysLag, string RollAdj, string PayAdj, string DayCount, string LagPayFromRecordDate, double FaceValue, double CleanPrice,
            int Freq, string DayCountYield, string Compounding);

        // will return yield on BondMultiCoupon according to given Freq, DayCount, compounding
        object BondMultiCouponYield(double Today, double StartDateBond, double EndDateBond, Excel.Range Coupons, string CouponTenor,
        string RuleGenerator, int SettlementDaysLag, string RollAdj, string PayAdj, string DayCount, string LagPayFromRecordDate, double FaceValue, double CleanPrice,
            int Freq, string DayCountYield, string Compounding);

        // will return a column vector of coupon payment date, useful for multi coupon bond
        double[,] GetCouponDates(double StartDateBond, double EndDateBond, string CouponTenor, string RuleGenerator, string RollAdj,
            string LagPayFromRecordDate, string PayAdj);
        #endregion

        #region LoadInMemory.
        // will Load in memory (populate BondDictionary)some bond description using as key idCode
        string LoadBondFixedCoupon(string idCode, double Today, double StartDateBond, double EndDateBond, double Coupon, string BondFixedCouponType);

        // given a clean price and idCode will return the yield of the corresponding bond in BondDictionary
        object YieldFromDictionary(string idCode, double Today, double CleanPrice, int Freq, string DayCount, string Compounding);

        // given yield return clean price: look for bond in dictionary calculate clean price starting from yield
        object CleanFromYieldFromDictionary(string idCode, double Today, double Yield, int Freq, string DayCount, string Compounding);
        #endregion

        #region BondSerialization.

        #region BondFormulas.

        // look for bond in serialized file
        object YieldFromFile(string idCode, double Today, double CleanPrice, int Freq, string DayCount, string Compounding, string FileFullName);

        // look for bond in serialized file calculate clean price starting from yield
        object CleanFromYieldFromFile(string idCode, double Today, double Yield, int Freq, string DayCount, string Compounding, string FileFullName);

        #endregion

        #region File and serialized dictionary utilities.

        string LoadBondFixedCouponSerialLoadBondFixedCoupon(string idCode, double Today, // Add a bond to dictionary
            double StartDateBond, double EndDateBond, double Coupon, string BondFixedCouponType, string FileFullPath);

        int ClearDictionary(); // Clear the dictionary

        string UpdateSerializedDictionary(string FileFullPath); // Serialize a fixed coupon bond on a file

        string UpdateToday(double Today, string FileFullName, bool OnOff); // Update reference date of the dictionary

        string FileDelete(string FileFullName); // Delete a serialization file 

        object[] ReportOnDictionary(string FileFullName, bool OnOff); // Getting information from the file

        string CheckCode(string idCode, string FileFullName, bool OnOff); // check id idCode is in dictionary

        #endregion

        #endregion
    }

}
