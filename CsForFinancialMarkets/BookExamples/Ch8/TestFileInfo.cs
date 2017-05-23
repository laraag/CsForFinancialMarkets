// TestFileInfo.cs
//
// Showing the use of FileInfo, DirectoryInfo and Drive classes.
//
// (C) Datasim Education BV  2002-2013

using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

public class FileInfo_etc
{
    public static void Main()
    {
        FileInfo myFile;
        try 
        {   
                // CHANGE FILE NAME IN YOUR OWN DIRECTORY!!
                myFile = new FileInfo(@"c:\daniel\temp\test.dat");

                Console.WriteLine(myFile.Exists);         // true

                Console.WriteLine(myFile.Name);           // test.dat
                Console.WriteLine(myFile.FullName);       // c:\daniel\temp\test.dat
                Console.WriteLine(myFile.DirectoryName);  // c:\daniel\temp
                Console.WriteLine(myFile.Directory.Name); // temp
                Console.WriteLine(myFile.Extension);      // .dat
                Console.WriteLine(myFile.Length);         // 9



                Console.WriteLine(myFile.Attributes);    // ReadOnly,Archive,Hidden,Encrypted
                Console.WriteLine(myFile.CreationTime);
                Console.WriteLine(myFile.LastAccessTime);
                Console.WriteLine(myFile.LastWriteTime);
        }
        catch (Exception e)
		{
			Console.WriteLine("Error: {0}", e.Message);
            return;
		}

      
        Console.WriteLine("File static methods..");

        // Using the static methods in File
        string filePath = "c:\\daniel\\temp\\test.dat";
        
        // Get creation time
        DateTime creationTime = File.GetCreationTime(filePath);
        Console.WriteLine("Creation Time: {0} ", creationTime);

        // Get file attributes 
        FileAttributes fileAtt = File.GetAttributes(filePath);
        Console.WriteLine(fileAtt.ToString());
    
        // Directory Class
        string dirPath = @"c:\daniel\temp";
        string dirPath1 = @"c:\daniel\temp\Excel";
        string dirPath2 = @"c:\daniel\temp\BondData";
        string dirPath3 = @"c:\daniel\temp\Currency";

        // Create directories
        if (!Directory.Exists(dirPath1))
               Directory.CreateDirectory(dirPath1);
        if (!Directory.Exists(dirPath2))
                Directory.CreateDirectory(dirPath2);
        if (!Directory.Exists(dirPath3))
                Directory.CreateDirectory(dirPath3);

        // Enumerate the sub-directories
        foreach (string s in Directory.GetDirectories(dirPath))
        {
            Console.WriteLine(s);
        }

        try
        {
            myFile.MoveTo(@"c:\daniel\temp\backup.dat");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e.Message);
            return;
        }

        DirectoryInfo dir = myFile.Directory;
        Console.WriteLine(dir.Name);             // temp
        Console.WriteLine(dir.FullName);         // c:\temp
        Console.WriteLine(dir.Parent.FullName);  // c:\
        dir.CreateSubdirectory("SubFolder");

        FileSecurity sec;
        try
        {
            sec = File.GetAccessControl(@"c:\daniel\temp\test.dat");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e.Message);
            return;
        }

        AuthorizationRuleCollection rules = sec.GetAccessRules(true, true,
                                                       typeof(NTAccount));
        foreach (FileSystemAccessRule rule in rules)
        {
            Console.WriteLine(rule.AccessControlType);         // Allow or Deny
            Console.WriteLine(rule.FileSystemRights);          // e.g. FullControl
            Console.WriteLine(rule.IdentityReference.Value);   // e.g. MyDomain/Joe
        }

        FileSystemAccessRule newRule =
                       new FileSystemAccessRule("Users", FileSystemRights.ExecuteFile, AccessControlType.Allow);
        sec.AddAccessRule(newRule);
        File.SetAccessControl(@"c:\daniel\temp\test.dat", sec);

        DriveInfo drv = new DriveInfo("C");       // Query the C: drive.

        long totalSize = drv.TotalSize;             // Size in bytes.
        long freeBytes = drv.TotalFreeSpace;        // Ignores disk quotas.
        long freeToMe = drv.AvailableFreeSpace;     // Takes quotas into account.

        // An exception is thrown if a volume is not ready, e.g. 
        // CDRom not inserted. That's the reason why code is commented out.
        try
        {
            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                Console.WriteLine(d.Name);
                Console.WriteLine(d.DriveType);
                // Console.WriteLine(d.DriveFormat);  // NFTS or FAT32.
                Console.WriteLine(d.RootDirectory);
                // Console.WriteLine(d.VolumeLabel); 

                // An exception is thrown if a volume is not ready,
                // e.g. CDRom not inserted.
                if (d.IsReady) // Is the drive ready?
                {
                    Console.WriteLine(d.VolumeLabel);
                    Console.WriteLine(d.DriveFormat);
                }
            }
         }
         catch (Exception e)
         {
             Console.WriteLine(e.Message);
         }
                

    }
}

