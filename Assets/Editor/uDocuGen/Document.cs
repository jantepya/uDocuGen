using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using UnityEngine;
using Object = System.Object;

namespace uDocuGen
{
    public class Document
    {

        public string DataPath = Application.dataPath;

        public IList<Obj> Files = new List<Obj>();

        public string ProjectTitle = "uDocuGen";

        public string AuthorName = "Jonathan and Eric";

        public string Title = "uDocuGen";

        public string Version = "1.0.0";

        public string LastUpdate = "15/9/2018";

        public string outputBlock;

        public bool GenerateDocument()
        {
            try
            {
                if (this.ParseAssets(DataPath))
                {
                    outputBlock = this.Stitch();
                }
                return true;
            }
             catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private bool ParseAssets(string path)
        {
            try
            {
                foreach (string item in System.IO.Directory.GetFileSystemEntries(path))
                {
                    if (path.Substring(DataPath.Length) != "\\Editor")
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
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
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
            char[] trimValues = {' ', '\t', '*'};

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
                    obj.Scope = allText.Substring(0, allText.IndexOf("class"));
                    obj.Name = allText.Substring(allText.IndexOf("class") + 5,
                        allText.IndexOf(":") - obj.Scope.Length - 5);
                    obj.Inheritance = allText.Substring(allText.IndexOf(":"));
                    obj.Description = description;
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
                            v.Scope = firstString;
                            v.Type = allText.Substring(first, second - first);
                            if (allText.Contains("="))
                            {
                                v.Name = allText.Substring(second, allText.IndexOf(' ', second + 1) - second);
                            }
                            else
                            {
                                v.Name = allText.Substring(second, allText.Length - second - 1);
                            }

                            /*
                            Debug.Log(v.state);
                            Debug.Log(v.type);
                            Debug.Log(v.name);
                            */
                        }
                        else
                        {
                            v.Scope = "";
                            v.Type = firstString;
                            if (allText.Contains("="))
                            {
                                v.Name = allText.Substring(first, allText.IndexOf(' ', first + 1) - first);
                            }
                            else
                            {
                                v.Name = allText.Substring(first, allText.Length - first - 1);
                            }

                            /*
                            Debug.Log(v.state);
                            Debug.Log(v.type);
                            Debug.Log(v.name);
                            */
                        }

                        v.Description = description;
                        v.UniqueID = TemplateParser.GenerateUniqueId(v.Name);
                        obj.Variables.Add(v);
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
                            f.Scope = firstString;
                            f.Type = allText.Substring(first, second - first);

                            f.Name = allText.Substring(second, allText.IndexOf(')', second) - second + 1);
                            /*
                            Debug.Log(f.state);
                            Debug.Log(f.type);
                            Debug.Log(f.name);
                            */
                        }
                        else
                        {
                            f.Scope = "";
                            f.Type = firstString;
                            f.Name = allText.Substring(first, allText.IndexOf(')', first) - first + 1);
                            /*
                            Debug.Log(f.state);
                            Debug.Log(f.type);
                            Debug.Log(f.name);
                            */
                        }

                        f.Description = description;
                        f.UniqueID = TemplateParser.GenerateUniqueId(f.Name);
                        obj.Functions.Add(f);
                        description = "";
                    }
                }
            }

            obj.UniqueId = TemplateParser.GenerateUniqueId(obj.Name);

            reader.Close();
            this.Files.Add(obj);
        }

        public string Stitch()
        {
            string finalDoc = "";
            Dictionary<string, string> baseTemp = new Dictionary<string, string>();
            baseTemp["#Project_Title"] = ProjectTitle;
            TemplateParser templateParser = new TemplateParser();
            finalDoc = templateParser.ParseRegion("base_template", baseTemp);
            //Debug.Log(finalDoc);
            finalDoc += templateParser.ParseRegion("body_begin");
            //Debug.Log(finalDoc);
            Dictionary<string, string> homeBase = new Dictionary<string, string>();
            homeBase["#author_name"] = AuthorName;
            homeBase["#Title"] = Title;
            homeBase["#version"] = Version;
            homeBase["#last_update"] = LastUpdate;
            homeBase["#free_space"] = "";
            List<string> list = new List<string>();
            foreach (Obj obj in Files)
            {
                TemplateParser tempParse = new TemplateParser();
                string className = obj.Name;
                //Debug.Log("Class Name "+ className);
                string classDescription = obj.Description;
                string uniqueid = obj.UniqueId;
                //Debug.Log("Unique ID" + uniqueid);
                Dictionary<string, string> essentials = new Dictionary<string, string>();
                essentials["#UNIQUEID"] = uniqueid;
                essentials["#Class_Description"] = classDescription;
                essentials["#Class_Name"] = className;
                try
                {
                    string htmlRep = tempParse.ParseRegion("class_content", essentials);
                    //Debug.Log("htmlrep: " + htmlRep + " Class name: " + className);
                    list.Add(htmlRep);
                }
                catch (Exception e)
                { 
                    Debug.Log(e);
                }
               
            }
            foreach (string obj in list)
            {
                homeBase["#free_space"] += obj;
            }

            try
            {
                finalDoc += templateParser.ParseRegion("home_base", homeBase);
            }
            catch (Exception e)
            {
                Debug.Log("issue " + e);
            }
            //Debug.Log(finalDoc);
            Dictionary<string, string> selected_file_start = new Dictionary<string, string>();
            list.Clear();
            foreach (Obj obj in Files)
            {
                string html_rep = "";
                selected_file_start["#UNIQUEID"] = obj.UniqueId;
                selected_file_start["#class_name"] = obj.Name;

                //Generate all the variable cards
                Dictionary<string, string> accordian_insertion = new Dictionary<string, string>();
                List<string> variableCardList = new List<string>();
                foreach (Variable var in obj.Variables)
                {
                    Dictionary<string, string> card_content = new Dictionary<string, string>();
                    card_content["#collapseName"] = var.UniqueID;
                    card_content["#scope_type"] = var.Scope;
                    card_content["#object_type"] = var.Type;
                    card_content["#ref_name"] = var.Name;
                    card_content["#body_content"] = var.Description;
                    //Debug.Log("Var name: " + var.Name + "Var Description: " + var.Description);
                    html_rep = templateParser.ParseRegion("card_content", card_content);
                    variableCardList.Add(html_rep);
                }

                accordian_insertion["#free_space"] = "";
                foreach (string card in variableCardList)
                {
                    accordian_insertion["#free_space"] += card;
                }
                string variablesAccordion = templateParser.ParseRegion("accordion_container", accordian_insertion);
                selected_file_start["#accordion_insertion_var"] = variablesAccordion;


                //Generate all the function cards
                Dictionary<string, string> accordian_insertion_func = new Dictionary<string, string>();
                List<string> functionCardList = new List<string>();
                foreach (Function func in obj.Functions)
                {
                    Dictionary<string, string> card_content = new Dictionary<string, string>();
                    card_content["#collapseName"] = func.UniqueID;
                    card_content["#scope_type"] = func.Scope;
                    card_content["#object_type"] = func.Type;
                    card_content["#ref_name"] = func.Name;
                    card_content["#body_content"] = func.Description;
                    html_rep = templateParser.ParseRegion("card_content", card_content);
                    functionCardList.Add(html_rep);
                }

                accordian_insertion_func["#free_space"] = "";
                foreach (string card in functionCardList)
                {
                    accordian_insertion_func["#free_space"] += card;
                }
                string functionAccordion = templateParser.ParseRegion("accordion_container", accordian_insertion_func);
                selected_file_start["#accordion_insertion_func"] = functionAccordion;


                html_rep = templateParser.ParseRegion("selected_file_start", selected_file_start);
                list.Add(html_rep);
            }

            foreach (string card in list)
            {
                finalDoc += card; 
            }
            //Debug.Log("Body End " + templateParser.ParseRegion("body_end"));
            
            finalDoc += templateParser.ParseRegion("body_end");
            finalDoc += templateParser.ParseRegion("base_end");
<<<<<<< HEAD
=======
            //Debug.Log("Final output: " + finalDoc);
>>>>>>> 7ba681e16b1a36ae4b16a898816f034f789d83b9
            return finalDoc;

        }

        public string ReverseSlashes(string dir)
        {
            string path = "";
            for (int i = 0; i < dir.Length; i += 1)
            {
                if (dir[i] == '/')
                {
                    path += "\\";
                }
                else
                {
                    path += dir[i];
                }
            }
            return path;
        }

        public void Save(string saveDir)
        {
            try
            {
                string Output = ReverseSlashes(saveDir) + "\\" + this.Title;

                string CSSToCopy = ReverseSlashes(Application.dataPath) + "\\Editor\\uDocuGen\\HTML\\css\\style.css";
                string JSToCopy = ReverseSlashes(Application.dataPath) + "\\Editor\\uDocuGen\\HTML\\js\\.script.js";

                if (!Directory.Exists(Output))
                {
                    Directory.CreateDirectory(Output);
                }
                File.WriteAllText(Output + "\\test.html", outputBlock);

                if (!Directory.Exists(Output + "\\css\\"))
                {
                    Directory.CreateDirectory(Output + "\\css\\");
                }
                File.Copy(CSSToCopy, Output + "\\css\\" + Path.GetFileName(CSSToCopy));

                if (!Directory.Exists(Output + "\\js\\"))
                {
                    Directory.CreateDirectory(Output + "\\js\\");
                }
                File.Copy(JSToCopy, Output + "\\js\\" + Path.GetFileName(JSToCopy));
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            
        }

            
        
    }
}