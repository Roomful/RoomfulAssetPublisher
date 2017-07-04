using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RF.AssetWizzard {

	#if UNITY_EDITOR
	[ExecuteInEditMode]
	#endif

	public class PropThumbnail : ExtendedBounds, PropComponent {


		public int ImageIndex = 0;
		public Texture2D Thumbnail;

		public GameObject Corner;
		public GameObject Border;



		//--------------------------------------
		// Initialisaction
		//--------------------------------------

		void Awake() {
			Thumbnail = Resources.Load ("logo_square") as Texture2D;

		
			Transform c = GetLayer (FrameLayers.BorderParts).Find ("Corner");
			if(c != null) {
				Corner = c.gameObject;
			}

			Transform b = GetLayer (FrameLayers.BorderParts).Find ("Border");
			if(b != null) {
				Border = b.gameObject;
			}


			#if UNITY_EDITOR
			Update ();
			#endif
		}



		//--------------------------------------
		// Unity Editor
		//--------------------------------------


		#if UNITY_EDITOR
	
		public void Update() {
			CheckhHierarchy ();
			GenerateFrame ();
		}

		#endif

		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public void PrepareForUpalod() {
			if(Border != null) {
				Border.SetActive (true);
			}

			if(Corner != null) {
				Corner.SetActive (true);
			}

			DestroyImmediate (GetLayer (FrameLayers.GeneratedBorder).gameObject);
			DestroyImmediate (Canvas.GetComponent<Renderer> ().sharedMaterial = null);
		
			RemoveSilhouette ();
			DestroyImmediate (this);
		}

		public void RemoveSilhouette() {
			DestroyImmediate (Silhouette.gameObject);
		}


		public Transform GetLayer(FrameLayers layer) {
			Transform hLayer = transform.Find (layer.ToString ());
			if(hLayer == null) {
				GameObject go = new GameObject (layer.ToString());
				hLayer = go.transform;
			} 

			hLayer.parent = transform;
			hLayer.localPosition = Vector3.zero;
			hLayer.localRotation = Quaternion.identity;
			hLayer.localScale = Vector3.one;

			return hLayer;
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


		//--------------------------------------
		// Public Methods
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

		public Transform Silhouette {
			get {
				Transform silhouette = Prop.GetLayer (HierarchyLayers.Silhouette).Find (gameObject.GetInstanceID ().ToString());
				if(silhouette == null) {
					silhouette = new GameObject (gameObject.GetInstanceID ().ToString()).transform;
					silhouette.parent = Prop.GetLayer (HierarchyLayers.Silhouette);
				}

				silhouette.localPosition = transform.localPosition;
				silhouette.localRotation = transform.localRotation;
				silhouette.localScale = transform.localScale;

				return silhouette;
			}
		}

		public PropAsset Prop {
			get {
				return GameObject.FindObjectOfType<PropAsset> ();
			}
		}


		public bool IsFixedRatio {
			get {
				return transform.Find ("CanvasRatio") != null;
			}
		}

		public int XRatio {
			get {
				Transform ratio = GetCanvasRatio ();
				return System.Convert.ToInt32 (ratio.GetChild(0).name);
			}

			set {
				Transform ratio = GetCanvasRatio ();
				ratio.GetChild (0).name = value.ToString ();
			}
		}

		public int YRatio {
			get {
				Transform ratio = GetCanvasRatio ();
				return System.Convert.ToInt32 (ratio.GetChild(1).name);
			}

			set {
				Transform ratio = GetCanvasRatio ();
				ratio.GetChild (1).name = value.ToString ();
			}
		}



		//--------------------------------------
		// Private Methods
		//--------------------------------------

		private Transform GetCanvasRatio() {

			Transform ratio = transform.Find ("CanvasRatio");
			if(ratio ==  null) {
				ratio = new GameObject ("CanvasRatio").transform;
				ratio.parent = transform;
			}

			if(ratio.childCount == 0) {
				new GameObject ("1").transform.parent = ratio;
				new GameObject ("1").transform.parent = ratio;
			}

			if (ratio.childCount == 1) {
				new GameObject ("1").transform.parent = ratio;
			}

			return ratio;

		}



		private void CheckhHierarchy() {
			transform.parent = Prop.GetLayer (HierarchyLayers.Thumbnails);

		
			if(IsFixedRatio) {
				Crop ();
			} else {
				Resize ();
			}


			if(Corner != null) {
				Corner.transform.parent = GetLayer (FrameLayers.BorderParts);
				Corner.gameObject.SetActive (false);
				Corner.gameObject.name = "Corner";

				if(Corner == Border) { Border = null; }
			}


			if(Border != null) {
				
				Border.transform.parent = GetLayer (FrameLayers.BorderParts);
				Border.gameObject.SetActive (false);
				Border.gameObject.name = "Border";
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




		private void GenerateFrame() {
			if(Border != null && Corner != null) {

				Transform GeneratedBorder = GetLayer (FrameLayers.GeneratedBorder);

				// remove all chields from GeneratedBorder
				var children = new List<GameObject>();
				foreach (Transform child in GeneratedBorder) children.Add(child.gameObject);
				children.ForEach(child => DestroyImmediate(child));


				GameObject corner_left_top = InstantiateBorderPart (Corner.gameObject); 
				PutObjectAt (corner_left_top, VertexX.Left, VertexY.Top, VertexX.Right, VertexY.Bottom);


				GameObject corner_right_top = InstantiateBorderPart (Corner.gameObject); 
				corner_right_top.transform.Rotate (Vector3.forward, 90f);
				PutObjectAt (corner_right_top, VertexX.Right, VertexY.Top, VertexX.Left, VertexY.Bottom);



				GameObject corner_right_bottom = InstantiateBorderPart (Corner.gameObject); 
				corner_right_bottom.transform.Rotate (Vector3.forward, 180);
				PutObjectAt (corner_right_bottom, VertexX.Right, VertexY.Bottom, VertexX.Left, VertexY.Top);

				GameObject corner_left_bottom = InstantiateBorderPart (Corner.gameObject); 
				corner_left_bottom.transform.Rotate (Vector3.forward, 270);
				PutObjectAt (corner_left_bottom, VertexX.Left, VertexY.Bottom, VertexX.Right, VertexY.Top);




				GameObject border_top = InstantiateBorderPart (Border.gameObject); 


				float canvasW = Canvas.GetComponent<Renderer> ().bounds.extents.x;
				float borderW = border_top.GetRendererBounds ().extents.x;

				float canvasH = Canvas.GetComponent<Renderer> ().bounds.extents.y;
		


				float scaleX = canvasW / borderW;
				float scaleY = canvasH / borderW;

				Vector3 orogonalScale = new Vector3 (border_top.transform.localScale.x, border_top.transform.localScale.y, border_top.transform.localScale.z);
				Vector3 xScale = new Vector3 (orogonalScale.x * scaleX, orogonalScale.y, orogonalScale.z);
				Vector3 yScale = new Vector3 (orogonalScale.x * scaleY, orogonalScale.y, orogonalScale.z);


				border_top.transform.localScale = xScale;
				PutObjectAt (border_top, VertexX.Left, VertexY.Top, VertexX.Left, VertexY.Bottom);


				GameObject border_bottom = InstantiateBorderPart (Border.gameObject); 
				border_bottom.transform.localScale = xScale;
				border_bottom.transform.Rotate (Vector3.forward, 180);
				PutObjectAt (border_bottom, VertexX.Left, VertexY.Bottom, VertexX.Left, VertexY.Top);

			

				GameObject border_right = InstantiateBorderPart (Border.gameObject); 
				border_right.transform.localScale = yScale;
				border_right.transform.Rotate (Vector3.forward, 90);
				PutObjectAt (border_right, VertexX.Right, VertexY.Top, VertexX.Left, VertexY.Top);



				GameObject border_left = InstantiateBorderPart (Border.gameObject); 
				border_left.transform.localScale = yScale;
				border_left.transform.Rotate (Vector3.forward, 270);
				PutObjectAt (border_left, VertexX.Left, VertexY.Top, VertexX.Right, VertexY.Top);


				GenerateSilhouette ();



			} else {
				GenerateSilhouette ();
				DestroyImmediate (GetLayer (FrameLayers.GeneratedBorder).gameObject);
			}
		}


		private void GenerateSilhouette() {
			Silhouette.Clear ();

			if (Border != null && Corner != null) {
				Transform GeneratedBorder = GetLayer (FrameLayers.GeneratedBorder);
				GameObject borderSilhouette = Instantiate (GeneratedBorder.gameObject) as GameObject;
				borderSilhouette.transform.parent = Silhouette;
				borderSilhouette.Reset ();
			}


			GameObject canvasSilhouette = Instantiate (Canvas.gameObject) as GameObject;
			canvasSilhouette.transform.parent = Silhouette;
			canvasSilhouette.transform.Reset ();
			canvasSilhouette.transform.Clear ();
			canvasSilhouette.transform.localScale = Canvas.localScale;
			canvasSilhouette.transform.localRotation = Canvas.localRotation;
			canvasSilhouette.AddComponent<SilhouetteCustomMaterial> ();
		}


		private GameObject InstantiateBorderPart(GameObject reference) {
			GameObject p = Instantiate (reference) as GameObject;
			p.SetActive (true);
			p.transform.parent = GetLayer (FrameLayers.GeneratedBorder);
			p.transform.localScale = reference.transform.localScale;
			return p;
		}

		private void PutObjectAt(GameObject obj, VertexX CanvasVertexX , VertexY CanvasVertexY, VertexX ObjectVertexX , VertexY ObjectVertexY) {
			obj.transform.position = Canvas.GetComponent<Renderer> ().bounds.GetVertex(CanvasVertexX, CanvasVertexY, VertexZ.Front);

			Vector3 rendererPoint = obj.GetVertex (ObjectVertexX, ObjectVertexY, VertexZ.Back);
			Vector3 diff = obj.transform.position - rendererPoint;
			obj.transform.position += diff;
		}




		#if UNITY_EDITOR
		private void CheckSelection() {
			Debug.Log (UnityEditor.Selection.activeGameObject.name);
			if(UnityEditor.Selection.activeGameObject == Canvas.gameObject) {
				UnityEditor.Selection.activeGameObject = gameObject;
			}
		}

		#endif


	}



}
