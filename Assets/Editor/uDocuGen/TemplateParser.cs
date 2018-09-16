using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;

namespace uDocuGen
{
    public class TemplateParser
    {
        public readonly string[] Template;

        public TemplateParser()
        {
            var templatePath = Application.dataPath + "//Editor//uDocuGen//HTML//templates.txt";
            //Debug.Log(File.ReadAllText(templatePath));
            Template = File.ReadAllLines(templatePath);
        }

        // Filter down part of the template and replace accordingly
        public string ParseRegion(string regionName, Dictionary<string, string> replace)
        {
            var finalStr = "";
            var correctBlock = false;
            foreach (var line in Template)
                //Find region
                if (line.Contains(regionName))
                {
                    correctBlock = true;
                }
                 else if (line.Contains("# ") && correctBlock) //end if out of region
                {
                    return finalStr;
                }
                else if (correctBlock)
                {
                    if (line.Contains("#"))
                    {
                        bool isHref = false;
                        var indicies = new List<int>();
                        for (var i = 0; i < line.Length; i++)
                            if (line[i] == '#')
                                indicies.Add(i);

                        finalStr += line;
                        foreach (var tagIndex in indicies)
                        {

                            try
                            {
                                if (tagIndex - 6 > 0 && ((line.Substring(tagIndex - 7, 7).Contains("href"))))
                                {
                                    isHref = true;
          
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.Log("Line: " + line + " " + e);
                            }

                            var specifiedTag = "#";
                            var finalIndex = tagIndex + 1;

                            while (finalIndex < line.Length &&
                                   (char.IsLetter(line[finalIndex]) || line[finalIndex] == '_'))
                            {
                                specifiedTag += line[finalIndex];
                                //Debug.Log("Current char: "+ line[finalIndex]);
                                finalIndex++;
                            }

                            //Debug.Log("Specified Tag:" + specifiedTag);
                            try
                            {
                                specifiedTag = specifiedTag.Replace(" ", String.Empty);
<<<<<<< HEAD
                                foreach (var key in replace.Keys) Debug.Log("Key " + key);
                                if ((isHref) && specifiedTag != "#accordion")
=======
                                foreach (var key in replace.Keys) //Debug.Log("Key " + key);
                                if (isHref && specifiedTag != "#accordion")
>>>>>>> 7ba681e16b1a36ae4b16a898816f034f789d83b9
                                {
                                    finalStr = finalStr.Replace(specifiedTag, "#" + replace[specifiedTag]);
                                }
                                else if (specifiedTag != "#accordion" && replace.ContainsKey(specifiedTag) && !isHref)
                                    finalStr = finalStr.Replace(specifiedTag, replace[specifiedTag]);
                                 
                            }
                            catch (Exception e)
                            {
                                Debug.Log(e);
                            }
                        }
                    }
                    else
                    {
                        finalStr += line;
                    }
                }

            return finalStr;
        }

        public string ParseRegion(string regionName)
        {
            var finalStr = "";
            var correctBlock = false;
            foreach (var line in Template)
                //Find region
                if (line.Contains(regionName))
                    correctBlock = true;
                else if (line.Contains("# ") && correctBlock) //end if out of region
                    return finalStr;
                else if (correctBlock) finalStr += line;

            return finalStr;
        }

        public static string GenerateUniqueId(string name)
        {
            var rand = new Random();
            string id = string.Format("{0}{1}", name, rand.Next(100000000));
            id = id.Replace(" ", string.Empty);
            id = id.Replace("(", string.Empty);
            id = id.Replace(")", string.Empty);
            return id;
        }
    }
}