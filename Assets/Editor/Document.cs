using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Document
{
    public string dataPath = Application.dataPath;

    public Document() {}

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
                this.ParseFile(item);
            }
        }
    }

    private void ParseFile(string file)
    {
        StreamReader reader = new StreamReader(file);
        string allText;
        while ((allText = reader.ReadLine()) != null)
        {
            if (allText.StartsWith("/*"))
            {
                while (true)
                {
                    allText = reader.ReadLine();
                    if (allText.EndsWith(" */"))
                    {
                        break;
                    }
                    Debug.Log(allText);
                }
                break;
            } 
            else if (allText.StartsWith("///") || allText.StartsWith("//!"))
            {
                while (true)
                {
                    allText = reader.ReadLine();
                    if (!allText.StartsWith("///"))
                    {
                        break;
                    }
                    Debug.Log(allText.Substring(3));
                }
                break;
            }
        }
        reader.Close();
    }

    public void Save()
    {

    }
}
