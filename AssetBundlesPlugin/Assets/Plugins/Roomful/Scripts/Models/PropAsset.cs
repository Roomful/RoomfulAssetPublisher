﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard {

	#if UNITY_EDITOR
	[ExecuteInEditMode]
	#endif

	public class PropAsset : MonoBehaviour {
		
		[SerializeField] [HideInInspector]
		private AssetTemplate _Template;
		public float Scale = 1f;
		public bool ShowBounds = true;


		public PropDisplayMode DisplayMode = PropDisplayMode.Normal;

		public Texture2D Icon;
		public Mesh Silhouette;


		private Bounds _Size = new Bounds (Vector3.zero, Vector3.zero);

	

		//--------------------------------------
		// Initialization
		//--------------------------------------

		void Awake () {
			FinalVisualisation ();
		}

		//--------------------------------------
		// Unity Editor
		//--------------------------------------


		#if UNITY_EDITOR

		void Update () {
			PreliminaryVisualisation ();


			CheckhHierarchy ();

		}



		protected virtual void OnDrawGizmos () {
			if (!ShowBounds) {
				return;
			}

			Gizmos.color = Color.blue;
			DrawCube (_Size.center, transform.rotation, _Size.size);
		}

		public static void DrawCube (Vector3 position, Quaternion rotation, Vector3 scale) {
			Matrix4x4 cubeTransform = Matrix4x4.TRS (position, rotation, scale);
			Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

			Gizmos.matrix *= cubeTransform;

			Gizmos.DrawWireCube (Vector3.zero, Vector3.one);

			Gizmos.matrix = oldGizmosMatrix;
		}

		#endif


	
		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public void SynchTemplate () {
			Scale = 1f;
			DisplayMode = PropDisplayMode.Normal;
			Template.SilhouetteMeshData = SilhouetteMeshData;
		}

		public void PrepareForUpload () {
			DestroyImmediate (GetLayer (HierarchyLayers.Silhouette).gameObject);
		}

		public void Refresh() {
			SetTemplate (Template);
		}


		public void SetTemplate (AssetTemplate tpl) {

			_Template = tpl;


			if (!string.IsNullOrEmpty (_Template.SilhouetteMeshData)) {

				GetLayer (HierarchyLayers.Silhouette).Clear ();

	
				GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);
				go.name = "Restored Silhouette Mesh";
				go.transform.parent = GetLayer (HierarchyLayers.Silhouette);
				go.transform.localPosition = Vector3.zero;


				DestroyImmediate (go.GetComponent<BoxCollider> ());


				byte[] bytesToEncode = System.Convert.FromBase64String (_Template.SilhouetteMeshData);
				go.GetComponent<MeshFilter> ().sharedMesh = MeshSerializer.ReadMesh (bytesToEncode);
				go.GetComponent<MeshFilter> ().sharedMesh.name = "Silhouette";  

			}
		}

		public Transform GetLayer (HierarchyLayers layer) {
			Transform hLayer = Model.Find (layer.ToString ());
			if (hLayer == null) {
				GameObject go = new GameObject (layer.ToString ());
				go.transform.parent = Model;
				go.transform.localPosition = Vector3.zero;
				go.transform.localScale = Vector3.one;
				go.transform.localRotation = Quaternion.identity;

				hLayer = go.transform;
			}

			return hLayer;
		}
			

		//--------------------------------------
		// Get / Set
		//--------------------------------------

		public AssetTemplate Template {
			get {
				if (_Template == null) {
					_Template = new AssetTemplate ();
				}

				return _Template;
			}
		}

		public Transform Model {
			get {
				Transform model = transform.Find ("Model");
				if (model == null) {
					GameObject go = new GameObject ("Model");
					go.transform.parent = transform;
					go.transform.localPosition = Vector3.zero;
					go.transform.localScale = Vector3.one;
					go.transform.localRotation = Quaternion.identity;

					model = go.transform;
				}

				return model;
			}
		}


		public GameObject Environment {
			get {

				var rig =  GameObject.Find ("Environment");
				if(rig == null) {
					rig = PrefabManager.CreatePrefab ("Environment");
				}

				rig.transform.SetSiblingIndex (0);
					
				return rig;
			}
		}

		public float MaxAxisValue {
			get {
				float val = Mathf.Max (Size.x, Size.y);
				return  Mathf.Max (val, Size.z);
			}
		}


		public Vector3 Size {
			get {
				return _Size.size / Scale;
			}
		}


		public float MaxScale {
			get {

				if (MaxAxisValue == 0) {
					return 1;
				}
				return Template.MaxSize / MaxAxisValue;
			}
		}

		public float MinScale {
			get {
				if (MaxAxisValue == 0) {
					return 1;
				}
				return Template.MinSize / MaxAxisValue;
			}
		}





		public string SilhouetteMeshData {
			get {


				Vector3 storedPos = transform.position;
				transform.position = Vector3.zero;
				GetLayer (HierarchyLayers.Silhouette).gameObject.SetActive (true);


				MeshFilter[] meshFilters = GetLayer (HierarchyLayers.Silhouette).GetComponentsInChildren<MeshFilter> ();
				CombineInstance[] combine = new CombineInstance[meshFilters.Length];
				int i = 0;
				while (i < meshFilters.Length) {
					combine [i].mesh = meshFilters [i].sharedMesh;
					combine [i].transform = meshFilters [i].transform.localToWorldMatrix;
					i++;
				}

				Mesh m = new Mesh ();
				m.CombineMeshes (combine);
				byte[] array = MeshSerializer.WriteMesh (m);


				GetLayer (HierarchyLayers.Silhouette).gameObject.SetActive (false);
				transform.position = storedPos;
			
				return System.Convert.ToBase64String (array);
			}

		}


		//--------------------------------------
		// Private Methods
		//--------------------------------------


		private void PreliminaryVisualisation () {
			
			foreach (HierarchyLayers layer in System.Enum.GetValues(typeof(HierarchyLayers))) {
				GetLayer (layer).gameObject.SetActive (true);
			}

			GetLayer (HierarchyLayers.Silhouette).gameObject.SetActive (false);
			GetLayer (HierarchyLayers.IgnoredGraphics).gameObject.SetActive (false);





		}


		private void FinalVisualisation () {


			foreach (HierarchyLayers layer in System.Enum.GetValues(typeof(HierarchyLayers))) {
				GetLayer (layer).gameObject.SetActive (true);
			}

			switch(DisplayMode) {

			case PropDisplayMode.Normal:
				GetLayer (HierarchyLayers.Silhouette).gameObject.SetActive (false);
				break;
			case PropDisplayMode.Silhouette:
				foreach (HierarchyLayers layer in System.Enum.GetValues(typeof(HierarchyLayers))) {
					if(layer != HierarchyLayers.Silhouette){
						GetLayer (layer).gameObject.SetActive (false);
					}
				}

				Renderer[] silhouetteRenderers = transform.GetComponentsInChildren<Renderer> ();

				foreach (Renderer r in silhouetteRenderers) {
					if (r.sharedMaterial != null) {
						r.sharedMaterial = new Material (Shader.Find ("Roomful/Silhouette"));
					}
				}


				break;
			}

		}

		public void AutosizeCollider () {


			//_Size = transform.GetRendererBounds ();

			bool hasBounds = false;

			_Size = new Bounds (Vector3.zero, Vector3.zero);
			Renderer[] ChildrenRenderer = GetComponentsInChildren<Renderer> ();

			Quaternion oldRotation = transform.rotation;
			transform.rotation = Quaternion.identity;

			foreach (Renderer child in ChildrenRenderer) {
	
				if (child.transform.IsChildOf (GetLayer (HierarchyLayers.IgnoredGraphics))) {
					continue;
				}

				if (child.transform.IsChildOf (GetLayer (HierarchyLayers.Silhouette))) {
					continue;
				}

				if (!hasBounds) {
					_Size = child.bounds;
					hasBounds = true;
				} else {
					_Size.Encapsulate (child.bounds);
				}
			}

			transform.rotation = oldRotation;

			Template.Size = _Size.size;
		}




		private void CheckhHierarchy () {


			if(Icon == null) {
				Icon = Template.Icon.Thumbnail;
			}


			Model.localPosition = Vector3.zero;
			Model.localScale = Vector3.one * Scale;
			Model.localRotation = Quaternion.identity;

			Environment.transform.parent = null;
			Environment.transform.position = Vector3.zero;
			Environment.transform.rotation = Quaternion.identity;
			Environment.transform.localScale = Vector3.one;



			List<Transform> UndefinedObjects = new List<Transform> ();

			//check undefined chields
			foreach (Transform child in transform) {
				if (child != Model) {
					UndefinedObjects.Add (child);
				}
			}


			foreach (Transform child in Model) {
				if (!AssetHierarchySettings.HierarchyLayers.Contains (child.name)) {
					UndefinedObjects.Add (child);
				}
			}


			//check undefined scene objects
			Transform[] allObjects = UnityEngine.Object.FindObjectsOfType<Transform> ();
			foreach (Transform child in allObjects) {
				if (child == transform) {
					continue;
				}

				if (child == Model) {
					continue;
				}

				if (child == Environment.transform) {
					continue;
				}

				if (child.parent != null) {
					continue;
				}
					
				UndefinedObjects.Add (child);
			}

			foreach (Transform undefined in UndefinedObjects) {
				undefined.position = Vector3.zero;
			}



			if (DisplayMode == PropDisplayMode.Silhouette) {

				foreach (Transform undefined in UndefinedObjects) {
					undefined.parent = GetLayer (HierarchyLayers.Silhouette);
				}
			} else {
				foreach (Transform undefined in UndefinedObjects) {
					undefined.parent = GetLayer (HierarchyLayers.Graphics);
				}
			}


			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;

			if(Template.Placing == Placing.Floor) {
				transform.position = Vector3.zero;

				Vector3 rendererPoint = transform.GetVertex (VertexX.Center, VertexY.Bottom, VertexZ.Center);
				Vector3 diff = transform.position - rendererPoint;
				transform.position += diff;

			}

			if(Template.Placing == Placing.Wall) {
				transform.position = new Vector3 (0, 1.5f, -1.5f);

				Vector3 rendererPoint = transform.GetVertex (VertexX.Center, VertexY.Center, VertexZ.Back);
				Vector3 diff = transform.position - rendererPoint;
				transform.position += diff;
			}


			AutosizeCollider ();
			FinalVisualisation ();

		}




	}
}