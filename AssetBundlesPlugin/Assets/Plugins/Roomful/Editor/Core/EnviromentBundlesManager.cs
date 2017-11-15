using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using RF.AssetBundles.Serialization;
using RF.AssetBundles;

namespace RF.AssetWizzard.Editor {
    public static class EnviromentBundlesManager
    {


        public static void CreateNewEnviroment(EnviromentTemplate tpl) {
            if (string.IsNullOrEmpty(tpl.Title)) {
                Debug.Log("Prop's name is empty");
                return;
            }

            EditorApplication.delayCall = () => {
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
                WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);

                EnviromentAsset asset = new GameObject(tpl.Title).AddComponent<EnviromentAsset>();
                asset.SetTemplate(tpl);
            };
        }

    }
}
