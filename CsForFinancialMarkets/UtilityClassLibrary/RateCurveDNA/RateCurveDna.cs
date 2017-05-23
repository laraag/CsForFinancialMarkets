// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// RateCurveDna.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Security.Permissions;
using ExcelDna.Integration;

namespace RateCurveDna
{
    public class RateCurveDna : IExcelAddIn
    {
         // Excel Auto Open
      public void AutoOpen()
      {
          Inizialize();         
      }
         // Excel Auto Close
      public void AutoClose()
      {
           // Only called when add-in is removed from the add-ins dialog
          CurveDictionary.Clear();
          CapletVolDictionary.Clear();
          SwaptionVolDictionary.Clear();
      }

       // My databases    
      public static Dictionary<string, IRateCurve> CurveDictionary;
      public static Dictionary<string, BilinearInterpolator> CapletVolDictionary;
      public static Dictionary<string, BilinearInterpolator> SwaptionVolDictionary;

      // We store the pair Caplet Vol Matrix name/RateCurve. This is important because each caplets' volatility matrix refers to the IRateCurve used in the volatility stripping
      public static Dictionary<string, string> PairCapletVolRateCurve;
      
       // called in AutoOpen
      public static void Inizialize()
      {
          CurveDictionary = new Dictionary<string, IRateCurve>();
          CapletVolDictionary = new Dictionary<string, BilinearInterpolator>();
          SwaptionVolDictionary = new Dictionary<string, BilinearInterpolator>();
          PairCapletVolRateCurve = new Dictionary<string, string>();

          #region ClearContents
          
          #endregion
      }
        
        #region Reading data from Excel, building curve/ matrix and adding to the right dictionary

        #region LoadMultiCurve Data from Excel

       // Read data from Excel and call a method to build a curve using data read (multi-curve)
      public static void LoadCurveA()
      {
          Load("DfRates", "FwdRates", "DfTenor", "FwdTenor", "CurveName", "RefDate",
              "DfType", "FwdType", "FixingTenor", "FixingValue", "Message");  
      }

       // Read data from Excel and call a method to build a curve using data read (multi-curve)
      public static void LoadCurveB()
      {
          Load("DfRates2", "FwdRates2", "DfTenor2", "FwdTenor2", "CurveName2", "RefDate2",
                "DfType2", "FwdType2", "FixingTenor2", "FixingValue2", "Message2");
      }

       // Read data from Excel and call a method to build a curve using data read (multi-curve)
      public static void LoadCurveC()
      {
          Load("DfRates2", "FwdRates2", "DfTenor2", "FwdTenor2", "CurveName2", "RefDate2",
          "DfType2", "FwdType2", "FixingTenor2", "FixingValue2", "Message2");
      }

       // Method used to build a curve starting from read data (multi-curve)
      static void Load(string DfRates, string FwdRates, string DfTenor,
             string FwdTenor, string CurveName, string RefDate,
             string DfType, string FwdType, string FixingTenor, string FixingValue, string Message)
      {
          try
          {
              dynamic xlApp;
              xlApp = ExcelDnaUtil.Application;
              double[] dfRates = myArray<double>(xlApp.Range[DfRates].Value2);
              double[] fwdRates = myArray<double>(xlApp.Range[FwdRates].Value2);
              string[] dfTenor = myArray<string>(xlApp.Range[DfTenor].Value2);
              string[] fwdTenor = myArray<string>(xlApp.Range[FwdTenor].Value2);
              string outPut = AddMultiCurve(xlApp.Range[CurveName].Value2, xlApp.Range[RefDate].Value2,
              dfTenor, dfRates, xlApp.Range[DfType].Value2, fwdTenor, fwdRates,
              xlApp.Range[FwdType].Value2, xlApp.Range[FixingTenor].Value2, xlApp.Range[FixingValue].Value2);
              xlApp.Range[Message].Value2 = outPut;
          }
          catch (Exception e)
          {
              MessageBox.Show("Error: " + e.ToString());
          }
      }

      #region Storing Data

        // Add multi-curve to the dictionary
      public static string AddMultiCurve(string idCode, double RefDate,
          string[] DfTenor, double[] DfRates, string DfType,
          string[] FwdTenor, double[] FwdRates, string FwdType,
          string FixingTenor, double FixingValue)
      {
          #region Market Rates for discounting
          RateSet dfMktRates = new RateSet(new Date(RefDate));
          BuildingBlockType dfType = (BuildingBlockType)Enum.Parse(typeof(BuildingBlockType), DfType);
          for (int i = 0; i < DfTenor.Count(); i++)
          {
              dfMktRates.Add(DfRates[i], DfTenor[i], dfType);

          }
          #endregion

          #region Market Rates for forwarding
          RateSet fwdMktRates = new RateSet(new Date(RefDate));
          BuildingBlockType fwdType = (BuildingBlockType)Enum.Parse(typeof(BuildingBlockType), FwdType);
          fwdMktRates.Add(FixingValue, FixingTenor, BuildingBlockType.EURDEPO);
          for (int i = 0; i < FwdTenor.Count(); i++)
          {
              fwdMktRates.Add(FwdRates[i], FwdTenor[i], fwdType);
          }
          #endregion

          #region InizializeMyCurve
          SingleCurveBuilderStandard<OnDf, LinearInterpolator> DCurve = new SingleCurveBuilderStandard<OnDf, LinearInterpolator>(dfMktRates, OneDimensionInterpolation.Linear);
          MultiCurveBuilder<SimpleCubicInterpolator> MultiCurve = new MultiCurveBuilder<SimpleCubicInterpolator>(fwdMktRates, DCurve);

          #endregion

          #region UpDating Dictionary
          try
          {
              if (CurveDictionary.ContainsKey(idCode) == true)  // check if idCode is in dictionary
              {
                  CurveDictionary[idCode] = MultiCurve;  // if true, updates it                   
              }
              else
              {
                  CurveDictionary.Add(idCode, MultiCurve);  // if false, adds it                
              }
              return "Loaded @ " + DateTime.Now.ToString();  // return time of last load
          }
          catch (Exception e)
          {
              return (string)e.ToString();
          }
          #endregion
      }

      #endregion

      #endregion
        
        #region Load Single Curve Data From Excel

       // Read data from Excel and build a single-curve using data read
      public static void LoadSingleCurve() 
      {
          try
          {
              dynamic xlApp;
              xlApp = ExcelDnaUtil.Application;
              #region Load Data from Excel
              double[] ratesS = myArray<double>(xlApp.Range["RatesS"].Value2);              
              string[] tenorS = myArray<string>(xlApp.Range["TenorS"].Value2);
              string[] typeS = myArray<string>(xlApp.Range["TypeS"].Value2);
              string doInterpS = (string) xlApp.Range["DoInterpS"].Value2;
              string interpolationS = (string)xlApp.Range["InterpolationS"].Value2;
              string strategyS = (string)xlApp.Range["StrategyS"].Value2;
              double RefDate = (double)xlApp.Range["RefDateS"].Value2;
              #endregion

              #region building the curve
              RateSet MktRates = new RateSet(new Date(RefDate));        
              for (int i = 0; i < ratesS.Count(); i++)
              {
                  MktRates.Add(ratesS[i], tenorS[i], (BuildingBlockType)Enum.Parse(typeof(BuildingBlockType),typeS[i]));
              }
               // Reflection stuff. From Excel string to C#
              string myTypeDef = strategyS + "`2[" + doInterpS + "," + interpolationS + "]";
              string asmLocation = System.Reflection.Assembly.GetAssembly(typeof(OnDf)).Location;
         
              Assembly asm = Assembly.LoadFile(asmLocation);
          
              Type SC1 = asm.GetType(myTypeDef);

               // I decide to use linear interpolation. You can allow the choice from spreadsheet
              OneDimensionInterpolation bi = OneDimensionInterpolation.Linear;
              ISingleRateCurve c1 = null;

               // SingleCurveBuilderInterpBestFit has constructor with different number of arguments
              if (strategyS == "SingleCurveBuilderInterpBestFit")
              {                 
                  c1 = (ISingleRateCurve)Activator.CreateInstance(SC1, MktRates); }
              else
              {

                  c1 = (ISingleRateCurve)Activator.CreateInstance(SC1, MktRates, bi);
              }

              #region UpDating Dictionary
              string idCode = (string)xlApp.Range["CurveNameS"].Value2;
              try
              {
                  if (CurveDictionary.ContainsKey(idCode) == true)  // check if idCode is in dictionary
                  {
                      CurveDictionary[idCode] = c1;  // if true, updates it                   
                  }
                  else
                  {
                      CurveDictionary.Add(idCode, c1);  // if false, adds it                
                  }
                  xlApp.Range["MessageS"].Value2 = "Loaded @ " + DateTime.Now.ToString();  // return time of last load
              }
              catch (Exception e)
              {
                  xlApp.Range["MessageS"].Value2 = (string)e.ToString();
              }
              #endregion              
              #endregion           
          }
          catch (Exception e)
          {
              MessageBox.Show("Error: " + e.ToString());
          }
      }
      #endregion
        
        #region Load Cap Vol matrix and calibrate

       // Read cap matrix from Excel, create a stripped caplet matrix and store it in the dictionary 
      public static void LoadCapVol()
      {
          dynamic xlApp;
          xlApp = ExcelDnaUtil.Application;
          string idCode = (string)xlApp.Range["CapRateCurve"].Value2;
          string idCodeVol = (string)xlApp.Range["VolCapName"].Value2;
          string strippingType = xlApp.Range["StrippingType"].Value2;          
          if (string.IsNullOrEmpty(idCode) || string.IsNullOrEmpty(idCodeVol) || string.IsNullOrEmpty(strippingType))
          {
              if (string.IsNullOrEmpty(idCode)) MessageBox.Show("Rate Curve is missing");
              if (string.IsNullOrEmpty(idCodeVol)) MessageBox.Show("Volatility Curve is missing");
              if (string.IsNullOrEmpty(strippingType)) MessageBox.Show("StrippingType is missing");
          }
          else
          {
              try
              {
                  #region Load Data from Excel
                  string[] tenor = myArray<string>(xlApp.Range["CapTenor"].Value2);
                  double[] strike = myArray<double>(xlApp.Range["CapStrike"].Value2);

                  Object[,] O = xlApp.Range["CapData"].Value2;
                  List<double[]> Vol = new List<double[]>();
                  for (int c = O.GetLowerBound(1); c <= O.GetUpperBound(1); c++)
                  {
                      List<double> VolArr = new List<double>();
                      for (int r = O.GetLowerBound(0); r <= O.GetUpperBound(0); r++)
                      {
                          VolArr.Add((double)O[r, c]);
                      }
                      Vol.Add(VolArr.ToArray());
                  }
                  // string strippingType = xlApp.Range["StrippingType"].Value2;

                  #endregion

                  #region BootstrappingVol

                  #region UpDating Dictionary
                  
                  try
                  {
                      if (CurveDictionary.ContainsKey(idCode) == true)
                      {
                          IRateCurve Curve = CurveDictionary[idCode];

                          BilinearInterpolator BI = new BilinearInterpolator();
                          #region multi choice
                          if (strippingType == "LinearInterpCapVol")
                          {
                              BI = (new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderInputInterpLinear>(tenor, Curve, strike, Vol)).BI;
                          }
                          else if (strippingType == "CubicInterpCapVol")
                          {
                              BI = (new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderInputInterpCubic>(tenor, Curve, strike, Vol)).BI;
                          }
                          else if (strippingType == "Smooth")
                          {
                              BI = (new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitSmooth>(tenor, Curve, strike, Vol)).BI;
                          }
                          else if (strippingType == "BestFitStd")
                          {
                              BI = (new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitStd>(tenor, Curve, strike, Vol)).BI;
                          }
                          else if (strippingType == "BestFitParam")
                          {
                              BI = (new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitFunct>(tenor, Curve, strike, Vol)).BI;
                          }
                          else if (strippingType == "BestFitCubicCapletVol")
                          {
                              BI = (new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitCubic>(tenor, Curve, strike, Vol)).BI;
                          }
                          else if (strippingType == "BestFitLinearCapletVol")
                          {
                              BI = (new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderBestFitPWL>(tenor, Curve, strike, Vol)).BI;
                          }
                          else if (strippingType == "PieceWiseConstant")
                          {
                              BI = (new CapletMatrixVolBuilder<MonoStrikeCapletVolBuilderPWC>(tenor, Curve, strike, Vol)).BI;
                          }
                          else
                          {
                              MessageBox.Show("Selection Error");
                          }

                          #endregion
                          if (CapletVolDictionary.ContainsKey(idCodeVol) == true)  // check if idCode is in dictionary
                          {
                              CapletVolDictionary[idCodeVol] = BI;  // if true, updates it  
                              PairCapletVolRateCurve[idCodeVol] = idCode; // assign the rate curve
                          }
                          else
                          {
                              CapletVolDictionary.Add(idCodeVol, BI);  // if false, adds it      
                              PairCapletVolRateCurve.Add(idCodeVol, idCode); // add the assigned rate curve
                          }
                          xlApp.Range["MessageCap"].Value2 = "Loaded @ " + DateTime.Now.ToString();  // return time of last load
                          xlApp.Range["MessageCap2"].Value2 = strippingType;
                      }
                      else
                      {
                          xlApp.Range["MessageCap"].Value2 = "Rate Curve Not Found";
                      }
                  }

                  catch (Exception e)
                  {
                      xlApp.Range["MessageCap"].Value2 = (string)e.ToString();
                  }
                  #endregion
              }
                  #endregion
              catch (Exception e)
              {
                  MessageBox.Show("Error: " + e.ToString());
              }
          }
      }
      #endregion

        #region Load Swaption Vol matrix
        
        // Read swaption volatility matrix from Excel, insert values into dictionary
        public static void LoadSwaptionVol()
      {
          dynamic xlApp;
          xlApp = ExcelDnaUtil.Application;
          string idCode = (string)xlApp.Range["SwaptionRateCurve"].Value2;
          string idCodeVol = (string)xlApp.Range["SwaptionVolCurve"].Value2;
         
          if (string.IsNullOrEmpty(idCode) || string.IsNullOrEmpty(idCodeVol))
          {
              if (string.IsNullOrEmpty(idCode)) MessageBox.Show("Rate Curve is missing");
              if (string.IsNullOrEmpty(idCodeVol)) MessageBox.Show("Volatility Curve is missing");
          }
          else 
          { 
              try
              {
                  #region Load Data from Excel
                  string[] Expiry = myArray<string>(xlApp.Range["SwaptionExpiry"].Value2);
                  string[] Tenor = myArray<string>(xlApp.Range["SwaptionTenor"].Value2);                        
                  List<double[]> Vol = FromXlMatrix<double>(xlApp.Range["SwaptionVolData"].Value2);
                  #endregion

                  #region UpDating Dictionary
              try
              {
                  if (CurveDictionary.ContainsKey(idCode) == true)
                  {
                      IRateCurve curve = CurveDictionary[idCode];  // getting my curve                   
                      Date refDate = curve.RefDate(); // curve ref date
                      Date today = refDate.add_workdays(-2);
                      
                      double[] SExpiry = (from e in Expiry
                                          select today.YF_365(refDate.add_period(e, false).add_workdays(-2))).ToArray();

                      double[] STenor = (from t in Tenor
                                         select (new Period(t)).TimeInterval()).ToArray();
                     BilinearInterpolator BI = new BilinearInterpolator(SExpiry, STenor, Vol);
                      if (SwaptionVolDictionary.ContainsKey(idCodeVol) == true)  // check if idCode is in dictionary
                      {
                          SwaptionVolDictionary[idCodeVol] = BI;  // if true, updates it                   
                      }
                      else
                      {
                          SwaptionVolDictionary.Add(idCodeVol, BI);  // if false, adds it                
                      }
                      xlApp.Range["MessageSwaption"].Value2 = "Loaded @ " + DateTime.Now.ToString();  // return time of last load
                  }
                  else 
                  {
                      xlApp.Range["MessageSwaption"].Value2 = "Rate Curve Not Found";
                  }
              }
              catch (Exception e)
              {
                  xlApp.Range["MessageSwaption"].Value2 = (string)e.ToString();
              }
                  #endregion                  
              }
              catch (Exception e)
              {
              MessageBox.Show("Error: " + e.ToString());
              }
          }
      }
        #endregion  
        #endregion
      
        #region Calculation for swaps
        
        // At the money forward swap
      [ExcelFunction(IsVolatile=true, Category ="RateCurveDna", Name ="FwdSwapDna")]
       public static object FwdSwap(string idCode, double StartDate, string SwapTenor)
       {
           try
           {
               IRateCurve MyCurve = null; // initialise a multi curve
               if (CurveDictionary.TryGetValue(idCode, out MyCurve))  // is the idCode in dictionary?
               {
                   return MyCurve.SwapFwd(new Date(StartDate), SwapTenor);
               }
               else
               {
                   return "IdCode not found";  // curve not found
               }
           }
           catch (Exception e)
           {
               return (string)e.ToString();
           }
       }

        // At the money forward swap using string args
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna",Name ="FwdSwapXDna")]
        public static object FwdXSwapDna(string idCode, string StartTenor, string SwapTenor)
        {
            try
            {
                IRateCurve MyCurve = null; // initialise a multi curve
                if (CurveDictionary.TryGetValue(idCode, out MyCurve))  // is the idCode in dictionary?
                {
                    Date StartDate = MyCurve.RefDate().add_period(StartTenor,false);
                    return MyCurve.SwapFwd(new Date(StartDate), SwapTenor);                    
                }
                else
                {
                    return "IdCode not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }

         // Return type of curve 
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna", Name = "CurveTypeDna")]
        public static string CurveType(string idCode) 
        {
            try
            {
                IRateCurve MyCurve = null; // initialise a multi curve
                if (CurveDictionary.TryGetValue(idCode, out MyCurve))  // is the idCode in dictionary?
                {
                    return MyCurve.GetType().ToString();
                }
                else
                {
                    return "IdCode not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }
        
         // Return the rate starting on the start date (same tenor of floating leg of swaps used as building blocks)
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna", Name = "FwdFloatDna")]
        public static object FwdFloat(string idCode, double StartDate)
        {
            try
            {
                IRateCurve MyCurve = null; // initialise 
                if (CurveDictionary.TryGetValue(idCode, out MyCurve))  // is the idCode in dictionary?
                {
                    return MyCurve.Fwd(new Date(StartDate));
                }
                else
                {
                    return "IdCode not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }

         // Discount factor
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna", Name = "DfDna")]
        public static object Df(string idCode, double DfDate)
        {
            try
            {
                IRateCurve MyCurve = null; // initialise 
                if (CurveDictionary.TryGetValue(idCode, out MyCurve))  // is the idCode in dictionary?
                {
                    return MyCurve.Df(new Date(DfDate));
                }
                else
                {
                    return "IdCode not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }

         // calculate basis swap spread 6m vs 3m
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna", Name = "FwdBasis6M3MDna")]
        public static object FwdBasis6M3M(double SwapStartDate, string SwapTenor, string Curve6M, string Curve3M)
        {
             // Get Fwd Start Swap according to underlying rate floating tenor used in building carve (es 3m or 6m,..)
            try
            {
                // we suppose they are multi curve
                IRateCurve MultiCurve6m = null; // initialise a curve
                IRateCurve MultiCurve3m = null; // initialise a curve
                if (CurveDictionary.TryGetValue(Curve6M, out MultiCurve6m) &&
                    CurveDictionary.TryGetValue(Curve3M, out MultiCurve3m))  // is the idCode in dictionary?
                {
                    if (MultiCurve6m.GetSwapStyle().GetType() == typeof(EurSwapVs6m) &&
                        MultiCurve3m.GetSwapStyle().GetType() == typeof(EurSwapVs3m))
                    {
                        return (double)Formula.FwdBasis(new Date(SwapStartDate), SwapTenor, (IMultiRateCurve)MultiCurve6m, (IMultiRateCurve)MultiCurve3m);
                    }
                    else
                    {
                        return "Curve error";
                    }
                }
                else
                {
                    return "Curve not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }

        // calculate basis swap spread with respect two generic curves
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna", Name = "FwdBasisDna")]
        public static object FwdBasis(double SwapStartDate, string SwapTenor, string Curve1, string Curve2)
        {
            // Get Fwd Start Swap according to underlying rate floating tenor used in building carve (es 3m or 6m,..)
            try
            {
                // we suppose they are multi curve
                IRateCurve MCurve1 = null; // initialise a curve
                IRateCurve MCurve2 = null; // initialise a curve
                if (CurveDictionary.TryGetValue(Curve1, out MCurve1) &&
                    CurveDictionary.TryGetValue(Curve2, out MCurve2))  // is the idCode in dictionary?
                {
                    return (double)Formula.FwdBasis(new Date(SwapStartDate), SwapTenor, (IRateCurve)MCurve1, (IRateCurve)MCurve2);
                }                    
                else
                {
                    return "Curve not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }

         // Swap net present value 
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna", Name = "SwapNPVDna")]
        public static object SwapNPV(string idCode, double Rate, string SwapTenor, bool PayOrRec, double Nominal)
        {
            try
            {
                IRateCurve MultiCurve = null; // initialise a multi curve
                if (CurveDictionary.TryGetValue(idCode, out MultiCurve))  // is the idCode in dictionary?
                {
                    // Standard swap
                    Type SwapType = MultiCurve.GetSwapStyle().GetType();
                    Date myRefDate = MultiCurve.RefDate();
                    // using reflection
                    SwapStyle mySwap = (SwapStyle)Activator.CreateInstance(SwapType, myRefDate, Rate, SwapTenor);
                    return Formula.NPV(mySwap, MultiCurve, PayOrRec) * Nominal;                    
                }
                else
                {
                    return "Curve not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }
        #endregion
        
        #region CapFloor
     
         // Cap price
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object CapPrice(string idRate, string idVol, string tenor, double strike, double nominal)
        {
            try
            {
                IRateCurve MyCurve = null; // initialise a multi curve
                BilinearInterpolator VolCapletMatrix = null;
                
                if (CapletVolDictionary.TryGetValue(idVol, out VolCapletMatrix))
                {
                    if (CurveDictionary.TryGetValue(idRate, out MyCurve))  // is the idCode in dictionary?
                    {
                        return Formula.CapBlack(tenor, strike, nominal, CurveDictionary[idRate], CapletVolDictionary[idVol]);
                    }
                    else
                    {
                    return "IdCode not found";  // curve not found
                    }
                }
                else
                {
                    return "CapletVolMatrix not found";  // vol not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }
        
         // Cap price using a given cap volatility
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object CapPriceFromVol(string idRate, double CapVol, string tenor, double strike, double nominal)
        {
            try
            {
                IRateCurve MyCurve = null; // initialise a multi curve
                
                    if (CurveDictionary.TryGetValue(idRate, out MyCurve))  // is the idCode in dictionary?
                    {
                        BilinearInterpolator bi = new BilinearInterpolator();
                        VanillaCap vc = new VanillaCap(tenor, MyCurve, bi, nominal);
                        return vc.Price(CapVol, strike);                             
                    }
                    else
                    {
                        return "IdCode not found";  // curve not found
                    }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }

         // Cap implied volatility from price
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object CapImplVolFromPrice(string idRate, double price, string tenor, double strike, double nominal, double guess)
        {
            try
            {
                IRateCurve MyCurve = null; // initialise a multi curve

                if (CurveDictionary.TryGetValue(idRate, out MyCurve))  // is the idCode in dictionary?
                {
                    BilinearInterpolator bi = new BilinearInterpolator();
                    VanillaCap vc = new VanillaCap(tenor, MyCurve, bi, nominal);
                    return vc.ImplVolFromPrice(price, strike, guess);
                }
                else
                {
                    return "IdCode not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }
        
         // Look up the right caplet volatility in the caplet volatility matrix
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object GetCapletVol(string idCapVol, double refDate, double targetDate, double strike) 
        {
            try
            {
                BilinearInterpolator bi = null;
                if (CapletVolDictionary.TryGetValue(idCapVol, out bi))  // is the idCode in dictionary?
                {
                    Date refD = new Date(refDate);
                    Date tgtD = new Date(targetDate);
                    double T = refD.YF_365(tgtD);
                    return bi.Solve(T,strike);
                }
                else
                {
                    return "IdCode not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }

        // Look up the right caplet volatility in the caplet volatility matrix
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object GetCapletVolT(string idCapVol, double T, double strike)
        {
            try
            {
                BilinearInterpolator bi = null;
                if (CapletVolDictionary.TryGetValue(idCapVol, out bi))  // is the idCode in dictionary?
                {
                    return bi.Solve(T, strike);
                }
                else
                {
                    return "IdCode not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }

         // Generate a cap schedule
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object[,] CapSchedule(string tenor, string idRate) 
        {
            try
            {
                IRateCurve MyCurve = null; // initialise a multi curve

                if (CurveDictionary.TryGetValue(idRate, out MyCurve))  // is the idCode in dictionary?
                {
                    VanillaCap vc = new VanillaCap(tenor, MyCurve, new BilinearInterpolator(), 1);
                    double[] fromDate = Date.GetSerialValue(vc.capSchedule.fromDates);
                    double[] toDate = Date.GetSerialValue(vc.capSchedule.toDates);
                    double[] payDate = Date.GetSerialValue(vc.capSchedule.payDates);
                    double[] t = vc.T;
                    double[] fwd = vc.fwd;
                    double[] df = vc.df;
                    double[] yf = vc.yf;
                    object[,] result = new object[60, 7];
                    for (int i = 0; i < fromDate.Length; i++)
                    {
                        result[i, 0] = fromDate[i];
                        result[i, 1] = toDate[i];
                        result[i, 2] = payDate[i];
                    }
                    for (int i = 0; i < t.Length; i++)
                    {
                        int j = i + 1;
                        result[j, 3] = t[i];
                        result[j, 4] = yf[i];
                        result[j, 5] = fwd[i];
                        result[j, 6] = df[i];
                    }                  
                    return result;
                }
                else
                {
                    object[,] result = new object[0, 0];
                    result[0, 0] = "IdCode not found";
                    return result;
                }
            }
            catch (Exception e)
            {
                object[,] result = new object[60, 7];
                result[0,0] = (string)e.ToString();
                return result;
            }
        }

         // Caplet price
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object CapletPrice(double T, double yf, double fwd, double df, double capletVol, double strike, double nominal)
        {
            try
            {
             return Formula.CapletBlack(T,yf,nominal,strike,capletVol,df,fwd);
            }
            catch (Exception e)
            {
                return e.ToString();

            }
        }
        
         // Floorlet price
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object FloorletPrice(double T, double yf, double fwd, double df, double floorletVol, double strike, double nominal)
        {
            try
            {
                return Formula.FloorletBlack(T, yf, nominal, strike, floorletVol, df, fwd);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object GetCapletVolBound(bool Xbound,string idCapVol, bool OutColumn) 
        {
            try
            {
                BilinearInterpolator bi = null;
                if (CapletVolDictionary.TryGetValue(idCapVol, out bi))  // is the idCode in dictionary?
                {
                    double[] arr = null;
                    if (Xbound)
                    {
                        arr = bi.GetXarr;
                    }
                    else 
                    {
                        arr = bi.GetYarr;
                    }

                    object[,] result = null;
                    if (OutColumn) 
                    {
                        result= new object[arr.Length,1];
                        for (int i = 0; i < arr.Length; i++) 
                        {
                            result[i, 0] = arr[i];
                        }
                    }
                    else
                    {
                        result = new object[1,arr.Length];
                        for (int i = 0; i < arr.Length; i++)
                        {
                            result[0, i] = arr[i];
                        }
                    }                 

                    return result;
                }
                else
                {
                    return "IdCode not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }

        // Check the correspondence between the rate curve and the stripped caplet matrix. (Is the rate curve the one used in caplet stripping process?) 
        // Note that each caplet matrix depends on the rate curve used in stripping process.
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object PairCapletVolRateCurveNameChecker(string idCapVol, string idRate) 
        {            
            try
            {
                BilinearInterpolator VolCapletMatrix = new BilinearInterpolator();
                if (CapletVolDictionary.TryGetValue(idCapVol, out VolCapletMatrix))
                {
                    string ItsRateCurve = null;
                    if (PairCapletVolRateCurve.TryGetValue(idCapVol, out ItsRateCurve))
                    {
                        if (ItsRateCurve == idRate)
                        {
                            return "check is ok.";
                        }
                        else 
                        {
                            return "the rate curve should be: " + ItsRateCurve.ToString();
                        }
                    }
                    else
                    {
                        return "IdCode not found " + ItsRateCurve.ToString();
                    }
                }
                else
                {
                    return "CapletVol IdCode not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }

         // From caplet volatility matrix, get the corresponding rate curve used in stripping process 
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object GetRateCurveNameFromCapVol(string idCapVol) 
        {
            string ItsRateCurve = null;
            try
            {
                if (PairCapletVolRateCurve.TryGetValue(idCapVol, out ItsRateCurve))
                {
                    return ItsRateCurve;
                }
                else
                {
                    return "CapletVol not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }

        // From caplet volatility matrix, get the corresponding reference date 
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object GetRefDateFromCapVol(string idCapVol)
        {
            string ItsRateCurve = null;
            try
            {
                if (PairCapletVolRateCurve.TryGetValue(idCapVol, out ItsRateCurve))
                {
                    return (double) CurveDictionary[ItsRateCurve].RefDate().SerialValue;
                }
                else
                {
                    return "CapletVol not found";  // curve not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }
        #endregion

        #region Swaption

         // Swaption price
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object SwaptionPrice(string idRate, string idVol, string expiry, string tenor, double strike, double nominal, string PayerOrRec)
        {
            try
            {
                IRateCurve MyCurve = null; // initialise a multi curve
                BilinearInterpolator Vol = null; // initialise bilinear interpolator

                if (SwaptionVolDictionary.TryGetValue(idVol, out Vol))
                {
                    if (CurveDictionary.TryGetValue(idRate, out MyCurve))  // is the idCode in dictionary?
                    {
                        bool isPayer = false;
                        if (PayerOrRec == "Payer") isPayer = true;

                        return Formula.Swaption(nominal, strike, expiry, tenor, isPayer, SwaptionVolDictionary[idVol], CurveDictionary[idRate]);                         
                    }
                    else
                    {
                        return "Rate Curve not found";  // curve not found
                    }
                }
                else
                {
                    return "SwaptionVolMatrix not found";  // vol not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }
        }

         // Swaption simple price (no use of rate curve)
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object SwaptionPriceSimple(double N, double S, double K, double sigma, double T, string PayerOrRec, double Annuity) 
        {
            bool isPayer = false;
            if (PayerOrRec == "Payer") isPayer = true;
            return Formula.Swaption(N, S, K, sigma, T, isPayer, Annuity);        
        }

         // Look up swaption volatility in the swaption volatility matrix
        [ExcelFunction(IsVolatile = true, Category = "RateCurveDna")]
        public static object GetSwaptionVol(string idRate, string expiry, string tenor, string idVol) 
        {
            try
            {
                BilinearInterpolator Vol = null;

                if (SwaptionVolDictionary.TryGetValue(idVol, out Vol))
                {
                    Date refDate = CurveDictionary[idRate].RefDate(); // curve ref date
                     // false/true with fwd swap matrix
                    Date startDate = refDate.add_period(expiry, false); // swap start 2 business days after the expiry
                    Date expDate = startDate.add_workdays(-2);  // expiry of swaption
                    Date today = refDate.add_workdays(-2);
                    double Exp = today.YF_365(expDate);
                    double Tenor = (new Period(tenor)).TimeInterval();
                    return SwaptionVolDictionary[idVol].Solve(Exp, Tenor);                 
                }
                else
                {
                    return "SwaptionVolMatrix not found";  // vol not found
                }
            }
            catch (Exception e)
            {
                return (string)e.ToString();
            }        
        }
        #endregion

        #region Risk For Rate Curve

         // Risk of a vanilla swap with respect discounting and forwarding curves (it works only for multi-curve)
        public static void RunRisk() 
        {
            dynamic xlApp;
            xlApp = ExcelDnaUtil.Application;
            try
            {
                #region reading input
                string idCode = (string)xlApp.Range["RiskCurveName1"].Value2;
                string SwapTenor = (string)xlApp.Range["RiskSwapTenor1"].Value2;
                double Rate = (double)xlApp.Range["RiskRate1"].Value2;
                bool PayOrRec = true;
                if (xlApp.Range["PorR1"].Value2 == "Rec") { PayOrRec = false; };
                double Nominal = (double)xlApp.Range["Notional1"].Value2;
                double shift = (double)xlApp.Range["RiskShift1"].Value2;
                #endregion
                
                IRateCurve MultiCurve = null; // initialise a multi curve
                if (CurveDictionary.TryGetValue(idCode, out MultiCurve))  // is the idCode in dictionary?
                {                   
                    VanillaSwap swap = new VanillaSwap((IMultiRateCurve) MultiCurve, Rate, SwapTenor, PayOrRec, Nominal);
                    dynamic range = xlApp.Range["Data1"];
                    
                    range.Value = toColumnVect(swap.BPVShiftedDCurve(shift));

                     xlApp.Range["OutParDF1"] = swap.BPVParallelShiftDCurve(shift);
                     range = xlApp.Range["Data2"];                    
                     range.Value = toColumnVect(swap.BPVShiftedFwdCurve(shift));
                     xlApp.Range["OutParFwd1"].Value2 = swap.BPVParallelShiftFwdCurve(shift);
                }                
            }
            catch (Exception e)
            {
              MessageBox.Show("Error: " + e.ToString());
            }
        }
        
        #endregion

        #region Utility for used in spreadsheet VBA Macro

        #region Input output excel range tools
         // From Excel To Code
         // This is called from a button from a sheet that run a macro
        static T[] myArray<T>(Object[,] O)
        {             
            if ((O.GetLowerBound(1) == O.GetUpperBound(1)) & (O.GetUpperBound(0) != O.GetUpperBound(1)))
            {
                List<T> l = new List<T>();
                for (int i = O.GetLowerBound(0); i <= O.GetUpperBound(0); i++)
                {
                    int firstCol = O.GetLowerBound(1);
                    l.Add((T)O[i, firstCol]);
                }
                return l.ToArray<T>();
            }
            if ((O.GetLowerBound(0) == O.GetUpperBound(0)) & (O.GetUpperBound(0) != O.GetUpperBound(1)))
            {
                List<T> l = new List<T>();
                for (int i = O.GetLowerBound(1); i <= O.GetUpperBound(1); i++)
                {
                    int firstCol = O.GetLowerBound(0);
                    l.Add((T)O[firstCol, i]);
                }
                return l.ToArray<T>();
            }
            return null;
        }

         // Casting from Excel matrix to C# list 
        static List<T[]> FromXlMatrix<T>(Object[,] O)
        {
            List<T[]> data = new List<T[]>();
            for (int c = O.GetLowerBound(1); c <= O.GetUpperBound(1); c++)
            {
                List<T> ArRaw = new List<T>();
                for (int r = O.GetLowerBound(0); r <= O.GetUpperBound(0); r++)
                {
                    ArRaw.Add((T)O[r, c]);
                }
                data.Add(ArRaw.ToArray());
            }
            return data;
        }

         // Casting from C# array To Excel
        static object[,] toColumnVect<T>(T[] o)
        {
            object[,] outPut = new object[o.Count(), 1];
            for (int i = 0; i < o.Count(); i++)
            {
                outPut[i, 0] = o[i];
            }
            return outPut;
        }
        #endregion
         
         // Update curve list given an array of Excel range names
        public static void UpdateCurveList(object[] myRange)
        {
            List<string> rangeList = myRange.Select(i => i.ToString()).ToList();
            List<string> curveNames = new List<string>(CurveDictionary.Keys);
            foreach (string s in rangeList)
            {
                UpdateList(s, curveNames);
            }
        }

         // Update caplet volatility matrix list, given an array of Excel range names
        public static void UpdateCapVolList(object[] myRange)
        {
            try
            {
                List<string> rangeList = myRange.Select(i => i.ToString()).ToList();
                List<string> curveNames = new List<string>(CapletVolDictionary.Keys);
                foreach (string s in rangeList)
                {
                    UpdateList(s, curveNames);
                }
            }
            catch { }
        }

         // Update swaption volatility list given an array of Excel range names
        public static void UpdateSwaptionVolList(object[] myRange)
        {
            try
            {
                List<string> rangeList = myRange.Select(i => i.ToString()).ToList();
                List<string> curveNames = new List<string>(SwaptionVolDictionary.Keys);
                foreach (string s in rangeList)
                {
                    UpdateList(s, curveNames);
                }
            }
            catch { }
        }

         // Update curve list (getting multi-curve only) given an Excel range name
        public static void UpdateCurveListMCOnly(object myRange)
        {                      
            List<string> curveNames = (from c in CurveDictionary
                                             where typeof(IMultiRateCurve).IsAssignableFrom(c.Value.GetType()) == true
                                             select c.Key).ToList<string>();

            UpdateList(myRange.ToString(), curveNames);            
        }

         // Update a list, used in many methods
        public static void UpdateList(string myRange, List<string> validationValues)
        {
            try
            {
                string values = string.Join(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator.ToString(), validationValues);
                
                dynamic xlApp;                
                xlApp = ExcelDnaUtil.Application;
                xlApp.Range[myRange].Validation.Delete();
                xlApp.Range[myRange].Validation.Add(3, 1, 1, values, Type.Missing); //thanks to Govert!!
                xlApp.Range[myRange].Validation.IgnoreBlank = true;
                xlApp.Range[myRange].Validation.InCellDropdown = true;
            }
            catch { }
        }
         
         // Cleaning validation list in Excel, given an array of Excel name ranges   
        public static void CleanList(object[] myRange) 
        {
            List<string> rangeList = myRange.Select(i => i.ToString()).ToList();
            dynamic xlApp;
            xlApp = ExcelDnaUtil.Application;
        
            foreach (string s in rangeList) 
            {
                xlApp.Range[s].Validation.Delete();
                xlApp.Range[s].Value2 = "";
            }            
        }

         // Write in Excel the details referring to the rate curves
        public static void WriteCurveData(object rangeName) 
        {
            try
            {
                dynamic xlApp;
                xlApp = ExcelDnaUtil.Application;
                dynamic range = xlApp.Range[rangeName.ToString()];
                int i = 0;
                foreach (KeyValuePair<string, IRateCurve> k in CurveDictionary)
                {
                    range.Offset(i, 0).Value2 = k.Key;
                    range.Offset(i, 1).Value2 = k.Value.RefDate().DateValue;
                    range.Offset(i, 2).Value2 = k.Value.GetType().ToString();
                    range.Offset(i, 3).Value2 = k.Value.GetSwapStyle().ToString();
                    i++;
                }
                if (i == 0)
                {
                    MessageBox.Show("No data to show");
                }
            }
            catch(Exception e)
          {
              MessageBox.Show("Error: " + e.ToString());
          }
        }

         // Write in Excel the details referring to the caplet vol matrices
        public static void WriteCapVolData(object rangeName)
        {
            try
            {
                dynamic xlApp;
                xlApp = ExcelDnaUtil.Application;
                dynamic range = xlApp.Range[rangeName.ToString()];
                int i = 0;
                foreach (KeyValuePair<string, string> k in PairCapletVolRateCurve)
                {
                    range.Offset(i, 0).Value2 = k.Key;
                    range.Offset(i, 1).Value2 = k.Value;
                    i++;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.ToString());
            }
        }

         // Write in Excel the details referring to the swaption vol matrices
        public static void WriteSwaptionVolData(object rangeName)
        {
            try
            {
                dynamic xlApp;
                xlApp = ExcelDnaUtil.Application;             
                dynamic range = xlApp.Range[rangeName.ToString()];
                int i = 0;
                foreach (KeyValuePair<string, BilinearInterpolator> k in SwaptionVolDictionary)
                {
                    range.Offset(i, 0).Value2 = k.Key;
                    i++;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.ToString());
            }
        }

         // Clean a range and 'n' offset cells   
        public static void IterateClean(object rangeName, int n) 
        {
            dynamic xlApp;
            xlApp = ExcelDnaUtil.Application;             
            dynamic range = xlApp.Range[rangeName.ToString()];
            int i = 0;
            while (range.Offset(i, 0).Value2.ToString() != null)
            {
                for (int j = 0; j < n; j++ )
                {
                    range.Offset(i, j).Value2 = "";
                }
                i++;
            }        
        }

         // Clear each dictionary contents
        public static void ClearDictionaryContents()
        {
            CurveDictionary.Clear();
            CapletVolDictionary.Clear();
            SwaptionVolDictionary.Clear();
            PairCapletVolRateCurve.Clear();
        }

        #endregion
    }   
}

