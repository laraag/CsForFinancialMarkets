// Option_IIIData.cs
//
// Class that uses DataContractSerializer for persistence. We 
// now use data contract names
//
// (C) Datasim Education BV 2011
//

using System;
using System.IO;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;



    [DataContract(Name = "EuropeanOption")]
    public class OptionData
    {
        // Member data public for convenience
        [DataMember(Name = "Interest")]
        public double r;		// Interest rate
        [DataMember(Name = "Volatility")]
        public double sig;   // Volatility
        [DataMember(Name = "Strike")]
        public double K;		    // Strike price
        [DataMember(Name = "Expiry")]
        public double T;		    // Expiry date
        [DataMember(Name = "CostCarry")]
        public double b;		// Cost of carry

        [DataMember(Name = "OptionType")]
        public string otyp;   // Option name (call, put)

        public void print()
        {
            Console.WriteLine("\nOption data:\n");
            Console.WriteLine("Interest: {0}", r);
            Console.WriteLine("Volatility: {0}", sig);
            Console.WriteLine("Strike: {0}", K);
            Console.WriteLine("Expiry: {0}", T);
            Console.WriteLine("Cost of carry: {0}", b);
            Console.WriteLine("Option type: {0}", otyp);
        }
    }
