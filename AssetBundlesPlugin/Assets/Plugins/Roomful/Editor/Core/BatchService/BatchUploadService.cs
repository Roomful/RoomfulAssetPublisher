using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RF.AssetWizzard.Network.Request;
using SA.Common.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;
using Application = UnityEngine.Application;

namespace RF.AssetWizzard.Editor {
    public static class BatchUploadService {
        private static string RELATIVE_ASSETS_RESOURCES_LOCATION = "Batch Downloader Cache";
        private static string FULL_RESOURCES_LOCATION = "Assets/" + RELATIVE_ASSETS_RESOURCES_LOCATION;

        public static void UploadAllAssets() {
            MarkAssetsToUpload();
            UploadProps();
        }

        private static void MarkAssetsToUpload() {
            var exportTargets = AssetBundlesSettings.Instance.TargetPlatforms;
            if (FolderUtils.IsFolderExists(RELATIVE_ASSETS_RESOURCES_LOCATION)) {
                var subfolders = FolderUtils.GetSubfolders(RELATIVE_ASSETS_RESOURCES_LOCATION);
                subfolders.ForEach(f => {
                    var folderName = f.Split(Path.DirectorySeparatorChar).Last();
                    if (File.Exists(f + "/" + folderName + ".prefab") && File.Exists(f + "/" + folderName + ".json")) {
                        exportTargets.ForEach(target => {
                            TextWriter tw = new StreamWriter(f + "/" + target + ".target", false);
                            tw.Write(string.Empty);
                            tw.Close();
                        });
                    }
                });
            }
        }

        private static void UploadProps() {
            var folders = GetFoldersToUploadFrom();
            PrepareAssetsForUpload(folders);
            CreateAssetBundles();
            RestoreUploadQueue(BatchUploadServiceConfigManager.GetConfig().uploadQueue);
            SendNextAsset();    
        }

        private static void PrepareAssetsForUpload(List<string> sourceFolders) {
            sourceFolders.ForEach(folder => { PrepareAssetToUpload(folder); });
        }

        private static void PrepareAssetToUpload(string folderPath) {
            var folderName = folderPath.Split(Path.DirectorySeparatorChar).Last();
            var prefabPath = string.Format("{0}/{1}.prefab", folderPath, folderName);
            var relativePrefabPath = "Assets" + prefabPath.Replace(Application.dataPath, "");
            var templatePath = string.Format("{0}/{1}.json", folderPath, folderName);
            
            var template = new PropTemplate(File.ReadAllText(templatePath));
            var gameObject = GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(relativePrefabPath));
            gameObject.name = template.Title;
            var bundleManager = new PropBundleManager();
            var asset = bundleManager.CreateDownloadedAsset(template, gameObject);
            asset.PrepareForUpload();
            File.WriteAllText(templatePath, Json.Serialize(asset.GetTemplate().ToDictionary()));
            GameObject.DestroyImmediate(asset.Component);
            AssetBundlesSettings.Instance.TargetPlatforms.ForEach(platform => {
                var prefabFileName = prefabPath.Split('/').Last();
                var bundleFolderName = relativePrefabPath.Replace(prefabFileName, platform.ToString());
                FolderUtils.CreateFolder(bundleFolderName.Replace("Assets/", string.Empty));
                string platformPrefabPath = bundleFolderName + "/" + prefabFileName;
                PrefabUtility.CreatePrefab(platformPrefabPath, gameObject);
                string assetBundleName = template.Title + "_" + platform;
                assetBundleName = assetBundleName.ToLower();
                AssetImporter assetImporter = AssetImporter.GetAtPath(platformPrefabPath);
                assetImporter.assetBundleName = assetBundleName;
                BatchUploadServiceConfigManager.GetConfig().uploadQueue.Add(new BatchUploadServiceConfig.UploadItem(template.Title, templatePath, platform.ToString()));
            });
            BatchUploadServiceConfigManager.Save();
            GameObject.DestroyImmediate(gameObject);
        }

        private static void CreateAssetBundles() {
            var platforms = AssetBundlesSettings.Instance.TargetPlatforms.GetRange(0, AssetBundlesSettings.Instance.TargetPlatforms.Count);
            if (platforms.Contains(EditorUserBuildSettings.activeBuildTarget)) {
                platforms.Remove(EditorUserBuildSettings.activeBuildTarget);
                platforms.Insert(0, EditorUserBuildSettings.activeBuildTarget);
            }
            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
            AssetBundlesSettings.Instance.TargetPlatforms.ForEach(platform => { BuildPipeline.BuildAssetBundles(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION, BuildAssetBundleOptions.UncompressedAssetBundle, platform); });
            BatchUploadServiceConfigManager.GetConfig().state = BatchUploadServiceConfig.State.PREPARED_BUNDLES;
            BatchUploadServiceConfigManager.Save();
        }

        private static List<UploadSingleAsset> s_uploadAssetQueue = new List<UploadSingleAsset>();

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
#if UNITY_EDITOR
            var config = BatchUploadServiceConfigManager.GetConfig();
            switch (config.state) {
                case BatchUploadServiceConfig.State.IDLE:
                    break;
                case BatchUploadServiceConfig.State.PREPARED_BUNDLES:
                case BatchUploadServiceConfig.State.UPLOADING:
                    RestoreUploadQueue(config.uploadQueue);
                    SendNextAsset();
                    break;
            }
#endif
        }

        private static void RestoreUploadQueue(List<BatchUploadServiceConfig.UploadItem> configUploadQueue) {
            s_uploadAssetQueue.Clear();
            configUploadQueue.ForEach(queueItem => {
                var template = new PropTemplate(File.ReadAllText(queueItem.TemplatePath));
                string assetBundleName = template.Title + "_" + queueItem.Platform;
                byte[] assetBytes = File.ReadAllBytes(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION + "/" + assetBundleName);
                var platform = (BuildTarget) Enum.Parse(typeof(BuildTarget), queueItem.Platform);
                s_uploadAssetQueue.Add(new UploadSingleAsset(platform, template, assetBytes));
                
            });
        }

        private static void SendNextAsset() {
            if (s_uploadAssetQueue.Count > 0) {
                s_uploadAssetQueue[0].Upload((template, platform) => {
                    var item = BatchUploadServiceConfigManager.GetConfig().uploadQueue.Find(listItem => listItem.TemplateTitle.Equals(template.Title) && listItem.Platform.Equals(platform.ToString()));
                    if (item != null) {
                        BatchUploadServiceConfigManager.GetConfig().uploadQueue.Remove(item);
                        BatchUploadServiceConfigManager.Save();
                    }
                    s_uploadAssetQueue.RemoveAt(0);
                    SendNextAsset();
                });
            } else {
                AssetsSent();
            }
        }

        private static void AssetsSent() {
            BatchUploadServiceConfigManager.GetConfig().state = BatchUploadServiceConfig.State.IDLE;
            BatchUploadServiceConfigManager.Save();
            UnityEditor.AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        private static Queue<AssetData> s_uploadQueue = new Queue<AssetData>();

        private static List<string> GetFoldersToUploadFrom() {
            var result = new List<string>();
            var target = EditorUserBuildSettings.activeBuildTarget;
            if (FolderUtils.IsFolderExists(RELATIVE_ASSETS_RESOURCES_LOCATION)) {
                var subfolders = FolderUtils.GetSubfolders(RELATIVE_ASSETS_RESOURCES_LOCATION);
//                subfolders.ForEach(f => {
                var f = subfolders[0];
                var folderName = f.Split(Path.DirectorySeparatorChar).Last();
                var prefabPath = string.Format("{0}/{1}.prefab", f, folderName);
                var templatePath = string.Format("{0}/{1}.json", f, folderName);
                if (File.Exists(prefabPath) && File.Exists(templatePath)) {
                    result.Add(f);
                }

//                });
            }

            return result;
        }


        public static void ContinueUpload() { }

        private class AssetData {
            public string TemplateTitle;
            public string PrefabPath;
            public string TemplateJSON;
            public Texture2D Icon;
        }


        private class UploadSingleAsset {
            private Template m_template;
            private BuildTarget m_platform;
            private byte[] m_assetBytes;

            public UploadSingleAsset(BuildTarget platform, Template template, byte[] assetBytes) {
                m_template = template;
                m_platform = platform;
                m_assetBytes = assetBytes;
            }

            public void Upload(Action<Template, BuildTarget> callback) {
                Debug.Log("Upload 1");
                EditorProgressBar.AddProgress(m_template.Title, "Getting Asset Upload URL (" + m_platform + ")", 0.25f);
                var uploadLinkRequest = new GetUploadLink(m_template.Id, m_platform.ToString(), m_template.Title);
                uploadLinkRequest.PackageCallbackText = linkToUploadTo => {
                    Debug.Log("Upload 2");

                    EditorProgressBar.AddProgress(m_template.Title, "Uploading Asset (" + m_platform + ")", 0.25f);

                    Network.Request.UploadAsset uploadRequest = new UploadAsset(linkToUploadTo, m_assetBytes);

                    float currentUploadProgress = EditorProgressBar.UploadProgress;
                    uploadRequest.UploadProgress = (float progress) => {
                        float p = progress / 2f;
                        EditorProgressBar.UploadProgress = currentUploadProgress + p;
                        EditorProgressBar.AddProgress(m_template.Title, "Uploading Asset (" + m_platform + ")", 0f);
                    };

                    uploadRequest.PackageCallbackText = (uploadCallback) => {
                        Debug.Log("Upload 3");
                        EditorProgressBar.AddProgress(m_template.Title, "Waiting Asset Upload Confirmation (" + m_platform + ")", 0.25f);
                        Network.Request.UploadConfirmation confirm = new UploadConfirmation(m_template.Id, m_platform.ToString());
                        confirm.PackageCallbackText = (confirmCallback) => {
                            Debug.Log("Upload 4");
                            callback.Invoke(m_template, m_platform);
                            EditorProgressBar.FinishUploadProgress();
                        };
                        confirm.Send();
                    };
                    uploadRequest.Send();
                };
                uploadLinkRequest.Send();
            }
        }

        private class UploadSingleProp {
            public void UploadAssetThumbnail(Template template, Texture2D icon, Action<Template> callback) {
                EditorProgressBar.AddProgress(template.Title, "Requesting Thumbnail Upload Link", 0.1f);
                var getIconUploadLink = new RF.AssetWizzard.Network.Request.GetUploadLink_Thumbnail(template.Id);
                getIconUploadLink.PackageCallbackText = (linkCallback) => {
                    EditorProgressBar.AddProgress(template.Title, "Uploading Asset Thumbnail", 0.1f);
                    var uploadRequest = new RF.AssetWizzard.Network.Request.UploadAsset_Thumbnail(linkCallback, icon);

                    float currentUploadProgress = EditorProgressBar.UploadProgress;
                    uploadRequest.UploadProgress = (float progress) => {
                        float p = progress / 2f;
                        EditorProgressBar.UploadProgress = currentUploadProgress + p;
                        EditorProgressBar.AddProgress(template.Title, "Uploading Asset Thumbnail", 0f);
                    };

                    uploadRequest.PackageCallbackText = (string uploadCallback) => {
                        EditorProgressBar.AddProgress(template.Title, "Waiting Thumbnail Upload Confirmation", 0.3f);
                        var confirmRequest = new Network.Request.UploadConfirmation_Thumbnail(template.Id);
                        confirmRequest.PackageCallbackText = (string resData) => {
                            template.Icon = new Resource(resData);
                            callback.Invoke(template);
                        };
                        confirmRequest.Send();
                    };
                    uploadRequest.Send();
                };
                getIconUploadLink.Send();
            }
        }
    }
}