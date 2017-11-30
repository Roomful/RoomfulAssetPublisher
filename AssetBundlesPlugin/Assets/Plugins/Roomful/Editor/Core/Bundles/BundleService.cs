using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard.Editor
{
    public static class BundleService
    {

        private static List<IBundleManager> s_bundles;


        //--------------------------------------
        // Initialization
        //--------------------------------------

        static BundleService() {
            s_bundles = new List<IBundleManager>();

            s_bundles.Add(new PropBundleManager());
            s_bundles.Add(new EnvironmentBundleManager());
            s_bundles.Add(new StyleBundleManager());
        }


        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public static void Create<T>(T tpl) where T : Template {
            IBundleManager bundle = GetBundleByTemplateType(typeof(T));
            bundle.Create(tpl);
        }

        public static void Download<T>(T tpl) where T : Template {
            IBundleManager bundle = GetBundleByTemplateType(typeof(T));
            bundle.Download(tpl);
        }

        public static void Upload<A>(A asset) where A : IAsset {
            IBundleManager bundle = GetBundleByAssetType(typeof(A));
            bundle.Upload(asset);
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

        private static IBundleManager GetBundleByTemplateType(System.Type type) {
            foreach (var bundle in s_bundles) {
                if(bundle.TemplateType == type) {
                    return bundle;
                }
            }
            return null;
        }

        private static IBundleManager GetBundleByAssetType(System.Type type) {
            foreach (var bundle in s_bundles) {
                if (bundle.AssetType == type) {
                    return bundle;
                }
            }
            return null;
        }

        //--------------------------------------
        // Unity Event Handlers
        //--------------------------------------

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
            
            foreach(var bundle in s_bundles) {
                if(bundle.IsUploadInProgress) {
                    bundle.ResumeUpload();
                }
            }

        }
    }
}