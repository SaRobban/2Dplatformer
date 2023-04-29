using UnityEngine;
using System;
using System.IO;
using System.Text;
public static class IO_FileManager
{
    //Store on HDD
    public static void SaveToHDD(string fullPath, string jsonString)
    {
        // Open a file in write mode. This will create the file if it's missing.
        // It is assumed that the path already exists.
        // The "using" statement will automatically close the stream after we leave
        // the scope - this is VERY important
        using (FileStream stream = File.OpenWrite(fullPath))
        {
            // Truncate the file if it exists (we want to overwrite the file)
            stream.SetLength(0);

            // Convert the string into bytes. Assume that the character-encoding is UTF8.
            Byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

            // Write the bytes to the hard-drive
            stream.Write(bytes, 0, bytes.Length);
        }
    }

    public static string LoadFromHDD(string fullPath)
    {
        if (!File.Exists(fullPath))
        {
            Debug.Log("<color=red>No File</color> : " + fullPath);
            return null;
        }

        // Open a stream for the supplied file name as a text file
        using (StreamReader stream = File.OpenText(fullPath))
        {
            // This assumes that we've written the file in UTF-8
            string filecontent = stream.ReadToEnd();
            if (string.IsNullOrEmpty(filecontent))
            {
                Debug.LogError("<color=red>Nothing to load</color> : " + fullPath);
            }
            return filecontent;
        }
    }

   
}
