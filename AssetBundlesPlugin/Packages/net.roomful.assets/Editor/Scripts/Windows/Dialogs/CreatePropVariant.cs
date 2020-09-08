using UnityEngine;
using UnityEditor;
using System;
using StansAssets.Plugins.Editor;

namespace net.roomful.assets.Editor
{
    public class CreatePropVariant : EditorWindow
    {
        public event Action<bool, string> OnCreateClickEvent = delegate { };
        string m_Name = "prop variant";
        bool m_IsFocused;
        bool m_IsSuccessful;

        void OnGUI() {
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
                var headerContent = new GUIContent("Enter variant name");
                GUILayout.FlexibleSpace();
                GUILayout.Label(headerContent);
                GUILayout.FlexibleSpace();
            }

            GUILayout.Space(15f);
            using (new IMGUIBeginHorizontal()) {
                GUILayout.FlexibleSpace();
                GUI.SetNextControlName(m_Name);
                m_Name = GUILayout.TextField(m_Name, GUILayout.Width(200));
                GUILayout.FlexibleSpace();
                if (!m_IsFocused) {
                    EditorGUI.FocusTextInControl(m_Name);
                    m_IsFocused = true;
                }
            }

            GUILayout.Space(25f);
            using (new IMGUIBeginHorizontal()) {
                GUILayout.FlexibleSpace();
                var cancel = GUILayout.Button("Cancel", EditorStyles.miniButton, GUILayout.Width(100));
                if (cancel) {
                    Dismiss();
                }

                GUILayout.Space(50);
                var create = GUILayout.Button("Create", EditorStyles.miniButton, GUILayout.Width(100));
                if (create) {
                    m_IsSuccessful = true;
                    Dismiss();
                }

                GUILayout.FlexibleSpace();
            }
        }

        void Dismiss() {
            Close();
        }

        private void OnDisable() {
            OnCreateClickEvent(m_IsSuccessful, m_Name);
        }
    }
}