// TestADE.cs
//
// One-factor Black Scholes using ADE method.
//
// (C) Datasim Education BV 2010-2013
//
// 2012-11-15 DD clean-up of code 
//

using System;
using System.Collections.Generic;
using System.Reflection;

class BSTestMain
{
   
    // 0. Choose which option factory to use
    public static Option CreateOption()
    {
        Console.Write( "\nFactory: 1) Console, 2) Prototype: " );
        int i = Convert.ToInt32( Console.ReadLine() );
        
               
        if(i == 1 )
        {
             return new OptionConsoleFactory().CreateOption();
        }
        else
        {
            return new OptionPrototypeFactory().CreateOption();
        }
    }

    public static void Main()
    {
            // 1. Create the option (Factory Method pattern)
            Option myOption = CreateOption();

            // 2. Define the pde of concern (Bridge pattern)
            IIBVPImp pde = new BSIBVPImp(myOption);
       
            // 3. Discrete mesh sizes.
            int J = 325;
            int N = J;

            Console.Write("NS: ");
            J = Convert.ToInt32(Console.ReadLine());
        
            Console.Write("NT: ");
            N = Convert.ToInt32(Console.ReadLine());

            // 4. The domain in which the PDE is defined.
            Range<double> rangeX = new Range<double>(0.0, myOption.FarFieldCondition);
            Range<double> rangeT = new Range<double>(0.0, myOption.ExpiryDate);
            

            // 5. Create FDM Solver.
            IBVPFDM fdm = new ADE(pde, rangeX, rangeT, J, N);

   
            // 6. Calculate the matrix result.
            NumericMatrix<double> sol = fdm.result();
   

            // 7. Display the results in Excel.
            ExcelMechanisms exl = new ExcelMechanisms();

          
            try
            {
                 exl.printOneExcel(fdm.XValues, fdm.vecNew, "ADE method", "Stock", "Value", "V");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    

 

