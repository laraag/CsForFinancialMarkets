using System;

public class Chars
{
  public static void Main()
  {
    char a='A';		        // ‘A’ character
    char newline='\n';	    // Newline (\n) character
    char one1='\u0031';	    // ‘1’ character (hexadecimal)
    char one2='\x0031';	    // ‘1’ character (hexadecimal)

    Console.WriteLine(a);
    Console.WriteLine(newline);
    Console.WriteLine(one1);
    Console.WriteLine(one2);

  }
}