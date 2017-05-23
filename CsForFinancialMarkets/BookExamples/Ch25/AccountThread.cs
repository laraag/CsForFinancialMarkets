// AccountThread.cs
//
// Test program for account class.
// We are trying to call the withdraw method of the Account from
// two threads.
// If one thread is interrupted in the Withdraw() method
// by another thread, it could result in a negative balance.
// This should not be possible, therefore every method that access the
// state should be synchronized.
//
// (C) Datasim Education BV  2002-2013

using System;
using System.Threading;
using System.Collections;

public class MainClass
{
	public static void Main()
	{
		Console.WriteLine("Synchronized account example.");
		Console.WriteLine("Two threads will try to withdraw money from the same account.");
		Console.WriteLine("When the first thread gets interrupted in the withdraw method,");
		Console.WriteLine("the balance can become negative which normally should not happen.");

		Console.WriteLine("1. No synchronisation");
		Console.WriteLine("2. Use synchronisation");

		// Read	input
		string input=Console.ReadLine();

		// Determine if	we must	use	synchronisation
		bool synchronize;
		if (input.Equals("1"))
		{
			synchronize=false;
			Console.WriteLine("\nWithdraws without synchronisation");
		}
		else
		{
			synchronize=true;
			Console.WriteLine("\nWithdraws using synchronisation");
		}

		// Create account object
		Account account=new	Account(100);
		Console.WriteLine("Created account object with balance: " + account.Balance);

		// Creating	threads
		Console.WriteLine("Creating and starting two threads");
		AccountThread at1=new AccountThread(account, 1, synchronize);
		AccountThread at2=new AccountThread(account, 2, synchronize);
		Thread t1=new Thread(new ThreadStart(at1.Run));
		Thread t2=new Thread(new ThreadStart(at2.Run));

		// Start the threads
		t1.Start();
		t2.Start();

		// Wait	for	the	threads	to finish
		t1.Join();
		t2.Join();

		// Print resulting balance
		Console.WriteLine("Resulting balance: " + account.Balance);



		// Unsynchronized array list.
		ArrayList array=new ArrayList();

		// Create synchronized adapter.
		ArrayList synchronizedArray=ArrayList.Synchronized(array);

		// Now ArrayList operations are thread safe
		synchronizedArray.Add(new Account());
	}

}

// Thread that changes an account's balance
public class AccountThread
{
	private Account m_account;				// The account to work on
	private	int	m_id;						// Thread id
	private bool m_isSynchronized;			// Use synchronized or normal Withdraw()

	// Create AccountThread object with id
	public AccountThread(Account account, int id, bool synchronize)
	{ 
		m_account=account;
		m_id=id;
		m_isSynchronized=synchronize;
	}

	// Try to withdraw from the account
	public void	Run()
	{ 
		try
		{
			Console.WriteLine("Thread " + m_id + " tries to withdraw 75");

			// Withdraw
			if (m_isSynchronized==true) m_account.WithdrawSynchronized1(75);
			else m_account.Withdraw(75);

			Console.WriteLine("Thread " + m_id + " withdraw succeeded");
		}
		catch (Account.NoFundsException)
		{ 
			// Withdraw failed
			Console.WriteLine("Thread " + m_id + " withdraw failed. Insufficient balance.");
		}
	}
}

