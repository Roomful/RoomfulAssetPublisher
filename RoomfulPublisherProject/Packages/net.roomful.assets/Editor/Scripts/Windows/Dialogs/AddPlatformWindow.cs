using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    internal class AddPlatformWindow : EditorWindow
    {
        private BuildTarget m_platform = BuildTarget.NoTarget;

        void OnGUI() {
            GUILayout.BeginVertical();

            EditorGUILayout.Space();

            GUILayout.Label("Enter name:");
            m_platform = (BuildTarget) EditorGUILayout.EnumPopup("New Platform: ", m_platform);

            if (GUILayout.Button("Add")) {
                if (m_platform == BuildTarget.NoTarget) {
                    Debug.Log("You must choose platform before adding!");
                }

                ResetAndCLose();
            }

            if (GUILayout.Button("Cancel")) {
                ResetAndCLose();
            }

            GUILayout.EndVertical();
        }

        private void ResetAndCLose() {
            m_platform = BuildTarget.NoTarget;
            Close();
        }
    }
}