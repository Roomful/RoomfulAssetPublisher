using System;
using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using UnityEditor.SceneManagement;

namespace RF.AssetWizzard.Editor
{

    public abstract class AssetWizzard<A> : WizzardUIComponent, IAssetWizzard where A : IAsset {


        //--------------------------------------
        // Abstract Methods
        //--------------------------------------

        public abstract void OnGUI(bool GUIState);

        public abstract void Create();
        public abstract void Upload();
        public abstract void Download();

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public void DrawTitleFiled(bool GUIState) {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Title: ", GUILayout.Width(100));
            GUI.enabled = false;
            Template.Title = EditorGUILayout.TextField(Template.Title, GUILayout.Width(240));
            GUI.enabled = GUIState;
            GUILayout.EndHorizontal();
        }

        public void DrawTags() {
            GUILayout.BeginVertical(GUILayout.Width(225));
            {
                ReorderableListGUI.Title("Asset Tags");
                try {
                    ReorderableListGUI.ListField(Template.Tags, TagListItem, DrawEmptyTag);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
            GUILayout.EndVertical();
        }

        public void DrawControlButtons() {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            Rect buttonRect1 = new Rect(460, 360, 120, 18);
            Rect buttonRect2 = new Rect(310, 360, 120, 18);
            Rect buttonRect3 = new Rect(460, 390, 120, 18);
            Rect buttonRect4 = new Rect(310, 390, 120, 18);

            if (Asset.GetTemplate().IsNew) {
                bool upload = GUI.Button(buttonRect1, "Upload");
                if (upload) {
                    Upload();
                }

            } else {
                bool upload = GUI.Button(buttonRect1, "Reupload");
                if (upload) {
                    Upload();
                }

                bool refresh = GUI.Button(buttonRect2, "Refresh");
                if (refresh) {
                    Download();
                }
            } 
            if (Asset is PropAsset) {
                bool createIcon = GUI.Button(buttonRect4, "Create icon");
                if (createIcon) {
                    CreateIcon();
                }
            }

            bool create = GUI.Button(buttonRect3, "Create New");
            if (create) {
                Create();
            }


            GUILayout.Space(40f);
            GUILayout.EndHorizontal();
        }

        private void CreateIcon() {
            if (!(Asset is PropAsset)) {
                return;
            }
            var savedLayer = Asset.gameObject.layer;
            const int screenShotLayer = 31;
            SetLayerForAsset(screenShotLayer);
            var enviroment = EditorSceneManager.GetActiveScene().GetComponentInScene<Environment>();
            var cameraHolder = new GameObject();
            cameraHolder.transform.SetParent(enviroment.gameObject.transform);
            var camera = cameraHolder.AddComponent<Camera>();
            PropAsset prop = Asset as PropAsset;
            if (prop.Template.Placing == Placing.Floor) {
                SetupCameraForFloorProp(prop.Template.Size, prop.gameObject.transform.position, camera);
            }
            else {
                SetupCameraForWallProp(prop.Template.Size, prop.gameObject.transform.position, camera);
            }
            Texture2D whiteScreenshot = MakeScreenshotWithBackground(camera, Color.white, screenShotLayer);
            Texture2D blackScreenshot = MakeScreenshotWithBackground(camera, Color.black, screenShotLayer);

            Texture2D resultIcon = MakeTextureFromIntersection(whiteScreenshot, blackScreenshot);
            prop.Icon = resultIcon;

            SetLayerForAsset(savedLayer);
            GameObject.DestroyImmediate(cameraHolder);
        }

        private void SetLayerForAsset(int savedLayer) {
            foreach (Transform trans in Asset.gameObject.GetComponentsInChildren<Transform>(true)) {
                trans.gameObject.layer = savedLayer;
            }
        }

        private Texture2D MakeScreenshotWithBackground(Camera camera, Color background, int layer) {

            camera.cullingMask = (1 << layer);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = background;

            RenderTexture rt = new RenderTexture(256, 256, 32);
            Texture2D screenshot = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
            camera.targetTexture = rt;
            camera.Render();

            RenderTexture.active = rt;
            screenshot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            screenshot.Apply();
            RenderTexture.active = null;
            rt.DiscardContents();
            return screenshot;
        }

        private Texture2D MakeTextureFromIntersection(Texture2D left, Texture2D right) {
            if (left.width != right.width || left.height != right.height) {
                throw new ArgumentException("Textures dimentions must be equal");
            }
            Texture2D resultIcon = new Texture2D(left.width, left.height, TextureFormat.RGBA32, false);
            var leftColors = left.GetPixels32();
            var rightColors = right.GetPixels32();
            Color32[] resultColors = new Color32[leftColors.Length];
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
            float halfX = size.x /2 ;
            float halfY = size.y /2 ;
            float halfZ = size.z /2 ;
            float diagonalXY = Mathf.Sqrt(halfX * halfX + halfY * halfY);
            float diagonal = Mathf.Sqrt(diagonalXY * diagonalXY + halfZ * halfZ);
            float distance = Math.Max(diagonal / Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2), camera.nearClipPlane);
            var camPozition = new Vector3(position.x, position.y + halfY, position.z);
            camera.transform.position = camPozition ;
            camera.transform.rotation = Quaternion.Euler(30, 210, 0);
            camera.transform.position -= camera.transform.forward.normalized * distance;
        }

        private static float GetHorizontalFieldOfView(Camera camera) {
            var aLength = 1f;
            var bLenght = aLength / Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2);
            bLenght = bLenght * camera.aspect;
            var angle = Mathf.Atan(bLenght / aLength) * Mathf.Rad2Deg;
            return angle;
        }

        private static void SetupCameraForWallProp(Vector3 size, Vector3 propPosition, Camera camera) {
            Vector3 center = new Vector3(propPosition.x, propPosition.y, propPosition.z);
            float deltaZ = size.y / 2 / Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2);
            float propAspectRatio = size.x / size.y;
            if (propAspectRatio > camera.aspect) {
                deltaZ = deltaZ / camera.aspect * propAspectRatio;
            }
            center.z += size.z / 2;
            center.z += deltaZ;
            camera.transform.position = center;
            camera.transform.rotation = Quaternion.Euler(0,180,0);
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------


        public bool HasAsset {
            get {
                return Asset != null;
            }
        }

        protected Template Template  {
            get {
                return Asset.GetTemplate();
            }
        }


        protected A Asset {
            get {
                return FindObjectWithType<A>();
            }
        }



        //--------------------------------------
        // Private Methods
        //--------------------------------------


        private string TagListItem(Rect position, string itemValue) {
            if (itemValue == null)
                itemValue = "new_tag";
            return EditorGUI.TextField(position, itemValue);
        }

        private void DrawEmptyTag() {
            GUILayout.Label("No items in list.", EditorStyles.miniLabel);
        }



        private T FindObjectWithType<T>() {
            var allFindedObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (GameObject gameObject in allFindedObjects) {
                T target = gameObject.GetComponent<T>();

                if (target != null) {
                    return target;
                }
            }
            return default(T);
        }

    }
}