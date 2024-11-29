using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace net.roomful.assets.editor.svc
{
    static class ShaderVariantCollectionUtil
    {
        public const string ShaderVariantCollectionExtension = "shadervariants";
        public const string BaseShaderVariantCollectionDirectory = "Packages/net.roomful.assets";
        public const string BaseShaderVariantCollectionName = "BaseShaderVariantCollection";
        public const string NewShaderVariantCollectionName = "NewShaderVariantCollection";
        
        public static string BaseShaderVariantCollectionPath => $"{BaseShaderVariantCollectionDirectory}/{BaseShaderVariantCollectionName}.{ShaderVariantCollectionExtension}";

        public static string SaveCurrentCollection(string path = null)
        {
            if(string.IsNullOrEmpty(path))
                path = EditorUtility.SaveFilePanelInProject("Save current ShaderVariantCollection", 
                    "NewShaderVariantCollection", ShaderVariantCollectionExtension, "This file will be deleted after update.");
            
            typeof(ShaderUtil).GetMethod("SaveCurrentShaderVariantCollection", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { path });

            return path;
        }

        public static void ClearCurrentCollection()
        {
            typeof(ShaderUtil).GetMethod("ClearCurrentShaderVariantCollection", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[0]);
        }
        
        public static bool TryLoadCollectionWithFilePanel(string title, string directory, out (string path, ShaderVariantCollection collection) result)
        {
            var collectionPath = EditorUtility.OpenFilePanel(title, directory, ShaderVariantCollectionExtension);
            var collection = UnityEditor.AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(collectionPath);
            result = (collectionPath, collection);
            
            if (string.IsNullOrEmpty(collectionPath) || collection == null)
            {
                Debug.LogError($"Failed to open ShaderVariantCollection by path: {collectionPath}. Try again or report to tech support.");
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryLoadCollection(string path, out ShaderVariantCollection collection)
        {
            collection = UnityEditor.AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(path);
            
            if (string.IsNullOrEmpty(path) || collection == null)
            {
                Debug.LogError($"Failed to open ShaderVariantCollection by path: {path}. Try again or report to tech support.");
            }
            return collection != null;
        }

        public static bool TryLoadCollection(out ShaderVariantCollection collection)
        {
            var baseShaderVariantCollectionPath = BaseShaderVariantCollectionPath;
            collection = UnityEditor.AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(baseShaderVariantCollectionPath);
            if(collection == null)
                Debug.LogError($"Failed to load BaseShaderVariantCollection by path: {baseShaderVariantCollectionPath}");
            
            return collection != null;
        }
        
        public static bool TryLoadShaderCollection(out ShaderCollection collection)
        {
            var path = "Packages/net.roomful.assets/ShaderCollection.asset";
            collection = UnityEditor.AssetDatabase.LoadAssetAtPath<ShaderCollection>(path);
            if(collection == null)
                Debug.LogError($"Failed to load ShaderCollection by path: {path}");
            
            return collection != null;
        }
    }
}
