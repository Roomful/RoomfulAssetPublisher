using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RF.AssetWizzard.Network.Request;
using SA.Common.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

namespace RF.AssetWizzard.Editor {
    public static class BatchUploadService {
        private static string RELATIVE_ASSETS_RESOURCES_LOCATION = "Batch Downloader Cache";
        private static string FULL_RESOURCES_LOCATION = "Assets/" + RELATIVE_ASSETS_RESOURCES_LOCATION;

        private static Queue<PropTemplate> s_downloadQueue = new Queue<PropTemplate>();

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
            GetUploadQueueForCurrentPlatform();
            UploadAssetsForCurrentPlatform(() => {
                if (AllAssetsUploaded()) {
                    Finish();
                } else {
                    SwitchToNextPlatform();
                    UploadProps();
                }
            });
        }

        private static void SwitchToNextPlatform() {
            throw new NotImplementedException();
        }

        private static void Finish() {
            throw new NotImplementedException();
        }

        private static bool AllAssetsUploaded() {
            throw new NotImplementedException();
        }

        private static void UploadAssetsForCurrentPlatform(Action action) {
            var list = s_uploadQueue.ToList();
            list.ForEach(queueItem => {
                var template = new PropTemplate(queueItem.TemplateJSON);
                var gameObject = GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(queueItem.PrefabPath));
                gameObject.name = template.Title;
                var bundleManager = new PropBundleManager();
                var asset = bundleManager.CreateDownloadedAsset(template, gameObject);
                asset.PrepareForUpload();
                if (asset.GetIcon() != null) {
                    queueItem.Icon = new Texture2D(1,1);
                    queueItem.Icon.LoadRawTextureData(asset.GetIcon().GetRawTextureData());
                }
            /*    new UploadSingleProp().UploadAssetThumbnail(asset.GetTemplate(), asset.GetIcon(), (uploadedTemplate) => { 
                }); */
                queueItem.TemplateJSON = Json.Serialize(asset.GetTemplate().ToDictionary());
                GameObject.DestroyImmediate(asset.Component);
                AssetBundlesSettings.Instance.TargetPlatforms.ForEach(platform => {
                    var prefabFileName = queueItem.PrefabPath.Split('/').Last();
                    var folderName = queueItem.PrefabPath.Replace(prefabFileName, platform.ToString());
                    FolderUtils.CreateFolder(folderName.Replace("Assets/", string.Empty));
                    string prefabPath = folderName + "/" + prefabFileName;
                    PrefabUtility.CreatePrefab(prefabPath, gameObject);
                    string assetBundleName = template.Title + "_" + platform;
                    assetBundleName = assetBundleName.ToLower();
                    AssetImporter assetImporter = AssetImporter.GetAtPath(prefabPath);
                    assetImporter.assetBundleName = assetBundleName;
                });
                GameObject.DestroyImmediate(gameObject);
            });
            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
            AssetBundlesSettings.Instance.TargetPlatforms.ForEach(platform => {
                BuildPipeline.BuildAssetBundles(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION, BuildAssetBundleOptions.UncompressedAssetBundle, platform);
            });
            list.ForEach(queueItem => {
                Template template = new Template(queueItem.TemplateJSON);
                AssetBundlesSettings.Instance.TargetPlatforms.ForEach(platform => {
                    string assetBundleName = template.Title + "_" + platform;
                    byte[] assetBytes = File.ReadAllBytes(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION + "/" + assetBundleName);
                    s_uploadAssetQueue.Enqueue(new UploadSingleAsset(platform, template, assetBytes));
                });
            });
            SendNextAsset();
        }

        private static Queue<UploadSingleAsset> s_uploadAssetQueue = new Queue<UploadSingleAsset>();

        private static void SendNextAsset() {
            if (s_uploadAssetQueue.Count > 0) {
                s_uploadAssetQueue.Peek().Upload(() => {
                    s_uploadAssetQueue.Dequeue();
                    SendNextAsset();
                });
            } else {
                AssetsSent();
            }
        }
        
        private static void AssetsSent() {
            UnityEditor.AssetDatabase.RemoveUnusedAssetBundleNames();
        }
  
        private static Queue<AssetData> s_uploadQueue = new Queue<AssetData>(); 
        private static void GetUploadQueueForCurrentPlatform() {
            var target = EditorUserBuildSettings.activeBuildTarget;
            if (FolderUtils.IsFolderExists(RELATIVE_ASSETS_RESOURCES_LOCATION)) {
                var subfolders = FolderUtils.GetSubfolders(RELATIVE_ASSETS_RESOURCES_LOCATION);
//                subfolders.ForEach(f => {
                var f = subfolders[0];
                    var folderName = f.Split(Path.DirectorySeparatorChar).Last();
                    var prefabPath = string.Format("{0}/{1}.prefab", f, folderName);
                    var templatePath = string.Format("{0}/{1}.json", f, folderName);
                    var markerPath = string.Format("{0}/{1}.target", f, target);
                    if (File.Exists(prefabPath) && File.Exists(templatePath) && File.Exists(markerPath) ) {
                        var assetData = new AssetData();
                        assetData.PrefabPath = string.Format("{0}/{1}/{1}.prefab", FULL_RESOURCES_LOCATION , folderName);
                        assetData.TemplateJSON = File.ReadAllText(templatePath);
                        s_uploadQueue.Enqueue(assetData);
                    }
//                });
            } 
        }


        public static void ContinueUpload() { 
        }

        private class AssetData {
            public string PrefabPath;
            public string TemplateJSON;
            public Texture2D Icon;
            public bool ThumbnailUploaded;
        }

        
        private class UploadSingleAsset {
            private Template m_template;
            private BuildTarget m_platform; 
            private byte[] m_assetBytes; 

            public UploadSingleAsset( BuildTarget platform, Template template, byte[] assetBytes) {
                m_template = template;
                m_platform = platform;
                m_assetBytes = assetBytes;
            } 
            public void Upload(Action callback) {

            Debug.Log("Upload 1");
            EditorProgressBar.AddProgress(m_template.Title,"Getting Asset Upload URL (" + m_platform + ")", 0.2f);
            var uploadLinkRequest = new GetUploadLink(m_template.Id, m_platform.ToString(), m_template.Title);
            uploadLinkRequest.PackageCallbackText = linkToUploadTo => {
                Debug.Log("Upload 2");

                EditorProgressBar.AddProgress(m_template.Title, "Uploading Asset (" + m_platform + ")", 0.2f);
                
                Network.Request.UploadAsset uploadRequest = new UploadAsset(linkToUploadTo, m_assetBytes);

                float currentUploadProgress = EditorProgressBar.UploadProgress;
                uploadRequest.UploadProgress = (float progress) => {
                    float p = progress / 2f;
                    EditorProgressBar.UploadProgress = currentUploadProgress + p;
                    EditorProgressBar.AddProgress(m_template.Title, "Uploading Asset (" + m_platform + ")", 0f);
                };

                uploadRequest.PackageCallbackText = (uploadCallback) => {
                    Debug.Log("Upload 3");
                    EditorProgressBar.AddProgress(m_template.Title, "Waiting Asset Upload Confirmation (" + m_platform + ")", 0.2f / (float)AssetBundlesSettings.Instance.TargetPlatforms.Count);
                    Network.Request.UploadConfirmation confirm = new UploadConfirmation(m_template.Id, m_platform.ToString());
                    confirm.PackageCallbackText = (confirmCallback) => { 
                        Debug.Log("Upload 4");
                        callback.Invoke();
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