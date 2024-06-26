using System;
using net.roomful.api;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace net.roomful.assets.editor
{
    internal static class PropAssetScreenshotTool
    {
        public static void CreateIcon(bool useEditorCamera, PropAsset prop) {
            var savedLayer = prop.gameObject.layer;
            const int screenShotLayer = 31;
            SetLayerForAsset(screenShotLayer, prop);
            var environment = SceneManager.GetActiveScene().GetComponentInScene<Environment>();
            var cameraHolder = new GameObject();
            cameraHolder.transform.SetParent(environment.gameObject.transform);
            var camera = cameraHolder.AddComponent<Camera>();

            if (useEditorCamera) {
                var sceneView = SceneView.lastActiveSceneView;
                camera.transform.position = sceneView.camera.transform.position;
                camera.transform.rotation = sceneView.camera.transform.rotation;
            }
            else {
                if (prop.Template.Placing == PlacingType.Floor) {
                    SetupCameraForFloorProp(prop.Template.Size, prop.gameObject.transform.position, camera);
                }
                else {
                    SetupCameraForWallProp(prop.Template.Size, prop.gameObject.transform.position, camera);
                }
            }

            var whiteScreenshot = MakeScreenshotWithBackground(camera, Color.white, screenShotLayer);
            var blackScreenshot = MakeScreenshotWithBackground(camera, Color.black, screenShotLayer);

            var resultIcon = MakeTextureFromIntersection(whiteScreenshot, blackScreenshot);
            prop.Icon = resultIcon;

            SetLayerForAsset(savedLayer, prop);
            Object.DestroyImmediate(cameraHolder);
        }
        public static void CreateIcon(bool useEditorCamera, PropAsset prop, out Texture2D icon) {
            var savedLayer = prop.gameObject.layer;
            const int screenShotLayer = 31;
            SetLayerForAsset(screenShotLayer, prop);
            var environment = SceneManager.GetActiveScene().GetComponentInScene<Environment>();
            var cameraHolder = new GameObject();
            cameraHolder.transform.SetParent(environment.gameObject.transform);
            var camera = cameraHolder.AddComponent<Camera>();

            if (useEditorCamera) {
                var sceneView = SceneView.lastActiveSceneView;
                camera.transform.position = sceneView.camera.transform.position;
                camera.transform.rotation = sceneView.camera.transform.rotation;
            }
            else {
                if (prop.Template.Placing == PlacingType.Floor) {
                    SetupCameraForFloorProp(prop.Template.Size, prop.gameObject.transform.position, camera);
                }
                else {
                    SetupCameraForWallProp(prop.Template.Size, prop.gameObject.transform.position, camera);
                }
            }

            var whiteScreenshot = MakeScreenshotWithBackground(camera, Color.white, screenShotLayer);
            var blackScreenshot = MakeScreenshotWithBackground(camera, Color.black, screenShotLayer);

            var resultIcon = MakeTextureFromIntersection(whiteScreenshot, blackScreenshot);
            icon = resultIcon;

            SetLayerForAsset(savedLayer, prop);
            Object.DestroyImmediate(cameraHolder);
        }

        private static void SetLayerForAsset(int savedLayer, PropAsset prop) {
            foreach (var trans in prop.gameObject.GetComponentsInChildren<Transform>(true)) {
                trans.gameObject.layer = savedLayer;
            }
        }

        private static Texture2D MakeScreenshotWithBackground(Camera camera, Color background, int layer) {
            camera.cullingMask = (1 << layer);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = background;

            var rt = new RenderTexture(256, 256, 32);
            var screenshot = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
            camera.targetTexture = rt;
            camera.Render();

            RenderTexture.active = rt;
            screenshot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            screenshot.Apply();
            RenderTexture.active = null;
            rt.DiscardContents();
            return screenshot;
        }

        private static Texture2D MakeTextureFromIntersection(Texture2D left, Texture2D right) {
            if (left.width != right.width || left.height != right.height) {
                throw new ArgumentException("Textures dimentions must be equal");
            }

            var resultIcon = new Texture2D(left.width, left.height, TextureFormat.RGBA32, false);
            var leftColors = left.GetPixels32();
            var rightColors = right.GetPixels32();
            var resultColors = new Color32[leftColors.Length];
            for (var i = resultColors.Length - 1; i >= 0; i--) {
                if (leftColors[i].Equals(rightColors[i])) {
                    resultColors[i] = leftColors[i];
                }
                else {
                    resultColors[i] = Color.clear;
                }
            }

            resultIcon.SetPixels32(resultColors);
            resultIcon.Apply();
            return resultIcon;
        }

        public static void SetupCameraForFloorProp(Vector3 size, Vector3 position, Camera camera) {
            var halfX = size.x / 2;
            var halfY = size.y / 2;
            var halfZ = size.z / 2;
            var diagonalXY = Mathf.Sqrt(halfX * halfX + halfY * halfY);
            var diagonal = Mathf.Sqrt(diagonalXY * diagonalXY + halfZ * halfZ);
            var distance = Math.Max(diagonal / Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2), camera.nearClipPlane);
            var camPozition = new Vector3(position.x, position.y + halfY, position.z);
            camera.transform.position = camPozition;
            camera.transform.rotation = Quaternion.Euler(30, 210, 0);
            camera.transform.position -= camera.transform.forward.normalized * distance;
        }

        private static void SetupCameraForWallProp(Vector3 size, Vector3 propPosition, Camera camera) {
            var center = new Vector3(propPosition.x, propPosition.y, propPosition.z);
            var deltaZ = size.y / 2 / Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2);
            var propAspectRatio = size.x / size.y;
            if (propAspectRatio > camera.aspect) {
                deltaZ = deltaZ / camera.aspect * propAspectRatio;
            }

            center.z += size.z / 2;
            center.z += deltaZ;
            camera.transform.position = center;
            camera.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}