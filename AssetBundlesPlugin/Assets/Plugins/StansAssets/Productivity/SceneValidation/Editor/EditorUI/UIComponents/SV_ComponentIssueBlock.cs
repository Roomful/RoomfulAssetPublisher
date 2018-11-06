using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;

namespace SA.Productivity.SceneValidator
{


    [Serializable]
    public class SV_ComponentIssueBlock : IDisposable
    {

        private int m_indentLevel;

        public SV_ComponentIssueBlock(Component component) {

            GUIContent header = new GUIContent();
            header.text = ObjectNames.GetInspectorTitle(component);// component.GetType().Name;
           
            GUIContent imageContent = new GUIContent();
            imageContent.image = EditorGUIUtility.ObjectContent(null, component.GetType()).image;
            if(imageContent.image == null) {
                imageContent.image = EditorGUIUtility.FindTexture("cs Script Icon");
            }

            using (new SA_GuiBeginHorizontal()) {
                GUILayout.Space(-5);
                EditorGUILayout.LabelField(imageContent, GUILayout.Width(20));
                GUILayout.Space(-5);
                EditorGUILayout.LabelField(header, SV_Skin.ComponentIssuesHeader);
                EditorGUILayout.Space();
            }

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
        }
    }
}