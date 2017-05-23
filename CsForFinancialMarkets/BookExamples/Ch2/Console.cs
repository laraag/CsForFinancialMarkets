// Console.cs
//
// Some examples of the Console class
//
// (C) Datasim Education BV 2010
//

using System;

public class ConsoleTest
{
    public static void Main()
	{

        int k = 1;
        Console.Write(k);       // No carriage return/line terminator
        Console.WriteLine(k);   // No carriage return/line terminator


        string s = "Hola";
        Console.WriteLine(s);
        Console.WriteLine(s+" World");   

        // Making noise
        Console.Beep();         // One-off beep

        // Create a 'musical' note consisting of a fequency and a duration
        Console.Write("Give the frequency in range [37,32767]: ");
        int frequency = Convert.ToInt32(Console.ReadLine());
        Console.Write("Give the duration: ");
        int duration = Convert.ToInt32(Console.ReadLine());

        Console.Beep(frequency, duration); 

        // Copying and creating strings
        string s1 = "is this a string that I see before me?";

        string s2 = string.Copy(s1);
        string s3 = (string)s2.Clone();

	}
}

