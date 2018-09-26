using System.Collections.Generic;
using RF.AssetWizzard.Network.Request;
using UnityEditor;
using UnityEngine;

namespace RF.AssetWizzard.Editor {
    public static class BatchDownloadService {
        private static string RELATIVE_ASSETS_RESOURCES_LOCATION = "Batch Downloader Cache";
        private static string FULL_RESOURCES_LOCATION = "Assets/" + RELATIVE_ASSETS_RESOURCES_LOCATION;

        private static Queue<PropTemplate> s_downloadQueue = new Queue<PropTemplate>();

        public static void DownloadAllAssets() {
            var allAssetsRequest = new GetAllProps();
            allAssetsRequest.DownloadAll((list) => {
                list.ForEach(s_downloadQueue.Enqueue);
                FolderUtils.CreateFolder(RELATIVE_ASSETS_RESOURCES_LOCATION);
                DownloadNextProp();
            });
        }
        
        private static void DownloadNextProp() {
            if (s_downloadQueue.Count == 0) {
                UnityEditor.EditorUtility.DisplayDialog("Success", " All assets has been successfully downloaded!", "Ok");
                return;
            }

            PropTemplate template = s_downloadQueue.Dequeue();
            DownloadPropByTemplate(template);
        }

        private static void DownloadPropByTemplate(PropTemplate template) {
            string url = GetDownloadUrlForCurrentPlatform(template);
            if (string.IsNullOrEmpty(url)) {
                DownloadNextProp();
                return;
            }

            if (FolderUtils.IsFolderExists(RELATIVE_ASSETS_RESOURCES_LOCATION + "/" + template.Title)) {
                DownloadNextProp();
                return;
            }
            FolderUtils.CreateFolder(RELATIVE_ASSETS_RESOURCES_LOCATION + "/" + template.Title);
            
            BundleUtility.SaveTemplateToFile(FULL_RESOURCES_LOCATION + "/" + template.Title+"/"+ template.Title+".json", template);
            
            DownloadAsset loadAsset = new DownloadAsset(url);
            loadAsset.PackageCallbackData = (byte[] assetData) => {
  
                string bundlePath = FULL_RESOURCES_LOCATION + "/" + template.Title+"/"+ template.Title+".unity3d" ;
                FolderUtils.WriteBytes(bundlePath, assetData);

                var bundle = AssetBundle.LoadFromFile(bundlePath);
                var bundleObject = bundle.LoadAsset<UnityEngine.Object>(template.Title);
                if (bundleObject == null) {
                    if (EditorUtility.DisplayDialog("Error", template.Title + "AssetBundle is corrupted", "Ok")) {
                        DownloadNextProp();
                    }
                    return;
                }

                GameObject gameObject = (GameObject)GameObject.Instantiate(bundleObject) as GameObject;
                gameObject.name = template.Title;
                var bundleManager = new PropBundleManager();
                var asset = bundleManager.CreateDownloadedAsset(template, gameObject);
                BundleManager<PropTemplate, PropAsset>.RunCollectors(asset, new AssetDatabase(RELATIVE_ASSETS_RESOURCES_LOCATION));

                PrefabUtility.CreatePrefab(FULL_RESOURCES_LOCATION + "/" + template.Title +"/"+ template.Title+".prefab", gameObject);
                GameObject.DestroyImmediate(gameObject);
                DownloadNextProp();
            };

            loadAsset.Send();
        }

        private static string GetDownloadUrlForCurrentPlatform(PropTemplate template) {
            AssetUrl assetUrl = template.Urls.Find(url => url.Platform.Equals(UnityEditor.EditorUserBuildSettings.activeBuildTarget.ToString()));
            return assetUrl == null ? null : assetUrl.Url;
        }

        public static bool IsDownloadingProps {
            get { return s_downloadQueue.Count > 0; }
        }
    }
}