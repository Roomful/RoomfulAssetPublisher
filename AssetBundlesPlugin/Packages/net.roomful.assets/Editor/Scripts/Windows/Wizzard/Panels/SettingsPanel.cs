using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Rotorz.ReorderableList;
using StansAssets.Foundation;
using StansAssets.Plugins.Editor;

namespace net.roomful.assets.Editor
{
    public class SettingsPanel : Panel
    {
        public SettingsPanel(EditorWindow window) : base(window) { }

        public override void OnGUI() {
            EditorGUI.BeginChangeCheck();

            ReorderableListGUI.Title("Build Platforms");
            var platformsList = new List<string>();
            foreach (var buildTarget in AssetBundlesSettings.Instance.TargetPlatforms) {
                platformsList.Add(buildTarget.ToString());
            }

            ReorderableListGUI.ListField(platformsList, DrawPlatformListItem, DrawEmptyPlatform);

            AssetBundlesSettings.Instance.TargetPlatforms.Clear();

            foreach (var val in platformsList) {
                var parsed = EnumUtility.ParseEnum<BuildTarget>(val);
                AssetBundlesSettings.Instance.TargetPlatforms.Add(parsed);
            }

            ReorderableListGUI.Title("Plugin Settings");
            GUILayout.Space(10f);
            AssetBundlesSettings.Instance.m_showWebInLogs = IMGUILayout.ToggleFiled("WEB IN Logs", AssetBundlesSettings.Instance.m_showWebInLogs, IMGUIToggleStyle.ToggleType.YesNo);
            AssetBundlesSettings.Instance.m_showWebOutLogs = IMGUILayout.ToggleFiled("WEB OUT Logs", AssetBundlesSettings.Instance.m_showWebOutLogs, IMGUIToggleStyle.ToggleType.YesNo);
            AssetBundlesSettings.Instance.m_automaticCacheClean = IMGUILayout.ToggleFiled("Automatic Cache Clean", AssetBundlesSettings.Instance.m_automaticCacheClean, IMGUIToggleStyle.ToggleType.YesNo);
            AssetBundlesSettings.Instance.m_downloadAssetAfterUploading = IMGUILayout.ToggleFiled("Download And Show Asset After Uploading", AssetBundlesSettings.Instance.m_downloadAssetAfterUploading, IMGUIToggleStyle.ToggleType.YesNo);

            if (EditorGUI.EndChangeCheck()) {
                AssetBundlesSettings.Save();
            }

            GUILayout.Space(10f);
            ReorderableListGUI.Title("Actions");
            GUILayout.Space(10f);

            if (GUILayout.Button("Clear local cache")) {
                BundleUtility.ClearLocalCache();
            }
        }

        private string DrawPlatformListItem(Rect position, string itemValue) {
            position.y += 2;
            if (string.IsNullOrEmpty(itemValue)) {
                itemValue = BuildTarget.iOS.ToString();
            }

            position.width -= 25;

            var buildTarget = EnumUtility.ParseEnum<BuildTarget>(itemValue);
            buildTarget = (BuildTarget) EditorGUI.EnumPopup(position, buildTarget);

            position.x += position.width + 2;
            position.width = 20;
            position.height = 15;

            var buttonContent = new GUIContent();
            buttonContent.image = IconManager.GetIcon(Icon.refresh_black);

            if (EditorUserBuildSettings.activeBuildTarget == buildTarget) {
                GUI.enabled = false;
            }

            var switchPlatform = GUI.Button(position, buttonContent, EditorStyles.miniButton);
            if (switchPlatform) {
                var group = BuildTargetGroup.Unknown;
                switch (buildTarget) {
                    case BuildTarget.iOS:
                        group = BuildTargetGroup.iOS;
                        break;
                    case BuildTarget.WebGL:
                        group = BuildTargetGroup.WebGL;
                        break;
                }

                EditorUserBuildSettings.SwitchActiveBuildTargetAsync(group, buildTarget);
            }

            GUI.enabled = true;

            return buildTarget.ToString();
        }

        private void DrawEmptyPlatform() {
            GUILayout.Label("Please select at least one platform", EditorStyles.miniLabel);
        }
    }
}