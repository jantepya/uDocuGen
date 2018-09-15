using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;

namespace uDocuGen
{
    public class TemplateParser : MonoBehaviour
    {
        public string[] Template; 
        public TemplateParser()
        {
            string templatePath = Application.dataPath + "//Editor//uDocuGen//HTML//templates.txt";
            Template = File.ReadAllLines(templatePath);
        }

        // Filter down part of the template and replace accordingly
        public string ParseRegion(string region_name, Dictionary<string,string> replace)
        {
            string finalStr = "";
            bool correctBlock = false;
            foreach (string line in Template)
            {
                //Find region
                if (line.Contains(region_name))
                {
                    correctBlock = true; 
                } else if (line.Contains("# ")) //end if out of region
                {
                    return finalStr;
                } else if (correctBlock)
                {
                    if (line.Contains("#"))
                    {
                       int tagIndex = line.IndexOf( "#");
                       string specifiedTag = "#";
                       int finalIndex = tagIndex;

                       while (Char.IsLetter(line[finalIndex]))
                       {
                           specifiedTag += line[finalIndex];
                           finalIndex++;
                       }
                       Console.WriteLine(specifiedTag);
                        try
                        {
                            string replacedText = line.Substring(0, tagIndex) + replace[specifiedTag] +
                                                  line.Substring(finalIndex + 1);
                            finalStr += replacedText;
                        }
                        catch (Exception e)
                        {
                            print(e);
                        }
                        
                    }
                    else
                    {
                        finalStr += line;
                    }
                }
            }

            return finalStr;
        }

        public string GenerateUniqueID(string name)
        {
            Random rand = new Random();

            return name + rand.Next(100000000).ToString();
        }
    }
}
