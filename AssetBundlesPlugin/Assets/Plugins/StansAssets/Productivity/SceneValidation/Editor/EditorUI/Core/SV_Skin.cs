using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;

namespace SA.Productivity.SceneValidator
{
    public static class SV_Skin 
    {


        private const string ICONS_PATH = SV_Settings.PLUGIN_FOLDER + "Editor/Art/Icons/";

        public static Texture2D SettingsWindowIcon {
            get {
                if (EditorGUIUtility.isProSkin) {
                    return SA_EditorAssets.GetTextureAtPath(ICONS_PATH + "validation_icon_pro.png");
                } else {
                    return SA_EditorAssets.GetTextureAtPath(ICONS_PATH + "validation_icon.png");
                }
            }
        }


        private static GUIStyle s_componentIssuesHeader = null;
        public static GUIStyle ComponentIssuesHeader {
            get {
                if (s_componentIssuesHeader == null) {
                    s_componentIssuesHeader = new GUIStyle();
                    s_componentIssuesHeader.fontStyle = FontStyle.Bold;
                    s_componentIssuesHeader.normal.textColor = EditorStyles.label.normal.textColor;
                    s_componentIssuesHeader.normal.textColor = SA_PluginSettingsWindowStyles.DisabledImageColor;
                    s_componentIssuesHeader.fontSize = 11;
                }

                return s_componentIssuesHeader;
            }
        }

    }
}