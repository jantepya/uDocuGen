﻿using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class uDocGenInterface : ScriptableWizard
{
    [MenuItem("Tools/Generate Documentation")]
    static void uDocGenInterfaceWizard()
    {
        ScriptableWizard.DisplayWizard<uDocGenInterface>("Generate Documentation", "Generate");
    }

    // Called by Generate Button
    // Creates new Document Object
    void OnWizardCreate()
    {
        Document doc = new Document();
        doc.GenerateDocument();
    }
}