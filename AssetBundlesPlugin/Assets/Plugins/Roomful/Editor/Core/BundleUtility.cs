using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor
{

    public static class BundleUtility
    {

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public static void GenerateUploadPrefab(IAsset asset) {

            string prefabName = asset.GetTemplate().Title;
            GameObject prefabSource = asset.gameObject;
            Object.DestroyImmediate(asset.Component);

            CreatePrefab(prefabName, prefabSource);

            Object.DestroyImmediate(prefabSource);
        }

        public static void ClearLocalCache() {
            if (FolderUtils.IsFolderExists(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION)) {
                FolderUtils.DeleteFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
            }

            if (FolderUtils.IsFolderExists(AssetBundlesSettings.ASSETS_PREFABS_LOCATION)) {
                FolderUtils.DeleteFolder(AssetBundlesSettings.ASSETS_PREFABS_LOCATION);
            }
        }


        //--------------------------------------
        // Private Methods
        //--------------------------------------

        private static void CreatePrefab(string name, GameObject source) {
            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_PREFABS_LOCATION + "temp/");
            PrefabUtility.CreatePrefab(AssetBundlesSettings.FULL_ASSETS_PREFABS_LOCATION + "temp/" + name + ".prefab", source);
        }


        

    }
}