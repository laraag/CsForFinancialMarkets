// ---------------------------------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// XlRateCurve.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ---------------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace XlRateCurve
{
     // / <summary>
     // / ICalculator interface.
     // / By default a dual interface is created which can be used for both early binding and late binding.
     // / </summary>
    [ComVisible(true)]                                   // Makes the interface visible in COM regardless of the assembly COM visible attribute.
    [Guid("3B885ABF-975A-43CF-AF55-8158280460CD")]       // Explicit GUID for the interface. 
    public interface IRate
    {
        string AddMultiCurve(string idCode, double RefDate, object DfTenor, object DfRates, string DfType,
           object FwdTenor, object FwdRates, string FwdType, string FixingTenor, double FixingValue); // Add multi-curve
        object FwdSwap(string idCode, double StartDate, string SwapTenor); // Forward swap
        object FwdSwapX(string idCode, string StartTenor, string SwapTenor); // Forward swap using string args
        object FwdFloat(string idCode, double StartDate); // Forward Short Rate, tenor is the one of the swap floating leg
        object FwdBasis6M3M(double SwapStartDate, string SwapTenor, string Curve6M, string Curve3M); // Basis spread 6m vs. 3m
        object FwdBasis(double SwapStartDate, string SwapTenor, string Curve1, string Curve2); // Basis spread
        object Df(string idCode, double DfDate); // Discount factor
        object SwapRisk(string idCode, double Rate, string SwapTenor, bool PayOrRec, double Nominal, double shift, int caseSwitch); // Plain vanilla swap risk (multi-curve only is supported)
        object SwapNPV(string idCode, double Rate, string SwapTenor, bool PayOrRec, double Nominal); // NPV of a swap
        
        void UpdateList(string myRange);
        void CleanRangeList(string myRange); // Clean a single range validation list
        void CleanRangesList(object myRange); // Clean a array of range validation list
        void WriteCurveData(string rangeName);
        void ClearDictionaryContents();
        void IterateClean(string rangeName, double n);
    }

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
    [ComVisible(true)]                                   // Makes the class visible in COM regardless of the assembly COM visible attribute.
    [ProgId("XlRateCurveCOM.Rate")]                      // Explicit ProgID. 
    [Guid("C67F42FA-8BED-469A-84AE-6385A15C39FA")]       // Explicit GUID. 
    [ClassInterface(ClassInterfaceType.None)]            // We implement COM interfaces.
    public class XlRate : ProgrammableBase, IRate, Extensibility.IDTExtensibility2
    {
        static Dictionary<string, IMultiRateCurve> MCDictionary = new Dictionary<string, IMultiRateCurve>();

          // The Excel application. Used in volatile worksheet functions.
        private Excel.Application m_xlApp =
            (Excel.Application)Marshal.GetActiveObject("Excel.Application");
        
        #region Storing Data
        string IRate.AddMultiCurve(string idCode, double RefDate, object DfTenor, object DfRates, string DfType,
            object FwdTenor, object FwdRates, string FwdType, string FixingTenor, double FixingValue) 
        {
            #region Market Rates for discounting
            RateSet dfMktRates = new RateSet(new Date(RefDate));

            double[] dfRates = (double[])DfRates;
            string[] dfTenor = (string[])DfTenor;
            int MaxDfRates = dfTenor.Count();
            BuildingBlockType dfType = (BuildingBlockType)Enum.Parse(typeof(BuildingBlockType), DfType);
            for (int i = 0; i < MaxDfRates; i++)
            {
                dfMktRates.Add(dfRates[i], dfTenor[i], dfType);               
            }
            #endregion

            #region Market Rates for forwarding
            RateSet fwdMktRates = new RateSet(new Date(RefDate));
            double[] fwdRates = (double[])FwdRates;
            string[] fwdTenor = (string[])FwdTenor;
            int MaxFwdRates = fwdTenor.Count();
            BuildingBlockType fwdType = (BuildingBlockType)Enum.Parse(typeof(BuildingBlockType), FwdType);
            fwdMktRates.Add(FixingValue, FixingTenor, BuildingBlockType.EURDEPO);
            for (int i = 0; i < MaxFwdRates; i++)
            {
                fwdMktRates.Add(fwdRates[i], fwdTenor[i], fwdType);
            }
            #endregion

            #region InizializeMyCurve
            SingleCurveBuilderStandard<OnDf, LinearInterpolator> DCurve = new SingleCurveBuilderStandard<OnDf, LinearInterpolator>(dfMktRates, OneDimensionInterpolation.Linear);
            MultiCurveBuilder<SimpleCubicInterpolator> MultiCurve = new MultiCurveBuilder<SimpleCubicInterpolator>(fwdMktRates, DCurve);
           
            #endregion

            #region UpDating Dictionary
            try
            {
                if (MCDictionary.ContainsKey(idCode) == true)  // check if idCode is in dictionary
                {
                    MCDictionary[idCode] = MultiCurve;  // if true, updates it
                }
                else
                {
                    MCDictionary.Add(idCode, MultiCurve);  // if false, adds it
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

        #region Computation

         // At the money forward swap
        object IRate.FwdSwap(string idCode, double StartDate, string SwapTenor)
        {
             // make it volatile
            if (m_xlApp != null) m_xlApp.Volatile(true);
            try
            {
                IMultiRateCurve MultiCurve = null; // initialise a multi curve
                if (MCDictionary.TryGetValue(idCode, out MultiCurve))  // is the idCode in dictionary?
                {
                    return MultiCurve.SwapFwd(new Date(StartDate),SwapTenor);
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
        object IRate.FwdSwapX(string idCode, string StartTenor, string SwapTenor)
        {
             // make it volatile
            if (m_xlApp != null) m_xlApp.Volatile(true);
            try
            {
                IMultiRateCurve MultiCurve = null; // initialise a multi curve
                if (MCDictionary.TryGetValue(idCode, out MultiCurve))  // is the idCode in dictionary?
                {
                    Date StartDate = MultiCurve.RefDate().add_period(StartTenor,true);
                    return MultiCurve.SwapFwd(new Date(StartDate), SwapTenor);
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
        object IRate.FwdFloat(string idCode, double StartDate)
        {
             // make it volatile
            if (m_xlApp != null) m_xlApp.Volatile(true);
            try
            {
                IMultiRateCurve MultiCurve = null; // initialise a multi curve
                if (MCDictionary.TryGetValue(idCode, out MultiCurve))  // is the idCode in dictionary?
                {
                    return MultiCurve.Fwd(new Date(StartDate));
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
        object IRate.Df(string idCode, double DfDate)
        {
             // make it volatile
            if (m_xlApp != null) m_xlApp.Volatile(true);
            try
            {
                IMultiRateCurve MultiCurve = null; // initialise a multi curve
                if (MCDictionary.TryGetValue(idCode, out MultiCurve))  // is the idCode in dictionary?
                {
                    return MultiCurve.Df(new Date(DfDate));
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

         // calculate basis swap spread for 6M vs 3M basis swap
        object IRate.FwdBasis6M3M(double SwapStartDate, string SwapTenor, string Curve6M, string Curve3M)
        {
             // Get Fwd Start Swap according to underlying rate floating tenor used in building carve (es 3m or 6m,..)
            if (m_xlApp != null) m_xlApp.Volatile(true);
            try
            {
                IMultiRateCurve MultiCurve6m = null; // initialise a multi curve
                IMultiRateCurve MultiCurve3m = null; // initialise a multi curve
                if (MCDictionary.TryGetValue(Curve6M, out MultiCurve6m) &&
                    MCDictionary.TryGetValue(Curve3M, out MultiCurve3m))  // is the idCode in dictionary?
                {
                    if (MultiCurve6m.GetSwapStyle().GetType() == typeof(EurSwapVs6m) &&
                        MultiCurve3m.GetSwapStyle().GetType() == typeof(EurSwapVs3m) )
                    {
                        return (double)Formula.FwdBasis(new Date(SwapStartDate), SwapTenor, MultiCurve6m, MultiCurve3m); 
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

        // calculate basis swap spread
        object IRate.FwdBasis(double SwapStartDate, string SwapTenor, string Curve1, string Curve2)
        {
            // Get Fwd Start Swap according to underlying rate floating tenor used in building carve (es 3m or 6m,..)
            if (m_xlApp != null) m_xlApp.Volatile(true);
            try
            {
                IMultiRateCurve MCurve1 = null; // initialise a multi curve
                IMultiRateCurve MCurve2 = null; // initialise a multi curve
                if (MCDictionary.TryGetValue(Curve1, out MCurve1) &&
                    MCDictionary.TryGetValue(Curve2, out MCurve2))  // is the idCode in dictionary?
                {
                    return (double)Formula.FwdBasis(new Date(SwapStartDate), SwapTenor, MCurve1, MCurve2);                    
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

        // Risk of a vanilla swap with respect discounting and forwarding curves (it works only for multi-curve)
        object IRate.SwapRisk(string idCode, double Rate, string SwapTenor, bool PayOrRec, double Nominal, double shift, int caseSwitch) 
        {
             // Get Fwd Start Swap according to underlying rate floating tenor used in building carve (es 3m or 6m,..)
            if (m_xlApp != null) m_xlApp.Volatile(true);
            try
            {
                IMultiRateCurve MultiCurve = null; // initialise a multi curve
                if (MCDictionary.TryGetValue(idCode, out MultiCurve))  // is the idCode in dictionary?
                {
                    object OutPut = null;
                    VanillaSwap swap = new VanillaSwap(MultiCurve, Rate, SwapTenor, PayOrRec, Nominal);
                    switch(caseSwitch)
                    {
                        case 1:
                            OutPut = (double[]) swap.BPVShiftedDCurve(shift);
                            break;
                        case 2:
                            OutPut = (double)swap.BPVParallelShiftDCurve(shift);
                            break;
                        case 3:
                            OutPut = (double[])swap.BPVShiftedFwdCurve(shift);
                            break;
                        case 4:
                            OutPut = (double)swap.BPVParallelShiftFwdCurve(shift);
                            break;                       
                        default:
                         return 0;
                             // break;
                    }
                    return OutPut;
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
        object IRate.SwapNPV(string idCode, double Rate, string SwapTenor, bool PayOrRec, double Nominal) 
        {
             // Get Fwd Start Swap according to underlying rate floating tenor used in building carve (es 3m or 6m,..)
            if (m_xlApp != null) m_xlApp.Volatile(true);
            try
            {
                IMultiRateCurve MultiCurve = null; // initialise a multi curve
                if (MCDictionary.TryGetValue(idCode, out MultiCurve))  // is the idCode in dictionary?
                {
                    VanillaSwap swap = new VanillaSwap(MultiCurve, Rate, SwapTenor, PayOrRec, Nominal);
                    return swap.NPV();
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

         // Update curve list, writing it in the given Excel range name
        void IRate.UpdateList(string myRange)
        {
            List<string> validationValues = MCDictionary.Keys.ToList();
          
            try
            {
                string values = string.Join(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator.ToString(), validationValues);
              
                    m_xlApp.get_Range(myRange).Validation.Delete();
                    m_xlApp.get_Range(myRange).Validation.Add(Excel.XlDVType.xlValidateList,
                        Excel.XlDVAlertStyle.xlValidAlertInformation,
                        Excel.XlFormatConditionOperator.xlBetween, values, Type.Missing);
                    m_xlApp.get_Range(myRange).Validation.IgnoreBlank = true;
                    m_xlApp.get_Range(myRange).Validation.InCellDropdown = true;
               
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
         
         // Clean the validation list in the given Excel range name
        void IRate.CleanRangeList(string myRange)
        {            
            m_xlApp.Range[myRange].Validation.Delete();
            m_xlApp.Range[myRange].Value2 = "";           
        }

         // Clean the validation list in the given Excel range names array
        void IRate.CleanRangesList(object myRange)
        {
            Type strings = myRange.GetType();
            int n = (int)strings.InvokeMember("Length", BindingFlags.GetProperty, null, myRange, null);
           
            object[] e = new object[1];
            for (int i = 0; i < n; i++)
            {
                e[0] = i;
                string s = strings.InvokeMember("GetValue", BindingFlags.InvokeMethod, null, myRange, e).ToString();
                m_xlApp.Range[s].Validation.Delete();
                m_xlApp.Range[s].Value2 = "";
            }
        }

         // Write in Excel the details referring to the rate curves
        void IRate.WriteCurveData(string rangeName)
        {
            try
            { 
                int i = 0;
                foreach (KeyValuePair<string, IMultiRateCurve> k in MCDictionary)
                {
                    m_xlApp.Range[rangeName].Offset[i, 0].Value2 = k.Key;
                    m_xlApp.Range[rangeName].Offset[i, 1].Value2 = k.Value.RefDate().DateValue;
                    m_xlApp.Range[rangeName].Offset[i, 2].Value2 = k.Value.GetType().ToString();
                    m_xlApp.Range[rangeName].Offset[i, 3].Value2 = k.Value.GetSwapStyle().ToString();
                    i++;
                }
                if (i == 0) 
                {
                    MessageBox.Show("No data to show");
                }
            }
            catch
            {
               
            }
        }

         // Clear dictionary contents
        void IRate.ClearDictionaryContents()
        {
            MCDictionary.Clear();
        }

         // Clean a range and 'n' offset cells   
        void IRate.IterateClean(string rangeName, double n)
        {
            int i = 0;

            while (m_xlApp.Range[rangeName].Offset[i, 0].Value2 != null)
            {
                for (int j = 0; j < n; j++)
                {
                    m_xlApp.Range[rangeName].Offset[i, j].Value2 = "";
                }
              
                i++;
            }
        }
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
            m_xlApp = application as Excel.Application;
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