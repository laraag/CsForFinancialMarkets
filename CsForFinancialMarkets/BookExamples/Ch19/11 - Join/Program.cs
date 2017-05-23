// Example showing LINQ joiing.
//
// (C) Datasim Education BV  2009-2011

using System;
using System.Collections.Generic;
using System.Linq;

static class Program
{
	static void Main(string[] args)
	{
		Console.WriteLine("{0:c}", 3.14);
		Join();
		GroupJoin();
	}

	// Example of join.
	static void Join()
	{
		Console.WriteLine("\n*** Join ***");

		// Create a list with customers with customer ID.
		List<Customer> customers=new List<Customer>();
		customers.Add(new Customer("Dave", 1));
		customers.Add(new Customer("Susan", 2));
		customers.Add(new Customer("Daniel", 3));

		// Create a list with orders associated with customers.
		List<Order> orders=new List<Order>();
		orders.Add(new Order("Computer", 499, 1));
		orders.Add(new Order("Printer", 59, 1));
		orders.Add(new Order("Laptop", 699, 2));
		orders.Add(new Order("USB stick", 19.99, 2));

		// Get the customers with orders. (inner join (only customers with orders))
		var query=from customer in customers join order in orders on customer.CustomerID equals order.CustomerID
				  select String.Format("{0} bought {1}", customer.Name, order.Name);
		query.Print("\nCustomers with orders (inner join): ");

		// where and orderby clause can appear at various places in a join query.
		var query2=from customer in customers where customer.Name.StartsWith("D")	
		 	 	   join order in orders on customer.CustomerID equals order.CustomerID
				   orderby order.Price
				   select String.Format("{0} bought {1}", customer.Name, order.Name);
		query2.Print("\nCustomers starting with A sorted by price: ");
	}

	// Example of group join.
	static void GroupJoin()
	{
		Console.WriteLine("\n*** Group Join ***");

		// Create a list with customers with customer ID.
		List<Customer> customers=new List<Customer>();
		customers.Add(new Customer("Dave", 1));
		customers.Add(new Customer("Susan", 2));
		customers.Add(new Customer("Daniel", 3));

		// Create a list with orders associated with customers.
		List<Order> orders=new List<Order>();
		orders.Add(new Order("Computer", 499, 1));
		orders.Add(new Order("Printer", 59, 1));
		orders.Add(new Order("Laptop", 699, 2));
		orders.Add(new Order("USB stick", 19.99, 2));

		{
			// Get the customers with orders as hierarchical data.
			// Also returns customers without orders (left outer join).
			// Only outputs order properties, not customer properties.
			var query=from customer in customers
					  join order in orders on customer.CustomerID equals order.CustomerID
					  into custOrders
					  select custOrders;

			// Display the result.
			// First iterate the customers.
			// Then iterate the orders in the customers.
			Console.WriteLine("\nLeft outer join (no customer data)");
			foreach (var customer in query)
			{
				Console.WriteLine("Customer?");	// No customer data in customer :-(
				foreach (var order in customer) Console.WriteLine("- {0}", order);
			}
		}

		{
			// Get the customers with orders as hierarchical data.
			// Also returns customers without orders (left outer join).
			// Output anonymous class with customer name and order collection.
			var query=from customer in customers
					  join order in orders on customer.CustomerID equals order.CustomerID
					  into custOrders
					  select new { Name=customer.Name, Orders=custOrders };

			// Display the result.
			// First iterate the customers.
			// Then iterate the orders in the customers.
			Console.WriteLine("\nLeft outer join (with customer data)");
			foreach (var customer in query)
			{
				Console.WriteLine("Customer {0}", customer.Name);
				foreach (var order in customer.Orders) Console.WriteLine("- {0}", order);
			}
		}

		{
			// Get the customers with orders as hierarchical data.
			// Do not return customers without orders (inner join).
			// Output anonymous class with customer name and order collection.
			var query=from customer in customers
					  join order in orders on customer.CustomerID equals order.CustomerID
					  into custOrders where custOrders.Any()
					  select new { Name=customer.Name, Orders=custOrders };

			// Display the result.
			// First iterate the customers.
			// Then iterate the orders in the customers.
			Console.WriteLine("\nInner join (with customer data)");
			foreach (var customer in query)
			{
				Console.WriteLine("Customer {0}", customer.Name);
				foreach (var order in customer.Orders) Console.WriteLine("- {0}", order);
			}
		}

		{
			// Get the customers with orders as hierarchical data.
			// Do not return customers without orders (inner join).
			// Get only customers starting with ‘S’ and orders with price > $500.
			var query=from customer in customers
					  where customer.Name.StartsWith("S")
//					  join order in orders on customer.CustomerID equals order.CustomerID where order.Price>500
					  join order in orders.Where(order => order.Price>500) on customer.CustomerID equals order.CustomerID
					  into custOrders
					  where custOrders.Any()
					  select new { Name=customer.Name, Orders=custOrders };

			// Display the result.
			// First iterate the customers.
			// Then iterate the orders in the customers.
			Console.WriteLine("\nInner join (filtered)");
			foreach (var customer in query)
			{
				Console.WriteLine("Customer {0}", customer.Name);
				foreach (var order in customer.Orders) Console.WriteLine("- {0}", order);
			}
		}
	}

	// Extension method to print a collections.
	static void Print<T>(this IEnumerable<T> collection, string msg)
	{
		Console.Write(msg);
		foreach (T value in collection) Console.Write("{0}, ", value);
		Console.WriteLine();
	}
}

