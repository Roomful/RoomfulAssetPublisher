
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Editor {

    public class AutomaticReloader {

        public static void ReloadAllAssets() {
            AssetBundlesSettings.Instance.LocalPropTemplates.Clear();
            IsInReUploadGrogress = true;

            var allAssetsRequest = new RF.AssetWizzard.Network.Request.GetPropsList(0, 2, "ccc");
            allAssetsRequest.PackageCallbackText = (allAssetsCallback) => {
                List<object> allAssetsList = SA.Common.Data.Json.Deserialize(allAssetsCallback) as List<object>;

                foreach (object assetData in allAssetsList) {
                    PropTemplate at = new PropTemplate(new JSONData(assetData).RawData);
                    AssetBundlesSettings.Instance.LocalPropTemplates.Add(at);
                }

                AssetBundlesSettings.Save();

                ContinueReUploadLoop();
            };

            allAssetsRequest.Send();
        }

        public static void ContinueReUploadLoop() {

            if (AssetBundlesSettings.Instance.LocalPropTemplates.Count > 0) {

                bool isUrlValid = false;
#if UNITY_EDITOR

                string pl = UnityEditor.EditorUserBuildSettings.activeBuildTarget.ToString();
                var asset = Peek();

                Debug.Log("AutomaticReloader ReUploading: " + asset.Title + ". At index: " + Index + ". Assets left: " + (AssetBundlesSettings.Instance.LocalPropTemplates.Count - 1));
                Index = Index + 1;
                foreach (var u in asset.Urls) {
                    if (u.Platform.Equals(pl)) {
                        isUrlValid = !string.IsNullOrEmpty(u.Url);

                        break;
                    }
                }

#endif
                if (isUrlValid) {
                    BundleUtility.ClearLocalCache();

                    PropAsset.PropInstantieted += PropInstantiedtedHandler;
                    BundleService.Download(Dequeue());

                } else {
                    Debug.Log("Url is invalid, load next");
                    Dequeue();
                    ContinueReUploadLoop();
                }

            } else {
                IsInReUploadGrogress = false;
                UnityEditor.EditorUtility.DisplayDialog("Success", " All selected Assets has been successfully reUploaded!", "Ok");
            }
        }

        private static void PropInstantiedtedHandler() {
            PropAsset.PropInstantieted -= PropInstantiedtedHandler;

            UnityEditor.EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= OnUpdate;

            BundleService.Upload(CurrentProp);
#endif
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
#if UNITY_EDITOR
            if (IsInReUploadGrogress) {
                BundleService.OnBundleUploadedEvent += AssetBundleUploadedHandler;
            }
#endif
        }

        public static void AssetBundleUploadedHandler() {
            BundleService.OnBundleUploadedEvent -= AssetBundleUploadedHandler;
            if (IsInReUploadGrogress) {
                ContinueReUploadLoop();
            }
        }

        public static bool IsInReUploadGrogress {
            get {
                bool result = FolderUtils.IsFolderExists(AssetBundlesSettings.FULL_AUTOMATIC_REUPLOADER_TEMP_LOCATION);
                return result;
            }
            private set {
                if (value) {
                    FolderUtils.CreateFolder(AssetBundlesSettings.FULL_AUTOMATIC_REUPLOADER_TEMP_LOCATION);
                    FolderUtils.Write(AssetBundlesSettings.FULL_AUTOMATIC_REUPLOADER_TEMP_LOCATION + "settings.txt", "download=" + AssetBundlesSettings.Instance.DownloadAssetAfterUploading);

                    AssetBundlesSettings.Instance.DownloadAssetAfterUploading = false;
                    Index = 0;
                } else {
                    string settings = FolderUtils.Read(AssetBundlesSettings.FULL_AUTOMATIC_REUPLOADER_TEMP_LOCATION + "settings.txt");
                    if (settings.Length > 9) {
                        settings = settings.Substring(9);
                    }
                    if (!settings.Equals(string.Empty)) {
                        AssetBundlesSettings.Instance.DownloadAssetAfterUploading = System.Convert.ToBoolean(settings);
                    } else {
                        AssetBundlesSettings.Instance.DownloadAssetAfterUploading = true;
                    }
                    FolderUtils.DeleteFolder(AssetBundlesSettings.FULL_AUTOMATIC_REUPLOADER_TEMP_LOCATION);
                }
            }
        }

        private static int Index {
            get {
                
                string index = FolderUtils.Read(AssetBundlesSettings.FULL_AUTOMATIC_REUPLOADER_TEMP_LOCATION + "index.txt");
                index = index.Substring(6);
                return int.Parse(index);
            }
            set {
                FolderUtils.Write(AssetBundlesSettings.FULL_AUTOMATIC_REUPLOADER_TEMP_LOCATION + "index.txt", "index=" + value);
            }
        }

        private static PropAsset CurrentProp {
            get {
                return GameObject.FindObjectOfType<PropAsset>();
            }
        }

        private static PropTemplate Dequeue() {
            int indexOfLast = AssetBundlesSettings.Instance.LocalPropTemplates.Count - 1;
            PropTemplate curr = AssetBundlesSettings.Instance.LocalPropTemplates[indexOfLast];

            AssetBundlesSettings.Instance.LocalPropTemplates.Remove(curr);
            AssetBundlesSettings.Save();

            return curr;
        }


        private static PropTemplate Peek() {
            int indexOfLast = AssetBundlesSettings.Instance.LocalPropTemplates.Count - 1;

            return AssetBundlesSettings.Instance.LocalPropTemplates[indexOfLast];
        }
    }

}

