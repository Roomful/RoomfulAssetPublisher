using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard
{

    [ExecuteInEditMode]
    public class EnvironmentAsset : Asset<EnvironmentTemplate>
    {


        //--------------------------------------
        // Initialization
        //--------------------------------------

        public void Start() {
            ApplyEnvironment(); 
        }

        public void SetTemplate(EnvironmentTemplate tpl) {
            _Template = tpl;
        }


        //--------------------------------------
        // Unity Editor
        //--------------------------------------


        public void Update() {
            CheckhHierarchy();

            

        }


        //--------------------------------------
        // Public Methods
        //--------------------------------------

        [ContextMenu("Prepare For Upload")]
        public override void PrepareForUpload() {

            CleanUpSilhouette();
            PrepareCoponentsForUpload();
        }

        protected override void PrepareCoponentsForUpload() {

            base.PrepareCoponentsForUpload();

#if UNITY_EDITOR
            string cubemapPath = AssetDatabase.GetAssetPath(Settings.ReflectionCubemap);

            if (System.IO.File.Exists(cubemapPath)) {

                //remove Assets/ string from a path. Yes I know that is not stable hack.
                //If you know a better way, make it happend
                cubemapPath = cubemapPath.Substring(7, cubemapPath.Length - 7);
                byte[] data = SA.Common.Util.Files.ReadBytes(cubemapPath);
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


        public SerializedEnviromnent Settings {
            get {

                var settings = GetComponent<SerializedEnviromnent>();
                if (settings == null) {
                    settings = gameObject.AddComponent<SerializedEnviromnent>();
                }

                settings.hideFlags = HideFlags.HideInInspector;

                return settings;
            }
        }


        public MeshRenderer SkyRenderer {
            get {

                var renderer = GetComponent<MeshRenderer>();
                if (renderer == null) {
                    renderer = gameObject.AddComponent<MeshRenderer>();
                }

                renderer.hideFlags = HideFlags.HideInInspector;

                return renderer;
            }
        }



        //--------------------------------------
        // Private Methods
        //--------------------------------------


        protected override void CheckhHierarchy() {

            base.CheckhHierarchy();

            transform.Reset();
            Environment.transform.parent = null;
            Environment.transform.Reset(); 

            List<Transform> UndefinedObjects = new List<Transform>();
            Transform[] allObjects = FindObjectsOfType<Transform>();
            foreach (Transform child in allObjects) {
                if (child == transform) {
                    continue;
                }

                if (child.parent != null) {
                    continue;
                }

                if (child == Environment.transform) {
                    continue;
                }

                UndefinedObjects.Add(child);
            }

            foreach (Transform undefined in UndefinedObjects) {
                undefined.SetParent(transform);
                undefined.localPosition = Vector3.zero;
            }

        }


    }
}