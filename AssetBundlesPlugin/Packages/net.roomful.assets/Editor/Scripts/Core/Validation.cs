using System.Collections.Generic;
using UnityEditor;

namespace net.roomful.assets.Editor
{
    internal static class Validation
    {
        private static readonly List<BuildTarget> s_allowedTargets = new List<BuildTarget>();

        static Validation() {
            s_allowedTargets.Add(BuildTarget.iOS);
            s_allowedTargets.Add(BuildTarget.WebGL);
            s_allowedTargets.Add(BuildTarget.Android);
            s_allowedTargets.Add(BuildTarget.StandaloneWindows64);
        }

        public static bool Run(EnvironmentAsset asset) {
            if (!IsValidAsset(asset)) {
                return false;
            }

            return true;
        }

        public static bool Run(StyleAsset asset) {
            if (!IsValidAsset(asset)) {
                return false;
            }

            return true;
        }

        public static bool Run(PropAsset asset) {
            if (!IsValidAsset(asset)) {
                return false;
            }

            if (!asset.ValidSize) {
                EditorUtility.DisplayDialog("Error", "Your prop's default size doesn't follow our guidelines. We recommend you keep your prop between 50cm and 3m", "Ok");
                return false;
            }

            if (asset.IsEmpty) {
                EditorUtility.DisplayDialog("Error", "Asset is empty!", "Ok");
                return false;
            }

            if (!asset.HasCollision) {
                EditorUtility.DisplayDialog("Error", "Your asset has no colliders, consider to add one.", "Ok");
                return false;
            }

            return true;
        }

        public static bool IsValidAsset(IAsset asset) {
            var icon = asset.GetIcon();
            if (icon == null) {
                EditorUtility.DisplayDialog("Error", "Asset does not have Icon", "Ok");
                WindowManager.Wizzard.SwitchTab(WizardTabs.Wizzard);

                return false;
            }

            var path = UnityEditor.AssetDatabase.GetAssetPath(icon);
            var ti = (TextureImporter) AssetImporter.GetAtPath(path);
            if (ti != null) {
                if (!ti.isReadable) {
                    ti.isReadable = true;
                }

                var currentPlatformSettings = ti.GetPlatformTextureSettings(EditorUserBuildSettings.activeBuildTarget.ToString());

                currentPlatformSettings.textureCompression = TextureImporterCompression.Uncompressed;
                currentPlatformSettings.maxTextureSize = 128;

                var defaultSettings = ti.GetDefaultPlatformTextureSettings();
                defaultSettings.textureCompression = TextureImporterCompression.Uncompressed;
                defaultSettings.maxTextureSize = 128;

                ti.SetPlatformTextureSettings(defaultSettings);
                ti.SetPlatformTextureSettings(currentPlatformSettings);

                UnityEditor.AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }

            if (AssetBundlesSettings.Instance.TargetPlatforms.Count > 0) {
                foreach (var target in AssetBundlesSettings.Instance.TargetPlatforms) {
                    var buildTarget = target;
                    if (!s_allowedTargets.Contains(buildTarget)) {
                        EditorUtility.DisplayDialog("Error", buildTarget + " target is not supported", "Ok");
                        WindowManager.Wizzard.SwitchTab(WizardTabs.Platforms);
                        return false;
                    }
                }
            }
            else {
                EditorUtility.DisplayDialog("Error", "Please select at least one target to upload", "Ok");
                WindowManager.Wizzard.SwitchTab(WizardTabs.Platforms);
                return false;
            }

            return true;
        }
    }
}