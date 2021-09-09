using System.Collections;
using System.Collections.Generic;
using System.IO;
using SA.Foundation.Config;
using UnityEngine;
using UnityEditor;

using StansAssets.Plugins.Editor;
using SA.Foundation.UtilitiesEditor;


namespace SA.Productivity.GoogleSheets
{
    public class GD_SpreadsheetsTab : IMGUILayoutElement
	{
        GUIContent docDisplayLabel = new GUIContent("Display Name [?]", "Wou will use this document name as data source");
        GUIContent docPublicUrlLabel = new GUIContent("Public Key [?]", "Document public key");

        GUIContent tableListNameLabel = new GUIContent("Name [?]", "Worksheet name");
        GUIContent tableListIdLabel = new GUIContent("Worksheet ID [?]", "Worksheet public id");


        [SerializeField] List<int> m_openedDocIds = new List<int>();
          
		public override void OnLayoutEnable() {
            base.OnLayoutEnable();
        }
	    
	    public override void OnGUI() {
            using (new IMGUIBlockWithIndent(new GUIContent("Spreadsheets"))) {

                using (new IMGUIBeginHorizontal()) {
                    GUILayout.Space(15);

                    using (new IMGUIBeginVertical()) {
                        Documents();
                    }
                }

                EditorGUILayout.Space();
            }

		}

        private bool IsDocOpen(int id) {
            return m_openedDocIds.Contains(id);
        }

        private void SetDocOpenState(bool isOpen, int id) {
            if(m_openedDocIds.Contains(id)) {
                m_openedDocIds.Remove(id);
            }

            if(isOpen) {
                m_openedDocIds.Add(id);
            }
        }



        private void Documents() {

            if (GD_Settings.Instance.Documents.Count == 0) {
                EditorGUILayout.HelpBox("You haven't added any Spreadsheets yet", MessageType.Error);
            }

            for(int i = 0; i < GD_Settings.Instance.Documents.Count; i++) {
                GD_DocTemplate doc = GD_Settings.Instance.Documents[i];


                EditorGUI.indentLevel = 0;
                EditorGUILayout.BeginVertical (GUI.skin.box);

                EditorGUILayout.BeginHorizontal();

                bool isOpen = IsDocOpen(i);

                EditorGUI.BeginChangeCheck();
                isOpen = EditorGUILayout.Foldout(isOpen, doc.Name);
                if (EditorGUI.EndChangeCheck()) {
                    SetDocOpenState(isOpen, i);
                }
                
                EditorGUILayout.Space();

                if (doc.HasCache) {
                    Color oldColor = GUI.color;
                    GUI.color = Color.green;
                    EditorGUILayout.LabelField(doc.CreationTime, GD_Skin.RighltAllignedLabled);
                    GUI.color = oldColor;
                    DrawDocButtons(doc);
                } else {
                    Color oldColor = GUI.color;
                    GUI.color = Color.red;
                    EditorGUILayout.LabelField("-- : --", GD_Skin.RighltAllignedLabled);
                    GUI.color = oldColor;
                    DrawDocButtons(doc);
                }

                EditorGUILayout.EndHorizontal();
               

                EditorGUILayout.Space();
                if(isOpen) {

                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(docDisplayLabel);
					string newDocName = EditorGUILayout.TextField(doc.Name);

                    if (newDocName != doc.Name) {
                        if (File.Exists(GD_Settings.GetCachePath(doc.Name))) {
                            // Check for the file with the same name
                            if (!File.Exists(GD_Settings.GetCachePath(newDocName))) {
                                File.Move(GD_Settings.GetCachePath(doc.Name), GD_Settings.GetCachePath(newDocName));
                            }
                            AssetDatabase.Refresh();
                        }

                       doc.Name = newDocName;
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(docPublicUrlLabel);
                    doc.Key     = EditorGUILayout.TextField(doc.Key);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    if (doc.Worksheets.Count > 0) {
                       
                        using(new IMGUIBeginHorizontal()) {
                            EditorGUILayout.LabelField("Worksheets", EditorStyles.boldLabel, GUILayout.Width(90));
                            GUILayout.FlexibleSpace();
                        }

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginVertical();

                        IMGUILayout.ReorderablList(doc.Worksheets,
                                (GD_WorksheetTemplate worksheet) => {
                                    return worksheet.ListName;
                                },
                                (GD_WorksheetTemplate worksheet) => {

                                //Warnings
                                if (doc.WorksheetExist(worksheet.ListName)) {
                                        EditorGUILayout.HelpBox("Worksheet with such Name already exist", MessageType.Warning);
                                    }

                                    if (doc.WorksheetExist(worksheet.ListId) && doc.Worksheets.IndexOf(worksheet) != 0) {
                                        EditorGUILayout.HelpBox("Worksheet with such Id already exist", MessageType.Warning);
                                    }

                                //Editor UI
                                if (doc.Worksheets.IndexOf(worksheet) == 0) {
                                        GUI.enabled = false;
                                    }

                                    worksheet.ListId = IMGUILayout.IntField(tableListIdLabel, worksheet.ListId);
                                    GUI.enabled = true;
                                    worksheet.ListName = IMGUILayout.TextField(tableListNameLabel, worksheet.ListName);

                                },
                                () => {
                                    doc.AddNewWorksheet();
                                },

                                (GD_WorksheetTemplate worksheet) => {
                                    if (GD_Skin.RefreshButtonClick()) {
                                         GD_SettingsWindow.UpdateWorksheet(doc, worksheet);
                                    }

                                    if (doc.Worksheets.IndexOf(worksheet) == 0) {
                                        GUI.enabled = false;
                                    }

                                    if (GD_Skin.TrashButtonClick()) {
                                        doc.RemoveWorksheet(worksheet);
                                        GUIUtility.ExitGUI();
                                    }
                                    GUI.enabled = true;



                                }
                            );
                        
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                       
                    }
                }
                
                EditorGUILayout.EndVertical ();
                
            }


            EditorGUI.indentLevel = 0;
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.Space();

            if(GUILayout.Button("Add New Spreadsheet",  GUILayout.Width(150))) {
                GD_DocTemplate newDoc = new GD_DocTemplate();
                newDoc.InitDoc();
                GD_Settings.Instance.AddDoc(newDoc);
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawDocButtons(GD_DocTemplate doc) {

            if (GD_Skin.RefreshButtonClick()) {
                #if UNITY_WEBPLAYER
                        EditorUtility.DisplayDialog("Update Not Available", "Document cash update is not available under the Web Player platform. Since writing files is forbidden in web player platform. Switch to any other platform for updating documents cash. ", "Okay");
                #else
                    GD_API.RetrievePublicSheetData(doc);
                #endif
            }
            
            if(GD_Skin.ViewButtonClick()) {
                GD_Settings.OpenDocURL(doc);
        
            }
  
            if(GD_Skin.TrashButtonClick()) {
                GD_Settings.Instance.RemoveDoc(doc);

               // File.Delete(GD_Settings.GetCachePath(doc.Name));
               // AssetDatabase.Refresh();
                GUIUtility.ExitGUI();
            }
        }
    }
}