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

		private Bounds Size = new Bounds(Vector3.zero, Vector3.zero);
	

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
			DrawCube (Size.center, transform.rotation,  Size.size);
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
			Transform hLayer = transform.Find (layer.ToString ());
			if(hLayer == null) {
				GameObject go = new GameObject (layer.ToString());
				go.transform.parent = transform;
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


		//--------------------------------------
		// Private Methods
		//--------------------------------------

		public void AutosizeCollider() {

			bool hasBounds = false;
			Size = new Bounds(Vector3.zero, Vector3.zero);
			Renderer[] ChildrenRenderer = GetComponentsInChildren<Renderer>();

			Quaternion oldRotation = transform.rotation;
			transform.rotation = Quaternion.identity;

			foreach(Renderer child in ChildrenRenderer) {
	
				if(child.transform.IsChildOf(GetLayer (HierarchyLayers.IgnoredGraphics))) {
					continue;
				}

				if(!hasBounds) {
					Size = child.bounds;
					hasBounds = true;
				} else {
					Size.Encapsulate(child.bounds);
				}
			}

			transform.rotation = oldRotation;

			Template.Size = Size.size;
		}


		private void CheckhHierarchy() {

			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;

			List<Transform> UndefinedObjects = new List<Transform> ();

			//check undefined chields
			foreach(Transform child in transform) {
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