using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileManager : MonoBehaviour
{ 
    public static List<string> GetFileNames()
    {
        List<string> filenames = new List<string>();
        DirectoryInfo d = new DirectoryInfo(Application.persistentDataPath);
        // Load all filenames
        FileInfo[] files = d.GetFiles();
        foreach (FileInfo info in files)
        {
            if (info.Name != "logs.txt") {
                filenames.Add(info.Name);
            }
        }
        return filenames;
    }

    public static string LoadFile(string fileName)
    {
        return File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName);
    }

    public static void SaveFile(string fileName, string contents)
    {
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName, contents);
    }

    public static void DeleteFile(string filename)
    {
        File.Delete(Application.persistentDataPath + Path.DirectorySeparatorChar + filename);
    }
}
