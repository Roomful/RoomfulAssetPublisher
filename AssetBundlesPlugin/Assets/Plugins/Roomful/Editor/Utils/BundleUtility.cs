﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using SA.Common.Data;

namespace RF.AssetWizzard.Editor
{

    public static class BundleUtility
    {

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public static void SaveTemplateToFile(string path, Template tpl) {
            string content = Json.Serialize(tpl.ToDictionary());
            File.WriteAllText(path, content);
        }

        public static T LoadTemplateFromFile<T>(string path) where T : Template {

            string content = File.ReadAllText(path);
            if(string.IsNullOrEmpty(content)) {
                return null;
            }

            T tpl = (T)System.Activator.CreateInstance(typeof(T), content);
            return tpl;
        }

        public static void DeleteTemplateFile(string path) {
            File.Delete(path);
        }

        public static bool FileExists(string path) {
            return File.Exists(path);
        }




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
        }

        public static void ClearLocalCacheForAsset(Template tpl) {
            string path = AssetBundlesSettings.ASSETS_RESOURCES_LOCATION + "/" + tpl.Title;

            if (FolderUtils.IsFolderExists(path)) {
                FolderUtils.DeleteFolder(path);
            }
        }



        public static void DeleteTempFiles() {
            FolderUtils.DeleteFolder(AssetBundlesSettings.ASSETS_TEMP_LOCATION);
        }



        //--------------------------------------
        // Private Methods
        //--------------------------------------

        private static void CreatePrefab(string name, GameObject source) {
            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_TEMP_LOCATION);
            PrefabUtility.CreatePrefab(AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION + name + ".prefab", source);
        }


        

    }
}