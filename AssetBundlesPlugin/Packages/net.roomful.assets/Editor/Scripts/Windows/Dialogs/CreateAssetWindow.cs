using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.Editor
{
    public class CreateAssetWindow : EditorWindow
    {
        private PropTemplate m_template = new PropTemplate();

        void OnGUI() {
            var wizardIcon = IconManager.GetIcon(Icon.wizard);
            var wizardContent = new GUIContent(wizardIcon, "");
            EditorGUI.LabelField(new Rect(10, 10, 70, 70), wizardContent);

            var headerContent = new GUIContent("Please provide general information about \nyour new Roomful Prop");
            EditorGUI.LabelField(new Rect(100, 10, 300, 40), headerContent);

            EditorGUI.LabelField(new Rect(100, 50, 300, 16), "Name:");
            m_template.Title = EditorGUI.TextField(new Rect(170, 50, 180, 16), m_template.Title);

            EditorGUI.LabelField(new Rect(100, 70, 300, 16), "Placement:");
            m_template.Placing = (Placing) EditorGUI.EnumPopup(new Rect(170, 70, 180, 16), m_template.Placing);

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
            m_template = new PropTemplate();
            Close();
        }
    }
}