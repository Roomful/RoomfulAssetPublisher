using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;


namespace RF.AssetWizzard
{

    [SelectionBase]
    [ExecuteInEditMode]
    public class PropThumbnail : BaseComponent, IPropComponent {



        public int ImageIndex = 0;
		public Texture2D Thumbnail;

		//--------------------------------------
		// Initialisaction
		//--------------------------------------

		void Awake() {
			Thumbnail = Resources.Load ("logo_square") as Texture2D;
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

        public void Refresh() {
            CheckhHierarchy();
            GenerateSilhouette();
        }

		public void PrepareForUpalod() {

            DestroyImmediate (Canvas.gameObject);
			DestroyImmediate (this);
        }
			

        public void SetThumbnail(Texture2D newTex) {
			Thumbnail = newTex;
			Canvas.GetComponent<Renderer>().sharedMaterial =  new Material (Shader.Find ("Unlit/Transparent")); 
			Canvas.GetComponent<Renderer> ().sharedMaterial.mainTexture = Thumbnail;
		}


		//--------------------------------------
		// Get / Set
		//--------------------------------------


		public Transform Canvas {
			get {
				Transform canvas = transform.Find ("Canvas");
				if (canvas == null) {
					GameObject c = GameObject.CreatePrimitive (PrimitiveType.Quad); 

					canvas = c.transform;

					canvas.name = "Canvas";
					canvas.parent = transform;
					canvas.GetComponent<Renderer> ().sharedMaterial = new Material (Shader.Find ("Unlit/Transparent"));

				}

				if(canvas.GetComponent<Renderer> ().sharedMaterial == null) {
					canvas.GetComponent<Renderer> ().sharedMaterial = new Material (Shader.Find ("Unlit/Transparent"));
				}

				canvas.localRotation = Quaternion.Euler (0, 180, 0);
				canvas.localPosition = Vector3.zero;

                if(canvas.childCount > 0) {
                    foreach(Transform child in canvas.transform) {
                        child.parent = gameObject.transform;
                    }
                }



				return canvas;
			}
		}


        public AbstractPropFrame Frame {
            get {
                return gameObject.GetComponent<AbstractPropFrame>();
            }
        }

		public SerializedThumbnail Settings {
			get {

				var settings = GetComponent<SerializedThumbnail> ();
				if(settings == null) {
					settings = gameObject.AddComponent<SerializedThumbnail> ();
				}

				settings.hideFlags = HideFlags.HideInInspector;

				return settings;
			}
		}


        public Priority UpdatePriority {
            get {
                return Priority.High;
            }
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------



        private void CheckhHierarchy() {

			if(Settings.IsFixedRatio) {
				Crop ();
			} else {
				Resize ();
			}
		}


		private void Resize() {
			float ratio;
			if(Thumbnail.width > Thumbnail.height) {
				ratio = (float)Thumbnail.height / (float)Thumbnail.width;
				Canvas.localScale = new Vector3 (1f,1f * ratio, 0.01f);
			} else {
				ratio = (float) Thumbnail.width / (float) Thumbnail.height;
				Canvas.localScale = new Vector3 (1f * ratio, 1f , 0.01f);
			}

			Canvas.GetComponent<Renderer> ().sharedMaterial.mainTexture = Thumbnail;
		}


		private void Crop() {
			float ratio = (float) Settings.XRatio /  (float) Settings.YRatio;

		
			float yScale = 1f / ratio;
			Canvas.localScale = new Vector3 (1f, yScale, 0.01f);

			Canvas.GetComponent<Renderer> ().sharedMaterial.mainTexture = Crop(Thumbnail);
		}


		public Texture2D Crop(Texture2D orTexture) {
			float surfaceAspectRatio = (float) Settings.XRatio / Settings.YRatio;

			float textureRatio = (float) orTexture.width / orTexture.height;

            //print(surfaceAspectRatio + " " + textureRation);

			int x, y, newWidth, newHeight;
			if(surfaceAspectRatio > textureRatio) {
				newWidth = orTexture.width;
				newHeight = (int)(newWidth / surfaceAspectRatio);
				x = 0;
				y = (int)((orTexture.height - newHeight) * 0.5f);
			} else {
				newHeight = orTexture.height;
				newWidth = (int)(newHeight * surfaceAspectRatio);
				x = (int)((orTexture.width - newWidth) * 0.5f);
				y = 0;
			}


			if(newWidth == 0) {
				newWidth = 1;
			}

			if(newHeight == 0) {
				newHeight = 1;
			}


            //print(orTexture.width + " " + newWidth + " " + x);
            //print(orTexture.height + " " + newHeight + " " + y);

			var pix = orTexture.GetPixels(x, y, newWidth, newHeight);
			var t = new Texture2D(newWidth, newHeight);
			t.SetPixels(pix);
			t.Apply();


			return t;
		}


		private void GenerateSilhouette() {
			Silhouette.Clear ();

			GameObject canvasSilhouette = Instantiate (Canvas.gameObject) as GameObject;
			canvasSilhouette.transform.parent = Silhouette;
			canvasSilhouette.transform.Reset ();
			canvasSilhouette.transform.Clear ();
			canvasSilhouette.transform.localScale = Canvas.localScale;
			canvasSilhouette.transform.localRotation = Canvas.localRotation;
			canvasSilhouette.AddComponent<SilhouetteCustomMaterial> ();

            if(Frame != null) {
                Frame.GenerateSilhouette();
            }

		}


		#if UNITY_EDITOR
		private void CheckSelection() {
			if(UnityEditor.Selection.activeGameObject == Canvas.gameObject) {
				UnityEditor.Selection.activeGameObject = gameObject;
			}
		}

		#endif

	}


}
