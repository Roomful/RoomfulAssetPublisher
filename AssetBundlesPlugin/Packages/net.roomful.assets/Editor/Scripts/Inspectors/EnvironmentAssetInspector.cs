using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.Editor
{
    [CustomEditor(typeof(EnvironmentAsset))]
    internal class EnvironmentAssetInspector : AssetInspector<EnvironmentAssetTemplate, EnvironmentAsset>
    {
        public override void OnInspectorGUI() {
            serializedObject.Update();

            // EditorGUILayout.ObjectField

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Ambient:", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            Asset.SkyRenderer.sharedMaterial = (Material) EditorGUILayout.ObjectField("Skybox:", Asset.SkyRenderer.sharedMaterial, typeof(Material), true);
            Asset.Settings.AmbientIntensity = EditorGUILayout.Slider("Intensity", Asset.Settings.AmbientIntensity, 0f, 8f);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Reflection:", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            Asset.Settings.ReflectionCubemap = (Cubemap) EditorGUILayout.ObjectField("Cubemap:", Asset.Settings.ReflectionCubemap, typeof(Cubemap), true);
            Asset.Settings.ReflectionIntensity = EditorGUILayout.Slider("Intensity", Asset.Settings.ReflectionIntensity, 0f, 1f);
            EditorGUI.indentLevel--;

            if (EditorGUI.EndChangeCheck()) {
                Asset.ApplyEnvironment();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Options:", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            DrawEnvironmentSwitch();
            DrawActionButtons();
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }

        protected override EnvironmentAsset Asset => target as EnvironmentAsset;
    }
}