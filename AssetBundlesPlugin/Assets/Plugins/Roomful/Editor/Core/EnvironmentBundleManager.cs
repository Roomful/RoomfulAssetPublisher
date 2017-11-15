using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using RF.AssetBundles.Serialization;
using RF.AssetBundles;

namespace RF.AssetWizzard.Editor {
    public static class EnvironmentBundleManager
    {


        public static void CreateNewEnvironment(EnvironmentTemplate tpl) {
            if (string.IsNullOrEmpty(tpl.Title)) {
                Debug.Log("Prop's name is empty");
                return;
            }

            EditorApplication.delayCall = () => {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
                WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);

                EnvironmentAsset asset = new GameObject(tpl.Title).AddComponent<EnvironmentAsset>();
                asset.SetTemplate(tpl);
            };
        }



        public static void UploadAssets(EnvironmentAsset asset) {

            if (!Validation.Run(asset)) { return; }

            //just to mark that uploading process has started
            // AssetBundlesSettings.Instance.UploadTemplate = prop.Template;

            asset.PrepareForUpload();


            EditorProgressBar.StartUploadProgress("Updating Asset Template");

            /*
            Network.Request.CreatePropMetaData createMeta = new RF.AssetWizzard.Network.Request.CreatePropMetaData(prop.Template);
            createMeta.PackageCallbackText = (callback) => {
                prop.Template.Id = new AssetTemplate(callback).Id;
                UploadAssetBundle(prop);
            };

            createMeta.Send();
            */
            
        }

    }
}
