using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace RF.AssetWizzard.Editor
{
    public static class Validation 
    {
        private static List<BuildTarget> s_allowedPlatfroms = new List<BuildTarget>();

        static Validation() {
            s_allowedPlatfroms.Add(BuildTarget.iOS);
            s_allowedPlatfroms.Add(BuildTarget.WebGL);
        }


        public static bool Run(EnvironmentAsset asset) {
            if (!IsValidAsset(asset)) { return false; }


            return true;
        }


        public static bool Run(PropAsset asset) {

            if(!IsValidAsset(asset)) { return false; }

            float max = Mathf.Max(asset.Size.x, asset.Size.y, asset.Size.z);

            if (max < AssetBundlesSettings.MIN_ALLOWED_SIZE) {
                EditorUtility.DisplayDialog("Error", "Your Asset is too small", "Ok");
                return false;
            }

            if (max > AssetBundlesSettings.MAX_AlLOWED_SIZE) {
                //EditorUtility.DisplayDialog ("Error", "Your Asset is too big", "Ok");
               // return false;
            }

            if (asset.Model.childCount < 1) {
                EditorUtility.DisplayDialog("Error", "Asset is empty!", "Ok");
                return false;
            }

            return true;
        }



        public static bool IsValidAsset(IAsset asset) {
            var icon = asset.GetIcon();
            string path = UnityEditor.AssetDatabase.GetAssetPath(icon);
            TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(path);
            if (ti != null) {
                if (!ti.isReadable) {
                    ti.isReadable = true;
                }

                TextureImporterPlatformSettings currentPlatfromSettings = ti.GetPlatformTextureSettings(EditorUserBuildSettings.activeBuildTarget.ToString());

                currentPlatfromSettings.textureCompression = TextureImporterCompression.Uncompressed;
                currentPlatfromSettings.maxTextureSize = 128;

                TextureImporterPlatformSettings defaultsettings = ti.GetDefaultPlatformTextureSettings();
                defaultsettings.textureCompression = TextureImporterCompression.Uncompressed;
                defaultsettings.maxTextureSize = 128;


                ti.SetPlatformTextureSettings(defaultsettings);
                ti.SetPlatformTextureSettings(currentPlatfromSettings);


                UnityEditor.AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }


            if (AssetBundlesSettings.Instance.TargetPlatforms.Count > 0) {
                foreach (BuildTarget platfrom in AssetBundlesSettings.Instance.TargetPlatforms) {
                    if (!s_allowedPlatfroms.Contains(platfrom)) {
                        EditorUtility.DisplayDialog("Error", platfrom.ToString() + " platfrom is not supported", "Ok");
                        WindowManager.Wizzard.SiwtchTab(WizardTabs.Platforms);
                        return false;
                    }
                }

            } else {
                EditorUtility.DisplayDialog("Error", "Please select at least one platfrom to upload", "Ok");
                WindowManager.Wizzard.SiwtchTab(WizardTabs.Platforms);
                return false;
            }



            return true;
        }
    }
}
