using System;
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
            /*var allAssetsRequest = new GetAllProps();
            allAssetsRequest.DownloadAll((list) => {
                list.ForEach(s_downloadQueue.Enqueue);
                FolderUtils.CreateFolder(RELATIVE_ASSETS_RESOURCES_LOCATION);
                DownloadNextProp();
            });*/

            var allAssetsRequest = new GetAllPropsWithIds();
            var ids = new List<string>();
            ids.Add("b6psg4qrkskr1k");
            ids.Add("kc7863ghrhrdm2");
            ids.Add("bgtzwdg3wbrxkb");
            ids.Add("fk1nfx7qswp44q");
            ids.Add("8zcr7qg74xz7k1");
            ids.Add("57sr0fwd2cwf1w");
            ids.Add("w7rq9zgv28w7c3");
            ids.Add("xgb0n93v7zdzdw");
            ids.Add("vzx4smw6srgxz6");
            ids.Add("kdhcx8mfh2nd3t");
            ids.Add("gn9207kmpx4r2d");
            ids.Add("fw3k410799ps2w");
            ids.Add("484mq4r2rbbwc8");
            ids.Add("rw487sd17mzhfw");
            ids.Add("wp584s9gn16302");
            ids.Add("72n7m3vq4mdbvq");
            ids.Add("dd4nnxsv0g8169");
            ids.Add("h2xbbcmfstt0t2");
            ids.Add("pqqhw7wh97xg87");
            allAssetsRequest.DownloadAll(ids, (list) => {
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
            Debug.Log("Downloading " + template.Title);
            DownloadAsset loadAsset = new DownloadAsset(url);
            loadAsset.PackageCallbackData = (byte[] assetData) => {
                Debug.Log("Downloaded asset " + template.Title);

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
                try {
                    var asset = bundleManager.CreateDownloadedAsset(template, gameObject);
                    BundleManager<PropTemplate, PropAsset>.RunCollectors(asset, new AssetDatabase(RELATIVE_ASSETS_RESOURCES_LOCATION));
#if UNITY_2018_3_OR_NEWER
                    PrefabUtility.SaveAsPrefabAsset(gameObject, FULL_RESOURCES_LOCATION + "/" + template.Title + "/" + template.Title + ".prefab");
#else
                    PrefabUtility.CreatePrefab(FULL_RESOURCES_LOCATION + "/" + template.Title + "/" + template.Title + ".prefab", gameObject);

#endif
                }
                catch (Exception e) {
                    Debug.LogError("Failed to create " + template.Title + " error was " + e.Message);
                    FolderUtils.DeleteFolder(RELATIVE_ASSETS_RESOURCES_LOCATION + "/" + template.Title);
                }
                if (gameObject) {
                    GameObject.DestroyImmediate(gameObject);
                }
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