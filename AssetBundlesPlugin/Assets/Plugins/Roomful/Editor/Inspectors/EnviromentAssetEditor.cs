using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace RF.AssetWizzard.Editor
{
    [CustomEditor(typeof(EnviromentAsset))]
    public class EnviromentAssetEditor : UnityEditor.Editor
    {



        public override void OnInspectorGUI() {


            bool upload = GUILayout.Button("Upload", EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(120) });
            if (upload) {
                //AssetBundlesManager.UploadAssets(Prop);
            }

        }

        public EnviromentAsset Enviroment {
            get {
                return target as EnviromentAsset;
            }
        }
    }


}