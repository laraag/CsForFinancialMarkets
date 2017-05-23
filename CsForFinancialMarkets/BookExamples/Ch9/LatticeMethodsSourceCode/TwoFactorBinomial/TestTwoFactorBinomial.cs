// TestTwoFactorBinomial.cs
//
// Simple test program for Two Factor Binomial method
//
// 2011-5-30 Best of and Worst of options
//
// (C) Datasim Eduction BV 2006-2013
//

using System;

class TestBinomialOptionPrice
{
    public static void Main()
    {

	    // Declare and initialise the parameters
	    TwoFactorBinomialParameters myData = new TwoFactorBinomialParameters();

	    // Clewlow and Strickland p. 47 
	    myData.sigma1 = 0.2; myData.sigma2 = 0.3;
	    myData.T = 1.0;	myData.r = 0.06; myData.K = 1.0;
	    myData.div1 = 0.03; myData.div2 = 0.04;
        myData.rho = 0.5; myData.exercise = true; // false;

        double S1 = 100.0;
        double S2 = 100.0;
        double w1 = 1.0; double w2 = -1.0; int cp = 1;
        myData.pay = new SpreadStrategy(cp, myData.K, w1, w2);
       
        // Min-max option
    /*    myData.sigma1 = 0.3;
	    myData.sigma2 = 0.3;

	    myData.T = 10.0;					
	    myData.r = 0.1;
	    myData.K = 40.0;
	    myData.div1 = 0.0;
	    myData.div2 = 0.0;
	    myData.rho = 0.5;
	    myData.exercise = true;
        double S1 = 40.0;
        double S2 = 40.0;
        

        int cp = 1;
        int dMinMax = 1; // 1 == Max, else Min
        myData.pay = new RainbowStrategy(cp, myData.K, dMinMax);*/


	    // Topper 2005 page 198
   /*     myData.sigma1 = 0.1; myData.sigma2 = 0.1;
        myData.T = 0.05; myData.r = 0.1; myData.K = 40.0;
        myData.div1 = 0.0; myData.div2 = 0.0;
        myData.rho = 0.5; myData.exercise = false;

        double S1 = 18.0; double S2 = 20.0;
        double w1 = 1.0; double w2 = 1.0; int cp = -1; // Weights; put option
        myData.pay = new BasketStrategy(myData.K, cp, w1, w2);*/

        
    /*    myData.sigma1 = 0.2; myData.sigma2 = 0.2;
        myData.T = 0.5; myData.r = 0.1; myData.K = 10.0;
        myData.div1 = 0.0; myData.div2 = 0.0;
        myData.rho = 0.5; myData.exercise = false;*/

      //  double S1 = 122.0; double S2 = 120.0;
    //    int a = 1; int b = -1; int pp = 1;
       // myData.pay = new BasketStrategy(myData.K, pp, a, b);
      //  myData.pay = new  BestofTwoStrategy(myData.K, pp);
      //  myData.pay = new WorstofTwoStrategy(myData.K, -1);
     //   myData.pay = new SpreadStrategy(-pp, myData.K, 1,-1);
         //   cout << "How many timesteps: ";
        int NumberOfSteps = 50;
        //    cin >> NumberOfSteps;

      //      myData.sigma1 = 0.3; myData.sigma2 = 0.2;
      //      myData.T = 0.95; myData.r = Math.Log(1.1);
      //      myData.div1 = Math.Log(1.05); myData.div2 = Math.Log(1.05);
      ////      myData.div1 = 0; myData.div2 = 0;
      //      myData.rho = 0.99; myData.exercise = false;

        //    double S1 = 100; double S2 = 100;
      //      double w1 = 1.0; double w2 = 1.0; int cp = -1; // Weights; put option
           // myData.pay = new BasketStrategy(myData.K, cp, w1, w2); //double strike, int cp,double weight1, double weight2)
           // myData.pay = new WorstofTwoStrategy(myData.K, cp);
            //myData.pay = new RainbowStrategy(cp, myData.K, 1); //(int cp, double strike, int DMinDMax)
          //  myData.pay = new DualStrikeStrategy(110, 100, +1, -1); //(int cp, double strike, int DMinDMax)
            //myData.pay = new ExchangeStrategy();

       //     int n1 = 1; int n2 = 1; myData.K = 40.0;
       //     myData.pay = new PortfolioStrategy(n1, n2, myData.K, -1);
        Console.WriteLine("Computing...");
        TwoFactorBinomial myTree = new TwoFactorBinomial(myData, NumberOfSteps, S1, S2);
        Console.WriteLine("Price is now: {0}", myTree.Price());
          //  Console.WriteLine(myTree.Price());
            // Now examine the convergence of 2-factor Binomial method
        /*    int size = 12;
            Vector<int> meshSizes = new Vector<int>(size);
            int N = 2;
            for (int j = meshSizes.MinIndex; j <=  meshSizes.MaxIndex; j++)
            {
                meshSizes[j] = N;
                N *= 2;
			
            }
            //print(meshSizes);

            Vector<double> result = TwoFactorBinomial.Prices(myData, meshSizes, S1, S2);
            for (int j = meshSizes.MinIndex ; j <=  meshSizes.MaxIndex; j++)
            {
                Console.WriteLine(result[j]);
            }
          */
    }
}