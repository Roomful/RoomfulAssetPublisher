using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using net.roomful.api;
using Object = UnityEngine.Object;

namespace net.roomful.assets.editor
{ 
    abstract class BundleManager<T, TAsset> : IBundleManager where T : AssetTemplate where TAsset : IAsset
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
            if (!IsAssetMetadataValidate(bundleAsset)) {
                return;
            }

            var metaRequest = GenerateMeta_Update_Request(bundleAsset);

            metaRequest.PackageCallbackText = callback => {
                var assetTemplate = asset.GetTemplate();
                assetTemplate.Id = new AssetTemplate(callback).Id;

                var thumbnailUploader = new AssetThumbnailUploader(assetTemplate);
                thumbnailUploader.Upload(asset.GetIcon(), iconRes => {
                    assetTemplate.Icon = iconRes;
                    
                    EditorUtility.DisplayDialog("Updated", $"{asset.Title} metadata updated", "OK");
                });
            };
            metaRequest.Send();
        }

        public void Download(AssetTemplate tpl) {
            DownloadAsset((T) tpl);
        }

        public void Create(AssetTemplate tpl) {
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

        public virtual void Upload(IAsset asset) {
            if (asset.GetTemplate().IsNew) {
                UploadNewAsset((TAsset) asset);
            }
            else {
                UploadExistingAsset((TAsset) asset);
            }
        }
        
        public void ReUpload(IAsset asset)
        {
            PrepareForUpload((TAsset) asset);
            UploadAssetBundle((TAsset) asset);
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public Type TemplateType => typeof(T);

        public Type AssetType => typeof(TAsset);

        public bool IsUploadInProgress => BundleUtility.FileExists(PersistentTemplatePath);

        protected virtual string PersistentTemplatePath => AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION + typeof(T).Name + ".txt";

        //--------------------------------------
        // Virtual Methods
        //--------------------------------------

        protected abstract bool IsAssetValid(TAsset asset);

        protected bool IsAssetMetadataValidate(TAsset asset)
        {
            var thumbnailsSize = 128;
            if (asset is StyleAsset) {
                thumbnailsSize = 512;
            }

            return Validation.IsValidAsset(asset, thumbnailsSize);
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------

        void UploadNewAsset(TAsset asset) {
            UploadAsset(GenerateMeta_Create_Request(asset), asset);
        }

        void UploadExistingAsset(TAsset asset) {
            UploadAsset(GenerateMeta_Update_Request(asset), asset);
        }
       

        void UploadAsset(AssetMetadataRequest metaRequest, TAsset asset)
        {
            PrepareForUpload(asset);
            metaRequest.PackageCallbackText = callback => {
                var template = asset.GetTemplate();
                template.Id = new AssetTemplate(callback).Id;
               
                var thumbnailUploader = new AssetThumbnailUploader(template);
                thumbnailUploader.Upload(asset.GetIcon(), iconRes => {
                    template.Icon = iconRes;
                    UploadAssetBundle(asset);
                });
            };
            metaRequest.Send();
        }

        void PrepareForUpload(TAsset asset)
        {
            // Removing generated borders, since we are checking for same names
            foreach (var gb in Object.FindObjectsOfType<GameObject>()) {
                if (gb.name == BorderLayers.GeneratedBorder.ToString())
                    Object.DestroyImmediate(gb);
            }
            
            if (!IsAssetValid(asset) || !UniqueNamesChecker.SceneObjectsNamesAreUnique()) {
                return;
            }
            
            // Need to unpack all prefabs, otherwise all mats within one mesh will be shuffled
            // For example: mat1, mat2, mat3 will be represented as mat2, mat1, mat3 after upload
            foreach (var tr in asset.gameObject.GetComponentsInChildren<Transform>()) {
                if (PrefabUtility.GetPrefabAssetType(tr) != PrefabAssetType.NotAPrefab)
                    PrefabUtility.UnpackPrefabInstance(tr.gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            }

            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_TEMP_LOCATION);
            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);

            asset.PrepareForUpload();
            BundleUtility.SaveTemplateToFile(PersistentTemplatePath, asset.GetTemplate());
        }

        void DownloadAsset(T tpl) {
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

                        BundleUtility.ClearLocalCacheForAsset(tpl.Title);
                        AssetBundle.UnloadAllAssetBundles(true);
                        Resources.UnloadUnusedAssets();
                        Caching.ClearCache();


                        Object bundleObject = null;
                        try {
                            var bundle = AssetBundle.LoadFromFile(bundlePath);
                            var mainAssetName = bundle.GetAllAssetNames()[0];
                            bundleObject = bundle.LoadAsset<Object>(mainAssetName);
                        }
                        catch (Exception e) {
                            EditorUtility.DisplayDialog("Error", tpl.Title + $"AssetBundle is corrupted \n {e.Message}", "Ok");
                        }

                        if (bundleObject == null) {
                            bundleObject = new GameObject("Empty Asset");
                        }

                        var gameObject = (GameObject) Object.Instantiate(bundleObject);
                        gameObject.name = tpl.Title;

                        var asset = CreateDownloadedAsset(tpl, gameObject);
                        AssetDatabase.s_uniqueMeshesFound = false;
                        RunCollectors(asset, new AssetDatabase(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION));
                        
                        OriginalResourcesManager.DownloadAssetResourcesAndOverwrite(tpl.Id, tpl.Title);
                        UnityEditor.AssetDatabase.DeleteAsset(bundlePath);
                        if (AssetDatabase.s_uniqueMeshesFound)
                            EditorUtility.DisplayDialog("Warning!", asset.Title + " has unique meshes with the same names! Manual references fixes required", "Ok");
                    };

                    loadAsset.Send();
                };

                getAssetUrl.Send();
            };
        }

        public static void RunCollectors(IAssetBundle asset, AssetDatabase assetDatabase) {
            // Old renderer collector must be called ALWAYS earlier than Renderer collector!!!
            new V1_RendererCollector().SetAssetDatabase(assetDatabase).Run(asset);
            // EnvironmentCollector() mus be before RendererCollector()
            new EnvironmentCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new RendererCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new TextCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new MeshCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new MeshColliderCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new ComponentsCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new AnimationCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new AnimatorCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new UserClickMarkerCollector().SetAssetDatabase(assetDatabase).Run(asset);

            new V1_ThumbnailsCollector().SetAssetDatabase(assetDatabase).Run(asset);
            new V1_MarkersCollector().SetAssetDatabase(assetDatabase).Run(asset);
        }

        void UploadAssetBundle(TAsset asset) {
              var template = asset.GetTemplate();
              UnityEditor.AssetDatabase.Refresh();

              AssetBundlesSettings.Instance.ReplaceSavedTemplate(template);
              BundleUtility.SaveTemplateToFile(PersistentTemplatePath, template);

              if (asset is SceneStyleAsset)
              {
                  GenerateUploadScene(asset);
              }
              else
              {
                  BundleUtility.GenerateUploadPrefab(asset);
                  OriginalResourcesManager.UploadResourcesArchive(asset.GetTemplate().Id, asset.Title);
              }
                  
              AssetsUploadLoop(0, template);
        }

        void GenerateUploadScene(TAsset asset)
        {
            var tpl = asset.GetTemplate();
            var assetBundleName = $"{tpl.Title}_{tpl.Id}";
            
            var scenePath = $"{AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION}{assetBundleName}.unity";
            EditorSceneManager.SaveScene(asset.gameObject.scene, scenePath, true);
        }

        void UploadScene(TAsset asset)
        {
            var tpl = asset.GetTemplate();
            var assetBundleName = $"{tpl.Title}_{tpl.Id}";
            var scenePath = $"{AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION}{assetBundleName}.unity";
            
            var buildMap = new AssetBundleBuild[1];
            buildMap[0].assetBundleName = assetBundleName;
            buildMap[0].assetNames = new[] { scenePath };
            
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildPipeline.BuildAssetBundles(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION, buildMap, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);

            var assetBytes = System.IO.File.ReadAllBytes(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION + "/" + assetBundleName);
            Debug.Log(assetBytes.Length);
        }
        

        protected void AssetsUploadLoop(int platformIndex, AssetTemplate tpl) {
            AssetBundlesSettings.Instance.UploadPlatformIndex = platformIndex;

            if (platformIndex < AssetBundlesSettings.Instance.TargetPlatforms.Count) {
                var buildTarget = AssetBundlesSettings.Instance.TargetPlatforms[platformIndex];

                var assetBundleName = $"{tpl.Title}_{tpl.Id}";

                if (tpl is StyleAssetTemplate styleAssetTemplate && styleAssetTemplate.StyleType == StyleType.NonExtendable)
                {
                    var scenePath = $"{AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION}{assetBundleName}.unity";
                    var buildMap = new AssetBundleBuild[1];
                    buildMap[0].assetBundleName = assetBundleName;
                    buildMap[0].assetNames = new[] { scenePath };
                    BuildPipeline.BuildAssetBundles(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION, buildMap, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
                }
                else 
                {
                    var prefabPath = $"{AssetBundlesSettings.FULL_ASSETS_TEMP_LOCATION}{assetBundleName}.prefab";
                    assetBundleName = assetBundleName.ToLower();

                    var assetImporter = AssetImporter.GetAtPath(prefabPath);
                    assetImporter.assetBundleName = assetBundleName;
                    BuildPipeline.BuildAssetBundles(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);

                }
                
                ResumeUpload();
            }
        }

        protected virtual void GetUploadLink(AssetTemplate tpl, BuildTarget platform, Action<string> uploadLink) {
            var uploadLinkRequest = new GetUploadLink(tpl.Id, platform.ToString(), tpl.Title);
            uploadLinkRequest.PackageCallbackText = uploadLink.Invoke;
            uploadLinkRequest.Send();
        }

        protected virtual void ConfirmUpload(AssetTemplate tpl, BuildTarget platform, Action onComplete) {
            var confirm = new UploadConfirmation(tpl.Id, platform.ToString());
            confirm.PackageCallbackText = confirmCallback => {
                onComplete.Invoke();
            };
            confirm.Send();
        }

        public void ResumeUpload() {
            var tpl = BundleUtility.LoadTemplateFromFile<AssetTemplate>(PersistentTemplatePath);
            if (tpl == null) {
                return;
            }

            var platformIndex = AssetBundlesSettings.Instance.UploadPlatformIndex;
            var platform = AssetBundlesSettings.Instance.TargetPlatforms[platformIndex];

            var assetBundleName = $"{tpl.Title}_{tpl.Id}";

            EditorProgressBar.AddProgress(tpl.Title, "Getting Asset Upload URL (" + platform + ")", 0.2f);
            GetUploadLink(tpl, platform, linkToUploadTo => {
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

                    ConfirmUpload(tpl, platform, () => {
                        platformIndex++;
                        UnityEditor.AssetDatabase.RemoveUnusedAssetBundleNames();

                        if (platformIndex == AssetBundlesSettings.Instance.TargetPlatforms.Count) {
                            FinishAssetUpload();
                        }
                        else {
                            AssetsUploadLoop(platformIndex, tpl);
                        }
                    });
                };
                uploadRequest.Send();
            });
        }

        void FinishAssetUpload() {
            var tpl = BundleUtility.LoadTemplateFromFile<T>(PersistentTemplatePath);

            BundleUtility.DeleteTempFiles();

            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.AssetDatabase.SaveAssets();

            EditorApplication.delayCall = () => {
                FolderUtils.DeleteFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION, false);
                FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
            };

            EditorProgressBar.FinishUploadProgress();

            OnUploaded.Invoke();
            OnUploadFinished(tpl);
        }

        protected virtual void OnUploadFinished(T tpl)
        {
            //Download(tpl);
            EditorUtility.DisplayDialog("Success", tpl.Title + " Asset has been successfully uploaded!", "Ok");
        }
    }
}
