using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RF.AssetWizzard {

	#if UNITY_EDITOR
	[ExecuteInEditMode]
	#endif

	public class PropThumbnail : ExtendedBounds {


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

			/*UnityEditor.Selection.selectionChanged += () => {
				if(this == null || gameObject == null) {return;}
				CheckSelection();
			};*/
				
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
			DestroyImmediate (Canvas.gameObject);
			DestroyImmediate (this);
		}


		public Transform GetLayer(FrameLayers layer) {
			Transform hLayer = transform.Find (layer.ToString ());
			if(hLayer == null) {
				GameObject go = new GameObject (layer.ToString());
				go.transform.parent = transform;
				go.transform.localPosition = Vector3.zero;
				go.transform.localRotation = Quaternion.identity;

				hLayer = go.transform;
			}

			return hLayer;
		}


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

				return canvas;
			}
		}

		public PropAsset Prop {
			get {
				return GameObject.FindObjectOfType<PropAsset> ();
			}
		}



		//--------------------------------------
		// Private Methods
		//--------------------------------------

		private void CheckhHierarchy() {
			transform.parent = Prop.GetLayer (HierarchyLayers.Thumbnails);


			if(Canvas.GetComponent<Renderer> ().sharedMaterial == null) {
				Canvas.GetComponent<Renderer> ().sharedMaterial = new Material (Shader.Find ("Unlit/Transparent"));
			}

			Canvas.GetComponent<Renderer> ().sharedMaterial.mainTexture = Thumbnail;


			float ratio;
			if(Thumbnail.width > Thumbnail.height) {
				ratio = (float)Thumbnail.height / (float)Thumbnail.width;
				Canvas.localScale = new Vector3 (1f,1f * ratio, 0.01f);
			} else {
				ratio = (float) Thumbnail.width / (float) Thumbnail.height;
				Canvas.localScale = new Vector3 (1f * ratio, 1f , 0.01f);
			}
			Canvas.localRotation = Quaternion.Euler (0, 180, 0);




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



			} else {
				DestroyImmediate (GetLayer (FrameLayers.GeneratedBorder).gameObject);
			}
		}


		private GameObject InstantiateBorderPart(GameObject reference) {
			GameObject p = Instantiate (reference) as GameObject;
			p.SetActive (true);
			p.transform.parent = GetLayer (FrameLayers.GeneratedBorder);
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
