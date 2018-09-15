using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
namespace uDocuGen
{
    public class Document
    {

        public string dataPath = Application.dataPath;

        public IList<Obj> Files = new List<Obj>();

        public Document() { }

        public void GenerateDocument()
        {
            this.ParseAssets(dataPath);
        }

        private void ParseAssets(string path)
        {
            foreach (string item in System.IO.Directory.GetFileSystemEntries(path))
            {
                if (path.Substring(dataPath.Length) != "\\Editor")
                {
                    if (System.IO.Directory.Exists(item))
                    {
                        this.ParseAssets(item);
                    }
                    else if (item.Substring(item.Length - 3) == ".cs")
                    {
                        this.ParseFile(item, 0);
                    }
                }
            }
        }

        private void ParseFile(string filePath, int readIndex)
        {
            Obj obj = new Obj(filePath);
            StreamReader reader = new StreamReader(filePath);

            for (int i = 0; i < readIndex; i += 1)
            {
                reader.ReadLine();
            }

            string description = "";
            string allText;
            char[] trimValues = { ' ', '\t', '*' };

            while ((allText = reader.ReadLine()) != null)
            {
                allText = allText.TrimStart(trimValues);
                readIndex += 1;
                if (allText.StartsWith("using"))
                {
                    continue;
                }
                else if (allText.StartsWith("/**"))
                {
                    while (true)
                    {
                        allText = reader.ReadLine();
                        if (allText.EndsWith(" */"))
                        {
                            break;
                        }
                        description = description + allText;
                    }
                }
                else if (allText.StartsWith("///"))
                {
                    while (true)
                    {
                        allText = reader.ReadLine();
                        if (!allText.StartsWith("///"))
                        {
                            break;
                        }
                        description = description + allText;
                    }
                }
                else if (allText.Contains("class"))
                {
                    obj.state = allText.Substring(0, allText.IndexOf("class"));
                    obj.name = allText.Substring(allText.IndexOf("class") + 5, allText.IndexOf(":") - obj.state.Length - 5);
                    obj.inheritance = allText.Substring(allText.IndexOf(":"));
                    obj.description = description;
                    description = "";
                }
                else if (Regex.IsMatch(allText, @"^((((?:[a-z][a-z0-9_]*))+( )+((?:[a-zA-Z][a-zA-Z0-9_]*))))"))
                {
                    if (allText.Contains(";"))
                    {
                        Variable v = new Variable();
                        int first = allText.IndexOf(' ', 0);
                        string firstString = allText.Substring(0, first);

                        if (firstString == "public" || firstString == "private")
                        {
                            int second = allText.IndexOf(' ', first + 1);
                            v.state = firstString;
                            v.type = allText.Substring(first, second - first);
                            if (allText.Contains("="))
                            {
                                v.name = allText.Substring(second, allText.IndexOf(' ', second + 1) - second);
                            }
                            else
                            {
                                v.name = allText.Substring(second, allText.Length - second - 1);
                            }
                            /*
                            Debug.Log(v.state);
                            Debug.Log(v.type);
                            Debug.Log(v.name);
                            */
                        }
                        else
                        {
                            v.state = "";
                            v.type = firstString;
                            if (allText.Contains("="))
                            {
                                v.name = allText.Substring(first, allText.IndexOf(' ', first + 1) - first);
                            }
                            else
                            {
                                v.name = allText.Substring(first, allText.Length - first - 1);
                            }
                            /*
                            Debug.Log(v.state);
                            Debug.Log(v.type);
                            Debug.Log(v.name);
                            */
                        }
                        v.description = description;
                        obj.variables.Add(v);
                        description = "";
                    }
                    else
                    {
                        Function f = new Function();
                        int first = allText.IndexOf(' ', 0);
                        string firstString = allText.Substring(0, first);

                        if (firstString == "public" || firstString == "private")
                        {
                            int second = allText.IndexOf(' ', first + 1);
                            f.state = firstString;
                            f.type = allText.Substring(first, second - first);

                            f.name = allText.Substring(second, allText.IndexOf(')', second) - second + 1);
                            /*
                            Debug.Log(f.state);
                            Debug.Log(f.type);
                            Debug.Log(f.name);
                            */
                        }
                        else
                        {
                            f.state = "";
                            f.type = firstString;
                            f.name = allText.Substring(first, allText.IndexOf(')', first) - first + 1);
                            /*
                            Debug.Log(f.state);
                            Debug.Log(f.type);
                            Debug.Log(f.name);
                            */
                        }
                        f.description = description;
                        obj.functions.Add(f);
                        description = "";
                    }
                }
            }
            reader.Close();
            this.Files.Add(obj);
        }

        public void Save()
        {

        }
    }
}