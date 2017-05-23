// OptionData.cs
//

using System;

// Encapsulate all data in one place
public struct OptionData
{ // Option data + behaviour

    public double K;
    public double T;
    public double r;
    public double sig;

    public int type;		// 1 == call, -1 == put

    public double myPayOffFunction(double S)
    { // Payoff function

        if (type == 1)
        { // Call

            return Math.Max(S - K, 0.0);
        }
        else
        { // Put

           return Math.Max(K - S, 0.0);
        }
    }
}