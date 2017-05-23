// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// TestConsole.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace BookDashboard
{
    class TestConsole
    {
        static void Main(string[] args)
        {            
            Console.Title = "C# For Financial Markets";
            // Set the size
            Console.SetWindowSize(95, 30);
            Console.BufferWidth = 95;
            Console.BufferHeight = 30;

            #region Loop
         
            ExampleUtility d = new ExampleUtility();
            IEnumerable<DataSet> data = d.GetDatabase();
          
            int choice = 0;
            int choice2 = 0;

            StarLine();
            Title("C# for Financial Markets");
            StarLine();

            do  // Chapter menu loop (menu level 1)
            {                    
                var ch = data.GroupBy(g => g.chapter)
                             .Select((g, index) => new
                             {
                                 cha = g.Key,
                                 n = index + 1
                             });

                MinusLine();
                
                Console.WriteLine("List of Chapters:");                     
                
                Dictionary<int, string> SL = new Dictionary<int, string>(ch.ToDictionary(q => q.n, q => q.cha));
                SL.Add(0, "Exit");
               
                int midPoint = (ch.Count() / 2) + (ch.Count() & 1);

                for (int i = 0, j = midPoint; i < midPoint; i++, j++)
                {
                    Console.WriteLine("{0,2}. {1} \t \t{2,2}. {3} ", SL.ElementAt(i).Key, SL.ElementAt(i).Value, 
                        (j >= SL.Count() ? null : SL.ElementAt(j).Key.ToString() ),(j >= SL.Count() ? null : SL.ElementAt(j).Value));
                }
				Console.WriteLine("\nEnter Your chapter selection:");

                     // Get the choice of chapter
                if (Int32.TryParse(Console.ReadLine(), out choice)) 
                {
                    if (choice> 0 && choice < ch.Count()+1)
                    {
                        string selectedCh = ch.ElementAt(choice - 1).cha;
                        var exercises = data.Where(a => a.chapter == selectedCh)
                            .Select((a, index) => new
                            {
                                param = a.methodParemeter,
                                display = a.diplayed,
                                method = a.methodName,
                                className = a.className,
                                assemblyName = a.assemblyName,
                                bookRef = a.bookRef,
                                uRelection = a.useReflection,
                                n = index + 1
                            });
                        do
                        {
                            try
                            {
                                int m = -(int)(from c in exercises
                                               select c.display.Count()).Max();
                                MinusLine();
                                Console.WriteLine("{0}. List of exercises:", selectedCh);
                                foreach (var m1 in exercises)
                                {
                                    Console.WriteLine(String.Format("{0,2}. {1," + m + "}  ({2,-10})", m1.n, m1.display, m1.bookRef));
                                }
                            }
                            catch
                            {
                                MinusLine();
                                Console.WriteLine("No examples in this chapter"); 
                                MinusLine();
                            }
                            
                            Console.WriteLine("{0,2}. Back to list of Chapters",0);
                        
                            Console.WriteLine("\nEnter Your exercise selection");
                            if (Int32.TryParse(Console.ReadLine(), out choice2))
                            {
                                if (choice2 > 0 && choice2 < exercises.Count() + 1)
                                {
                                    var selectedEx = exercises.ElementAt(choice2 - 1);
                                    Console.WriteLine("You selected: '{0}' ({1}), press any key to start it...\n ", selectedEx.display, selectedEx.bookRef);
                                    Console.ReadKey();
                                    d.InvokeMethod(selectedEx.className,selectedEx.method,selectedEx.param, selectedEx.assemblyName, selectedEx.uRelection);
                                    Console.WriteLine("\nCompleted! Press any key to continue...");
                                    Console.ReadKey();                                    
                                }
                            }
                        }
                        while (choice2 != 0);
                    }
                    else if( choice!=0)
                    {
                        Console.WriteLine("You selected an invalid number. Press any key to continue...");
                        Console.ReadKey();  
                    }
                }
            } while (choice != 0);        

            #endregion
        }

        static void StarLine()
        {
            Console.WriteLine("**************************************");
        }
        
        static void MinusLine()
        {
            Console.WriteLine("______________________________________");
        }
        
        static void Title(string title)
        {
            Console.WriteLine("   +++ {0} +++  ", title.ToUpper());
        }
    }

    class ExampleUtility
    {
        public IEnumerable<DataSet> GetDatabase()
        {
            List<DataSet> md = new List<DataSet>();

            #region Ch 1
            md.Add(new DataSet("Chapter 1"));
            #endregion

            #region Ch 2
            md.Add(new DataSet("Chapter 2", "Section 2.1", "HelloWorld.exe", "HelloWorld.Main()", false));
            md.Add(new DataSet("Chapter 2", "Section 2.4", "TestReals.exe", "Floats.Main()", false));
            md.Add(new DataSet("Chapter 2", "Section 2.4", "TestIntegers.exe", "Integers.Main()", false));
            md.Add(new DataSet("Chapter 2", "Section 2.5", "TestChars.exe", "Chars.Main()", false));
            md.Add(new DataSet("Chapter 2", "Section 2.5", "TestStrings.exe", "Strings.Main()", false));
            md.Add(new DataSet("Chapter 2", "Section 2.6", "TestOperators.exe", "Operators.Main()", false));
            md.Add(new DataSet("Chapter 2", "Section 2.7", "TestConsole.exe", "ConsoleTest.Main()", false));
            md.Add(new DataSet("Chapter 2", "Section 2.8", "TestPoint.exe", "TestPoint.Main()", false));
            md.Add(new DataSet("Chapter 2", "Section 2.9", "TestOptionStruct.exe", "TestOption.Main()", false));
            #endregion

            #region Ch 3
            md.Add(new DataSet("Chapter 3", "Section 3.2", "TestPoint001.exe", "TestPoint001", false));
            md.Add(new DataSet("Chapter 3", "Section 3.4", "TestPointProperties.exe", "TestPointProperties.Main()",false));
            md.Add(new DataSet("Chapter 3", "Section 3.5", "TestPoint_static.exe", "TestPoint_static.Main()", false));
            md.Add(new DataSet("Chapter 3", "Section 3.5", "TestPoint_null.exe", "TestPoint_null.Main()", false));
            md.Add(new DataSet("Chapter 3", "Section 3.6", "TestPoint_new.exe", "TestPoint_new.Main()", false));            
            md.Add(new DataSet("Chapter 3", "Section 3.8", "TestEnum.exe", "Draw.Main()",false));
            md.Add(new DataSet("Chapter 3", "Section 3.8", "TestEnum.exe", "GetNamesTest.Main()", false));
            md.Add(new DataSet("Chapter 3", "Section 3.9", "TestOptionPresentation.exe", "TestOptionPresentation.Main()", false));
            md.Add(new DataSet("Chapter 3", "Section 3.10", "TestShape.exe", "TestShape", false));
            md.Add(new DataSet("Chapter 3", "Section 3.10", "TestSealedClass.exe", "SealedClass", false));
            md.Add(new DataSet("Chapter 3", "Section 3.11", "TestTwoAssetOption.exe", "TestTwoAssetOption.Main()", false));
            #endregion

            #region Ch 4
            md.Add(new DataSet("Chapter 4", "Section 4.3.2", "AffineModelsProject.exe", "TestAffineModels", false));            
            md.Add(new DataSet("Chapter 4", "Section 4.4.1", "TestPoint.exe", "Copying Objects", false));
            md.Add(new DataSet("Chapter 4", "Section 4.4.2", "TestPerson.exe", "TestPerson", false));
            md.Add(new DataSet("Chapter 4", "Section 4.4.4", "Thing.exe", "Thing", false));
            md.Add(new DataSet("Chapter 4", "Section 4.4.5", "TestPointResettable.exe", "Casting an Object to an Interface", false));
            md.Add(new DataSet("Chapter 4", "Section 4.5.4", "ODE_FDM.exe", "TestIVP", false));            
            md.Add(new DataSet("Chapter 4", "Section 4.6", "TestDelegate001.exe", "TestDelegate001", false));
            md.Add(new DataSet("Chapter 4", "Section 4.6.1", "TestDelegate002.exe", "TestDelegate002", false));
            md.Add(new DataSet("Chapter 4", "Section 4.9.1", "ProvideRequire.exe", "TestProvideRequire", false));
            md.Add(new DataSet("Chapter 4", "Section 4.10", "ClockSubscriber.exe", "ClockSubscriber", false));            
            #endregion

            #region Ch 5
            md.Add(new DataSet("Chapter 5", "Section 5.2.1-2", "TestArray.exe", "TestDates", false));
            md.Add(new DataSet("Chapter 5", "Section 5.3", "TestDates.exe", "TestDates", false));
            md.Add(new DataSet("Chapter 5", "Section 5.4", "TestEnum101.exe", "TestEnum101", false));
            md.Add(new DataSet("Chapter 5", "Section 5.6", "TestList.exe", "ListMain", false));
            md.Add(new DataSet("Chapter 5", "Section 5.7", "TestHashTable.exe", "TestHashTable", false));
            md.Add(new DataSet("Chapter 5", "Section 5.8", "TestDictionary.exe", "TestDictionary", false));
            md.Add(new DataSet("Chapter 5", "Section 5.9", "TestHashSet.exe", "TestHashSet", false));
            md.Add(new DataSet("Chapter 5", "Section 5.11.3", "TestSortedDictionary.exe", "TestSortedDictionary", false));
            md.Add(new DataSet("Chapter 5", "Section 5.12.2", "TestString.exe", "TestString", false));
            md.Add(new DataSet("Chapter 5", "Section 5.13", "TestFeaturesVersion4.exe", "TestFeaturesVersion4", false));
            #endregion
                        
            #region Ch 6
            md.Add(new DataSet("Chapter 6", "Section 6.2", "TestSection6.2.exe", "TestGenericStack", false));
            md.Add(new DataSet("Chapter 6", "Section 6.2.1", "TestGenericStack.exe", "TestDoubleStack", false));
            md.Add(new DataSet("Chapter 6", "Section 6.2.2", "TestGenericDelegate.exe", "TestGenericDelegate", false));
            md.Add(new DataSet("Chapter 6", "Section 6.2.2", "TestGenericMethod.exe", "TestGenericMethod", false));
            md.Add(new DataSet("Chapter 6", "Section 6.2.5", "TestGenericPair.exe", "TestGenericPair", false));
            md.Add(new DataSet("Chapter 6", "Section 6.4", "Test101ArraysandMatrices.exe", "Test101ArraysandMatrices", false));
            md.Add(new DataSet("Chapter 6", "Section 6.6", "TestSet.exe", "TestSet", false));
            md.Add(new DataSet("Chapter 6", "Section 6.7.1", "TestAssocArray.exe", "TestAssocArray", false));
            md.Add(new DataSet("Chapter 6", "Section 6.7.1", "TestAssocArrayII.exe", "TestAssocArrayII", false));
            md.Add(new DataSet("Chapter 6", "Section 6.8", "TestBasicArrays.exe", "TestBasicArray", false));
            md.Add(new DataSet("Chapter 6", "Section 6.10", "TestTupleParts.exe", "TestTupleParts", false)); 
            #endregion

            #region Ch 7
            md.Add(new DataSet("Chapter 7", "TestInterestRateCalculator.Main()", "Section 7.3.2", "TestInterestRateCalculator.exe", "BondFunctionality"));
            md.Add(new DataSet("Chapter 7", "TestDateSchedule.Example1()", "Section 7.6.2", "TestDateSchedule.exe", "Example1: constructors"));
            md.Add(new DataSet("Chapter 7", "TestDateSchedule.Example2()", "Section 7.6.2", "TestDateSchedule.exe", "Example2: year fractions"));
            md.Add(new DataSet("Chapter 7", "TestDateSchedule.Example3()", "Section 7.6.2", "TestDateSchedule.exe", "Example3: number of days"));
            md.Add(new DataSet("Chapter 7", "TestDateSchedule.Example4()", "Section 7.6.3", "TestDateSchedule.exe", "Example4: DateSchedule"));
            md.Add(new DataSet("Chapter 7", "TestDateSchedule.Example5()", "Section 7.7", "TestDateSchedule.exe", "Example5: associative matrix in excel"));
            md.Add(new DataSet("Chapter 7", "TestBasicBond.BondSample()", "Section 7.9", "TestBasicBond.exe", "BondSample"));
            #endregion

            #region Ch 8
            md.Add(new DataSet("Chapter 8", "Section 8.3.1", "TestFileStreams.exe", "MainClass.Main()", false));
            md.Add(new DataSet("Chapter 8", "Section 8.3.3", "TestCryptoStream.exe", "MainClassCrypto.Main(string() args)", false));
            md.Add(new DataSet("Chapter 8", "Section 8.3.3", "TestCryptoStreamII.exe", "MainClassCrypto.Main(string[] args)", false));
            md.Add(new DataSet("Chapter 8", "Section 8.3.4", "TestBinaryReaderWriter.exe", "BinaryReaderWriter.Main()", false));
            md.Add(new DataSet("Chapter 8", "Section 8.4.2", "TestDirectoryViewes.exe", "DirectoryViewer.Main(string() args)", false));
            md.Add(new DataSet("Chapter 8", "Section 8.4.2", "TestFileInfo.exe", "FileInfo_etc.Main()", false));
            md.Add(new DataSet("Chapter 8", "Section 8.5.1", "TestDataContract.exe", "TestDataContract", false));
            md.Add(new DataSet("Chapter 8", "Section 8.5.1", "TestDataContractII.exe", "TestDataContractII", false));
            md.Add(new DataSet("Chapter 8", "Section 8.5.1", "TestDataContractExtraAttribute.exe", "TestDataContractExtraAttribute", false));
            md.Add(new DataSet("Chapter 8", "Section 8.6", "TestDataContractBinarySerialiser.exe", "BinarySerialiser.Main()", false));
            md.Add(new DataSet("Chapter 8", "Section 8.7", "TesXMLSerialiser101.exe", "TesXMLSerialiser101", false));
            md.Add(new DataSet("Chapter 8", "Section 8.7.1, 8.7.2, 8.7.3", "TesXMLSerialiser.exe", "XMLSerialiser.Main()", false));
            #endregion

            #region Ch 9
            md.Add(new DataSet("Chapter 9", "Section 9.3.2", "TestLattice101.exe", "TestLattice101.Main()", false));
            md.Add(new DataSet("Chapter 9", "Section 9.3.3", "TestBinomialOptionPrice.exe", "TestBinomialOptionPrice.Main()", false));
            md.Add(new DataSet("Chapter 9", "Section 9.3.4", "TestLatticeExcel.exe", "TestLatticeExcel.Main()", false));
            md.Add(new DataSet("Chapter 9", "Section 9.6", "TestTwoFactorBinomial.exe", "TestTwoFactorBinomial.Main()", false));            
            #endregion

            #region Ch 10
            md.Add(new DataSet("Chapter 10", "Section 10.2", "TestTrinomial.exe", "TestTrinomial", false));            
            md.Add(new DataSet("Chapter 10", "Section 10.2", "TestEE.exe", "TestEE", false));
            md.Add(new DataSet("Chapter 10", "Section 10.5.1", "TestFDM.exe", "TestFDM", false));
            md.Add(new DataSet("Chapter 10", "Section 10.9", "TestADE.exe", "TestADE", false));
            md.Add(new DataSet("Chapter 10", "Bonus example", "TestADE_II.exe", "TestADE_II", false));   
            #endregion

            #region Ch 11
            md.Add(new DataSet("Chapter 11", "Section 11.4", "CommandLine.exe", "TestDynamicAssemblyLoading", false));
            md.Add(new DataSet("Chapter 11", "Section 11.4", "TestEnumeratedTypes.exe", "TestEnumeratedTypes", false));
            md.Add(new DataSet("Chapter 11", "Section 11.4.1", "EnumerateMethods.exe", "TestEnumerateMethods", false));
            md.Add(new DataSet("Chapter 11", "Section 11.4.4-5", "TestAttribute.exe", "TestAttribute", false));
            md.Add(new DataSet("Chapter 11", "Section 11.7.1", "TestCodeGeneration101.exe", "TestCodeGeneration101", false));
            md.Add(new DataSet("Chapter 11", "Section 11.8", "TestApplicationDomains.exe", "TestApplicationDomains", false));
            md.Add(new DataSet("Chapter 11", "Section 11.8", "TestApplicationDomains2.exe", "TestApplicationDomains2", false));
            md.Add(new DataSet("Chapter 11", "Section 11.8.2", "TestDomainAndThreads.exe", "TestDomainAndThreads", false));
            md.Add(new DataSet("Chapter 11", "Section 11.8.3", "TestIntraRemoting.exe", "TestIntraRemoting", false));
            md.Add(new DataSet("Chapter 11", "Section 11.8.3", "TestSharedData.exe", "TestSharedData", false));
            md.Add(new DataSet("Chapter 11", "Section 11.8.3", "IntraProcessRemoting.exe", "IntraProcessRemoting", false)); 
            #endregion

            #region Ch 12
            md.Add(new DataSet("Chapter 12", "Ch12.BondBasicxls()", "Section 12.6 and 12.8", "Ch12_Examples.exe", "Open the file BondBasic.xls"));
            md.Add(new DataSet("Chapter 12", "Ch12.BondLoadInMemoryxls()", "Section 12.7.1 and 12.8", "Ch12_Examples.exe", "Open the file BondLoadInMemory.xls"));
            md.Add(new DataSet("Chapter 12", "Ch12.BondSerializationxls()", "Section 12.7.2 and 12.8", "Ch12_Examples.exe", "Open the file BondSerialization.xls"));
            #endregion

            #region Ch 13
            md.Add(new DataSet("Chapter 13", "InterpolatorExample101.Example101()", "Section 13.14.1", "Example101.exe", "Example101"));
            md.Add(new DataSet("Chapter 13", "TestMonotoneConvex.SimplifiedMonotoneConvex()", "Section 13.14.1", "TestMonotoneConvex.exe", "SimplifiedMonotoneConvex"));
            md.Add(new DataSet("Chapter 13", "TestSimpleFormulas.SimpleFormulas()", "Section 13.14.2", "TestSimpleFormulas.exe", "SimpleFormulas"));
            md.Add(new DataSet("Chapter 13", "TestLogisticI.LogisticI()", "Section 13.14.3", "TestLogisticI.exe", "LogisticI"));
            md.Add(new DataSet("Chapter 13", "TestLogisticII.LogisticII()", "Section 13.14.3", "TestLogisticII.exe", "LogisticII"));
            md.Add(new DataSet("Chapter 13", "TestBilinearInterpolation1.BilinearInterpolation1()", "Section 13.14.4", "TestBilinearInterpolation1.exe", "BilinearInterpolation1"));
            md.Add(new DataSet("Chapter 13", "TestBilinearInterpolation2.BilinearInterpolation2()", "Section 13.14.4", "TestBilinearInterpolation2.exe", "BilinearInterpolation2"));
            #endregion

            #region Ch 14
            md.Add(new DataSet("Chapter 14", "TestListedST.Example1()", "Section 14.5", "Ch14_Examples.exe", "Example1"));
            md.Add(new DataSet("Chapter 14", "TestListedST.Example2()", "Section 14.6", "Ch14_Examples.exe", "Example2"));
            md.Add(new DataSet("Chapter 14", "TestListedST.Example3()", "Section 14.7", "Ch14_Examples.exe", "Example3"));
            md.Add(new DataSet("Chapter 14", "TestListedST.Example4()", "Section 14.7.1", "Ch14_Examples.exe", "Example4"));
            md.Add(new DataSet("Chapter 14", "TestListedST.Example5()", "Section 14.7.1", "Ch14_Examples.exe", "Example5"));
            md.Add(new DataSet("Chapter 14", "TestListedST.Example6()", "Section 14.8", "Ch14_Examples.exe", "Example6"));
            md.Add(new DataSet("Chapter 14", "TestListedST.Example7()", "Section 14.8", "Ch14_Examples.exe", "Example7"));
            #endregion

            #region Ch 15
            md.Add(new DataSet("Chapter 15", "TestSingleCurveBuilder.TestVanillaSwapFloatingLegNPV()", "Section 15.7.1", "Ch15_Examples.exe", "TestVanillaSwapFloatingLegNPV"));
            md.Add(new DataSet("Chapter 15", "TestSingleCurveBuilder.CheckInputsVs6m()", "Section 15.7.2", "Ch15_Examples.exe", "CheckInputsVs6m"));
            md.Add(new DataSet("Chapter 15", "TestSingleCurveBuilder.CheckInputsVs3m()", "Section 15.7.2", "Ch15_Examples.exe", "CheckInputsVs3m"));
            md.Add(new DataSet("Chapter 15", "TestSingleCurveBuilder.TimeForBestFitVs6m()", "Section 15.7.3", "Ch15_Examples.exe", "TimeForBestFitVs6m"));
            md.Add(new DataSet("Chapter 15", "TestSingleCurveBuilder.TimeForBestFitVs3m()", "Section 15.7.3", "Ch15_Examples.exe", "TimeForBestFitVs3"));
            md.Add(new DataSet("Chapter 15", "TestSingleCurveBuilder.CheckFwdRatesVs6m()", "Section 15.7.4", "Ch15_Examples.exe", "CheckFwdRatesVs6m"));
            md.Add(new DataSet("Chapter 15", "TestSingleCurveBuilder.CheckFwdRatesVs3m()", "Section 15.7.4", "Ch15_Examples.exe", "CheckFwdRatesVs3m"));
            md.Add(new DataSet("Chapter 15", "TestSingleCurveBuilder.CheckFwdRatesOIS3m()", "Section 15.7.4", "Ch15_Examples.exe", "CheckFwdRatesOIS3m"));
            md.Add(new DataSet("Chapter 15", "TestSingleCurveBuilder.FwdStartSwap()", "Section 15.7.5", "Ch15_Examples.exe", "FwdStartSwap"));
            md.Add(new DataSet("Chapter 15", "TestSingleCurveBuilder.Sensitivities()", "Section 15.7.6", "Ch15_Examples.exe", "Sensitivities"));
            md.Add(new DataSet("Chapter 15", "TestSingleCurveBuilder.MoreOnSensitivities()", "Section 15.7.7", "Ch15_Examples.exe", "MoreOnSensitivities"));
            #endregion

            #region Ch 16
            md.Add(new DataSet("Chapter 16", "TestMultiCurveBuilder.CheckInputs()", "Section 16.7.1", "Ch16_Examples.exe", "CheckInputs"));
            md.Add(new DataSet("Chapter 16", "TestMultiCurveBuilder.FwdInExcell()", "Section 16.7.2", "Ch16_Examples.exe", "FwdInExcell"));
            md.Add(new DataSet("Chapter 16", "TestMultiCurveBuilder.Sensitivities()", "Section 16.7.3", "Ch16_Examples.exe", "Sensitivities"));
            md.Add(new DataSet("Chapter 16", "TestMultiCurveBuilder.FwdMatrix()", "Section 16.7.4", "Ch16_Examples.exe", "FwdMatrix"));
            md.Add(new DataSet("Chapter 16", "TestMultiCurveBuilder.MTM_Differences()", "Section 16.7.5", "Ch16_Examples.exe", "MTM_Differences"));
            md.Add(new DataSet("Chapter 16", "TestMultiCurveBuilder.PerformanceMultiCurveProcess()", "Section 16.7.6", "Ch16_Examples.exe", "PerformanceMultiCurveProcess"));
            md.Add(new DataSet("Chapter 16", "TestMultiCurveBuilder.CheckInterpOnFwd()", "Section 16.7.7", "Ch16_Examples.exe", "CheckInterpOnFwd"));
            md.Add(new DataSet("Chapter 16", "TestMultiCurveBuilder.DiscountFactors()", "Section 16.7.8", "Ch16_Examples.exe", "DiscountFactors"));        
            #endregion

            #region Ch 17
            md.Add(new DataSet("Chapter 17", "TestCapFloorSwaption.SimpleCapletPrice()", "Section 17.6.1", "Ch17_Examples.exe", "SimpleCapletPrice"));
            md.Add(new DataSet("Chapter 17", "TestCapFloorSwaption.CapAsSumOfCaplets()", "Section 17.6.2", "Ch17_Examples.exe", "CapAsSumOfCaplets"));
            md.Add(new DataSet("Chapter 17", "TestCapFloorSwaption.SimpleBootstrapOneCaplet()", "Section 17.6.3", "Ch17_Examples.exe", "SimpleBootstrapOneCaplet"));
            md.Add(new DataSet("Chapter 17", "TestCapFloorSwaption.SimpleBootstrap()", "Section 17.6.4", "Ch17_Examples.exe", "SimpleBootstrap"));
            md.Add(new DataSet("Chapter 17", "TestCapFloorSwaption.SimpleBootstrap20Y()", "Section 17.6.5", "Ch17_Examples.exe", "SimpleBootstrap20Y"));
            md.Add(new DataSet("Chapter 17", "TestCapFloorSwaption.CapletVol20Y_InputInterp()", "Section 17.6.5", "Ch17_Examples.exe", "CapletVol20Y_InputInterp"));
            md.Add(new DataSet("Chapter 17", "TestCapFloorSwaption.VolOptimization()", "Section 17.6.5", "Ch17_Examples.exe", "VolOptimization"));
            md.Add(new DataSet("Chapter 17", "TestCapFloorSwaption.MatrixCaplet()", "Section 17.6.5", "Ch17_Examples.exe", "MatrixCaplet"));
            md.Add(new DataSet("Chapter 17", "TestCapFloorSwaption.MatrixCapletWithRateCurve()", "Section 17.6.5", "Ch17_Examples.exe", "MatrixCapletWithRateCurve"));          
            #endregion

            #region Ch 18
            md.Add(new DataSet("Chapter 18", "Section 18.9", "TestPoint.exe", "TestPoint", false));
            md.Add(new DataSet("Chapter 18", "Section 18.11", "TestDynamicBinding.exe", "TestDynamicBinding", false));            
            md.Add(new DataSet("Chapter 18", "Section 18.11.1", "TestOperators.exe", "TestOperators", false));            
            #endregion

            #region Ch 19
            md.Add(new DataSet("Chapter 19", "Section 19.1-7", "01 - LINQ Basics.exe", "LINQ Basics", false));
            md.Add(new DataSet("Chapter 19", "Section 19.1-7", "02 - Querying Objects.exe", "Querying Objects", false));
            md.Add(new DataSet("Chapter 19", "Section 19.1-7", "03 - Selection operators.exe", "Selection operators", false));
            md.Add(new DataSet("Chapter 19", "Section 19.1-7", "04 - Single value operators.exe", "Single value operators", false));
            md.Add(new DataSet("Chapter 19", "Section 19.1-7", "05 - Set Operations.exe", "Set Operations", false));
            md.Add(new DataSet("Chapter 19", "Section 19.1-7", "06 - Sub Queries.exe", "Sub Queries", false));
            md.Add(new DataSet("Chapter 19", "Section 19.1-7", "07 - Chaining Queries.exe", "Chaining Queries", false));
            md.Add(new DataSet("Chapter 19", "Section 19.1-7", "08 - Let.exe", "Let", false));
            md.Add(new DataSet("Chapter 19", "Section 19.1-7", "09 - Grouping.exe", "Grouping", false));
            md.Add(new DataSet("Chapter 19", "Section 19.1-7", "10 - SelectMany.exe", "SelectMany", false));
            md.Add(new DataSet("Chapter 19", "Section 19.1-7", "11 - Join.exe", "Join", false));            
            md.Add(new DataSet("Chapter 19", "TestLinq.DeferredExecution()", "Section 19.4.3", "Ch19_Examples.exe", "DeferredExecution"));
            md.Add(new DataSet("Chapter 19", "TestLinq.DeferredExecution2()", "Section 19.4.3", "Ch19_Examples.exe", "DeferredExecution2"));
            md.Add(new DataSet("Chapter 19", "TestLinq.Combination()", "Section 19.8.1", "Ch19_Examples.exe", "Combination"));
            md.Add(new DataSet("Chapter 19", "TestLinq.DateArray()", "Section 19.8.2", "Ch19_Examples.exe", "DateArray"));
            md.Add(new DataSet("Chapter 19", "TestLinq.PresentValue()", "Section 19.8.3", "Ch19_Examples.exe", "PresentValue"));
            md.Add(new DataSet("Chapter 19", "TestLinq.Scenario()", "Section 19.8.4", "Ch19_Examples.exe", "Scenario"));
            md.Add(new DataSet("Chapter 19", "TestLinq.TestFuncDelegate()", "Section 19.8.4", "Ch19_Examples.exe", "TestFuncDelegate"));
            md.Add(new DataSet("Chapter 19", "TestLinq.TestGenericActionDelegate()", "Section 19.8.4", "Ch19_Examples.exe", "TestGenericActionDelegate"));
            md.Add(new DataSet("Chapter 19", "TestLinq.CashFlowsAggregator()", "Section 19.8.5", "Ch19_Examples.exe", "CashFlowsAggregator"));
            md.Add(new DataSet("Chapter 19", "TestLinq.OrderBy()", "Section 19.8.6", "Ch19_Examples.exe", "OrderBy"));
            md.Add(new DataSet("Chapter 19", "TestLinq.OrderBy2()", "bonus example", "Ch19_Examples.exe", "OrderBy2"));
            md.Add(new DataSet("Chapter 19", "TestLinq.FwdEonia()", "Section 19.8.7", "Ch19_Examples.exe", "FwdEonia"));
            md.Add(new DataSet("Chapter 19", "Section 19.9", "Linq2Excel.exe", "Linq2Excel", false));
            #endregion

            #region Ch 20
            md.Add(new DataSet("Chapter 20", "Section 20.5.1", "StandAloneApplication.exe", "StandAloneApplication", false));   
            md.Add(new DataSet("Chapter 20", "Ch20.InterpolationExample()", "Section 20.8.4", "InterpolationExample.exe", "InterpolationExample"));   
            #endregion 

            #region Ch 21
            md.Add(new DataSet("Chapter 21", "Ch21.OpenAutomationAddInsV1()","Section 21.4", "Ch21_Examples.exe", "Calculator V1"));
            md.Add(new DataSet("Chapter 21", "Ch21.OpenAutomationAddInsV2()", "Section 21.5", "Ch21_Examples.exe", "Calculator V2"));
            md.Add(new DataSet("Chapter 21", "Ch21.InstallCOMAddIns()", "Section 21.9", "Ch21_Examples.exe", "Install COM Add-Ins"));   
            #endregion

            #region Ch 22
            md.Add(new DataSet("Chapter 22", "Ch22.RateCurveDNAxls()", "Section 22.9.3-4-5", "Ch22_Examples.exe", "Open the file RateCurveDNA.xls"));
            md.Add(new DataSet("Chapter 22", "Ch22.MultiCurveCOMxls()", "Section 22.9.10", "Ch22_Examples.exe", "Open the file MultiCurveCOM.xls"));            
            #endregion

            #region Ch 23
            md.Add(new DataSet("Chapter 23", "Ch23.OpenSineRtdServer()", "Section 23.4", "ConsoleMain.exe", "Open the file SineRtdServer.xlsx"));
            #endregion

            #region Ch 24
            md.Add(new DataSet("Chapter 24", "Section 24.2", "Processes.exe", "Processes", false));
            md.Add(new DataSet("Chapter 24", "Section 24.2", "Processes101.exe", "Processes101", false)); 
            md.Add(new DataSet("Chapter 24", "Section 24.3", "Chapter24WindowsProject.exe", "WindowsProject", false));
            md.Add(new DataSet("Chapter 24", "Section 24.4", "TestThread102.exe", "TestThread102", false));
            md.Add(new DataSet("Chapter 24", "Section 24.4", "Thread101.exe", "Thread101", false));
            md.Add(new DataSet("Chapter 24", "Section 24.5", "ThreadDataTransferI.exe", "1ThreadDataTransferI", false));
            md.Add(new DataSet("Chapter 24", "Section 24.5", "ThreadDataTransferII.exe", "ThreadDataTransferII", false));
            md.Add(new DataSet("Chapter 24", "Section 24.5", "ThreadDataTransferIII.exe", "ThreadDataTransferIII", false));
            md.Add(new DataSet("Chapter 24", "Section 24.5", "ThreadDataTransferIV.exe", "ThreadDataTransferIV", false));
            md.Add(new DataSet("Chapter 24", "Section 24.6.1", "SleepTest.exe", "SleepTest", false));
            md.Add(new DataSet("Chapter 24", "Section 24.6.3", "AbortThread.exe","AbortThread", false));
            md.Add(new DataSet("Chapter 24", "Section 24.6.3", "AbortThreadII.exe", "AbortThreadII", false));
            md.Add(new DataSet("Chapter 24", "Section 24.6.3", "InterruptThread.exe", "InterruptThread", false));
            md.Add(new DataSet("Chapter 24", "Section 24.6.2", "PowerThread.exe", "PowerThread", false));
            md.Add(new DataSet("Chapter 24", "Section 24.8", "TestThreadPool.exe", "TestThreadPool", false));
            md.Add(new DataSet("Chapter 24", "Section 24.9", "TestInterlocked101.exe", "TestInterlocked101", false));
            md.Add(new DataSet("Chapter 24", "Section 24.10", "TestThreadException.exe", "TestThreadException", false));
            md.Add(new DataSet("Chapter 24", "Section 24.11", "TestProducerConsumer.exe", "TestProducerConsumer", false));     
            md.Add(new DataSet("Chapter 24", "Section 24.11.1", "TestFilter.exe", "TestFilter", false));
            md.Add(new DataSet("Chapter 24", "TestMultiThreadOnDf.MultiThreadOnDf()", "Section 24.12", "MultiThreadOnDf.exe", "MultiThreadOnDf"));
            md.Add(new DataSet("Chapter 24", "MultiThreadOnSwap.SensitivitiesParallel()", "bonus example", "MultiThreadOnSwap.exe", "SensitivitiesParallel"));
            md.Add(new DataSet("Chapter 24", "MultiThreadOnSwap.SensitivitiesSequentialVsParallel()", "bonus example", "MultiThreadOnSwap.exe", "SensitivitiesSequentialVsParallel"));
            md.Add(new DataSet("Chapter 24", "MultiThreadOnSwap.VanillaSwaps()", "bonus example", "MultiThreadOnSwap.exe", "VanillaSwaps"));
            #endregion

            #region Ch 25
            md.Add(new DataSet("Chapter 25", "Section 25.2", "AccountThread.exe", "AccountThread",false));
            md.Add(new DataSet("Chapter 25", "Section 25.3.1", "TestThread101.exe", "TestThread101", false));
            md.Add(new DataSet("Chapter 25", "Section 25.3.2", "TestNestedLock.exe", "TestNestedLock", false));
            md.Add(new DataSet("Chapter 25", "Section 25.3.2", "TestNestedLocking.exe", "TestNestedLocking", false));
            md.Add(new DataSet("Chapter 25", "Section 25.4", "TestMutex.exe", "TestMutex", false));
            md.Add(new DataSet("Chapter 25", "Section 25.4", "TestSemaphore001.exe", "TestSemaphore001", false));
            md.Add(new DataSet("Chapter 25", "Section 25.5", "TestEWH101.exe", "TestEWH101", false));
            md.Add(new DataSet("Chapter 25", "Section 25.5", "TestEWH102.exe", "TestEWH102", false));
            md.Add(new DataSet("Chapter 25", "Section 25.5.1", "TestMonitorWaitPulse.exe", "TestMonitorWaitPulse", false));
            md.Add(new DataSet("Chapter 25", "Section 25.6", "TestAsynch001.exe", "TestAsynch001", false));
            md.Add(new DataSet("Chapter 25", "Section 25.7", "TestSynchronizedCollection.exe", "TestSynchronizedCollection", false));
            md.Add(new DataSet("Chapter 25", "Section 25.8", "TestTimers.exe", "TestTimers", false));
            md.Add(new DataSet("Chapter 25", "Section 25.8", "TestTimersWrapper.exe", "TestTimersWrapper", false));
            md.Add(new DataSet("Chapter 25", "Section 25.9", "TestBackgroundThread.exe", "TestBackgroundThread", false));
            md.Add(new DataSet("Chapter 25", "Section 25.10", "TestBackgroundWorker101.exe", "TestBackgroundWorker101", false));
            md.Add(new DataSet("Chapter 25", "Section 25.11.1", "TestParallelClass.exe", "TestParallelClass", false));
            md.Add(new DataSet("Chapter 25", "Section 25.12.1", "TestPLINQetc.exe", "TestPLINQetc", false));
            md.Add(new DataSet("Chapter 25", "Section 25.12.1", "TestTask.exe", "TestTask", false));
            md.Add(new DataSet("Chapter 25", "Section 25.13", "TestParallelDataStructures.exe", "TestParallelDataStructures", false));
            md.Add(new DataSet("Chapter 25", "Section 25.13.1", "TestProducerConsumer.exe", "TestProducerConsumer", false));
            md.Add(new DataSet("Chapter 25", "Section 25.13.2", "TestBarrier.exe", "TestBarrier", false));
            md.Add(new DataSet("Chapter 25", "Section 25.13.3", "TestParallelLINQ.exe", "TestParallelLINQ", false));
            md.Add(new DataSet("Chapter 25", "Section 25.15", "TestTaskParallelLibrary.exe", "Shifting Curves",false));            
            // still to add project are done
            #endregion

            #region Ch 26
            md.Add(new DataSet("Chapter 26", "Section 26.3", "TestNumericalIntegration.exe", "TestNumericalIntegration.Main()", false));
            md.Add(new DataSet("Chapter 26", "Section 26.5", "TestReaderWriterLocks.exe", "TestReaderWriterLocks.Main()", false));
            md.Add(new DataSet("Chapter 26", "Section 26.6", "TestMC.exe", "TestMC.Main()", false));
            md.Add(new DataSet("Chapter 26", "Section 26.6", "TestRandom101.exe", "TestRandom101.Main()", false));
            md.Add(new DataSet("Chapter 26", "Section 26.7", "TestStopWatch.exe", "TestStopWatch.Main()", false));
            md.Add(new DataSet("Chapter 26", "Section 26.8.2", "TestGCCollect.exe", "TestGCCollect.Main()", false));
            md.Add(new DataSet("Chapter 26", "Section 26.10", "TestCoalesce.exe", "TestCoalesce.Main()", false));
            md.Add(new DataSet("Chapter 26", "Section 26.4", "TestThreadPoolLoopOptimisation.exe", "TestThreadPoolLoopOptimisation.Main()", false));
            #endregion

            #region Appendix 1
            md.Add(new DataSet("Appendix 1", "Section A1.9", "TestPoint.exe", "TestPoint", false));
            #endregion

            #region Appendix 2
            md.Add(new DataSet("Appendix 2", "Section A2.5", "TestLevMaq101.exe","TestLevMaq101",false));
            md.Add(new DataSet("Appendix 2", "LevMarTest.LevMar().Go(\"Linear\")", "Section A2.7", "TestLevMaqCalibration.exe","Go(\"Linear\")"));
            md.Add(new DataSet("Appendix 2", "LevMarTest.LevMar().Go(\"Cubic\")", "Section A2.7", "TestLevMaqCalibration.exe", "Go(\"Cubic\")"));
            md.Add(new DataSet("Appendix 2", "LevMarTest.LevMar().Go(\"PWC\")", "Section A2.7", "TestLevMaqCalibration.exe", "Go(\"PWC\")"));
            md.Add(new DataSet("Appendix 2", "Bonus example", "TestLevMaq102.exe", "TestLevMaq102",false));
            #endregion
     
            #region Appendix 4
            md.Add(new DataSet("Appendix 4", "Appendix4.CapFloorSwaptionxls()", "Section A4.2-8", "A4_Examples.exe", "Open the file CapFloorSwaption.xls"));
            #endregion            
            return md;
        }

        public void InvokeMethod(string className, string methodName, string methodPar, string assemblyName, bool useReflection)
        {
            string fullPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string toBeRem = @"\BookDashboard\bin\Debug";
            string root = fullPath.Replace(toBeRem, "");
            string myAssembly = (from f in Directory.GetFiles(root, assemblyName, SearchOption.AllDirectories)
                                 where f.Contains(@"\bin\Debug")
                                 select f).First().ToString();
            Assembly a = Assembly.LoadFrom(myAssembly);
            
            if (useReflection==false)
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = (myAssembly);
                start.UseShellExecute = false;
                start.WorkingDirectory = myAssembly.Replace(assemblyName, "");
                Process p = new Process();
                p.StartInfo = start;
                p.Start();
                p.WaitForExit();                                     
            }
            else
            {
                Type myType = a.GetType(className);
            
                if (methodPar == null)
                {
                    if (myType.IsAbstract)
                    {
                        MethodInfo mI = myType.GetMethod(methodName);
                        mI.Invoke(null, null);
                    }
                    else
                    { // if not static
                        object myInstance = Activator.CreateInstance(myType);
                        myType.InvokeMember(methodName,
                            BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public,
                            null, myInstance, null);
                    }
                }
                else
                {               
                    {   //using params
                        object myInstance = Activator.CreateInstance(myType);
                        myType.InvokeMember(methodName,
                            BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                            null, myInstance, new object[] { methodPar });
                    }
                }
            }
        }
    }

    public struct DataSet
    {
        public string completeName, bookRef, chapter, methodName, className, assemblyName, methodParemeter, diplayed;
        public bool useReflection;

        public DataSet(string chapter, string completeName, string bookRef, string assemblyName)
        {
            int l = completeName.LastIndexOf(".");
            this.chapter = chapter; this.completeName = completeName; this.bookRef = bookRef; this.assemblyName = assemblyName;
            this.useReflection = true;

            int k = completeName.Count(c => c == '(');           
            if (k < 2)  // parameterless method
            {
                this.methodParemeter = null;
                this.className = completeName.Substring(0, l);
                this.methodName = completeName.Substring(l + 1, completeName.Length - l - 3);
                this.diplayed = this.methodName;
            }
            else
            {    // we consider only 1 parameter method
                this.methodParemeter = Regex.Match(completeName, @"\(""(\w+)""\)").Groups[1].Value;
                this.methodName = completeName.Substring(l+1,completeName.LastIndexOf("(")-l-1);
                this.diplayed = this.completeName;
                int b = completeName.IndexOf("(");
                this.className = completeName.Substring(0, b);
            }
        }

        public DataSet(string chapter, string completeName, string bookRef, string assemblyName, string displayed): this(chapter,completeName,bookRef,assemblyName)
        {
            this.diplayed = displayed;
        }
        
        public DataSet(string chapter, string completeName, string bookRef, string assemblyName, string displayed, string methodParemeter)
            : this(chapter, completeName, bookRef, assemblyName,displayed)
        {
            this.methodParemeter = methodParemeter;
        }

        public DataSet(string chapter, string bookRef, string assemblyName, string displayed, bool useReflection)            
        {
            this.completeName = null; this.methodName = null; this.className = null; this.methodParemeter = null;
            this.assemblyName = assemblyName; 
            this.bookRef = bookRef;
            this.chapter = chapter;           
            this.diplayed = displayed;
            this.useReflection = useReflection;
        }
        public DataSet(string chapter) 
            : this(chapter, null, null, null, false) {}
    }
}
