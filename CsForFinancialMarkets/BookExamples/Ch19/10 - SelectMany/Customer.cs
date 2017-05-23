using System;
using System.Collections.Generic;

public class Customer
{
	// Customer name and ID and the list of orders.
	private string m_name;
	private int m_customerID;
	private List<Order> m_orders=new List<Order>();

	// Constructor with name.
	public Customer(string name, int customerID)
	{
		m_name=name;
		m_customerID=customerID;
	}

	// Access the name.
	public string Name
	{
		get { return m_name; }
		set { m_name=value; }
	}

	// Access the customer ID.
	public int CustomerID
	{
		get { return m_customerID; }
		set { m_customerID=value; }
	}

	// Access the list with orders.
	public List<Order> Orders
	{
		get { return m_orders; }
	}

	// Add an order to the customer.
	public Order AddOrder(string name, double price)
	{
		Order o=new Order(name, price, m_customerID);
		m_orders.Add(o);
		return o;
	}

	// Get string of customer.
	public override string ToString()
	{
		return String.Format("Customer: {0} ({1})", m_name, m_customerID);
	}
}