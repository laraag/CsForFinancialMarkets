// TestDirectoryViewer.cs
//
// Small information that print the directory contents.
//
// (C) Datasim Education BV  2002 - 2013

using System;
using System.IO;

public class DirectoryViewer
{
	public static void Main(string[] args)
	{
		DirectoryInfo root;

		// Get root dir from command line, else use current directory
		if (args.Length==0) root=new DirectoryInfo(Directory.GetCurrentDirectory());
		else root=new DirectoryInfo(args[0]);

		// Get all directories and file
		DirectoryInfo[] directories=root.GetDirectories();
		FileInfo[] files=root.GetFiles();

		// Print root directory name
		Console.WriteLine("Directory of {0}", root.FullName);

		// Print all directories
		foreach (DirectoryInfo di in directories)
		{
			Console.WriteLine("{0}\t<DIR>\t{1}\t{2}", di.CreationTime, "-", di.Name);
		}

		// Print all files
		foreach (FileInfo fi in files)
		{
			Console.WriteLine("{0}\t<FILE>\t{1}\t{2}", fi.CreationTime, fi.Length, fi.Name);
		}

	}
}