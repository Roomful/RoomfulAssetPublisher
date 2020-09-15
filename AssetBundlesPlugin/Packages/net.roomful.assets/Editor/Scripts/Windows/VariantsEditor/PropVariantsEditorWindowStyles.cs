using UnityEditor;
using UnityEngine;

namespace net.roomful.assets.Editor
{
    public static class PropVariantsEditorWindowStyles
    {
        private static GUIStyle s_variantTitle;
        public static GUIStyle VariantTitle
        {
            get
            {
                if (s_variantTitle == null)
                {
                    s_variantTitle = "PreferencesSection";
                    s_variantTitle.alignment = TextAnchor.MiddleLeft;
                }

                return s_variantTitle;
            }
        }

        private static GUIStyle s_headerLabel;
        public static GUIStyle HeaderLabel
        {
            get
            {
                if (s_headerLabel == null)
                {
                    s_headerLabel = new GUIStyle(EditorStyles.largeLabel);
                    s_headerLabel.fontStyle = FontStyle.Bold;
                    s_headerLabel.fontSize = 18;
                    s_headerLabel.margin.top = -1;
                    s_headerLabel.margin.left++;

                    s_headerLabel.normal.textColor = !EditorGUIUtility.isProSkin ?
                        new Color(0.4f, 0.4f, 0.4f, 1f) : new Color(0.7f, 0.7f, 0.7f, 1f);
                }
                return s_headerLabel;
            }
        }
    }
}