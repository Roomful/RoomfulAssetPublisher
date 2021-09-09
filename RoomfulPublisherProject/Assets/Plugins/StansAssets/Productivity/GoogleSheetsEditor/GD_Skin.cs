using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using StansAssets.Plugins.Editor;
using SA.Foundation.Editor;
using StansAssets.Foundation.Editor;

namespace SA.Productivity.GoogleSheets
{
    public static class GD_Skin
    {
        private const string ICONS_PATH = GD_Settings.PLUGIN_FOLDER_PATH + "Editor/Art/Icons/";

        public static Texture2D SettingsWindowIcon {
            get {
                if (EditorGUIUtility.isProSkin) {
                    return EditorAssetDatabase.GetTextureAtPath(ICONS_PATH + "gdc_pro.png");
                } else {
                    return EditorAssetDatabase.GetTextureAtPath(ICONS_PATH + "gdc.png");
                }
            }
        }


        static GUIStyle m_righltAllignedLabled = null;
        public static GUIStyle RighltAllignedLabled {
            get {
                if (m_righltAllignedLabled == null) {
                    m_righltAllignedLabled = new GUIStyle(EditorStyles.label);
                    m_righltAllignedLabled.alignment = TextAnchor.MiddleRight;
                }

                return m_righltAllignedLabled;
            }
        }

        static GUIStyle m_leftAllignedLabled = null;
        public static GUIStyle LeftAllignedLabled {
            get {
                if (m_leftAllignedLabled == null) {
                    m_leftAllignedLabled = new GUIStyle(EditorStyles.label);
                    m_leftAllignedLabled.alignment = TextAnchor.MiddleLeft;
                }

                return m_leftAllignedLabled;
            }
        }


        static GUIStyle m_boldLabled = null;
        public static GUIStyle BoldLabled {
            get {
                if (m_boldLabled == null) {
                    m_boldLabled = new GUIStyle(EditorStyles.label);
                    m_boldLabled.fontStyle = FontStyle.Bold;
                }

                return m_boldLabled;
            }
        }



     

        public static bool TrashButtonClick() {
            Texture2D image = PluginsEditorSkin.GetGenericIcon("close.png");
            using (new IMGUIChangeContentColor(Color.red)) {
                return GUILayout.Button(new GUIContent(string.Empty, image), GUILayout.Height(16), GUILayout.Width(24));
            }
        }

        public static bool RefreshButtonClick() {
            Texture2D image = PluginsEditorSkin.GetGenericIcon("refresh.png");
            using (new IMGUIChangeContentColor(SettingsWindowStyles.SelectedElementColor)) {
                return GUILayout.Button(new GUIContent(string.Empty, image), GUILayout.Height(16), GUILayout.Width(24));
            }
        }

        public static bool ViewButtonClick() {
            Texture2D image = PluginsEditorSkin.GetGenericIcon("view.png");
            using (new IMGUIChangeContentColor(SettingsWindowStyles.SelectedElementColor)) {
                return GUILayout.Button(new GUIContent(string.Empty, image), GUILayout.Height(16), GUILayout.Width(24));
            }
        }
    }
}
