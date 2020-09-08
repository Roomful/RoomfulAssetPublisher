using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.Editor
{
    public class CreateEnvironmentWindow : EditorWindow
    {
        private EnvironmentTemplate m_template = new EnvironmentTemplate();

        void OnGUI() {
            var wizardIcon = IconManager.GetIcon(Icon.wizard);
            var wizardContent = new GUIContent(wizardIcon, "");
            EditorGUI.LabelField(new Rect(10, 10, 70, 70), wizardContent);

            var headerContent = new GUIContent("Please provide general information about \nyour new Roomful Environment");
            EditorGUI.LabelField(new Rect(100, 10, 300, 40), headerContent);

            EditorGUI.LabelField(new Rect(100, 50, 300, 16), "Name:");
            m_template.Title = EditorGUI.TextField(new Rect(160, 50, 190, 16), m_template.Title);

            GUILayout.Space(110f);
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                var cancel = GUILayout.Button("Cancel", EditorStyles.miniButton, GUILayout.Width(80));
                if (cancel) {
                    Dismiss();
                }

                var create = GUILayout.Button("Create", EditorStyles.miniButton, GUILayout.Width(80));
                if (create) {
                    BundleService.Create(m_template);
                    Dismiss();
                }

                GUILayout.Space(20f);
            }
            GUILayout.EndHorizontal();
        }

        private void Dismiss() {
            m_template = new EnvironmentTemplate();
            Close();
        }
    }
}