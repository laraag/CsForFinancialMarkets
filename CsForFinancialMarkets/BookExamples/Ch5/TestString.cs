// TestString.cs
//
// Testing char and string.
//
// (C) Datasim Education BV 2010
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Normal characters
      /*    char c1 = 'A';
            char myChar = '1';

            // Escape characters
            char newline = '\n';
            char backSlash = '\\';

            char formFeed = '\f';
            char alert = '\a';

            Console.WriteLine(alert);   // Makes noise in PC

            string first = "John";
            string second = "Smith";

            string s = "Hi, \a, there \a";
            Console.WriteLine(s);   // Makes more noise in PC

            // Concatenation
            string fullName = first + " " + second;

            // Using (mutable) StringBuilder
            StringBuilder sb = new StringBuilder();
            for (int j = 1; j <= 50; ++j) sb.Append(j + ",");
            Console.WriteLine(sb); Console.WriteLine("");


            // Insert at a certain position
            int pos = 10;
            sb.Insert(pos, 3.1415);

            pos = 51;
            sb.Insert(pos, "look at me");
            Console.WriteLine(sb); Console.WriteLine("");

            // Remove n chars starting at position p
            int p = 0;
            int n = 2;
            sb.Remove(p, n);
            Console.WriteLine(sb); Console.WriteLine("");

            // Replacing character occurrences by another occurrence
            char oldChar = '1';
            char newChar = 'A';
            sb.Replace(oldChar, newChar);
            Console.WriteLine(sb); Console.WriteLine("");

            // Working with strings
            string empty = string.Empty;
            string empty2 = "";

            // All tests evaluate to True
            Console.WriteLine(empty == "");
            Console.WriteLine(empty.Length == 0);
            Console.WriteLine(empty2 == string.Empty);

            empty = null; empty2 = null;

            // Acessing characters and substrings
            string sA = "12345";
            char cA = sA[0];    // '1'
            char cB = sA[4];    // '5'

            // Print the string sA. Prints "1,2,3,4,5,"
            foreach (char c in sA) Console.Write(c + ","); Console.WriteLine();


            // Case-sensitive searching in a string
            string sB = "Oh, what a beautiful mornung";
            Console.WriteLine(sB.Contains("WHAT"));     // False
            Console.WriteLine(sB.Contains("what"));     // True
            Console.WriteLine(sB.StartsWith("Oh,"));    // True
            Console.WriteLine(sB.EndsWith("ung"));      // True

            Console.WriteLine(sB.IndexOf("beautiful")); // Position 11
            */

            // String manipulation
            string s1= "Finn Mac Cumhail";
            
            int pos = 0; int len = 4;
            string firstName = s1.Substring(pos, len); 
            Console.WriteLine(firstName); // Finn

            pos = 5;
            string newName = s1.Insert(pos, "Thomas ");
            Console.WriteLine(newName); // Finn Thomas Mac Cumhail

            pos = 5;
            int numChars = 7;
            string newName2 = newName.Remove(pos, numChars);
            Console.WriteLine(newName2); // Finn Mac Cumhail

            int numPads = 20; // this will be the new size
            char leftPad = '*';
            char rightPad = '!';

            string paddedStringL = s1.PadLeft(numPads, leftPad);
            Console.WriteLine(paddedStringL); // ****Finn Mac Cumhail

            string paddedStringR = s1.PadRight(numPads, rightPad);
            Console.WriteLine(paddedStringR); // Finn Mac Cumhail!!!!

            // Remove them pads
            string trimmedLeft = paddedStringL.TrimStart(leftPad);
            Console.WriteLine(trimmedLeft); // Finn Mac Cumhail

            string trimmedRight = paddedStringR.TrimEnd(rightPad);
            Console.WriteLine(trimmedRight); // Finn Mac Cumhail

            // Upper and lower case
            string upp = s1.ToUpper();
            Console.WriteLine(upp); // FINN MAC CUMHAIL

            string low = s1.ToLower();
            Console.WriteLine(low); // finn mac cumhail

        }
    }
}
