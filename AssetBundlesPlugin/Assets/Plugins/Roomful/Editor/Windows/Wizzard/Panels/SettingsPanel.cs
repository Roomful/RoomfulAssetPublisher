using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Rotorz.ReorderableList;

namespace RF.AssetWizzard.Editor
{

    public class SettingsPanel : Panel
    {
        public SettingsPanel(EditorWindow window) : base(window) {}

       
     

        public override void OnGUI() {

            EditorGUI.BeginChangeCheck();


            ReorderableListGUI.Title("Build Platfroms");
            List<string> PlatfromsList = new List<string>();
            foreach (var platform in AssetBundlesSettings.Instance.TargetPlatforms) {
                PlatfromsList.Add(platform.ToString());
            }

            ReorderableListGUI.ListField(PlatfromsList, DrawPlatformListItem, DrawEmptyPlatform);

            AssetBundlesSettings.Instance.TargetPlatforms = new List<BuildTarget>();

            foreach (string val in PlatfromsList) {
                BuildTarget parsed = SA.Common.Util.General.ParseEnum<BuildTarget>(val);
                AssetBundlesSettings.Instance.TargetPlatforms.Add(parsed);
            }

            ReorderableListGUI.Title("Plugin Settings");
            GUILayout.Space(10f);
            AssetBundlesSettings.Instance.ShowWebInLogs = SA.Common.Editor.Tools.YesNoFiled("WEB IN Logs", AssetBundlesSettings.Instance.ShowWebInLogs);
            AssetBundlesSettings.Instance.ShowWebOutLogs = SA.Common.Editor.Tools.YesNoFiled("WEB OUT Logs", AssetBundlesSettings.Instance.ShowWebOutLogs);
            AssetBundlesSettings.Instance.AutomaticCacheClean = SA.Common.Editor.Tools.YesNoFiled("Automatic Cache Clean", AssetBundlesSettings.Instance.AutomaticCacheClean);
            AssetBundlesSettings.Instance.DownloadAssetAfterUploading = SA.Common.Editor.Tools.YesNoFiled("Download And Show Asset After Uploading", AssetBundlesSettings.Instance.DownloadAssetAfterUploading);


            
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

            BuildTarget buildTraget = SA.Common.Util.General.ParseEnum<BuildTarget>(itemValue);
            buildTraget = (BuildTarget)EditorGUI.EnumPopup(position, buildTraget);


            position.x += position.width + 2;
            position.width = 20;
            position.height = 15;


            GUIContent buttonContent = new GUIContent();
            buttonContent.image = IconManager.GetIcon(Icon.refresh_black);


            if (EditorUserBuildSettings.activeBuildTarget == buildTraget) {
                GUI.enabled = false;
            }

            bool switchPlatfrom = GUI.Button(position, buttonContent, EditorStyles.miniButton);
            if (switchPlatfrom) {

                BuildTargetGroup group = BuildTargetGroup.Unknown;
                switch (buildTraget) {
                    case BuildTarget.iOS:
                        group = BuildTargetGroup.iOS;
                        break;
                    case BuildTarget.WebGL:
                        group = BuildTargetGroup.WebGL;
                        break;
                }

                EditorUserBuildSettings.SwitchActiveBuildTargetAsync(group, buildTraget);
            }

            GUI.enabled = true;

            return buildTraget.ToString();
        }

        private void DrawEmptyPlatform() {
            GUILayout.Label("Please select at least one platform", EditorStyles.miniLabel);
        }
    }
}