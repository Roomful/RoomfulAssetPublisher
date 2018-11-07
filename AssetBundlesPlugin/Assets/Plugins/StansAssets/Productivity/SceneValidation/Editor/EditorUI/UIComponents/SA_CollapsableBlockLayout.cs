
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;


using SA.Foundation.Config;
using SA.Foundation.Utility;

using SA.Foundation.Editor;


namespace SA.Productivity.SceneValidator
{
    [Serializable]
    public class SA_CollapsableBlockLayout
    {

        private Action m_onGUI;

        private AnimBool m_showExtraFields = new AnimBool(false);

        private GUIContent m_content;

        public Action OnGUIExpanded {
            get {
                return m_onGUI;
            }

            set {
                m_onGUI = value;
            }
        }

        public GUIContent Content {
            get {
                return m_content;
            }

            set {
                m_content = value;
            }
        }

        public SA_CollapsableBlockLayout(GUIContent content, Action onGUI) {

            m_content = content;
            m_onGUI = onGUI;
           

        }

        protected virtual void OnAfterHeaderGUI() {

        }

        public void OnGUI() {
            GUILayout.Space(5);
            using (new SA_GuiBeginHorizontal()) {
               


                var textDimensions = EditorStyles.label.CalcSize(m_content);
                EditorGUILayout.LabelField(m_content, GUILayout.Width(textDimensions.x));
                GUILayout.FlexibleSpace();

                string buttonText = "Show";
                if(m_showExtraFields.target) {
                    buttonText = "Hide";
                }
                bool show = GUILayout.Button(buttonText, EditorStyles.miniButton, GUILayout.Width(50));
                if (show) {
                    m_showExtraFields.target = !m_showExtraFields.target;
                }
                OnAfterHeaderGUI();
            }

            using (new SA_GuiIndentLevel(1)) {
                if (EditorGUILayout.BeginFadeGroup(m_showExtraFields.faded)) {
                    GUILayout.Space(5);
                    m_onGUI.Invoke();
                    GUILayout.Space(5);
                }
                EditorGUILayout.EndFadeGroup();
            }

        }

    }
}