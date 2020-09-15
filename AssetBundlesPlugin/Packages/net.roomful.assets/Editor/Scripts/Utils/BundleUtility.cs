using UnityEngine;
using UnityEditor;
using System.IO;
using StansAssets.Foundation;

namespace net.roomful.assets.Editor
{
    internal static class BundleUtility
    {
        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public static void SaveTemplateToFile(string path, AssetTemplate tpl) {
            var content = Json.Serialize(tpl.ToDictionary());
            File.WriteAllText(path, content);
        }

        public static T LoadTemplateFromFile<T>(string path) where T : AssetTemplate {
            var content = File.ReadAllText(path);
            if (string.IsNullOrEmpty(content)) {
                return null;
            }

            var tpl = (T) System.Activator.CreateInstance(typeof(T), content);
            return tpl;
        }

        public static void DeleteTemplateFile(string path) {
            File.Delete(path);
        }

        public static bool FileExists(string path) {
            return File.Exists(path);
        }

        public static void GenerateUploadPrefab(IAsset asset) {
            var prefabName = asset.GetTemplate().Title;
            var prefabSource = asset.gameObject;
            Object.DestroyImmediate(asset.Component);

            CreatePrefab(prefabName, prefabSource);

            Object.DestroyImmediate(prefabSource);
        }

        public static void ClearLocalCache() {
            if (FolderUtils.IsFolderExists(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION)) {
                FolderUtils.DeleteFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
            }

            DeleteTempFiles();
        }

        public static void ClearLocalCacheForAsset(AssetTemplate tpl) {
            var path = AssetBundlesSettings.ASSETS_RESOURCES_LOCATION + "/" + tpl.Title;

            if (FolderUtils.IsFolderExists(path)) {
                FolderUtils.DeleteFolder(path);
            }
        }

        public static void DeleteTempFiles() {
            FolderUtils.DeleteFolder(AssetBundlesSettings.ASSETS_TEMP_LOCATION);
            EditorProgressBar.FinishUploadProgress();
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------

        private static void CreatePrefab(string name, GameObject source) {
            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_TEMP_LOCATION);
            PrefabUtility.SaveAsPrefabAsset(source, AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION + name + ".prefab");
        }
    }
}