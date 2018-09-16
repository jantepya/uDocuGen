using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace uDocuGen
{
    public class uDocGenInterface : EditorWindow
    {
        [MenuItem("Tools/Generate Documentation")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(uDocGenInterface));
        }

        public string projectName;
        public string authorName;
        public string saveDirectory;
        public string docuTitle;
        public string version;
        public string LastUpdate;
        public string status = "";

        public GUIContent ButtonLabel = new GUIContent("Choose a Directory");
        
        void OnGUI()
        {
            titleContent = new GUIContent("uDocuGen");
            minSize = new Vector2(215,310);
            GUILayout.Label("Project Name", EditorStyles.boldLabel);
            projectName = GUILayout.TextField(Application.productName);
            GUILayout.Label("Author", EditorStyles.boldLabel);
            authorName = GUILayout.TextField("");
            GUILayout.Label("Document Title", EditorStyles.boldLabel);
            docuTitle = GUILayout.TextField("");
            GUILayout.Label("Version", EditorStyles.boldLabel);
            version = GUILayout.TextField("");
            GUILayout.Label("LastUpdate", EditorStyles.boldLabel);
            LastUpdate = GUILayout.TextField("");
            
            GUILayout.Label("Directory", EditorStyles.boldLabel);
            if (EditorGUILayout.DropdownButton(ButtonLabel, FocusType.Passive))
            {
                string _path = EditorUtility.SaveFolderPanel("Choose Where Documentation is Saved", Application.dataPath, projectName);
                ButtonLabel = new GUIContent(_path);
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            if (GUILayout.Button("Generate"))
            {
                Document doc = new Document();
                if (doc.GenerateDocument())
                {
                    doc.Save(saveDirectory);
                    status = "Documentation Complete";
                }
                else
                {
                    status = "Error: Could not Generate Document";
                }
            }
            GUILayout.Label(status, EditorStyles.boldLabel);
        }
    }
}
