using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using SA.Foundation.Editor;

namespace SA.Productivity.SceneValidator
{
    [Serializable]
    public class SV_SceneReportBlock : IDisposable
    {

        private int m_indentLevel;

        public SV_SceneReportBlock(Scene scene) {


            string name = scene.name;
            if (string.IsNullOrEmpty(name)) {
                name = "Untiteled";
            }

            var header = new GUIContent(name);

            GUILayout.Space(10);
            using (new SA_GuiBeginHorizontal()) {
                GUILayout.Space(10);
                EditorGUILayout.LabelField(header, SA_PluginSettingsWindowStyles.ServiceBlockHeader);

                Texture2D image = SA_Skin.GetGenericIcon("refresh.png");
                using (new SA_GuiChangeContentColor(SA_PluginSettingsWindowStyles.DefaultImageContentColor)) {
                    bool refresh =  GUILayout.Button(new GUIContent(string.Empty, image), GUILayout.Height(16), GUILayout.Width(24));
                    if(refresh) {
                        SV_Validation.API.ValidateScene(scene);
                    }
                }
            }
            GUILayout.Space(5);



            m_indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15);
            EditorGUILayout.BeginVertical();

        }

        public void Dispose() {

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel = m_indentLevel;

            GUILayout.Space(10);
            EditorGUILayout.BeginVertical(SA_PluginSettingsWindowStyles.SeparationStyle);
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
        }
    }
}



