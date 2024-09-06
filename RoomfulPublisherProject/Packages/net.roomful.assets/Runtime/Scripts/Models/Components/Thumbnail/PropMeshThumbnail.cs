using UnityEngine;
using net.roomful.assets.serialization;
using StansAssets.Foundation.Extensions;

namespace net.roomful.assets
{
    [ExecuteInEditMode]
    internal class PropMeshThumbnail : BaseComponent, IPropPublihserComponent
    {
        public int ImageIndex = 0;
        public Texture2D Thumbnail;

        //--------------------------------------
        // Initialisation
        //--------------------------------------

        void Awake() {
            Thumbnail = Resources.Load("logo_square") as Texture2D;
        }

        //--------------------------------------
        // Unity Editor
        //--------------------------------------

        public void Update() {
            if (Canvas.sharedMaterial == null) {
                Canvas.sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));
            }

            if (Canvas.sharedMaterial.mainTexture != Thumbnail) {
                Canvas.sharedMaterial = new Material(Canvas.sharedMaterial);
                Canvas.sharedMaterial.name = gameObject.name;
                Canvas.sharedMaterial.mainTexture = Thumbnail;
            }
        }

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public void PrepareForUpload() {
            Canvas.sharedMaterial.mainTexture = null;
            DestroyImmediate(this);
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public Renderer Canvas {
            get {
                var r = gameObject.GetComponent<MeshRenderer>();

                if (r == null) {
                    r = gameObject.AddComponent<MeshRenderer>();
                }

                return r;
            }
        }

        public SerializedMeshThumbnail Settings {
            get {
                var settings = GetComponent<SerializedMeshThumbnail>();
                if (settings == null) {
                    settings = gameObject.AddComponent<SerializedMeshThumbnail>();
                }

                settings.hideFlags = HideFlags.HideInInspector;
                return settings;
            }
        }

        public PropComponentUpdatePriority UpdatePriority => PropComponentUpdatePriority.High;

    }
}
