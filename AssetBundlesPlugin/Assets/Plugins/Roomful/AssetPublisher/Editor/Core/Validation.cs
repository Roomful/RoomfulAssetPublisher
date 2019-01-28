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
            s_allowedPlatfroms.Add(BuildTarget.Android);
            s_allowedPlatfroms.Add(BuildTarget.StandaloneWindows64);
        }


        public static bool Run(EnvironmentAsset asset) {
            if (!IsValidAsset(asset)) { return false; }
            return true;
        }


        public static bool Run(StyleAsset asset) {
            if (!IsValidAsset(asset)) { return false; }
            return true;
        }


        public static bool Run(PropAsset asset) {

            if(!IsValidAsset(asset)) { return false; }


            if(!asset.ValidSize) {
                EditorUtility.DisplayDialog("Error", "Your prop's default size doesn't follow our guidelines. We recommend you keep your prop between 50cm and 3m", "Ok");
                return false;
            }

            if (asset.IsEmpty) {
                EditorUtility.DisplayDialog("Error", "Asset is empty!", "Ok");
                return false;
            }

            if (!asset.HasCollisison) {
                EditorUtility.DisplayDialog("Error", "Your asset has no colliders, consider to add one.", "Ok");
                return false;
            }

            if (asset.GetLayer(HierarchyLayers.Silhouette).transform.childCount == 0) {
                EditorUtility.DisplayDialog("Error", "Silhouette is empty! Please add some graphics!", "Ok");
                return false;
            }

            return true;
        }



        public static bool IsValidAsset(IAsset asset) {
            var icon = asset.GetIcon();
            if (icon == null) {
                EditorUtility.DisplayDialog("Error", "Asset does not have Icon", "Ok");
                //todo WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);

                return false;
            }
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
                        //todo WindowManager.Wizzard.SiwtchTab(WizardTabs.Platforms);
                        return false;
                    }
                }

            } else {
                EditorUtility.DisplayDialog("Error", "Please select at least one platfrom to upload", "Ok");
                //todo WindowManager.Wizzard.SiwtchTab(WizardTabs.Platforms);
                return false;
            }



            return true;
        }
    }
}
