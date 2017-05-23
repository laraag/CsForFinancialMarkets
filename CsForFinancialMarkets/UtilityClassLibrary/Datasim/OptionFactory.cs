// OptionFactory.cs
//
// File with multiple classes that implement an option factory
//
// (C) Datasim Education BV 2006-2013

using System;


public class OptionConsoleFactory : IOptionFactory
{
    public Option CreateOption()
    {
        Option opt = new Option();
       
        Console.Write( "Interest rate: " );
        opt.InterestRate = Convert.ToDouble( Console.ReadLine() );
        Console.Write( "\nVolatility: " );
        opt.Volatility = Convert.ToDouble( Console.ReadLine() );
        Console.Write( "\nStrike price: " );
        opt.StrikePrice = Convert.ToDouble( Console.ReadLine() );
             
        Console.Write( "\nExpiry: " );
        opt.ExpiryDate = Convert.ToDouble( Console.ReadLine() );
        Console.Write( "\nDividend in factor r - D: " );
        opt.CostOfCarry = Convert.ToDouble( Console.ReadLine() );
        Console.Write( "\nPayoff options:\n1) Call\n2) Put" );
        int i = Convert.ToInt32( Console.ReadLine() );
        
               
        if(i == 1 )
        {
            opt.Type = 'C';
            opt.PayOffStrategy = new Option.PayOffHandler( OneFactorPayOff.MyCallPayoffFN );
        }
        else
        {
            opt.Type = 'P';
            opt.PayOffStrategy = new Option.PayOffHandler( OneFactorPayOff.MyPutPayoffFN );
        }

        // Kangro's 2000 far field condition
        opt.FarFieldCondition = opt.StrikePrice * Math.Exp(2.0 * opt.Volatility * opt.Volatility * opt.ExpiryDate * Math.Log(100.0));

        Console.Write("\nDo you want to use default far field value (y/n)?: ");
        char ans = Convert.ToChar(Console.ReadLine());
        
        if (ans != 'y' && ans != 'Y')
        {
            Console.Write("Far Field Value: ");
            opt.FarFieldCondition = Convert.ToDouble(Console.ReadLine());
        }
       
        return opt;
    }
}
public class OptionPrototypeFactory : IOptionFactory
{
    public Option CreateOption()
    {
        Option opt = new Option();

        opt.InterestRate = 0.08;
        opt.Volatility = 0.3;
        opt.StrikePrice = 65.0;
        opt.ExpiryDate = 0.25;
        opt.CostOfCarry = opt.InterestRate; // Stock option
        Console.Write("\nPayoff options:\n1) Call\n2) Put");
        int i = Convert.ToInt32(Console.ReadLine());


        if (i == 1)
        {
            opt.Type = 'C';
            opt.PayOffStrategy = new Option.PayOffHandler(OneFactorPayOff.MyCallPayoffFN);
        }
        else
        {
            opt.Type = 'P';
            opt.PayOffStrategy = new Option.PayOffHandler(OneFactorPayOff.MyPutPayoffFN);
        }

        // Kangro's 2000 far field condition
        opt.FarFieldCondition = opt.StrikePrice * Math.Exp(2.0 * opt.Volatility * opt.Volatility * opt.ExpiryDate * Math.Log(100.0));


        Console.Write("\nDo you want to use default far field value (y/n)?: ");
        char ans = Convert.ToChar(Console.ReadLine());

        if (ans != 'y' && ans != 'Y')
        {
            Console.Write("Far Field Value: ");
            opt.FarFieldCondition = Convert.ToDouble(Console.ReadLine());
        }

        return opt;
    }
}


/*
public class OptionGUIFactory : IOptionFactory
{
    private Option opt;

    public Option CreateOption()
    {
        // create the new form
        OptionForm form = new OptionForm();

        // create and assign handler for the FillOption event
        OptionForm.FillOptionHandler dlg = new OptionForm.FillOptionHandler( SetOption );
        form.FillOption += dlg;

        // show form and wait for close
        form.ShowDialog();

        // should check if option is filled here?

        // return the filled option
        return opt;
    }

    public void SetOption( object sender, FillOptionEventArgs e )
    {
        opt = new Option( e.GetOption );
    }
}*/
