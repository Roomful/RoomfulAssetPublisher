#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace net.roomful.assets
{
    [ExecuteInEditMode]
    internal class EditorScenePlanarReflections : MonoBehaviour
    {
        // references
        [SerializeField] private Camera m_mainCamera;
        [SerializeField] private Camera m_reflectionCamera;
        [SerializeField] private Transform m_reflectionPlane;

        // statics
        private static Camera MainCamera;
        private static Camera ReflectionCamera;
        private static Transform s_mainCamTransform;
        private static Transform s_reflectionCamTransform;
        private static Transform s_reflectionPlaneTransform;

        private static readonly int m_reflectionTex = Shader.PropertyToID("_ReflectionTex");

        private void Update() {
            if (MainCamera == null)
                MainCamera = m_mainCamera;
            if (ReflectionCamera == null)
                ReflectionCamera = m_reflectionCamera;
            if (s_reflectionPlaneTransform == null)
                s_reflectionPlaneTransform = m_reflectionPlane;
            UpdatePlanarReflections(null);
        }

        public static void UpdatePlanarReflections(List<Material> matsList) {
            matsList = FindReflectiveShaderMaterials();
            if (matsList.Count > 0) {
                s_mainCamTransform = MainCamera.transform;
                var camRect = SceneView.lastActiveSceneView.camera.pixelRect;
                var camTransform = SceneView.lastActiveSceneView.camera.transform;
                s_mainCamTransform.rotation = camTransform.rotation;
                s_mainCamTransform.position = camTransform.position;
                s_reflectionCamTransform = ReflectionCamera.transform;
                ReflectionCamera.CopyFrom(MainCamera);
                ReflectionCamera.rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                var rendTexture = GetOrCreateRenderTexture((int) camRect.width, (int) camRect.height);
                ReflectionCamera.targetTexture = rendTexture;
                foreach (var mat in matsList) {
                    mat.SetTexture(m_reflectionTex, rendTexture);
                }

                RenderReflection();
            }
        }

        private static List<Material> FindReflectiveShaderMaterials() {
            var matsList = new List<Material>();
            var renderers = FindObjectsOfType<MeshRenderer>();
            foreach (var rend in renderers) {
                foreach (var mat in rend.sharedMaterials) {
                    if (mat?.shader.name == "Shader Graphs/ReflectiveShader" || mat?.shader.name == "Shader Graphs/ReflectiveWater") {
                        matsList.Add(mat);
                    }
                }
            }

            return matsList;
        }

        private static void RenderReflection() {
            // take main camera directions and position world space
            var cameraDirectionWorldSpace = s_mainCamTransform.forward;
            var cameraUpWorldSpace = s_mainCamTransform.up;
            var cameraPositionWorldSpace = s_mainCamTransform.position;

            // transform direction and position by reflection plane
            var cameraDirectionPlaneSpace = s_reflectionPlaneTransform.InverseTransformDirection(cameraDirectionWorldSpace);
            var cameraUpPlaneSpace = s_reflectionPlaneTransform.InverseTransformDirection(cameraUpWorldSpace);
            var cameraPositionPlaneSpace = s_reflectionPlaneTransform.InverseTransformPoint(cameraPositionWorldSpace);

            // invert direction and position by reflection plane
            cameraDirectionPlaneSpace.y *= -1;
            cameraUpPlaneSpace.y *= -1;
            cameraPositionPlaneSpace.y *= -1;

            // transform direction and position from reflection plane local space to world space
            cameraDirectionWorldSpace = s_reflectionPlaneTransform.TransformDirection(cameraDirectionPlaneSpace);
            cameraUpWorldSpace = s_reflectionPlaneTransform.TransformDirection(cameraUpPlaneSpace);
            cameraPositionWorldSpace = s_reflectionPlaneTransform.TransformPoint(cameraPositionPlaneSpace);

            // apply direction and position to reflection camera
            s_reflectionCamTransform.position = cameraPositionWorldSpace;
            s_reflectionCamTransform.LookAt(cameraPositionWorldSpace + cameraDirectionWorldSpace, cameraUpWorldSpace);
        }

        private static RenderTexture s_renderTexture;
        private static int m_textureWidth = -1;
        private static int m_textureHeight = -1;

        private static RenderTexture GetOrCreateRenderTexture(int width, int height) {
            if (s_renderTexture == null) {
                return CreateRenderTexture(width, height);
            }

            if (m_textureWidth != width || m_textureHeight != height) {
                return CreateRenderTexture(width, height);
            }

            return s_renderTexture;
        }

        private static RenderTexture CreateRenderTexture(int width, int height) {
            m_textureWidth = width;
            m_textureHeight = height;

            if (s_renderTexture != null) {
                if (Application.isPlaying) {
                    Destroy(s_renderTexture);
                }
                else {
                    DestroyImmediate(s_renderTexture);
                }
                s_renderTexture = null;
            }

            s_renderTexture = new RenderTexture(width, height, 16, DefaultFormat.HDR) { useMipMap = true };
            s_renderTexture.Create();
            return s_renderTexture;
        }
    }
}
#endif