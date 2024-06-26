﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace net.roomful.assets.editor
{
    [InitializeOnLoad]
    internal static class BundleService
    {
        private static readonly List<IBundleManager> s_bundles;
        private const string REQUIRED_UNITY_VERSION = "2021.3.21";

        //--------------------------------------
        // Initialization
        //--------------------------------------

        static BundleService() {
            s_bundles = new List<IBundleManager> {
                new PropBundleManager(),
                new EnvironmentBundleManager(),
                new StyleBundleManager(),
                new SkinBundleManager()
            };

            WebServer.OnRequestFailed += OnRequestFailed;
        }

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public static void Create<T>(T tpl) where T : AssetTemplate {
            var bundle = GetBundleByTemplateType(typeof(T));
            bundle.Create(tpl);
        }

        public static void Download<T>(T tpl) where T : AssetTemplate {
            var bundle = GetBundleByTemplateType(typeof(T));
            bundle.Download(tpl);
        }

        public static void UploadSkin(PropVariant variant, PropSkin skin, PropAsset asset) {
            if (!IsUploadAllowed() || !UniqueNamesChecker.SceneObjectsNamesAreUnique())
                return;
            
            skin.HeavySkin = CustomizableSkinGameObject.IsHeavySkinSkin(asset, skin);
            var skinAsset = asset.gameObject.AddComponent<SkinAsset>();
            var template = new PropSkinUploadModel(asset.GetTemplate(), skin, variant);
            skinAsset.SetTemplate(template);
            Object.DestroyImmediate(asset);
            Upload(skinAsset);
        }

        public static void PrepareSkinForUploadTest(PropVariant variant, PropSkin skin, PropAsset asset) {
            var skinAsset = asset.gameObject.AddComponent<SkinAsset>();
            var template = new PropSkinUploadModel(asset.GetTemplate(), skin, variant);
            skinAsset.SetTemplate(template);
            Object.DestroyImmediate(asset);

            skinAsset.PrepareForUpload();
        }

        public static void DownloadSkin(PropAsset asset, PropVariant variant, PropSkin skin) {
            var pl = EditorUserBuildSettings.activeBuildTarget.ToString();
            var getAssetUrl = new GetSkinUrl(skin.Id, pl);
            getAssetUrl.PackageCallbackText = skinUrl => {
                Debug.Log("url from pack: " + skinUrl);
                EditorApplication.delayCall = () => {
                    //var template = new PropSkinUploadModel(asset.GetTemplate(), skin);

                    Debug.Log($"Skin URL: {skinUrl}");
                    var loadAsset = new DownloadAsset(skinUrl);
                    loadAsset.PackageCallbackData = assetData => {
                        if (!FolderUtils.IsFolderExists(AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION)) {
                            FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION);
                        }

                        var bundlePath = AssetBundlesSettings.FULL_ASSETS_RESOURCES_LOCATION + "/" + skin.Name + "_" + pl;
                        FolderUtils.WriteBytes(bundlePath, assetData);

                        BundleUtility.ClearLocalCacheForAsset(skin.Name);
                        AssetBundle.UnloadAllAssetBundles(true);
                        Resources.UnloadUnusedAssets();
                        Caching.ClearCache();

                        var bundle = AssetBundle.LoadFromFile(bundlePath);
                        var bundleObject = bundle.LoadAsset<Object>(bundle.GetAllAssetNames()[0]);

                        if (bundleObject == null) {
                            EditorUtility.DisplayDialog("Error", skin.Name + "AssetBundle is corrupted", "Ok");
                            return;
                        }

                        var gameObject = (GameObject) Object.Instantiate(bundleObject);
                        gameObject.name = skin.Name + "__skin";

                        var skinAsset = gameObject.AddComponent<SkinAsset>();
                        var template = new PropSkinUploadModel(asset.GetTemplate(), skin, variant);
                        skinAsset.SetTemplate(template);

                        SkinBundleManager.RunCollectors(skinAsset, new AssetDatabase(AssetBundlesSettings.ASSETS_RESOURCES_LOCATION));
                      //  OriginalResourcesManager.DownloadSkinResourcesAndOverwrite(asset.GetTemplate().Id, skin.Id, skin.Name);

                        if (!VariantsService.DebugMode) {
                            SkinUtility.ApplySkin(asset.GetLayer(HierarchyLayers.Graphics), skinAsset.GetLayer(HierarchyLayers.Graphics));
                            Object.DestroyImmediate(gameObject);
                            UnityEditor.AssetDatabase.DeleteAsset(bundlePath);
                        }
                    };

                    loadAsset.Send();
                };
            };

            getAssetUrl.Send();
        }

        public static void Upload<TAsset>(TAsset asset) where TAsset : IAsset {
            if (!IsUploadAllowed())
                return;

            var bundle = GetBundleByAssetType(typeof(TAsset));
            bundle.Upload(asset);
        }

        private static bool IsUploadAllowed() {
            
            if (!Application.unityVersion.StartsWith(REQUIRED_UNITY_VERSION)) {

                EditorUtility.DisplayDialog("Error!",
                    $"Assets upload is only allowed with Unity {REQUIRED_UNITY_VERSION}",
                    "Okay");
                return false;
            }
            

            if (AssetBundlesSettings.Instance.TargetPlatforms.Count > 0) {
                foreach (var target in AssetBundlesSettings.Instance.TargetPlatforms) {
                    var buildTarget = target;
                    if (!Validation.s_allowedTargets.Contains(buildTarget)) {
                        EditorUtility.DisplayDialog("Error", buildTarget + " target is not supported", "Ok");
                        WindowManager.Wizzard.SwitchTab(WizardTabs.Platforms);
                        return false;
                    }
                }
            }
            else {
                EditorUtility.DisplayDialog("Error", "Please select at least one target platform to upload", "Ok");
                WindowManager.Wizzard.SwitchTab(WizardTabs.Platforms);
                return false;
            }
            
            return true;
        }


        public static void UpdateMeta<TAsset>(TAsset asset) where TAsset : IAsset {
            var bundle = GetBundleByAssetType(typeof(TAsset));
            bundle.UpdateMeta(asset);
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public static bool IsUploadInProgress {
            get {
                foreach (var bundle in s_bundles) {
                    if (bundle.IsUploadInProgress) {
                        return true;
                    }
                }

                return false;
            }
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------

        private static IBundleManager GetBundleByTemplateType(Type type) {
            foreach (var bundle in s_bundles) {
                if (bundle.TemplateType == type) {
                    return bundle;
                }
            }

            return null;
        }

        private static IBundleManager GetBundleByAssetType(Type type) {
            foreach (var bundle in s_bundles) {
                if (bundle.AssetType == type) {
                    return bundle;
                }
            }

            return null;
        }

        //--------------------------------------
        // Event Handlers
        //--------------------------------------

        static void OnRequestFailed(BaseWebPackage package) {
            Debug.LogError("Server Communication Error. Code: " + package.Error.Code + "\nMessage: " + package.Error.Message + "\nURL: " + package.Url);
            BundleUtility.DeleteTempFiles();
        }

        //--------------------------------------
        // Unity Event Handlers
        //--------------------------------------

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
            foreach (var bundle in s_bundles) {
                if (bundle.IsUploadInProgress) {
                    bundle.ResumeUpload();
                    return;
                }
            }
        }
    }
}
