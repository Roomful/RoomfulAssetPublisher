using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    [CustomEditor(typeof(StylePanel))]
    internal class StylePanelInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            if (Panel.IsFirstPanel) {
                EditorGUILayout.HelpBox("This is your default Start panel, no Icon needed", MessageType.Info);
            }
            else {
                if (Panel.IsLastPanel) {
                    EditorGUILayout.HelpBox("This is your default End panel, no Icon needed", MessageType.Info);
                }
                else {
                    Panel.Icon = (Texture2D) EditorGUILayout.ObjectField("Icon:", Panel.Icon, typeof(Texture2D), true);
                }
            }

            var def = Panel.Bounds.size * 100;
            EditorGUILayout.LabelField("Size(cm): " + (int) def.x + "x" + (int) def.y + "x" + (int) def.z);

            serializedObject.ApplyModifiedProperties();
        }

        private StylePanel Panel => target as StylePanel;
    }
}