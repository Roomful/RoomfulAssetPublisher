using System.Collections.Generic;
using UnityEditor;

namespace net.roomful.assets.editor
{
    static class Validation
    {
        internal static readonly List<BuildTarget> AllowedTargets = new List<BuildTarget>();

        static Validation() {
            AllowedTargets.Add(BuildTarget.iOS);
            AllowedTargets.Add(BuildTarget.WebGL);
            AllowedTargets.Add(BuildTarget.Android);
            AllowedTargets.Add(BuildTarget.StandaloneWindows64);
            AllowedTargets.Add(BuildTarget.StandaloneOSX);
        }

        public static bool Run(EnvironmentAsset asset) {
            if (!IsValidAsset(asset, 128)) {
                return false;
            }

            return true;
        }

        public static bool Run(BaseStyleAsset asset) {
            if (!IsValidAsset(asset, 512)) {
                return false;
            }

            return true;
        }

        public static bool Run(PropAsset asset) {
            if (!IsValidAsset(asset, 128)) {
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

        public static bool IsValidAsset(IAsset asset, int maxTextureSize) {
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
                currentPlatformSettings.maxTextureSize = maxTextureSize;

                var defaultSettings = ti.GetDefaultPlatformTextureSettings();
                defaultSettings.textureCompression = TextureImporterCompression.Uncompressed;
                defaultSettings.maxTextureSize = maxTextureSize;

                ti.SetPlatformTextureSettings(defaultSettings);
                ti.SetPlatformTextureSettings(currentPlatformSettings);

                UnityEditor.AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }

            return true;
        }
    }
}