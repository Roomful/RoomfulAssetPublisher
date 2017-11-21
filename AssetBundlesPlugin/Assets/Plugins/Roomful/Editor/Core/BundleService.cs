using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Editor
{
    public static class BundleService
    {

        //--------------------------------------
        // Public Methods
        //--------------------------------------


        public static void Create(PropTemplate tpl) {

        }


        public static void Upload(PropTemplate tpl) {

        }

        public static void Download(PropTemplate tpl) {

            if (AssetBundlesSettings.Instance.AutomaticCacheClean) {
                BundleUtility.ClearLocalCache();
            }


        }



        //--------------------------------------
        // Editor Event Handlers
        //--------------------------------------

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnDidReloadScripts() {

        }


    }
}