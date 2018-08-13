using System;
using System.Collections.Generic;
using RF.AssetWizzard.Network.Request;
using UnityEditor;
using UnityEngine;

namespace RF.AssetWizzard.Editor {
    public class AssetsDownloader {
        private static string RELATIVE_ASSETS_RESOURCES_LOCATION = "Downloads";
        private static string FULL_RESOURCES_LOCATION = "Assets/" + RELATIVE_ASSETS_RESOURCES_LOCATION;
        
        private static Queue<PropTemplate> s_templates = new Queue<PropTemplate>();
        public static void DownloadAllProp() {
            AssetBundlesSettings.Instance.LocalPropTemplates.Clear(); 
            var allAssetsRequest = new RF.AssetWizzard.Network.Request.GetPropsList(0, 999, "");
            allAssetsRequest.PackageCallbackText = (allAssetsCallback) => {
                List<object> allAssetsList = SA.Common.Data.Json.Deserialize(allAssetsCallback) as List<object>;
                foreach (object assetData in allAssetsList) {
                    PropTemplate at = new PropTemplate(new JSONData(assetData).RawData);
                    s_templates.Enqueue(at);
                }
                FolderUtils.CreateFolder(RELATIVE_ASSETS_RESOURCES_LOCATION);
                DownloadNextProp();
            };

            allAssetsRequest.Send();
        }

        private static void DownloadNextProp() {
            if (s_templates.Count == 0) {
                UnityEditor.EditorUtility.DisplayDialog("Success", " All assets has been successfully downloaded!", "Ok");
                return;
            }

            PropTemplate template = s_templates.Dequeue();
            DownloadPropByTemplate(template);
        }

        private static void DownloadPropByTemplate(PropTemplate template) {
            string url = GetDownloadUrlForCurrentPlatform(template);
            if (string.IsNullOrEmpty(url)) {
                DownloadNextProp();
                return;
            }

            var propFoldername = string.Format("{0}_{1}", template.Title, template.Id);

            FolderUtils.CreateFolder(RELATIVE_ASSETS_RESOURCES_LOCATION + "/" + propFoldername);
            FolderUtils.CreateFolder(RELATIVE_ASSETS_RESOURCES_LOCATION + "/" + propFoldername+"/Resources") ;
            
            BundleUtility.SaveTemplateToFile(FULL_RESOURCES_LOCATION + "/" + propFoldername+"/"+ template.Title+".json", template);
            
            DownloadAsset loadAsset = new DownloadAsset(url);
            loadAsset.PackageCallbackData = (byte[] assetData) => {
  
                string bundlePath = FULL_RESOURCES_LOCATION + "/" + propFoldername+"/"+ template.Title+".unity3d" ;
                FolderUtils.WriteBytes(bundlePath, assetData);

                var bundle = AssetBundle.LoadFromFile(bundlePath);
                var bundleObject = bundle.LoadAsset<UnityEngine.Object>(template.Title);
                if (bundleObject == null) {
                    EditorUtility.DisplayDialog("Error", template.Title + "AssetBundle is corrupted", "Ok");
                    return;
                }

                GameObject gameObject = (GameObject)GameObject.Instantiate(bundleObject) as GameObject;
                gameObject.name = template.Title;
                PrefabUtility.CreatePrefab(FULL_RESOURCES_LOCATION + "/" + propFoldername +"/"+ template.Title+".prefab", gameObject);
                GameObject.DestroyImmediate(gameObject);
                DownloadNextProp();
            };

            loadAsset.Send();
        }

        private static string GetDownloadUrlForCurrentPlatform(PropTemplate template) {
            AssetUrl assetUrl = template.Urls.Find(url => url.Platform.Equals(UnityEditor.EditorUserBuildSettings.activeBuildTarget.ToString()));
            return assetUrl == null ? null : assetUrl.Url;
        }
    }
}