using System.Collections;
using System.Collections.Generic;
using uDocuGen;
using UnityEngine;
using UnityEditor;

namespace uDocuGen
{

    public class UDocGenInterface : ScriptableWizard
    {
        [MenuItem("Tools/Generate Documentation")]
        static void UDocGenInterfaceWizard()
        {
            ScriptableWizard.DisplayWizard<UDocGenInterface>("Generate Documentation", "Generate");
        }

        // Called by Generate Button
        // Creates new Document Object
        void OnWizardCreate()
        {
            Document doc = new Document();
            doc.GenerateDocument();
        }
    }
}
