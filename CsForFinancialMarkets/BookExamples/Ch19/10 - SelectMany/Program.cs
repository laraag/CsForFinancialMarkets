// Example showing LINQ SelectMany.
// 
// SelectMany can be used
// - to select elements from collections in collections.
// - Flattening nested colections.
// - Joining collections.
//
// (C) Datasim Education BV  2009-2011

using System;
using System.Collections.Generic;
using System.Linq;

static class Program
{
	static void Main(string[] args)
	{
		CollectionsInCollections();
		CrossJoinCollections();
		SqlJoins();
	}

	// Use select many (2nd from clause) to selecte elements in collections in collections.
	static void CollectionsInCollections()
	{
		Console.WriteLine("\n*** Selecting elements from collections in collections ***");

		// Array with persons.
		var persons=new[] 
		{ 
			new { Name="Dennis", Scores=new[]{8.0, 9.5, 7.4} },
			new { Name="Bob", Scores=new[]{5.0, 5.5, 6.6} },
			new { Name="Daniel", Scores=new[]{8.4, 5.8, 7.7} },
			new { Name="Susan", Scores=new[]{3.0, 10.0, 9.0} },
			new { Name="Andrew", Scores=new[]{4.0, 3.9, 8.4} },
			new { Name="Diane", Scores=new[]{8.3, 9.7, 9.4} },
		};

		// Selects scores of 9 or higher, returns new object (flattening collection).
		var query=from person in persons from score in person.Scores where score>=9 select new { Name=person.Name, Score=score };
		query.Print("Top scores: ");
	}

	// Use select many (2nd from clause) to cross join two collections.
	static void CrossJoinCollections()
	{
		Console.WriteLine("\n*** Cross Join ***");

		// Array of shapes and colors.
		var shapes=new[] {"Square", "Circle", "Hexagon"};
		var colors=new[] {"Red", "Green", "Blue", "Yellow"};
		shapes.Print("Shapes: ");
		colors.Print("Colors: ");

		// Join all shapes with all colors.
		var query=from shape in shapes from color in colors select String.Format("{0} {1}", color, shape);
		query.Print("\nColored shapes: ");

		// Array of clubs.
		var clubs=new[] { "Ajax", "PSV", "Feyenoord" };
		clubs.Print("\nClubs: ");

		// Create club competition (home & out matches) (includes A vs. A).
		var query1=from homeClub in clubs from outClub in clubs select String.Format("{0} vs. {1}", homeClub, outClub);
		query1.Print("\nAll matches (includes A vs. A): ");

		// Create club competition (home & out matches) (excludes A vs. A).
		// Is also non-equi join since it does not use == on the join condition.
		var query2=from homeClub in clubs from outClub in clubs where homeClub!=outClub select String.Format("{0} vs. {1}", homeClub, outClub);
		query2.Print("\nAll matches (excludes A vs. A): ");

		// Create club tournament (no separate home & out matches).
		// Is also non-equi join since it does not use == on the join condition.
		var query3=from clubA in clubs from clubB in clubs where clubA.CompareTo(clubB)<0 select String.Format("{0} vs. {1}", clubA, clubB);
		query3.Print("\nAll matches (no separate home & out matches): ");
	}

	// Example simulating SQL type joins using select many.
	static void SqlJoins()
	{
		Console.WriteLine("\n*** SQL Joins ***");

		// Create a list with customers with customer ID.
		List<Customer> customers=new List<Customer>();
		customers.Add(new Customer("Dave", 1));
		customers.Add(new Customer("Susan", 2));
		customers.Add(new Customer("Daniel", 3));

		// Create a list with orders associated with customers.
		List<Order> orders=new List<Order>();
		orders.Add(customers[0].AddOrder("Computer", 499));
		orders.Add(customers[0].AddOrder("Printer", 59));
		orders.Add(customers[1].AddOrder("Laptop", 699));
		orders.Add(customers[1].AddOrder("USB stick", 19.99));

		// Get the customers with orders. (inner join (only customers with orders))
		// Selects orders on customer ID.
		var query1=from customer in customers 
				   from order in orders 
				   where customer.CustomerID==order.CustomerID 
				   select String.Format("{0} bought {1}", customer.Name, order.Name);
		query1.Print("\nCustomers with orders (inner join): ");

		// Get the customers with orders. (inner join (only customers with orders))
		// Uses customers' order collection so no filter required.
		var query2=from customer in customers
				   from order in customer.Orders
				   select String.Format("{0} bought {1}", customer.Name, order.Name);
		query2.Print("\nCustomers with orders (inner join): ");

		// Get the customers with orders. (outer join (also customers without orders))
		// Note, we needed the DefaultIfEmpty() on orders to also select customers without orders.
		// Also note that since the order can be null, we need to check for null before we can access the order members.
		var query3=from customer in customers
				   from order in customer.Orders.DefaultIfEmpty()
				   select String.Format("{0} bought {1}", customer.Name, order!=null?order.Name:"No order");
		query3.Print("\nCustomers with orders (left outer join): ");

		// Get the customers with orders higher than $100. (outer join (also customers without orders))
		// Note, using a normal where clause does not work since it is executed after DefaultIfEmpty() and can thus be null.
		// Solution is doing a where before DefaultIfEmpty().
		var query4=from customer in customers
//				   from order in customer.Orders.DefaultIfEmpty() where order.Price>500.0
				   from order in customer.Orders.Where(order => order.Price>500.0).DefaultIfEmpty()
				   select String.Format("{0} bought {1}", customer.Name, order!=null?order.Name:"No order");
		query4.Print("\nCustomers with orders (left outer join): ");
	}

	// Extension method to print a collections.
	static void Print<T>(this IEnumerable<T> collection, string msg)
	{
		Console.Write(msg);
		foreach (T value in collection) Console.Write("{0}, ", value);
		Console.WriteLine();
	}
}
