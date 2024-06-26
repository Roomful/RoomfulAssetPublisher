using UnityEngine;
using net.roomful.assets.serialization;

namespace net.roomful.assets
{
    [SelectionBase]
    [ExecuteInEditMode]
    internal class PropThumbnail : BaseComponent, IPropPublihserComponent
    {
        public int ImageIndex = 0;
        public Texture2D Thumbnail;

        private void Awake() {
            Thumbnail = Resources.Load("logo_square") as Texture2D;
            Refresh();
        }

        //--------------------------------------
        // Unity Editor
        //--------------------------------------


        public void Update() {
            Refresh();
        }

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        private void Refresh() {
            CheckHierarchy();
        }

        public void PrepareForUpload() {
            DestroyImmediate(Canvas.gameObject);
            DestroyImmediate(this);
        }

        public void SetThumbnail(Texture2D newTex) {
            Thumbnail = newTex;
            Canvas.GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));
            Canvas.GetComponent<Renderer>().sharedMaterial.mainTexture = Thumbnail;
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public Transform Canvas {
            get {
                var canvasName = "Canvas" + transform.name;
                var canvas = transform.Find(canvasName);
                // Removing redundant Canvases, created by duplication
                foreach (Transform child in transform) {
                    if (child.name.Contains("Canvas") && child.name != canvasName)
                        DestroyImmediate(child.gameObject);
                }
                if (canvas == null) {
                    var c = GameObject.CreatePrimitive(PrimitiveType.Quad);

                    canvas = c.transform;

                    canvas.name = canvasName;
                    canvas.parent = transform;
                    canvas.GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));
                }

                if (canvas.GetComponent<Renderer>().sharedMaterial == null) {
                    canvas.GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));
                }

                canvas.localRotation = Quaternion.Euler(0, 180, 0);
                canvas.localPosition = Vector3.zero;

                if (canvas.childCount > 0) {
                    foreach (Transform child in canvas.transform) {
                        child.parent = gameObject.transform;
                    }
                }

                return canvas;
            }
        }

        public AbstractPropFrame Frame => gameObject.GetComponent<AbstractPropFrame>();

        public SerializedThumbnail Settings {
            get {
                var settings = GetComponent<SerializedThumbnail>();
                if (settings == null) {
                    settings = gameObject.AddComponent<SerializedThumbnail>();
                }

                settings.hideFlags = HideFlags.HideInInspector;
                return settings;
            }
        }

        public PropComponentUpdatePriority UpdatePriority => PropComponentUpdatePriority.High;

        //--------------------------------------
        // Private Methods
        //--------------------------------------

        private void CheckHierarchy() {
            if (Settings.ScaleMode == ThumbnailScaleMode.DoNotScale) {
                Crop();
            }
            else {
                Resize();
            }
        }

        private void Resize() {
            Canvas.localScale = PropThumbnailScaler.GetScale(Thumbnail.width, Thumbnail.height, Settings.ScaleMode);
            Canvas.GetComponent<Renderer>().sharedMaterial.mainTexture = Thumbnail;
        }


        private void Crop() {
            var ratio = Settings.XRatio / (float) Settings.YRatio;

            var yScale = 1f / ratio;
            Canvas.localScale = new Vector3(1f, yScale, 0.01f);

            Canvas.GetComponent<Renderer>().sharedMaterial.mainTexture = Crop(Thumbnail);
        }

        private Texture2D Crop(Texture2D orTexture) {
            var surfaceAspectRatio = (float) Settings.XRatio / Settings.YRatio;

            var textureRatio = (float) orTexture.width / orTexture.height;

            //print(surfaceAspectRatio + " " + textureRation);

            int x, y, newWidth, newHeight;
            if (surfaceAspectRatio > textureRatio) {
                newWidth = orTexture.width;
                newHeight = (int) (newWidth / surfaceAspectRatio);
                x = 0;
                y = (int) ((orTexture.height - newHeight) * 0.5f);
            }
            else {
                newHeight = orTexture.height;
                newWidth = (int) (newHeight * surfaceAspectRatio);
                x = (int) ((orTexture.width - newWidth) * 0.5f);
                y = 0;
            }

            if (newWidth == 0) {
                newWidth = 1;
            }

            if (newHeight == 0) {
                newHeight = 1;
            }


            var pix = orTexture.GetPixels(x, y, newWidth, newHeight);
            var t = new Texture2D(newWidth, newHeight);
            t.SetPixels(pix);
            t.Apply();

            return t;
        }
    }
}
