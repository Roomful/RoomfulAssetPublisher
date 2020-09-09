using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using net.roomful.assets.Network.Request;
using System;
using Object = UnityEngine.Object;

namespace net.roomful.assets.Editor
{
    internal abstract class BundleManager<T, TAsset> : IBundleManager where T : Template where TAsset : IAsset
    {
        public event Action OnUploaded = delegate { };

        //--------------------------------------
        // Abstract Methods
        //--------------------------------------

        protected abstract void CreateAsset(T tpl);
        protected abstract IAsset CreateDownloadedAsset(T tpl, GameObject gameObject);

        protected abstract AssetMetadataRequest GenerateMeta_Create_Request(TAsset asset);
        protected abstract AssetMetadataRequest GenerateMeta_Update_Request(TAsset asset);

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public void UpdateMeta(IAsset asset) {
            var bundleAsset = (TAsset) asset;
            if (!IsAssetValid(bundleAsset)) {
                return;
            }

            var metaRequest = GenerateMeta_Update_Request(bundleAsset);

            metaRequest.PackageCallbackText = callback => {
                asset.GetTemplate().Id = new Template(callback).Id;
                UploadThumbnail(bundleAsset, template => {
                    EditorProgressBar.FinishUploadProgress();
                });
            };
            metaRequest.Send();
        }

        private void UploadThumbnail(TAsset asset, Action<Template> callback) {
            var template = asset.GetTemplate();
            EditorProgressBar.AddProgress(template.Title, "Requesting Thumbnail Upload Link", 0.1f);
            var getIconUploadLink = new GetUploadLink_Thumbnail(template.Id);
            getIconUploadLink.PackageCallbackText = linkCallback => {
                EditorProgressBar.AddProgress(template.Title, "Uploading Asset Thumbnail", 0.1f);
                var uploadRequest = new UploadAsset_Thumbnail(linkCallback, asset.GetIcon());

                var currentUploadProgress = EditorProgressBar.UploadProgress;
                uploadRequest.UploadProgress = progress => {
                    var p = progress / 2f;
                    EditorProgressBar.UploadProgress = currentUploadProgress + p;
                    EditorProgressBar.AddProgress(template.Title, "Uploading Asset Thumbnail", 0f);
                };

                uploadRequest.PackageCallbackText = uploadCallback => {
                    EditorProgressBar.AddProgress(template.Title, "Waiting Thumbnail Upload Confirmation", 0.3f);
                    var confirmRequest = new UploadConfirmation_Thumbnail(template.Id);
                    confirmRequest.PackageCallbackText = resData => {
                        template.Icon = new Resource(resData);

                        callback.Invoke(template);
                    };
                    confirmRequest.Send();
                };
                uploadRequest.Send();
            };
            getIconUploadLink.Send();
        }

        public void Download(Template tpl) {
            DownloadAsset((T) tpl);
        }

        public void Create(Template tpl) {
            if (string.IsNullOrEmpty(tpl.Title)) {
                EditorUtility.DisplayDialog("Error", "Name is empty!", "Ok");
                return;
            }

            EditorApplication.delayCall = () => {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
                WindowManager.Wizzard.SwitchTab(WizardTabs.Wizzard);

                CreateAsset((T) tpl);
            };
        }

        public void Upload(IAsset asset) {
            if (asset.GetTemplate().IsNew) {
                UploadNewAsset((TAsset) asset);
            }
            else {
                UploadExistingAsset((TAsset) asset);
            }
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public Type TemplateType => typeof(T);

        public Type AssetType => typeof(TAsset);

        public bool IsUploadInProgress => BundleUtility.FileExists(PersistentTemplatePath);

        private string PersistentTemplatePath => AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION + typeof(T).Name + ".txt";

        //--------------------------------------
        // Virtual Methods
        //--------------------------------------

        protected virtual bool IsAssetValid(TAsset asset) {
            return Validation.IsValidAsset(asset);
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------

        private void UploadNewAsset(TAsset asset) {
            UploadAsset(GenerateMeta_Create_Request(asset), asset);
        }

        private void UploadExistingAsset(TAsset asset) {
            UploadAsset(GenerateMeta_Update_Request(asset), asset);
        }

        private void UploadAsset(AssetMetadataRequest metaRequest, TAsset asset) {
            if (!IsAssetValid(asset)) {
                return;
            }

            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_TEMP_LOCATION);
            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);

            asset.PrepareForUpload();
            BundleUtility.SaveTemplateToFile(PersistentTemplatePath, asset.GetTemplate());

            metaRequest.PackageCallbackText = callback => {
                asset.GetTemplate().Id = new Template(callback).Id;
                UploadAssetBundle(asset);
            };
            metaRequest.Send();
        }

        private void DownloadAsset(T tpl) {
            if (AssetBundlesSettings.Instance.m_automaticCacheClean) {
                BundleUtility.ClearLocalCache();
            }

            EditorApplication.delayCall = () => {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

                var pl = EditorUserBuildSettings.activeBuildTarget.ToString();

                var getAssetUrl = new GetAssetUrl(tpl.Id, pl);
                getAssetUrl.PackageCallbackText = assetUrl => {
                    var loadAsset = new DownloadAsset(assetUrl);
                    loadAsset.PackageCallbackData = assetData => {
                        WindowManager.Wizzard.SwitchTab(WizardTabs.Wizzard);
                        if (!FolderUtils.IsFolderExists(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION)) {
                            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
                        }

                        var bundlePath = AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION + "/" + tpl.Title + "_" + pl;
                        FolderUtils.WriteBytes(bundlePath, assetData);

                        BundleUtility.ClearLocalCacheForAsset(tpl);
                        AssetBundle.UnloadAllAssetBundles(true);
                        Resources.UnloadUnusedAssets();
                        Caching.ClearCache();

                        var bundle = AssetBundle.LoadFromFile(bundlePath);
                        var bundleObject = bundle.LoadAsset<Object>(tpl.Title);

                        if (bundleObject == null) {
                            EditorUtility.DisplayDialog("Error", tpl.Title + "AssetBundle is corrupted", "Ok");
                            return;
                        }

                        var gameObject = (GameObject) Object.Instantiate(bundleObject);
                        gameObject.name = tpl.Title;

                        var asset = CreateDownloadedAsset(tpl, gameObject);
                        RunCollectors(asset, new AssetDatabase(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION));

                        UnityEditor.AssetDatabase.DeleteAsset(bundlePath);
                    };

                    loadAsset.Send();
                };

                getAssetUrl.Send();
            };
        }

        private static void RunCollectors(IAsset asset, AssetDatabase assetDatabase) {
            // Old renderer collector must be called ALWAYS earlier than Renderer collector!!!
            new V1_RendererCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new RendererCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new TextCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new MeshCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new ComponentsCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new AnimationCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new AnimatorCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new EnvironmentCollector().SetAssetDatabase(assetDatabase).Run(asset);

            new V1_ThumbnailsCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new V1_MarkersCollector().SetAssetDatabase(assetDatabase).Run(asset);
        }

        private void UploadAssetBundle(TAsset asset) {
            UploadThumbnail(asset, template => {
                AssetBundlesSettings.Instance.ReplaceSavedTemplate(template);
                BundleUtility.SaveTemplateToFile(PersistentTemplatePath, template);
                BundleUtility.GenerateUploadPrefab(asset);
                AssetsUploadLoop(0, template);
            });
        }

        private void AssetsUploadLoop(int platformIndex, Template tpl) {
            AssetBundlesSettings.Instance.UploadPlatformIndex = platformIndex;

            if (platformIndex < AssetBundlesSettings.Instance.TargetPlatforms.Count) {
                var buildTarget = AssetBundlesSettings.Instance.TargetPlatforms[platformIndex];
                var prefabPath = AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION + tpl.Title + ".prefab";
                var assetBundleName = tpl.Title + "_" + buildTarget;

                assetBundleName = assetBundleName.ToLower();

                var assetImporter = AssetImporter.GetAtPath(prefabPath);
                assetImporter.assetBundleName = assetBundleName;
                BuildPipeline.BuildAssetBundles(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION, BuildAssetBundleOptions.UncompressedAssetBundle, buildTarget);
                ResumeUpload();
            }
        }

        public void ResumeUpload() {
            var tpl = BundleUtility.LoadTemplateFromFile<Template>(PersistentTemplatePath);
            if (tpl == null) {
                return;
            }

            var platformIndex = AssetBundlesSettings.Instance.UploadPlatformIndex;
            var platform = AssetBundlesSettings.Instance.TargetPlatforms[platformIndex];

            var assetBundleName = tpl.Title + "_" + platform;

            EditorProgressBar.AddProgress(tpl.Title, "Getting Asset Upload URL (" + platform + ")", 0.2f);
            var uploadLinkRequest = new GetUploadLink(tpl.Id, platform.ToString(), tpl.Title);
            uploadLinkRequest.PackageCallbackText = linkToUploadTo => {
                EditorProgressBar.AddProgress(tpl.Title, "Uploading Asset (" + platform + ")", 0.2f);
                var assetBytes = System.IO.File.ReadAllBytes(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION + "/" + assetBundleName);
                var uploadRequest = new UploadAsset(linkToUploadTo, assetBytes);

                var currentUploadProgress = EditorProgressBar.UploadProgress;
                uploadRequest.UploadProgress = progress => {
                    var p = progress / 2f;
                    EditorProgressBar.UploadProgress = currentUploadProgress + p;
                    EditorProgressBar.AddProgress(tpl.Title, "Uploading Asset (" + platform + ")", 0f);
                };

                uploadRequest.PackageCallbackText = uploadCallback => {
                    EditorProgressBar.AddProgress(tpl.Title, "Waiting Asset Upload Confirmation (" + platform + ")", 0.2f / AssetBundlesSettings.Instance.TargetPlatforms.Count);
                    var confirm = new UploadConfirmation(tpl.Id, platform.ToString());
                    confirm.PackageCallbackText = confirmCallback => {
                        platformIndex++;
                        UnityEditor.AssetDatabase.RemoveUnusedAssetBundleNames();

                        if (platformIndex == AssetBundlesSettings.Instance.TargetPlatforms.Count) {
                            FinishAssetUpload();
                        }
                        else {
                            AssetsUploadLoop(platformIndex, tpl);
                        }
                    };
                    confirm.Send();
                };
                uploadRequest.Send();
            };
            uploadLinkRequest.Send();
        }

        private void FinishAssetUpload() {
            var tpl = BundleUtility.LoadTemplateFromFile<T>(PersistentTemplatePath);

            BundleUtility.DeleteTempFiles();

            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.AssetDatabase.SaveAssets();

            EditorApplication.delayCall = () => {
                FolderUtils.DeleteFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION, false);
                FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
            };

            EditorProgressBar.FinishUploadProgress();

            OnUploaded();

            if (AssetBundlesSettings.Instance.m_downloadAssetAfterUploading) {
                Download(tpl);
                EditorUtility.DisplayDialog("Success", tpl.Title + " Asset has been successfully uploaded!", "Ok");
            }
        }
    }
}