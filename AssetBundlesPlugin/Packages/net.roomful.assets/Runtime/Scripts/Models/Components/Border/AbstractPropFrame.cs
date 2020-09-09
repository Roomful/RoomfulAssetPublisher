
using StansAssets.Foundation.Extensions;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace net.roomful.assets
{
    public abstract class AbstractPropFrame : BaseComponent, IPropComponent
    {

        public GameObject Corner;
        public GameObject Border;
        public GameObject Back;

        protected const string CORNER_NAME = "Corner";
        protected const string BORDER_NAME = "Border";
        protected const string BACK_NAME = "Back";


        protected virtual void Awake() {

            var c = GetLayer(BorderLayers.BorderParts).Find(CORNER_NAME);
            if (c != null) {
                Corner = c.gameObject;
            }

            var b = GetLayer(BorderLayers.BorderParts).Find(BORDER_NAME);
            if (b != null) {
                Border = b.gameObject;
            }


            var back = GetLayer(BorderLayers.BorderParts).Find(BACK_NAME);
            if (back != null) {
                Back = back.gameObject;
            }

            UpdateFrame();
        }


		public void OnDestroy() {
			DestroyImmediate (GetLayer (BorderLayers.GeneratedBorder).gameObject);
		}

        public void UpdateFrame() {
            CheckhHierarchy();
            GenerateFrame();

            if(Prop != null) {
                Prop.Update();
            }
        }

        protected abstract void GenerateFrame();

        public void PrepareForUpload() {

            if (Border != null) {
                Border.SetActive(true);
            }

            if (Corner != null) {
                Corner.SetActive(true);
            }

            if (Back != null) {
                Back.SetActive(true);
            }


            DestroyImmediate(GetLayer(BorderLayers.GeneratedBorder).gameObject);
            DestroyImmediate(this);

        }
        
        protected Transform GetLayer(BorderLayers layer) {
            var hLayer = transform.Find(layer.ToString());
            if (hLayer == null) {
                var go = new GameObject(layer.ToString());
                hLayer = go.transform;
            }

            hLayer.parent = transform;
            hLayer.localPosition = Vector3.zero;
            hLayer.localRotation = Quaternion.identity;
            hLayer.localScale = Vector3.one;

            return hLayer;
        }



        protected Bounds Bounds {
            get {

  

                foreach (BorderLayers layer in System.Enum.GetValues(typeof(BorderLayers))) {
                    GetLayer(layer).gameObject.SetActive(false);
                }


                var bounds = Scene.GetBounds(gameObject);

                foreach (BorderLayers layer in System.Enum.GetValues(typeof(BorderLayers))) {
                    GetLayer(layer).gameObject.SetActive(true);
                }

                return bounds;
            }
        }

        public Priority UpdatePriority => Priority.Medium;

        protected abstract void CheckhHierarchy();

        public abstract void SetBackOffset(float offset);

        private static PropAsset CurrentProp => FindObjectOfType<PropAsset>();

        private GameObject InstantiateBorderPart(GameObject reference) {
            var p = Instantiate(reference);
            p.SetActive(true);
            p.transform.parent = GetLayer(BorderLayers.GeneratedBorder);
            p.transform.localScale = reference.transform.localScale / CurrentProp.Scale;
            p.transform.localRotation = Quaternion.identity;
            return p;
        }


       
        public bool IsPersistent(GameObject go) {
#if UNITY_EDITOR
            return EditorUtility.IsPersistent(gameObject);
#else
            return false;
#endif
        }


        public void Update() {

        }
    }
}