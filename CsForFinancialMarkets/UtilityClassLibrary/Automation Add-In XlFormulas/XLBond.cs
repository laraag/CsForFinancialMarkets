// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// XLBond.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections;
using Excel=Microsoft.Office.Interop.Excel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace XLFormulas
{

     // / <summary>
     // / Calculator automation add-in.
	 // / Derived from ProgrammableBase to add the "Programmable" sub key to the registry.
	 // / 
	 // / We explicitly set the ProgID and GUID. When not given they will be automatically generated.
	 // / The automatically generated ProgID will be "Namespace name"."Class name".
	 // / The automatically generated GUID will be derived from the assembly name, namespace name, class name and assembly version number.
	 // / This gives versioning problems when the version changes, so we specify it explicitly.
	 // / 
	 // / This time we use the ClassInterface.None option so no COM interface for this class will be generated. 
	 // / Instead we implement a COM interface. The ProgrammableBase class does not have to be COM visible now.
	 // / This has two advantages, first only the functions in the interface are exposed and not the functions from object.
	 // / Secondly, we don't have versioning problems with early binding clients as long as the interface doesn't change.
	 // / Any changes must be done in a new interface.
     // / </summary>
    /// </summary>   
    [ComVisible(true)]                                   // Makes the class visible in COM regardless of the assembly COM visible attribute.
    [ProgId("XLFormulas.Bond")]                          // Explicit ProgID. 
    [Guid("784C9CF3-A589-4bf1-89E3-37B041396D30")]       // Explicit GUID. 
    [ClassInterface(ClassInterfaceType.None)]            // We implement COM interfaces.
  [Serializable]
        public class XLBond: ProgrammableBase, ICalculator, Extensibility.IDTExtensibility2
    {
         // static dictionary populates using LoadBondFixedCoupon: if you use different session of excel you will loose BondDictionaryData 
         // it is used as example: it is better to use xml or serialize dictionary
        static Dictionary<string, BaseBond> BondDictionaryData = new Dictionary<string, BaseBond>();

		 // The Excel application. Used in volatile worksheet functions.
		private Excel.Application m_xlApp=null;
        
       #region BondBasic.
         // will add a period to a Date
        double ICalculator.AddPeriod(double StartDate, string period, bool adjust)
        {
            return (new Date(StartDate).add_period(period, adjust)).SerialValue;
        }
        
         // will return some descriptive data on BondBTP as example
        double[,] ICalculator.BondBTPData(double Today, double StartDateBond, double EndDateBond, double Coupon, double CleanPrice)
        {
             // make it volatile
            if (m_xlApp != null) m_xlApp.Volatile(true); 
            
             // initialise the bond
            BondBTP b = new BondBTP(new Date(Today), new Date(StartDateBond), new Date(EndDateBond), Coupon);

             // will return column with 6 element
            double[,] outPut = new double[6, 1];
            outPut[0, 0] = b.GetSettlementDate().SerialValue; // settlement date of the bond
            outPut[1, 0] = b.GetLastCouponDate().SerialValue; // last coupon date of the bond
            outPut[2, 0] = b.GetNextCouponDate().SerialValue; // next coupon date of the bond
            outPut[3, 0] = b.GetCurrentCoupon();  // current coupon rate
            outPut[4, 0] = b.DirtyPrice(CleanPrice);  // dirty price of the bond
            outPut[5, 0] = b.AccruedInterest();  // accrued interest of the bond as amount
            return outPut;  // here output
        }

         // will return yield on BondBTP according to given Freq, DayCount, compounding
        double ICalculator.BondBTPYield(double Today, double StartDateBond, double EndDateBond, double Coupon, double CleanPrice, int Freq, string DayCount, string Compounding)
        {
             // Freq, DayCount, compounding refer to yield calculation 

             // make it volatile
            if (m_xlApp != null) m_xlApp.Volatile(true);
            
             // initialise the bond
            BondBTP b = new BondBTP(new Date(Today), new Date(StartDateBond), new Date(EndDateBond), Coupon);
            
             // Parse enum Type
            Dc dc = (Dc)Enum.Parse(typeof(Dc), DayCount);
            Compounding comp = (Compounding)Enum.Parse(typeof(Compounding), Compounding);
            
             // return the yield
            return b.Yield(CleanPrice, Freq, dc, comp);
        }
        
         // will return yield on BondZeroCoupon
        double ICalculator.BondZeroCouponYield(double Today, double StartDateBond, double EndDateBond, int SettlementDaysLag, string RollAdj, string PayAdj,
        string LagPayFromRecordDate, double FaceValue, double CleanPrice, int Freq, string DayCountYield, string Compounding) 
        {
             // for details of input refer to comments on BondZeroCoupon class
             // make it volatile
            if (m_xlApp != null) m_xlApp.Volatile(true);

             // Parse enum Type
            BusinessDayAdjustment rollAdj = (BusinessDayAdjustment)Enum.Parse(typeof(BusinessDayAdjustment), RollAdj);
            BusinessDayAdjustment payAdj = (BusinessDayAdjustment)Enum.Parse(typeof(BusinessDayAdjustment), PayAdj);           
            Dc dcy = (Dc)Enum.Parse(typeof(Dc), DayCountYield);
            Compounding comp = (Compounding)Enum.Parse(typeof(Compounding), Compounding);

             // initialise zc bond
            BondZeroCoupon zc = new BondZeroCoupon(new Date(Today), new Date(StartDateBond), new Date(EndDateBond),
                SettlementDaysLag, rollAdj, payAdj, LagPayFromRecordDate, FaceValue);
            
             // return the yield
            return (double)zc.Yield(CleanPrice, Freq, dcy, comp);
        }

         // will return yield on BondFixedCoupon
        double ICalculator.BondFixedCouponYield(double Today, double StartDateBond, double EndDateBond, double Coupon, string CouponTenor,
        string RuleGenerator, int SettlementDaysLag, string RollAdj, string PayAdj, string DayCount, string LagPayFromRecordDate, double FaceValue, double CleanPrice,
            int Freq, string DayCountYield, string Compounding) 
        {
             // for details of input refer to comments on BondFixedCoupon class
             // make it volatile
            if (m_xlApp != null) m_xlApp.Volatile(true);

             // Parse enum Type                
            Rule ruleGenerator = (Rule)Enum.Parse(typeof(Rule), RuleGenerator);
            BusinessDayAdjustment rollAdj = (BusinessDayAdjustment)Enum.Parse(typeof(BusinessDayAdjustment), RollAdj);
            BusinessDayAdjustment payAdj = (BusinessDayAdjustment)Enum.Parse(typeof(BusinessDayAdjustment), PayAdj);
            Dc dc = (Dc)Enum.Parse(typeof(Dc), DayCount);
            Dc dcy = (Dc)Enum.Parse(typeof(Dc), DayCountYield);
            Compounding comp = (Compounding)Enum.Parse(typeof(Compounding), Compounding);
            
             // initialise bond
            BondFixedCoupon bfc = new BondFixedCoupon(new Date(Today), new Date(StartDateBond), new Date(EndDateBond)
                    , Coupon, CouponTenor, ruleGenerator, SettlementDaysLag, rollAdj, payAdj, dc, LagPayFromRecordDate, FaceValue);
            
             // return the yield
            return (double)bfc.Yield(CleanPrice, Freq, dcy, comp);
        }
        
         // will return yield on BondMultiCoupon 
        object ICalculator.BondMultiCouponYield(double Today, double StartDateBond, double EndDateBond, Excel.Range Coupons, string CouponTenor,
        string RuleGenerator, int SettlementDaysLag, string RollAdj, string PayAdj,
        string DayCount, string LagPayFromRecordDate, double FaceValue, double CleanPrice, int Freq, string DayCountYield, string Compounding)
        {
             // for details of input refer to comments on BondMultiCoupon class
             // make it volatile
            if (m_xlApp != null) m_xlApp.Volatile(true);
            try
            { 
                 // Parse enum Type                
                Rule ruleGenerator = (Rule)Enum.Parse(typeof(Rule), RuleGenerator);
                BusinessDayAdjustment rollAdj = (BusinessDayAdjustment)Enum.Parse(typeof(BusinessDayAdjustment), RollAdj);
                BusinessDayAdjustment payAdj = (BusinessDayAdjustment)Enum.Parse(typeof(BusinessDayAdjustment), PayAdj);
                Dc dc = (Dc)Enum.Parse(typeof(Dc), DayCount);
                Dc dcy = (Dc)Enum.Parse(typeof(Dc), DayCountYield);                
                Compounding comp = (Compounding)Enum.Parse(typeof(Compounding), Compounding);
                
                 // create the schedule to how many coupon dates I need
                Schedule sch = new Schedule(new Date(StartDateBond), new Date(EndDateBond), CouponTenor, ruleGenerator, rollAdj, LagPayFromRecordDate, payAdj);
                int rows = Coupons.Rows.Count;  // number of element in my input array
                int n = sch.toDates.GetLength(0);  // number of coupons
                List<double> coupons = new List<double>();  // list containing needed coupon
                
                 // iterate my input array to get no empty cell
                for (int r=1; r<=rows; r++)
				{
					 // Get the value of the current cell as double and add to running result.
                    if ((Coupons[r, 1] as Excel.Range).Value2 != null) // if the cell is not empty
                    { 
                        coupons.Add((double)(Coupons[r, 1] as Excel.Range).Value2); 
                    }                    
				}

                 // usable input coupon in coupons list should equal the number of needed coupon
                if (n != coupons.Count) return "Invalid coupons number";

                 // initialise BondMultiCoupon
                BondMultiCoupon bmc = new BondMultiCoupon(new Date(Today), new Date(StartDateBond), new Date(EndDateBond)
                    , coupons.ToArray(), CouponTenor, ruleGenerator, SettlementDaysLag, rollAdj, payAdj, dc, LagPayFromRecordDate, FaceValue);
                
                 // return the yield
                return (double) bmc.Yield(CleanPrice, Freq, dcy, comp);                
            }
            catch (Exception e) // exception
            {
                return (string)e.ToString();
            }
        }

         // will return a column vector of coupon payment date
       double[,] ICalculator.GetCouponDates(double StartDateBond, double EndDateBond, string CouponTenor, string RuleGenerator, string RollAdj, 
            string LagPayFromRecordDate, string PayAdj)
        {
             // for details of inputs refer to comments on BondBase class
             // make it volatile
            if (m_xlApp != null) m_xlApp.Volatile(true);

             // Parse enum Type                
            Rule ruleGenerator = (Rule)Enum.Parse(typeof(Rule), RuleGenerator);
            BusinessDayAdjustment rollAdj = (BusinessDayAdjustment)Enum.Parse(typeof(BusinessDayAdjustment), RollAdj);
            BusinessDayAdjustment payAdj = (BusinessDayAdjustment)Enum.Parse(typeof(BusinessDayAdjustment), PayAdj);
            
             // Create a schedule
            Schedule sch = new Schedule(new Date(StartDateBond), new Date(EndDateBond), CouponTenor, ruleGenerator, rollAdj, LagPayFromRecordDate, payAdj);
            int n = sch.toDates.GetLength(0);  // number of payment dates
            double[] serialDate = Date.GetSerialValue(sch.toDates);  // get dates and cast to double
            double[,] outPut = new double[n, 1];  // fill a matrix to return in excel
            for (int i = 0; i < n; i++ )
            {
                outPut[i, 0] =serialDate[i];
            }
            return outPut; // my serial coupon dates
        }
       #endregion

       #region LoadInMemory.
        // will Load in memory (populate BondDictionaryData)some bond description using as key idCode
       string ICalculator.LoadBondFixedCoupon(string idCode, double Today, double StartDateBond, double EndDateBond, double Coupon, string BondFixedCouponType) 
       {
            // for details of inputs refer to comments on BondDBR,BondBTAN,BondBTP classes

            // as example, I decide to use only 3 type of bond DBR,BTAN,BTP (arbitrary strings) 
           BaseBond bond = null;
           if (BondFixedCouponType == "DBR")  // BondDBR
           {
               bond = new BondDBR(new Date(Today), new Date(StartDateBond), new Date(EndDateBond), Coupon);
           }
           else if (BondFixedCouponType == "BTAN")  // BondBTAN
           {
               bond = new BondBTAN(new Date(Today), new Date(StartDateBond), new Date(EndDateBond), Coupon);
           }
           else if (BondFixedCouponType == "BTP")  // BondBTP
           {
               bond = new BondBTP(new Date(Today), new Date(StartDateBond), new Date(EndDateBond), Coupon);
           }
           else if (BondFixedCouponType == "BOT")  // BondBOT
           {
               bond = new BondBOT(new Date(Today), new Date(StartDateBond), new Date(EndDateBond));
           }
           else 
           {
               return "Bond not recognized"; // if not managed
           }
           try
           {
               if (BondDictionaryData.ContainsKey(idCode) == true)  // check if idCode is in dictionary
               {
                   BondDictionaryData[idCode] = bond;  // if true, updates it
               }
               else
               {
                   BondDictionaryData.Add(idCode, bond);  // if false, adds it                   
               }
               return "Loaded @ " + DateTime.Now.ToString();  // return time of last load
           }
           catch (Exception e)
           {
               return (string)e.ToString();
           }
       }
       
         // given a clean price and idCode will return the yield of the corresponding bond in BondDictionaryData
       object ICalculator.YieldFromDictionary(string idCode, double Today, double CleanPrice, int Freq, string DayCount, string Compounding)
       {
            // int Freq, string DayCount, string Compounding refers to yield calculation
            // string idCode is the identification code of bond in BondDictionaryData

            // make it volatile
           if (m_xlApp != null) m_xlApp.Volatile(true);
           try
           {
               BaseBond bond = null; // initialise a bond
               if (BondDictionaryData.TryGetValue(idCode, out bond))  // is the idCode in dictionary?
               {
                    // update today since I the bond in dictionary can be the same but I can change my reference date to do simulations
                   bond.SetNewToDay(new Date(Today));

                    // Parse enum Type 
                   Dc dc = (Dc)Enum.Parse(typeof(Dc), DayCount);
                   Compounding comp = (Compounding)Enum.Parse(typeof(Compounding), Compounding);

                    // return the yield given parameters
                   return bond.Yield(CleanPrice, Freq, dc, comp);
               }
               else
               {
                   return "IdCode not found";  // bond not found
               }
           }
           catch (Exception e)
           {
               return (string)e.ToString();
           }
       }
       
         // given yield return clean price: look for bond in dictionary calculate clean price starting from yield
       object ICalculator.CleanFromYieldFromDictionary(string idCode, double Today, double Yield, int Freq, string DayCount, string Compounding) 
       {
            // int Freq, string DayCount, string Compounding refers to yield calculation
            // string idCode is the identification code of bond in BondDictionaryData

            // make it volatile
           if (m_xlApp != null) m_xlApp.Volatile(true);
           try
           {
               BaseBond bond = null; // initialise a bond
               if (BondDictionaryData.TryGetValue(idCode, out bond))  // is the idCode in dictionary?
               {
                    // update today since I the bond in dictionary can be the same but I can change my reference date to do simulations
                   bond.SetNewToDay(new Date(Today));

                    // Parse enum Type 
                   Dc dc = (Dc)Enum.Parse(typeof(Dc), DayCount);
                   Compounding comp = (Compounding)Enum.Parse(typeof(Compounding), Compounding);

                    // if  yield is zero return a message
                   if (Yield == 0) return (string)"YldZero";

                    // return clean price from yield given parameters
                   return bond.CleanPriceFromYield(Yield, Freq, dc, comp);
               }
               else
               {
                   return "IdCode not found";  // bond not found
               }
           }
           catch (Exception e)
           {
               return (string)e.ToString();
           }
       }        
       #endregion

       #region BondSerialization.

       static Dictionary<string, BaseBond> BondDictionarySerial = new Dictionary<string, BaseBond>();
       
        #region BondFormulas.
       // given a clean price and idCode will return the yield of the corresponding bond in BondDictionaryData
       object ICalculator.YieldFromFile(string idCode, double Today, double CleanPrice, int Freq, string DayCount, string Compounding, string FileFullName)
       {
           // int Freq, string DayCount, string Compounding refers to yield calculation
           // string idCode is the identification code of bond in BondDictionaryData

           // make it volatile
           if (m_xlApp != null) m_xlApp.Volatile(true);
           try
           {
               BaseBond bond = null; // initialise a bond
               // deserialize
               BondDictionary SD = new BondDictionary(@FileFullName);
               SD.DeSerialize();

               if (SD.dic.TryGetValue(idCode, out bond))  // is the idCode in dictionary?
               {
                   // update today since I the bond in dictionary can be the same but I can change my reference date to do simulations
                   bond.SetNewToDay(new Date(Today));

                   // Parse enum Type 
                   Dc dc = (Dc)Enum.Parse(typeof(Dc), DayCount);
                   Compounding comp = (Compounding)Enum.Parse(typeof(Compounding), Compounding);

                   // if CleanPrice is zero return a message
                   if (CleanPrice == 0) return (string)"PxZero";

                   // return the yield given parameters
                   return bond.Yield(CleanPrice, Freq, dc, comp);
               }
               else
               {
                   return "IdCode not found";  // bond not found
               }
           }
           catch (Exception e)
           {
               return (string)e.ToString();
           }
       }

       // given a clean price and idCode will return the yield of the corresponding bond in BondDictionaryData
       object ICalculator.CleanFromYieldFromFile(string idCode, double Today, double Yield, int Freq, string DayCount, string Compounding, string FileFullName)
       {
           // int Freq, string DayCount, string Compounding refers to yield calculation
           // string idCode is the identification code of bond in BondDictionaryData

           // make it volatile
           if (m_xlApp != null) m_xlApp.Volatile(true);
           try
           {
               BaseBond bond = null; // initialise a bond
               // deserialize
               BondDictionary SD = new BondDictionary(@FileFullName);
               SD.DeSerialize();

               if (SD.dic.TryGetValue(idCode, out bond))  // is the idCode in dictionary?
               {
                   // update today since I the bond in dictionary can be the same but I can change my reference date to do simulations
                   bond.SetNewToDay(new Date(Today));

                   // Parse enum Type 
                   Dc dc = (Dc)Enum.Parse(typeof(Dc), DayCount);
                   Compounding comp = (Compounding)Enum.Parse(typeof(Compounding), Compounding);

                   // if  yield is zero return a message
                   if (Yield == 0) return (string)"YldZero";

                   // return clean price from yield given parameters
                   return bond.CleanPriceFromYield(Yield, Freq, dc, comp);
               }
               else
               {
                   return "IdCode not found";  // bond not found
               }
           }
           catch (Exception e)
           {
               return (string)e.ToString();
           }
       }              
        #endregion
       
       #region File and serialized dictionary utilities.
       
        // Clean dictionary
       int ICalculator.ClearDictionary()
       {
           BondDictionarySerial.Clear();
           return 0;
       }

        // Update serialized dictionary
       string ICalculator.UpdateSerializedDictionary(string FileFullPath)
       {
           try
           {
               BondDictionary SD = new BondDictionary(@FileFullPath);
               SD.DeSerialize();
               foreach (KeyValuePair<string, BaseBond> k in BondDictionarySerial)
               {
                   string idCode = k.Key;
                   BaseBond bond = k.Value;
                   if (SD.dic.ContainsKey(idCode) == true)  // check if idCode is in dictionary
                   {
                       SD.dic[idCode] = bond;  // if true, updates it                 
                   }
                   else
                   {
                       SD.dic.Add(idCode, bond);  // if false, adds it                   
                   }
               }
               SD.Serialize(); // serialize it
               BondDictionarySerial.Clear();
               return "Dictionary Serialized @ " + DateTime.Now.ToString();  // return time of last load
           }
           catch (Exception e)
           {
               return (string)e.ToString();
           }
       }

        // Add fixed coupon bond to the dictionary
       string ICalculator.LoadBondFixedCouponSerialLoadBondFixedCoupon(string idCode, double Today, double StartDateBond, double EndDateBond, double Coupon, string BondFixedCouponType, string FileFullPath)
       {
           // for details of inputs refer to comments on BondDBR,BondBTAN,BondBTP classes

           // as example, I decide to use only 3 type of bond DBR,BTAN,BTP (arbitrary strings) 
           BaseBond bond = null;
           if (BondFixedCouponType == "DBR")  // BondDBR
           {
               bond = new BondDBR(new Date(Today), new Date(StartDateBond), new Date(EndDateBond), Coupon);
           }
           else if (BondFixedCouponType == "BTAN")  // BondBTAN
           {
               bond = new BondBTAN(new Date(Today), new Date(StartDateBond), new Date(EndDateBond), Coupon);
           }
           else if (BondFixedCouponType == "BTP")  // BondBTP
           {
               bond = new BondBTP(new Date(Today), new Date(StartDateBond), new Date(EndDateBond), Coupon);
           }
           else if (BondFixedCouponType == "BOT")  // BondBOT
           {
               bond = new BondBOT(new Date(Today), new Date(StartDateBond), new Date(EndDateBond));
           }
           else
           {
               return "Bond not recognized"; // if not managed
           }
           try
           {
               if (BondDictionarySerial.ContainsKey(idCode) == true)  // check if idCode is in dictionary
               {
                   BondDictionarySerial[idCode] = bond;  // if true, updates it
               }
               else
               {
                   BondDictionarySerial.Add(idCode, bond);  // if false, adds it

               }
               return "Loaded @ " + DateTime.Now.ToString();  // return time of last load
           }
           catch (Exception e)
           {
               return (string)e.ToString();
           }
       }

        // Update Today for each bond in the dictionary
       string ICalculator.UpdateToday(double NewToday, string FileFullName, bool OnOff)
       {
           if (OnOff)
           {
               try
               {
                   BondDictionary BD = new BondDictionary(FileFullName);
                   BD.DeSerialize();
                   BD.SetRefDate(new Date(NewToday));
                   BD.Serialize();
                   return "Updated @ " + DateTime.Now.ToString();  // return time of last load
               }
               catch (Exception e) // exception
               {
                   return (string)e.ToString();
               }
           }
           else
           {
               return "";
           }
       }
        
        // Delete a file
       string ICalculator.FileDelete(string FileFullName) 
       {
           try
           {	
	           if (File.Exists(FileFullName))
	           {
	               DialogResult dr = MessageBox.Show("Are you sure to delete: \n" + FileFullName, "Delete File", MessageBoxButtons.YesNo);
	               if (dr == DialogResult.Yes)
	               {
	                   File.Delete(FileFullName);
	                   return (string) "File deleted";
	               }
	               else
	               {
	                   return (string)"Nothing done";
	
	               }
	           }
	           else
	           {
	               return (string)"File not found";
               }
           }
           catch (Exception e) 
           {
               return (string)e.ToString();
           }
       }       

        // Get information on file containing serialized dictionary
       object[] ICalculator.ReportOnDictionary(string FileFullName, bool OnOff) 
       {
           object[] output = new object[3];
           if (OnOff)
           {
               if (File.Exists(FileFullName))
               {
                   try
	               {
	                   BondDictionary SD = new BondDictionary(@FileFullName);
	                   SD.DeSerialize();
	
	                   // blank dictionary
	                   if (SD.dic.Count == 0)
	                   {
	                       output[0] = (string)"Empty Dictionary";
	                   }
	                   else
	                   {
	                       output[0] = (int)SD.dic.Count;
	
	                       FileInfo fi = new FileInfo(@FileFullName);
	                       output[1] = fi.LastWriteTime.ToString();
	
	                       double? date = null;
	
	                       foreach (KeyValuePair<string, BaseBond> b in SD.dic)
	                       {
	                           if (date == null)
	                           {
	                               date = b.Value.GetToday().SerialValue;
	                           }
	                           if (date == b.Value.GetToday().SerialValue)
	                           {
	                               date = b.Value.GetToday().SerialValue;
	                           }
	                           else
	                           {
	                               output[2] = (string)"Not equal refDates";
	                           }
	                           output[2] = date;
	                       }
	                   }                   
	               }
	               catch (Exception e) // exception
	               {
	                   output[0] = (string)e.ToString();
	               }
                   
               }
               else
               {
                   output[0] = (string) "File not found";
               }
               return output;               
           }
           else { return output; }         
       }

        // Look up a bond code in the dictionary
        string ICalculator.CheckCode(string idCode, string FileFullName, bool OnOff) 
       {
           if (OnOff)
           {
               try
               {
                   BondDictionary BD = new BondDictionary(FileFullName);
                   
                   BD.DeSerialize();
                   return BD.dic.ContainsKey(idCode).ToString();  // check for idCode
               }
               catch (Exception e) // exception
               {
                   return (string)e.ToString();
               }
           }
           else
           {
               return "";
           }
       
       }

      #endregion
       
       #endregion
       
       #region IDTExtensibility2 Members

		 // / <summary>
		 // / Implements the OnConnection method of the IDTExtensibility2 interface.
		 // / Receives notification that the Add-in is being loaded.
		 // / </summary>
		 // / <param name="application">Root object of the host application.</param>
		 // / <param name="connectMode">Describes how the Add-in is being loaded.</param>
		 // / <param name="addInInst">Object representing this Add-in.</param>
		 // / <param name="custom">Array of parameters that are host application specific.</param>
		void Extensibility.IDTExtensibility2.OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			 // Store reference to the Excel host application (m_xlApp will become null if not loaded by Excel).
			m_xlApp=application as Excel.Application;
		}

		 // / <summary>
		 // / Implements the OnDisconnection method of the IDTExtensibility2 interface.
		 // / Receives notification that the Add-in is being unloaded.
		 // / </summary>
		 // / <param name="removeMode">Describes how the Add-in is being unloaded.</param>
		 // / <param name="custom">Array of parameters that are host application specific.</param>
		void Extensibility.IDTExtensibility2.OnDisconnection(Extensibility.ext_DisconnectMode removeMode, ref Array custom)
		{
			 // Empty implementation.
		}
		 // / <summary>
		 // / Implements the OnAddInsUpdate method of the IDTExtensibility2 interface.
		 // / Receives notification that the collection of Add-ins has changed.
		 // / </summary>
		 // / <param name="custom">Array of parameters that are host application specific.</param>
		void Extensibility.IDTExtensibility2.OnAddInsUpdate(ref Array custom)
		{
			 // Empty implementation.
		}

		 // / <summary>
		 // / Implements the OnStartupComplete method of the IDTExtensibility2 interface.
		 // / Receives notification that the host application has completed loading.
		 // / </summary>
		 // / <param name="custom">Array of parameters that are host application specific.</param>
		void Extensibility.IDTExtensibility2.OnStartupComplete(ref Array custom)
		{
			 // Empty implementation.
		}
		
		 // / <summary>
		 // / Implements the OnBeginShutdown method of the IDTExtensibility2 interface.
		 // / Receives notification that the host application is being unloaded.
		 // / </summary>
		 // / <param name="custom">Array of parameters that are host application specific.</param>
		void Extensibility.IDTExtensibility2.OnBeginShutdown(ref Array custom)
		{
			 // Empty implementation.
		}
		#endregion
	}
}


