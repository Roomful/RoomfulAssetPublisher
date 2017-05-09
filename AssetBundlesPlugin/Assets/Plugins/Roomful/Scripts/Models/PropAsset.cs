using System.Collections;
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


		private Bounds _Size = new Bounds(Vector3.zero, Vector3.zero);

	

		//--------------------------------------
		// Initialization
		//--------------------------------------

		void Awake() {
			if(_Template != null && _Template.Thumbnail == null) {
				_Template.RestoreThumbnail ();
			}
		}

		//--------------------------------------
		// Unity Editor
		//--------------------------------------


		#if UNITY_EDITOR

		void Update() {
			CheckhHierarchy ();
			AutosizeCollider ();
		}



		protected virtual void OnDrawGizmos() {
			Gizmos.color = Color.blue;
			DrawCube (_Size.center, transform.rotation,  _Size.size);
		}
			
		public static void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale) {
			Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
			Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

			Gizmos.matrix *= cubeTransform;

			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

			Gizmos.matrix = oldGizmosMatrix;
		}

		#endif


	
		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public void SetTemplate(AssetTemplate tpl) {

			_Template = tpl;

			if (_Template.Thumbnail == null) {
				_Template.RestoreThumbnail ();
			}
		}

		public Transform GetLayer(HierarchyLayers layer) {
			Transform hLayer = Model.Find (layer.ToString ());
			if(hLayer == null) {
				GameObject go = new GameObject (layer.ToString());
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
				if(model == null) {
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
			

		public Light DirectionalLight {
			get {
				Light light = GameObject.FindObjectOfType<Light> ();
				if(light == null) {
					GameObject go = new GameObject ("Directional light");
					light = go.AddComponent<Light> ();
				}
					
				light.transform.parent = null;
				light.transform.position = Vector3.one;
				light.transform.SetSiblingIndex (0);

				return light;
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
			get{

				if(MaxAxisValue == 0) {
					return 1;
				}
				return Template.MaxSize / MaxAxisValue;
			}
		}

		public float MinScale {
			get{
				if(MaxAxisValue == 0) {
					return 1;
				}
				return Template.MinSize / MaxAxisValue;
			}
		}



		//--------------------------------------
		// Private Methods
		//--------------------------------------

		public void AutosizeCollider() {

			bool hasBounds = false;
			_Size = new Bounds(Vector3.zero, Vector3.zero);
			Renderer[] ChildrenRenderer = GetComponentsInChildren<Renderer>();

			Quaternion oldRotation = transform.rotation;
			transform.rotation = Quaternion.identity;

			foreach(Renderer child in ChildrenRenderer) {
	
				if(child.transform.IsChildOf(GetLayer (HierarchyLayers.IgnoredGraphics))) {
					continue;
				}

				if(!hasBounds) {
					_Size = child.bounds;
					hasBounds = true;
				} else {
					_Size.Encapsulate(child.bounds);
				}
			}

			transform.rotation = oldRotation;

			Template.Size = _Size.size;
		}


		private void CheckhHierarchy() {

			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;


			Model.localPosition = Vector3.zero;
			Model.localScale = Vector3.one * Scale;
			Model.localRotation = Quaternion.identity;



			List<Transform> UndefinedObjects = new List<Transform> ();

			//check undefined chields
			foreach(Transform child in transform) {
				if (child != Model) {
					UndefinedObjects.Add (child);
				}
			}


			foreach(Transform child in Model) {
				if (!AssetHierarchySettings.HierarchyLayers.Contains (child.name)) {
					UndefinedObjects.Add (child);
				}
			}


			//check undefined scene objects
			Transform[] allObjects = UnityEngine.Object.FindObjectsOfType<Transform>();
			foreach(Transform child in allObjects) {
				if(child == transform) {
					continue;
				}

				if(child == Model) {
					continue;
				}

				if(child == DirectionalLight.transform) {
					continue;
				}

				if(child.parent != null) {
					continue;
				}
					
				UndefinedObjects.Add (child);
			}


			foreach(Transform undefined in UndefinedObjects) {
				undefined.parent = GetLayer (HierarchyLayers.Graphics);
			}

		}
	}
}