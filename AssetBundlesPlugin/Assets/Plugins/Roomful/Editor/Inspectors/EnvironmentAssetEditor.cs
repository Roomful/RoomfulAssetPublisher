using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace RF.AssetWizzard.Editor
{
    [CustomEditor(typeof(EnvironmentAsset))]
    public class EnvironmentAssetEditor : UnityEditor.Editor
    {



        public override void OnInspectorGUI() {


            bool upload = GUILayout.Button("Upload", EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(120) });
            if (upload) {
                //AssetBundlesManager.UploadAssets(Prop);
            }

        }

        public EnvironmentAsset Environment {
            get {
                return target as EnvironmentAsset;
            }
        }
    }


}