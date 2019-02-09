using RF.AssetWizzard.Network.Request;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RF.AssetWizzard.Editor {
    public abstract class BundleManager<T, A> : IBundleManager where T : Template where A : IAsset {
        public event Action OnUploaded = delegate { };

        //--------------------------------------
        // Abstract Methods
        //--------------------------------------

        public abstract void CreateAsset(T tpl);
        public abstract IAsset CreateDownloadedAsset(T tpl, GameObject gameObject);

        protected abstract BaseWebPackage GenerateMeta_Create_Request(A asset);
        protected abstract BaseWebPackage GenerateMeta_Update_Request(A asset);


        //--------------------------------------
        // Public Methods
        //--------------------------------------
               
        public void UpdateMeta(IAsset asset) {
            A bundleAsset = (A)asset;
            if (!IsAssetValid(bundleAsset)) { return; }
            BaseWebPackage metaRequest = GenerateMeta_Update_Request(bundleAsset);

            metaRequest.PackageCallbackText = (callback) => {
                asset.GetTemplate().Id = new Template(callback).Id;
                UploadThumbnail(bundleAsset, (template) => {
                    EditorProgressBar.FinishUploadProgress();
                });
            };
            metaRequest.Send();

        }

        private void UploadThumbnail(A asset, Action<Template> callback) {
            var template = asset.GetTemplate();
            EditorProgressBar.AddProgress(template.Title, "Requesting Thumbnail Upload Link", 0.1f);
            var getIconUploadLink = new RF.AssetWizzard.Network.Request.GetUploadLinkThumbnail(template.Id);
            getIconUploadLink.PackageCallbackText = (linkCallback) => {

                EditorProgressBar.AddProgress(template.Title, "Uploading Asset Thumbnail", 0.1f);
                var uploadRequest = new RF.AssetWizzard.Network.Request.UploadAssetThumbnail(linkCallback, asset.GetIcon());

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


        public void Download(Template tpl) {
            DownloadAsset((T)tpl);
        }

        public void Create(Template tpl) {
            if (string.IsNullOrEmpty(tpl.Title)) {
                EditorUtility.DisplayDialog("Error", "Name is empty!", "Ok");
                return;
            }

            EditorApplication.delayCall = () => {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
                //todo WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);

                CreateAsset((T)tpl);
            };
        }

        public void Upload(IAsset asset) {
            if (asset.GetTemplate().IsNew) {
                UploadNewAsset((A)asset);
            }
            else {
                UploadExistingAsset((A)asset);
            }
        }


        //--------------------------------------
        // Get / Set
        //--------------------------------------


        public System.Type TemplateType {
            get {
                return typeof(T);
            }
        }

        public System.Type AssetType {
            get {
                return typeof(A);
            }
        }


        public bool IsUploadInProgress {
            get {
                return BundleUtility.FileExists(PersistentTemplatePath);
            }
        }

        protected string PersistentTemplatePath {
            get {
                return AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION + typeof(T).Name + ".txt";
            }
        }


        //--------------------------------------
        // Virtual Methods
        //--------------------------------------

        protected virtual bool IsAssetValid(A asset) {
            return Validation.IsValidAsset(asset);
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------


        private void UploadNewAsset(A asset) {
            UploadAsset(GenerateMeta_Create_Request(asset), asset);
        }

        public void UploadExistingAsset(A asset) {
            UploadAsset(GenerateMeta_Update_Request(asset), asset);
        }



        private void UploadAsset(BaseWebPackage metaRequest, A asset) {
            if (!IsAssetValid(asset)) { return; }


            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_TEMP_LOCATION);
            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);


            asset.PrepareForUpload();
            BundleUtility.SaveTemplateToFile(PersistentTemplatePath, asset.GetTemplate());

            metaRequest.PackageCallbackText = (callback) => {
                asset.GetTemplate().Id = new Template(callback).Id;
                UploadAssetBundle(asset);
            };
            metaRequest.Send();
        }



        private void DownloadAsset(T tpl) {

            if (AssetBundlesSettings.Instance.AutomaticCacheClean) {
                BundleUtility.ClearLocalCache();
            }


            EditorApplication.delayCall = () => {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

                string pl = EditorUserBuildSettings.activeBuildTarget.ToString();
                var url = tpl.Urls.Find(u => u.Platform.Equals(pl));
                if (url == null) {
                    Debug.LogErrorFormat("Asset {0} (id = {1}) does not have version for {2} platform", tpl.Title, tpl.Id, pl);
                    return;
                }
                Debug.Log(string.Join("\n", tpl.Urls.Select(u => u.Platform + " " + u.Url)));



                var loadResource = new DownloadResource(url.Url);
                loadResource.PackageCallbackData = (byte[] assetData) => {

                    //todo WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);
                    if (!FolderUtils.IsFolderExists(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION)) {
                        FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
                    }

                    string bundlePath = AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION + "/" + tpl.Title + "_" + pl;
                    FolderUtils.WriteBytes(bundlePath, assetData);


                    BundleUtility.ClearLocalCacheForAsset(tpl);
                    AssetBundle.UnloadAllAssetBundles(true);
                    Resources.UnloadUnusedAssets();
                    Caching.ClearCache();

                    var bundle = AssetBundle.LoadFromFile(bundlePath);
                    var bundleObject = bundle.LoadAsset<UnityEngine.Object>(tpl.Title);

                    if (bundleObject == null) {
                        EditorUtility.DisplayDialog("Error", tpl.Title + "AssetBundle is corrupted", "Ok");
                        return;
                    }

                    GameObject gameObject = (GameObject)GameObject.Instantiate(bundleObject) as GameObject;
                    gameObject.name = tpl.Title;


                    IAsset asset = CreateDownloadedAsset(tpl, gameObject);
                    RunCollectors(asset, new AssetDatabase(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION));


                    UnityEditor.AssetDatabase.DeleteAsset(bundlePath);
                };

                loadResource.Send();


            };
        }


        public static void RunCollectors(IAsset asset, AssetDatabase assetDatabase) {
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

        private void UploadAssetBundle(A asset) {

            UploadThumbnail(asset, (Template template) => {
                AssetBundlesSettings.Instance.ReplaceSavedTemplate(template);
                BundleUtility.SaveTemplateToFile(PersistentTemplatePath, template);
                BundleUtility.GenerateUploadPrefab(asset);
                AssetsUploadLoop(0, template);
            });


        }


        private void AssetsUploadLoop(int platformIndex, Template tpl) {
            AssetBundlesSettings.Instance.UploadPlatfromIndex = platformIndex;

            if (platformIndex < AssetBundlesSettings.Instance.TargetPlatforms.Count) {
                BuildTarget pl = AssetBundlesSettings.Instance.TargetPlatforms[platformIndex];
                string prefabPath = AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION + tpl.Title + ".prefab";
                string assetBundleName = tpl.Title + "_" + pl;

                assetBundleName = assetBundleName.ToLower();

                AssetImporter assetImporter = AssetImporter.GetAtPath(prefabPath);
                assetImporter.assetBundleName = assetBundleName;
                BuildPipeline.BuildAssetBundles(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION, BuildAssetBundleOptions.UncompressedAssetBundle, pl);
                ResumeUpload();
            }
        }

        public void ResumeUpload() {

            Template tpl = BundleUtility.LoadTemplateFromFile<Template>(PersistentTemplatePath);
            if (tpl == null) { return; }


            int platformIndex = AssetBundlesSettings.Instance.UploadPlatfromIndex;
            BuildTarget platform = AssetBundlesSettings.Instance.TargetPlatforms[platformIndex];

            string assetBundleName = tpl.Title + "_" + platform;

            EditorProgressBar.AddProgress(tpl.Title, "Getting Asset Upload URL (" + platform + ")", 0.2f);
            var uploadLinkRequest = new GetUploadLink(tpl.Id, platform.ToString(), tpl.Title);
            uploadLinkRequest.PackageCallbackText = linkToUploadTo => {

                EditorProgressBar.AddProgress(tpl.Title, "Uploading Asset (" + platform + ")", 0.2f);
                byte[] assetBytes = System.IO.File.ReadAllBytes(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION + "/" + assetBundleName);
                Network.Request.UploadAsset uploadRequest = new UploadAsset(linkToUploadTo, assetBytes);

                float currentUploadProgress = EditorProgressBar.UploadProgress;
                uploadRequest.UploadProgress = (float progress) => {
                    float p = progress / 2f;
                    EditorProgressBar.UploadProgress = currentUploadProgress + p;
                    EditorProgressBar.AddProgress(tpl.Title, "Uploading Asset (" + platform + ")", 0f);
                };

                uploadRequest.PackageCallbackText = (uploadCallback) => {
                    EditorProgressBar.AddProgress(tpl.Title, "Waiting Asset Upload Confirmation (" + platform + ")", 0.2f / (float)AssetBundlesSettings.Instance.TargetPlatforms.Count);
                    Network.Request.UploadConfirmation confirm = new UploadConfirmation(tpl.Id, platform.ToString());
                    confirm.PackageCallbackText = (confirmCallback) => {
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
            T tpl = BundleUtility.LoadTemplateFromFile<T>(PersistentTemplatePath);


            BundleUtility.DeleteTempFiles();

            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.AssetDatabase.SaveAssets();

            EditorApplication.delayCall = () => {
                FolderUtils.DeleteFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION, false);
                FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
            };

            EditorProgressBar.FinishUploadProgress();

            OnUploaded();

            if (AssetBundlesSettings.Instance.DownloadAssetAfterUploading) {
                Download(tpl);
                EditorUtility.DisplayDialog("Success", tpl.Title + " Asset has been successfully uploaded!", "Ok");
            }
        }

    }
}