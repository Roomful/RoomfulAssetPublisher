using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Utility;
using StansAssets.Plugins.Editor;



namespace SA.Productivity.GoogleSheets
{
    [Serializable]
    public class GD_LocalizationTab : IMGUILayoutElement
    {

        [SerializeField] int m_localizationDocIndex = 0;

        private string[] m_dataList = null;
        private GD_DocTemplate m_localizationDoc = null;
        private List<string> m_detectedLanguages = new List<string>();

        public override void OnLayoutEnable() {
            base.OnLayoutEnable();
            InitDocumentTemplate();
        }

        public override void OnGUI() {
           

          
            using(new IMGUIWindowBlockWithIndent(new GUIContent("Localization Spreadsheet"))) {


                m_dataList = GD_Settings.Instance.GetDocNames();

                if (m_localizationDocIndex + 1 > m_dataList.Length) {
                    m_localizationDocIndex = 0;
                }


                using(new IMGUIBeginHorizontal()) {
                    using(new IMGUIBeginVertical()) {
                        GUILayout.Space(4);
                        EditorGUI.BeginChangeCheck();
                        m_localizationDocIndex = EditorGUILayout.Popup(m_localizationDocIndex, m_dataList);
                        if (EditorGUI.EndChangeCheck()) {
                            OnListChanged();
                        }
                    }

                    GUI.enabled = m_localizationDoc != null;
                    if (GD_Skin.RefreshButtonClick()) {
                        GD_API.RetrievePublicSheetData(m_localizationDoc, true, () => {
                            OnListChanged();
                        });
                    }


                    if (GD_Skin.ViewButtonClick()) {
                        GD_Settings.OpenDocURL(m_localizationDoc);
                    }

                    GUI.enabled = true;

                }

            }

            using (new IMGUIWindowBlockWithIndent(new GUIContent("Document Info"))) {
                DocumentInfo();
            }

            using (new IMGUIWindowBlockWithIndent(new GUIContent("Actions"))) {
                Actions();
            }
        }





        private void DocumentInfo() {

            if(m_localizationDoc == null) {
                EditorGUILayout.HelpBox("No Localization doc selected.", MessageType.Error);
                return;
            }


            if (m_localizationDoc.HasCache) {
                Color oldColor = GUI.color;
                GUI.color = Color.green;
                EditorGUILayout.LabelField(m_localizationDoc.CreationTime);
                GUI.color = oldColor;
            } else {
                EditorGUILayout.HelpBox("The Document is not cached yet.", MessageType.Warning);
            }


            EditorGUILayout.LabelField(m_detectedLanguages.Count + " Languages:");



            EditorGUI.indentLevel++;
            int j = 0;
            for (int i = 0; i < m_detectedLanguages.Count; i++) {

                if (j == 0) { EditorGUILayout.BeginHorizontal(); }

                EditorGUILayout.LabelField(m_detectedLanguages[i].ToUpper(), GUILayout.Width(100));
                j++;

                if (j == 3) {
                    j = 0;
                    EditorGUILayout.EndHorizontal();
                }

            }

            if (j != 0) { EditorGUILayout.EndHorizontal(); }
            EditorGUI.indentLevel--;



            TextAnchor cachedAlligment = GUI.skin.label.alignment;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;

            string sh = " Sheet";
            if (m_localizationDoc.Worksheets.Count > 1) {
                sh = " Sheets";
            }

            EditorGUILayout.LabelField(m_localizationDoc.Worksheets.Count + sh + " Avaliable:");
 
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical();
           

            j = 0;
            for (int i = 0; i < m_localizationDoc.Worksheets.Count; i++) {
                if (j == 0) { EditorGUILayout.BeginHorizontal(); }

         
                using (new IMGUIBeginHorizontal(GUI.skin.box)) {
                    GUILayout.Space(-12);

                    int size =  (int)(m_Position.width / 2f) - 38;
                    EditorGUILayout.LabelField(m_localizationDoc.Worksheets[i].ListName, GD_Skin.LeftAllignedLabled, GUILayout.Width(size));
                    if (GD_Skin.RefreshButtonClick()) {
                        GD_SettingsWindow.UpdateWorksheet(m_localizationDoc, m_localizationDoc.Worksheets[i]);
                    }
                    /*
                    if (GD_Skin.ViewButtonClick()) {
                        GD_Settings.OpenDocURL(m_localizationDoc, m_localizationDoc.Worksheets[i]);
                    }*/
                }
                j++;

                if (j == 2) {
                    j = 0;
                    EditorGUILayout.EndHorizontal();
                }

            }

            if (j != 0) { EditorGUILayout.EndHorizontal(); }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            GUI.skin.label.alignment = cachedAlligment;
            
        }

        private void Actions() {
               
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Example Settings", GUILayout.Width(120))) {
                        GD_DocTemplate LE = new GD_DocTemplate();
                        LE.Name = "Google Doc Localization Example";
                        LE.Key = "1hJQHrMwhvQyQlhKkn1SIsaqXuIZ80rWfPa_j1xqTUAc";

                        GD_WorksheetTemplate ws = new GD_WorksheetTemplate("General", 0);
                        LE.Worksheets.Add(ws);

                        GD_Settings.Instance.AddDoc(LE);
                        GD_Settings.Instance.LocalizationDocKey = LE.Key;
                        GD_API.RetrievePublicSheetData(LE, true, () => {
                            InitDocumentTemplate();
                            OnListChanged();
                        });
                    }
                    
                    if (GUILayout.Button("Validate", GUILayout.Width(120))) {
                        ValidateLocalization();
                    }
                }
                EditorGUILayout.EndHorizontal();

        }

        
        private void ValidateLocalization() {
            var invalidData = new List<string>();
            
            RefreshLocalizationInfo();
            
            if (m_localizationDoc == null) {
                return;
            }

            var m_langSheetsDictionary = new Dictionary<string, Dictionary<string, List<string>>>();

            foreach (GD_LangSection section in Enum.GetValues(typeof(GD_LangSection))) {
                string workSheetName = section.ToString();

                GD_CellRange keysRange = new GD_CellRange("A2", GD_RanageDirection.Collumn);

                foreach (GD_LangCode lang in Enum.GetValues(typeof(GD_LangCode))) {
                    GD_CellDictionaryRange valuesRange = new GD_CellDictionaryRange(keysRange, (int)lang + 1, 0);
                    var dict = GD_API.GetDictionary<string, string>(m_localizationDoc.Name, valuesRange, workSheetName);
                    
                    foreach (KeyValuePair<string, string> pair in dict) {
                        if (!m_langSheetsDictionary.ContainsKey(workSheetName)) {
                            m_langSheetsDictionary.Add(workSheetName, new Dictionary<string, List<string>>()); //key, Array of values(for each language)
                        }

                        if (string.IsNullOrEmpty(pair.Key)) {
                            Debug.LogError(string.Concat("Token not found!", "Worksheet: ", workSheetName, " Lang:", lang.ToString()));
                            continue;
                        }

                        string validValue = pair.Value;
                        
                        if (string.IsNullOrEmpty(validValue)) {
                            Debug.LogError(string.Concat("Value not found!", "Worksheet: ", workSheetName, " Lang:", lang.ToString(), " Token:", pair.Key));
                            
                            validValue = string.Empty;
                            string errorDescr = string.Concat("Worksheet: ", workSheetName, " Lang:", lang.ToString(), " Token:", pair.Key);
                            invalidData.Add(errorDescr);
                        }
                        
                        if (!m_langSheetsDictionary[workSheetName].ContainsKey(pair.Key)) {
                            m_langSheetsDictionary[workSheetName].Add(pair.Key, new List<string>());
                        }
                        m_langSheetsDictionary[workSheetName][pair.Key].Add(validValue);
                    }
                }
            }

            Debug.Log("=============================");
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> pair in m_langSheetsDictionary) {
                Debug.Log("Worksheet:" + pair.Key + " | Data count:" + pair.Value.Count);
            }
            Debug.Log("=============================");
            
            if (invalidData.Count == 0) {
                Debug.Log("Validation successful!");
                return;
            }

            Debug.Log("=============================");
            foreach (var errorMessage in invalidData) {
                Debug.Log("ErrorMessage: " + errorMessage);
            }
            Debug.Log("=============================");
        }


        private void OnListChanged() {
            RefreshLocalizationInfo();

            if (m_localizationDoc == null) {
                return;
            }

            List<string> SheetsNames = new List<string>();

            foreach (GD_WorksheetTemplate sheet in m_localizationDoc.Worksheets) {
                string name = System.Text.RegularExpressions.Regex.Replace(sheet.ListName, @"\s+", string.Empty);
                SheetsNames.Add(name);
            }

            //Move to localization util internal class, and we probably need to show and update warning for that
            EditEnum("SA.Productivity.GoogleSheets", "GD_LangSection", SheetsNames);
            EditEnum("SA.Productivity.GoogleSheets", "GD_LangCode", m_detectedLanguages);

            AssetDatabase.Refresh();
        }

        private void RefreshLocalizationInfo() {
            //None option Selected
            if (m_localizationDocIndex == 0) {
                m_localizationDoc = null;
                GD_Settings.Instance.LocalizationDocKey = string.Empty;
                m_detectedLanguages = new List<string>();
                return;
            }

            m_localizationDoc = GD_Settings.Instance.GetDocByIndex(m_localizationDocIndex);
            
            if(m_localizationDoc == null || !m_localizationDoc.HasCache) {
                return;
            }
            
            GD_Settings.Instance.LocalizationDocKey = m_localizationDoc.Key;
            m_detectedLanguages = GD_API.GetList<string>(m_localizationDoc.Name, new GD_CellRange("B1"), m_localizationDoc.Worksheets[0].ListName);
        }

        private void EditEnum(string nameSpace, string enumName, List<string> newEnumElements) {
            string NewEnumScript = "namespace " + nameSpace + " { \n";
            string relativeFilePath = GD_Settings.LOCALIZATION_ENUMS_PATH + enumName + ".cs";
            string NewEnumPath = SA_PathUtil.ConvertRelativeToAbsolutePath(relativeFilePath);

            NewEnumScript += "\tpublic enum " + enumName + " {\n";

            for (int i = 0; i < newEnumElements.Count; i++) {
                if (i < newEnumElements.Count - 1) {
                    NewEnumScript += "\t\t" + newEnumElements[i] + ",\n";
                } else {
                    NewEnumScript += "\t\t" + newEnumElements[i];
                }
            }

            NewEnumScript += "\n\t}\n}";

            System.IO.File.WriteAllText(NewEnumPath, NewEnumScript);
        }

        private void InitDocumentTemplate() {
           
            m_localizationDoc = GD_Settings.Instance.GetDocByKey(GD_Settings.Instance.LocalizationDocKey);
            if (m_localizationDoc != null) {
                m_localizationDocIndex = GD_Settings.Instance.GetDocIndexByKey(GD_Settings.Instance.LocalizationDocKey);
            }

            RefreshLocalizationInfo();
        }


       
    }
}
