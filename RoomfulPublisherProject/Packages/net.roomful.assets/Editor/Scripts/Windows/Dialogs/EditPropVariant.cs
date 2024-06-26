using UnityEngine;
using UnityEditor;
using System;
using StansAssets.Plugins.Editor;

namespace net.roomful.assets.editor
{
    internal class EditPropVariant : EditorWindow
    {
        public event Action<bool, string> OnEditClickEvent = delegate { };
        private string m_Name = string.Empty;
        private bool m_IsFocused;
        private bool m_IsSuccessful;

        public void Init(string name)
        {
            m_Name = name;
        }

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
                var headerContent = new GUIContent("Enter new variant name:");
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
                var create = GUILayout.Button("Accept", EditorStyles.miniButton, GUILayout.Width(100));
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
            OnEditClickEvent(m_IsSuccessful, m_Name);
        }
    }
}