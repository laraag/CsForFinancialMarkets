// TestTrinomial.cs
//
// Simple test program for Trinomial method
//
// 2011-1-25 DD C# version.
//
// (C) Datasim Eduction BV 2006-2013
//

using System;

public class TestTrinomial
{
    public static void Main()
    {

        // Declare and initialise the parameters
        TrinomialParameters myData;

        // Clewlow p. 55 C = 8.42534 for N = 3
        myData.sigma = 0.2;
        myData.T = 1.0;		// One year
        myData.r = 0.06;
        myData.K = 100;
        myData.div = 0.03;
        myData.type = 'C';
        myData.exercise = false;
        myData.NumberOfSteps = 3;

        Console.WriteLine("How many timesteps: ");
        myData.NumberOfSteps = Convert.ToInt32(Console.ReadLine());

        // Now define option-related calculations and price
        TrinomialTree myTree = new TrinomialTree(myData);
        Console.WriteLine("Price {0}", myTree.Price(100));

        // Clewlow p. 14 check against binomial method C = 10.1457
        myData.sigma = 0.2;
        myData.T = 1.0;		// One year
        myData.r = 0.06;
        myData.K = 100;
        myData.div = 0.0;
        myData.type = 'C';
        myData.exercise = false;
        //myData.NumberOfSteps = 3;

        TrinomialTree myTree2 = new TrinomialTree(myData);
        Console.WriteLine("Price {0}", myTree2.Price(100));

        // Clewlow p. 63 American Put
        myData.sigma = 0.2;
        myData.T = 1.0;		// One year
        myData.r = 0.06;
        myData.K = 100;
        myData.div = 0.0;
        myData.type = 'P';				// Put
        myData.exercise = true;
        //myData.NumberOfSteps = 3;

        TrinomialTree myTree3 = new TrinomialTree(myData);
        Console.WriteLine("Price {0}", myTree3.Price(100));
                
    }
}