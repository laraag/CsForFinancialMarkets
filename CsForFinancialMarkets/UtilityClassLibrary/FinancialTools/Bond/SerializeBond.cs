// ----------------------------------------------------------------------
// IMPORTANT DISCLAIMER:
// The code is for demonstration purposes only, it comes with NO WARRANTY AND GUARANTEE.
// No liability is accepted by the Author with respect any kind of damage caused by any use
// of the code under any circumstances.
// Any market parameters used are not real data but have been created to clarify the exercises 
// and should not be viewed as actual market data.
//
// SerializeBond.cs
// Author Andrea Germani
// Copyright (C) 2012. All right reserved
// ------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Reflection;

[Serializable]
 // Generic Serializable dictionary: class for serialize and deserialize a generic dictionary  on a file, storing also LastUpdate time and 
public class SerializableDictionary<K,V> 
{
     // Data members
    public Dictionary<K, V> dic;  // dictionary to store information
    string fileName;  // file where serialize
    DateTime lastSave;  // last save time

     // Constructor: the full file name (like: @"c:\\xxx.bin) is needed
    public SerializableDictionary(string fullNameFile) 
    {
        dic = new Dictionary<K, V>();  // initialise dictionary
        fileName = @fullNameFile; 
    }

     // serialize on the file fileName
    public void Serialize()
    {
         // 2* page 630 c# in nutshell 4.0
        IFormatter formatter = new BinaryFormatter();
        
         // Serialize
        using (FileStream s = File.Create(fileName))
                formatter.Serialize(s, this);
        
         // record the time
        lastSave = DateTime.Now;        
    }

     // deserialize from the file fileName
    public void DeSerialize()
    {
         // if file exist deserialize
            if (File.Exists(fileName))
        {
            
            using (FileStream fs = File.OpenRead(fileName))
            {

                dic = ((SerializableDictionary<K, V>)DeserializationBinder.Deserialize(fs)).dic;
            }
        }
    }
}

[Serializable]
 // Derived Class for Bond: adding method SetRefDate
public class BondDictionary : SerializableDictionary<string, BaseBond> 
{
    public BondDictionary(string fullNameFile) : base(fullNameFile) { }
    
     // Update Today for each bond in dictionary
    public void SetRefDate(Date d)
    {
        foreach (KeyValuePair<string, BaseBond> b in dic)
        {
            b.Value.SetNewToDay(d);
        }
    }
}

 // see http: // social.msdn.microsoft.com/Forums/en-US/netfxbcl/thread/e5f0c371-b900-41d8-9a5b-1052739f2521/
public class DeserializationBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        return Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
    }

    public static object Deserialize(Stream stream)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Binder = new DeserializationBinder();
        return formatter.Deserialize(stream);
    }
}