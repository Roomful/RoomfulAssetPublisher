using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RF.AssetWizzard {

	#if UNITY_EDITOR
	[ExecuteInEditMode]
	#endif

	public class PropMeshThumbnail : BaseComponent, IPropComponent {



		public int ImageIndex = 0;
		public Texture2D Thumbnail;

		//--------------------------------------
		// Initialisaction
		//--------------------------------------

		void Awake() {
			Thumbnail = Resources.Load ("logo_square") as Texture2D;
		}



		//--------------------------------------
		// Unity Editor
		//--------------------------------------


		#if UNITY_EDITOR
	
		public void Update() {

            if(Canvas.sharedMaterial == null) {
                Canvas.sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));
            }

            if(Canvas.sharedMaterial.mainTexture != Thumbnail) {
                Canvas.sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));
                Canvas.sharedMaterial.mainTexture = Thumbnail;
            }

			GenerateSilhouette ();
		}

		#endif

		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public void PrepareForUpalod() {
			GameObject pointer = new GameObject (AssetBundlesSettings.THUMBNAIL_POINTER);
			pointer.transform.parent = transform;
			pointer.transform.Reset ();
			RemoveSilhouette ();

			DestroyImmediate (Canvas);
			DestroyImmediate (this);
		}


        public void SetResourceIndexBound(bool enabled) {
            if (enabled && !IsBoundToResourceIndex) {
                GameObject obj = new GameObject(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND);
                obj.transform.parent = transform;
            }

            if (!enabled && IsBoundToResourceIndex) {
                GameObject obj = transform.Find(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND).gameObject;
                DestroyImmediate(obj);
            }
        }



        //--------------------------------------
        // Get / Set
        //--------------------------------------


        public Renderer Canvas {
			get {
				MeshRenderer r = gameObject.GetComponent<MeshRenderer> ();

				if (r == null) {
					r = gameObject.AddComponent<MeshRenderer> ();
				}

				return r;
			}
        }

        public bool IsBoundToResourceIndex {
            get {
                return transform.Find(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND) != null;
            }
        }

        public int ResourceIndex {
            get {
                Transform obj = GetResourceIndexBound();
                return System.Convert.ToInt32(obj.GetChild(0).name);
            }

            set {
                Transform obj = GetResourceIndexBound();
                obj.GetChild(0).name = value.ToString();
            }
        }



        //--------------------------------------
        // Private Methods
        //--------------------------------------



        private Transform GetResourceIndexBound() {

            Transform obj = transform.Find(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND);
            if (obj == null) {
                obj = new GameObject(AssetBundlesSettings.THUMBNAIL_RESOURCE_INDEX_BOUND).transform;
                obj.parent = transform;
            }

            if (obj.childCount == 0) {
                new GameObject("0").transform.parent = obj;
            }

            return obj;
        }



        private void GenerateSilhouette() {
            Silhouette.Clear();

            GameObject canvasSilhouette = Instantiate(Canvas.gameObject) as GameObject;
            canvasSilhouette.transform.parent = Silhouette;
            canvasSilhouette.transform.Reset();
            canvasSilhouette.transform.Clear();
          

            canvasSilhouette.AddComponent<SilhouetteCustomMaterial>();


        }



    }



}
