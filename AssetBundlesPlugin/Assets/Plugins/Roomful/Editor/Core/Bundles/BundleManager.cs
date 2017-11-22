﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using RF.AssetWizzard.Network.Request;

namespace RF.AssetWizzard.Editor
{
    public abstract class BundleManager<T, A> : IBundleManager  where T : Template where A : IAsset
    {

        //--------------------------------------
        // Abstract Methods
        //--------------------------------------

        public abstract void  CreateAsset(T tpl);
        public abstract IAsset CreateDownloadedAsset(T tpl, GameObject gameObject);

        protected abstract AssetMetadataRequest GenerateMeta_Create_Request(A asset);
        protected abstract AssetMetadataRequest GenerateMeta_Update_Request(A asset);


        //--------------------------------------
        // Public Methods
        //--------------------------------------

            
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
                WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);

                CreateAsset((T) tpl);
            };
        }

        public void Upload(IAsset asset) {
            if(asset.GetTemplate().IsNew) {
                UploadNewAsset((A)asset);
            } else {
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

        private void UploadExistingAsset(A asset) {
            UploadAsset(GenerateMeta_Update_Request(asset), asset);
        }



        private void UploadAsset(AssetMetadataRequest metaRequest, A asset) {
            if (!IsAssetValid(asset)) { return; }


            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_TEMP_LOCATION);
            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);


            BundleUtility.SaveTemplateToFile(PersistentTemplatePath, asset.GetTemplate());
            asset.PrepareForUpload();

            metaRequest.PackageCallbackText = (updateCalback) => {
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

                GetAssetUrl getAssetUrl = new RF.AssetWizzard.Network.Request.GetAssetUrl(tpl.Id, pl);
                getAssetUrl.PackageCallbackText = (assetUrl) => {

                    DownloadAsset loadAsset = new RF.AssetWizzard.Network.Request.DownloadAsset(assetUrl);
                    loadAsset.PackageCallbackData = (byte[] assetData) => {

                        WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);
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
                        var bundleObject = bundle.LoadAsset<Object>(tpl.Title);

                        if (bundleObject == null) {
                            EditorUtility.DisplayDialog("Error", tpl.Title + "AssetBundle is corrupted", "Ok");
                            return;
                        }

                        GameObject gameObject = (GameObject)GameObject.Instantiate(bundleObject) as GameObject;
                        gameObject.name = tpl.Title;

                        IAsset asset = CreateDownloadedAsset(tpl, gameObject);

                        RunCollectors(asset);
                        UnityEditor.AssetDatabase.DeleteAsset(bundlePath);
                    };

                    loadAsset.Send();
                };

                getAssetUrl.Send();
            };
        }


        private void RunCollectors(IAsset asset) {
            // Old renderer collector must be called ALWAYS earlier than Renderer collector!!!
            new V1_RendererCollector().Run(asset); 
            new RendererCollector().Run(asset);
            new TextCollector().Run(asset);
            new MeshCollector().Run(asset);
            new ComponentsCollector().Run(asset);
            new AnimationCollector().Run(asset);
            new AnimatorCollector().Run(asset);

            new V1_ThumbnailsCollector().Run(asset);
            new V1_MarkersCollector().Run(asset);
        }


        private void UploadAssetBundle(A asset) {
            EditorProgressBar.AddProgress("Requesting Thumbnail Upload Link", 0.1f);
            var getIconUploadLink = new RF.AssetWizzard.Network.Request.GetUploadLink_Thumbnail(asset.GetTemplate().Id);
            getIconUploadLink.PackageCallbackText = (linkCallback) => {

                EditorProgressBar.AddProgress("Uploading Asset Thumbnail", 0.1f);
                var uploadRequest = new RF.AssetWizzard.Network.Request.UploadAsset_Thumbnail(linkCallback, asset.GetIcon());

                float currentUploadProgress = EditorProgressBar.UploadProgress;
                uploadRequest.UploadProgress = (float progress) => {
                    float p = progress / 2f;
                    EditorProgressBar.UploadProgress = currentUploadProgress + p;
                    EditorProgressBar.AddProgress("Uploading Asset Thumbnail", 0f);
                };

                uploadRequest.PackageCallbackText = (string uploadCallback) => {

                    EditorProgressBar.AddProgress("Waiting Thumbnail Upload Confirmation", 0.3f);
                    var confirmRequest = new Network.Request.UploadConfirmation_Thumbnail(asset.GetTemplate().Id);
                    confirmRequest.PackageCallbackText = (string resData) => {

                        var resInfo = new JSONData(resData);
                        var res = new Resource(resInfo);
                        asset.GetTemplate().Icon = res;
                        AssetBundlesSettings.Instance.ReplaceSavedTemplate(asset.GetTemplate());
                        BundleUtility.SaveTemplateToFile(PersistentTemplatePath, asset.GetTemplate());


                        BundleUtility.GenerateUploadPrefab(asset);


                        AssetsUploadLoop(0, asset.GetTemplate());
                    };
                    confirmRequest.Send();
                };
                uploadRequest.Send();
            };
            getIconUploadLink.Send();
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
            }
        }

        public void ResumeUpload() {

            
            Template tpl = BundleUtility.LoadTemplateFromFile<Template>(PersistentTemplatePath);
            if (tpl == null) { return; }


            int platformIndex = AssetBundlesSettings.Instance.UploadPlatfromIndex;
            BuildTarget platform = AssetBundlesSettings.Instance.TargetPlatforms[platformIndex];

            string assetBundleName = tpl.Title + "_" + platform;

            EditorProgressBar.AddProgress("Getting Asset Upload URL (" + platform + ")", 0.2f);
            var uploadLinkRequest = new GetUploadLink(tpl.Id, platform.ToString(), tpl.Title);
            uploadLinkRequest.PackageCallbackText = (linkCallback) => {

                EditorProgressBar.AddProgress("Uploading Asset (" + platform + ")", 0.2f);
                byte[] assetBytes = System.IO.File.ReadAllBytes(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION + "/" + assetBundleName);
                Network.Request.UploadAsset uploadRequest = new UploadAsset(linkCallback, assetBytes);

                float currentUploadProgress = EditorProgressBar.UploadProgress;
                uploadRequest.UploadProgress = (float progress) => {
                    float p = progress / 2f;
                    EditorProgressBar.UploadProgress = currentUploadProgress + p;
                    EditorProgressBar.AddProgress("Uploading Asset (" + platform + ")", 0f);
                };

                uploadRequest.PackageCallbackText = (uploadCallback) => {
                    EditorProgressBar.AddProgress("Waiting Asset Upload Confirmation (" + platform + ")", 0.2f / (float)AssetBundlesSettings.Instance.TargetPlatforms.Count);
                    Network.Request.UploadConfirmation confirm = new UploadConfirmation(tpl.Id, platform.ToString());
                    confirm.PackageCallbackText = (confirmCallback) => {
                        platformIndex++;
                        UnityEditor.AssetDatabase.RemoveUnusedAssetBundleNames();

                        if (platformIndex == AssetBundlesSettings.Instance.TargetPlatforms.Count) {
                            FinishAssetUpload();
                        } else {
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

            Download(tpl);
            EditorProgressBar.FinishUploadProgress();
            EditorUtility.DisplayDialog("Success", " Asset has been successfully uploaded!", "Ok");
        }

    }
}