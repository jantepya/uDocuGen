﻿using System;
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
            Debug.Log(File.ReadAllText(templatePath));
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
                        var indicies = new List<int>();
                        for (var i = 0; i < line.Length; i++)
                            if (line[i] == '#')
                                indicies.Add(i);

                        finalStr += line;
                        foreach (var tagIndex in indicies)
                        {
                            var specifiedTag = "#";
                            var finalIndex = tagIndex + 1;

                            Debug.Log("Current line: " + line);

                            while (finalIndex < line.Length &&
                                   (char.IsLetter(line[finalIndex]) || line[finalIndex] == '_'))
                            {
                                specifiedTag += line[finalIndex];
                                //Debug.Log("Current char: "+ line[finalIndex]);
                                finalIndex++;
                            }

                            Debug.Log("Specified Tag:" + specifiedTag);
                            try
                            {
                                specifiedTag.Replace(" ", String.Empty);
                                foreach (var key in replace.Keys) Debug.Log("Key " + key);
                                if (specifiedTag != "#accordion")
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
                else if (line.Contains("# ")) //end if out of region
                    return finalStr;
                else if (correctBlock) finalStr += line;

            return finalStr;
        }

        public static string GenerateUniqueId(string name)
        {
            var rand = new Random();

            return string.Format("{0}{1}", name, rand.Next(100000000));
        }
    }
}