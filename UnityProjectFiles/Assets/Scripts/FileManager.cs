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
        Debug.Log(files.Length + " files found in directory");
        foreach (FileInfo info in files)
        {
            Debug.Log(info.Name);
            filenames.Add(info.Name);
        }
        return filenames;
    }

    public static string LoadFile(string fileName)
    {
        string text = File.ReadAllText(Application.persistentDataPath + Path.PathSeparator + fileName);
        Debug.Log("Loading File: " + fileName + text);
        return text;
    }

    public static void SaveFile(string fileName, string contents)
    {
        Debug.Log("Saving File: " + fileName);
        File.WriteAllText(Application.persistentDataPath + Path.PathSeparator + fileName, contents);
    }
}
