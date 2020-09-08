using System;
using System.Collections.Generic;
using UnityEditor;

namespace net.roomful.assets.Editor
{
    [InitializeOnLoad]
    public static class BundleService
    {
        private static readonly List<IBundleManager> s_bundles;

        //--------------------------------------
        // Initialization
        //--------------------------------------

        static BundleService() {
            s_bundles = new List<IBundleManager>();

            s_bundles.Add(new PropBundleManager());
            s_bundles.Add(new EnvironmentBundleManager());
            s_bundles.Add(new StyleBundleManager());

            Network.WebServer.OnRequestFailed += OnRequestFiled;
        }

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public static void Create<T>(T tpl) where T : Template {
            var bundle = GetBundleByTemplateType(typeof(T));
            bundle.Create(tpl);
        }

        public static void Download<T>(T tpl) where T : Template {
            var bundle = GetBundleByTemplateType(typeof(T));
            bundle.Download(tpl);
        }

        public static void Upload<TAsset>(TAsset asset) where TAsset : IAsset {
            var bundle = GetBundleByAssetType(typeof(TAsset));
            bundle.Upload(asset);
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

        private static void OnRequestFiled(Network.Request.BaseWebPackage package) {
            EditorUtility.DisplayDialog("Server Communication Error", "Code: " + package.Error.Code + "\nMessage: " + package.Error.Message + "\nURL: " + package.Url, "Ok :(");
            BundleUtility.DeleteTempFiles();
        }

        //--------------------------------------
        // Unity Event Handlers
        //--------------------------------------

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
            UnityEngine.Debug.Log("Scripts reloaded");

            foreach (var bundle in s_bundles) {
                if (bundle.IsUploadInProgress) {
                    bundle.ResumeUpload();
                }
            }
        }
    }
}