using net.roomful.api;
using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    class CreateStyleWindow : EditorWindow
    {
        StyleAssetTemplate m_AssetTemplate = new StyleAssetTemplate();

        void OnGUI() {
            var wizardIcon = IconManager.GetIcon(Icon.wizard);
            var wizardContent = new GUIContent(wizardIcon, "");
            EditorGUI.LabelField(new Rect(10, 10, 70, 70), wizardContent);

            var headerContent = new GUIContent("Please provide general information about \nyour new Roomful Style");
            EditorGUI.LabelField(new Rect(100, 10, 300, 40), headerContent);

            EditorGUI.LabelField(new Rect(100, 50, 300, 16), "Name:");
            m_AssetTemplate.Title = EditorGUI.TextField(new Rect(160, 50, 190, 16), m_AssetTemplate.Title);

            EditorGUI.LabelField(new Rect(100, 70, 300, 16), "Type:");
            m_AssetTemplate.StyleType = (StyleType)EditorGUI.EnumPopup(new Rect(160, 70, 190, 16), m_AssetTemplate.StyleType);
            
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
                    BundleService.Create(m_AssetTemplate);
                    Dismiss();
                }

                GUILayout.Space(20f);
            }
            GUILayout.EndHorizontal();
        }

        void Dismiss() {
            m_AssetTemplate = new StyleAssetTemplate();
            Close();
        }
    }
}