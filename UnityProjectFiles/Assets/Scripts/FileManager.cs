using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileManager : MonoBehaviour
{ 
    public static List<string> GetFileNames(string folder)
    {
        List<string> filenames = new List<string>();
        DirectoryInfo d = new DirectoryInfo(Application.persistentDataPath + Path.DirectorySeparatorChar + folder);
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

    public static string LoadFile(string folder, string fileName)
    {
        return File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar + fileName);
    }

    public static void SaveFile(string folder, string fileName, string contents)
    {
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar + fileName, contents);
    }

    public static void DeleteFile(string folder, string filename)
    {
        File.Delete(Application.persistentDataPath + Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar + filename);
    }
}
