using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace RF.AssetWizzard.Editor
{
    [CustomEditor(typeof(EnvironmentAsset))]
    public class EnvironmentAssetInspector : AssetInspector<EnvironmentTemplate, EnvironmentAsset>
    {



        public override void OnInspectorGUI() {

            serializedObject.Update();

            // EditorGUILayout.ObjectField

            EditorGUI.BeginChangeCheck();
            
            Asset.SkyRenderer.sharedMaterial = (Material)EditorGUILayout.ObjectField("Skybox:", Asset.SkyRenderer.sharedMaterial, typeof(Material), true);
            Asset.Settings.AmbientIntensity = EditorGUILayout.Slider("Ambient Intensity", Asset.Settings.AmbientIntensity, 0f, 8f);

            if (EditorGUI.EndChangeCheck()) {
                Asset.ApplyEnvironment();
            }


            DrawEnvironmentSiwtch();
            DrawActionButtons();


            serializedObject.ApplyModifiedProperties();

        }

        public override EnvironmentAsset Asset {
            get {
                return target as EnvironmentAsset;
            }
        }
    }


}