using UnityEngine;
using UnityEditor;
using System;
using StansAssets.Plugins.Editor;

namespace net.roomful.assets.editor
{
     internal class DialogCreateVariant : EditorWindow
    {
        public event Action<bool, string> OnCreateClickEvent = delegate { };
        public string NameNewUnit  = "prop variant";
        public string HeaderName = "Enter variant name";
        bool m_IsFocused;
        bool m_IsSuccessful;

        private void OnGUI() {
            if (Event.current.type == EventType.KeyDown) {
                switch (Event.current.keyCode) {
                    case KeyCode.Return:
                    case KeyCode.KeypadEnter:
                        m_IsSuccessful = true;
                        Dismiss();
                        break;
                }
            }

            using (new IMGUIBeginHorizontal()) {
                var headerContent = new GUIContent(HeaderName);
                GUILayout.FlexibleSpace();
                GUILayout.Label(headerContent);
                GUILayout.FlexibleSpace();
            }

            GUILayout.Space(15f);
            using (new IMGUIBeginHorizontal()) {
                GUILayout.FlexibleSpace();
                GUI.SetNextControlName(NameNewUnit);
                NameNewUnit = GUILayout.TextField(NameNewUnit, GUILayout.Width(175));
                GUILayout.FlexibleSpace();
                if (!m_IsFocused) {
                    EditorGUI.FocusTextInControl(NameNewUnit);
                    m_IsFocused = true;
                }
            }

            GUILayout.Space(10f);
            using (new IMGUIBeginHorizontal()) {
                GUILayout.FlexibleSpace();
                var cancel = GUILayout.Button("Cancel", EditorStyles.miniButton, GUILayout.Width(75));
                if (cancel) {
                    Dismiss();
                }

                GUILayout.Space(20);
                var create = GUILayout.Button("Create", EditorStyles.miniButton, GUILayout.Width(75));
                if (create) {
                    m_IsSuccessful = true;
                    Dismiss();
                }
                GUILayout.FlexibleSpace();
            }
        }

        private void Dismiss() {
            Close();
        }

        private void OnDisable() {
            OnCreateClickEvent(m_IsSuccessful, NameNewUnit);
        }
    }
}