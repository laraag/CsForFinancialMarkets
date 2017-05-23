// Account.cs
//
// Account class to show synchronisation
//
// (C) Datasim Education BV  2002-2004

using System;
using System.Threading;

public class Account
{
	// Static counter to generate account number
	private static int s_counter=0;

	// Static function to generate next number.
	private static int GetNextNumber()
	{
		lock (typeof(Account))			// Lock the class, prevents generation of more than one account with the same number
		{
			return s_counter++;
		}
	}


	// The account number and balance
	private int m_accountNumber;
	private int m_balance;

	// Default constructor
	public Account()
	{
		m_accountNumber=GetNextNumber();	// Generate account number
		m_balance=0;
	}

	// Constructor with initial balance
	public Account(int balance)
	{
		m_accountNumber=GetNextNumber();	// Generate account number
		m_balance=balance;
	}

	// Account number property
	public int AccountNumber
	{
		get { return m_accountNumber; }
	}

	// Balance property
	public int Balance
	{ 
		get { return m_balance; }
		set { m_balance=value; }
	}

	// Withdraw an amount (not synchronized. Could cause undesirable results)
	public void Withdraw(int amount)
	{ 

		if (m_balance-amount>=0)
		{
			Thread.Sleep(1000);        // For testing we now give other threads a change to run

			m_balance-=amount;
		}
		else throw new NoFundsException();
	}

	// Withdraw an amount (locking using Monitor class)
	public void WithdrawSynchronized1(int amount)
	{ 
		Monitor.Enter(this);			// Acquire lock on this object (the account)
		
		if (m_balance-amount>=0)
		{
			Thread.Sleep(1000);			// For testing we now give other threads a change to run

			m_balance-=amount;
		}
		else throw new NoFundsException();

		Monitor.Exit(this);				// Release lock on this object (the account)
	}

	// Withdraw an amount (locking using lock keyword)
	public void WithdrawSynchronized2(int amount)
	{ 
		lock(this)						// Acquire lock on this object (the account)
		{
			if (m_balance-amount>=0)
			{
				Thread.Sleep(1000);     // For testing we now give other threads a change to run

				m_balance-=amount;
			}
			else throw new NoFundsException();
		}								// End of locked block. Release lock on this object (the account)
	}

	// Withdraw an amount (synchronized MethodImpl attribute)
	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
	public void WithdrawSynchronized3(int amount)
	{
		if (m_balance-amount>=0)
		{
			Thread.Sleep(1000);			// For testing we now give other threads a change to run

			m_balance-=amount;
		}
		else throw new NoFundsException();
	}

	// Nested class for exception
	public class NoFundsException: ApplicationException
	{
	}
}
