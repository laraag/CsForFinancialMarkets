using System;

public class Order
{
	// The order name and price.
	private string m_name;
	private double m_price;
	private int m_customerID;

	// Constructor with order name and price.
	public Order(string name, double price, int customerID)
	{
		m_name=name;
		m_price=price;
		m_customerID=customerID;
	}

	// Access the order name.
	public string Name
	{
		get { return m_name; }
		set { m_name=value; }
	}

	// Access the order price.
	public double Price
	{
		get { return m_price; }
		set { m_price=value; }
	}

	// Access the customer ID.
	public int CustomerID
	{
		get { return m_customerID; }
		set { m_customerID=value; }
	}

	// Get description of order.
	public override string ToString()
	{
		return string.Format("Order: {0}, Price: {1:c}", m_name, m_price);
	}
}