using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using net.roomful.assets.serialization;
using StansAssets.Foundation.Extensions;

namespace net.roomful.assets
{
    [ExecuteInEditMode]
    internal class EnvironmentAsset : Asset<EnvironmentAssetTemplate>
    {
        //--------------------------------------
        // Initialization
        //--------------------------------------

        public void Start() {
            ApplyEnvironment();
        }

        public void SetTemplate(EnvironmentAssetTemplate tpl) {
            m_Template = tpl;
        }

        //--------------------------------------
        // Unity Editor
        //--------------------------------------

        public void Update() {
            CheckHierarchy();
        }

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        [ContextMenu("Prepare For Upload")]
        public override void PrepareForUpload() {
            PrepareComponentsForUpload();
        }

        protected override void PrepareComponentsForUpload() {
            base.PrepareComponentsForUpload();

#if UNITY_EDITOR
            var cubemapPath = AssetDatabase.GetAssetPath(Settings.ReflectionCubemap);

            if (System.IO.File.Exists(cubemapPath)) {
                //remove Assets/ string from a path. Yes I know that is not stable hack.
                //If you know a better way, make it happened
                cubemapPath = cubemapPath.Substring(7, cubemapPath.Length - 7);
                var data = SA.Common.Util.Files.ReadBytes(cubemapPath);
                Settings.ReflectionCubemapFileData = data;
                Settings.ReflectionCubemapFileName = System.IO.Path.GetFileName(cubemapPath);

                Settings.ReflectionCubemapSettings = new SerializedTexture();
                Settings.ReflectionCubemapSettings.Serialize(Settings.ReflectionCubemap);
            }
#endif
        }

        public void ApplyEnvironment() {
            RenderSettings.skybox = SkyRenderer.sharedMaterial;
            RenderSettings.ambientIntensity = Settings.AmbientIntensity;

            RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;
            RenderSettings.customReflection = Settings.ReflectionCubemap;
            RenderSettings.reflectionIntensity = Settings.ReflectionIntensity;

            DynamicGI.UpdateEnvironment();
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public SerializedEnvironment Settings {
            get {
                var settings = GetComponent<SerializedEnvironment>();
                if (settings == null) {
                    settings = gameObject.AddComponent<SerializedEnvironment>();
                }
                
                return settings;
            }
        }

        public MeshRenderer SkyRenderer {
            get {
                var meshRenderer = GetComponent<MeshRenderer>();
                if (meshRenderer == null) {
                    meshRenderer = gameObject.AddComponent<MeshRenderer>();
                }

                meshRenderer.hideFlags = HideFlags.HideInInspector;

                return meshRenderer;
            }
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------

        protected override void CheckHierarchy() {
            base.CheckHierarchy();

            transform.Reset();
            Environment.transform.parent = null;
            Environment.transform.Reset();

            var undefinedObjects = new List<Transform>();
            var allObjects = FindObjectsOfType<Transform>();
            foreach (var child in allObjects) {
                if (child == transform) {
                    continue;
                }

                if (child.parent != null) {
                    continue;
                }

                if (child == Environment.transform) {
                    continue;
                }

                undefinedObjects.Add(child);
            }

            foreach (var undefined in undefinedObjects) {
                undefined.SetParent(transform);
                undefined.localPosition = Vector3.zero;
            }
        }
    }
}