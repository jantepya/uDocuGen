﻿using System.Collections;
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
        public string status;
        
        public GUIContent ButtonLabel = new GUIContent("Choose a Directory");
        public string savePath = "";

        void OnEnable()
        {
            titleContent = new GUIContent("uDocuGen");
            minSize = new Vector2(215, 310);
            projectName = Application.productName;
            authorName = "Jonathan and Eric";
            saveDirectory = "";
            docuTitle = "uDocuGen";
            version = "1.0.0";
            LastUpdate = "15/9/2018";
            status = "";
        }

        void OnGUI()
        {        
            GUILayout.Label("Project Name", EditorStyles.boldLabel);
            projectName = GUILayout.TextField(projectName);
            GUILayout.Label("Author", EditorStyles.boldLabel);
            authorName = GUILayout.TextField(authorName);
            GUILayout.Label("Document Title", EditorStyles.boldLabel);
            docuTitle = GUILayout.TextField(docuTitle);
            GUILayout.Label("Version", EditorStyles.boldLabel);
            version = GUILayout.TextField(version);
            GUILayout.Label("LastUpdate", EditorStyles.boldLabel);
            LastUpdate = GUILayout.TextField(LastUpdate);
            GUILayout.Label("Directory", EditorStyles.boldLabel);
            if (EditorGUILayout.DropdownButton(ButtonLabel, FocusType.Keyboard))
            {
                savePath = EditorUtility.SaveFolderPanel("Choose Where Documentation is Saved", Application.dataPath, projectName);
                ButtonLabel = new GUIContent(savePath);
            }
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            if (GUILayout.Button("Generate"))
            {
                if (savePath == "")
                {
                    Debug.Log("A Directory needs to be selected");
                    status = "Choose an output directory";
                }
                else
                {
                    Debug.Log("Generating Files");
                    Document doc = new Document();
                    doc.init(projectName, authorName, docuTitle, version, LastUpdate);
                    if (doc.GenerateDocument())
                    {
                        Debug.Log("Saving:" + savePath);
                        doc.Save(savePath);
                        status = "Documentation Complete";
                        Debug.Log("Saving Complete");
                    }
                    else
                    {
                        status = "Error: Could not Generate Document";
                    }
                }
                
            }
            GUILayout.Label(status, EditorStyles.boldLabel);
        }
    }
}
