using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetBundles.Serialization;


namespace RF.AssetWizzard {

	[ExecuteInEditMode]
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


		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public void PrepareForUpalod() {
			
			RemoveSilhouette ();

			DestroyImmediate (Canvas);
			DestroyImmediate (this);
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

		public SerializedMeshThumbnail Settings {
			get {

				var settings = GetComponent<SerializedMeshThumbnail> ();
				if(settings == null) {
					settings = gameObject.AddComponent<SerializedMeshThumbnail> ();
				}

				settings.hideFlags = HideFlags.HideInInspector;

				return settings;
			}
		}

      



        //--------------------------------------
        // Private Methods
        //--------------------------------------


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
