using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;


namespace RF.AssetWizzard
{

    [SelectionBase]
    [ExecuteInEditMode]
    public class PropThumbnail : BaseComponent, IPropComponent {



        public bool IsFixedRatio = false;
        public int XRatio = 1;
        public int YRatio = 1;


        public bool IsBoundToResourceIndex = false;
        public int ResourceIndex = 1;



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

            var info = new SerializedThumbnail();
            info.IsFixedRatio = IsFixedRatio;
            info.XRatio = XRatio;
            info.YRatio = YRatio;

            info.IsBoundToResourceIndex = IsBoundToResourceIndex;
            info.ResourceIndex = ResourceIndex;


            DestroyImmediate (Canvas.GetComponent<Renderer> ().sharedMaterial = null);
		
			RemoveSilhouette ();
			DestroyImmediate (this);

        }


        public void Restore(SerializedThumbnail info) {

            IsFixedRatio = info.IsFixedRatio;
            IsBoundToResourceIndex = info.IsBoundToResourceIndex;
            XRatio = info.XRatio;
            YRatio = info.YRatio;

            /*
            PlaceHolderText = info.PlaceHolderText;
            Color = info.Color;
            FontData.font = info.Font;
            FontData.fontSize = info.FontSize;
            FontData.lineSpacing = info.LineSpacing;
            FontData.fontStyle = info.FontStyle;
            FontData.alignment = info.Alignment;
            FontData.horizontalOverflow = info.HorizontalOverflow;
            FontData.verticalOverflow = info.VerticalOverflow;


            Source.DataProvider = info.DataProvider;
            Source.ResourceIndex = info.ResourceIndex;
            Source.ResourceContentSource = info.ResourceContentSource;

    */

            Refresh();

        }



        public void SetFixedRatioMode (bool enabled) {
			if(enabled && !IsFixedRatio) {
				GameObject ratio = new GameObject ("CanvasRatio");
				ratio.transform.parent = transform;
			}

			if(!enabled && IsFixedRatio) {
				GameObject ratio = transform.Find ("CanvasRatio").gameObject;
				DestroyImmediate (ratio);
			}
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

				return canvas;
			}
		}


        public PropBorder Border {
            get {
                return gameObject.GetComponent<PropBorder>();
            }
        }


        //--------------------------------------
        // Private Methods
        //--------------------------------------



        private void CheckhHierarchy() {

			if(IsFixedRatio) {
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
			float ratio = (float) XRatio /  (float) YRatio;

		
			float yScale = 1f / ratio;
			Canvas.localScale = new Vector3 (1f, yScale, 0.01f);

			Canvas.GetComponent<Renderer> ().sharedMaterial.mainTexture = Crop(Thumbnail);
		}


		public Texture2D Crop(Texture2D orTexture) {
			float surfaceAspectRatio = (float) XRatio / YRatio;

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

            if(Border != null) {
                Border.GenerateSilhouette();
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
