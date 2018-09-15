using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Document
{
    public string dataPath = Application.dataPath;

    public Document()
    {

    }
 
    public void GenerateDocument()
    {
        this.ParseAssets(dataPath);
    }

    private void ParseAssets(string path)
    {
        foreach (string item in System.IO.Directory.GetFileSystemEntries(path))
        {
            if (System.IO.Directory.Exists(item))
            {
                this.ParseAssets(item);

            }
            else if (item.Substring(item.Length - 3) == ".cs")
            {
                Debug.Log(item);
            }
        }
    }

    public void Save()
    {

    }
}
