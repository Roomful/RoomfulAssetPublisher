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

		//--------------------------------------
		// Initialization
		//--------------------------------------

		//--------------------------------------
		// Unity Editor
		//--------------------------------------


		void Update() {
			CheckhHierarchy ();
		}

	
		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public void SetTemplate(AssetTemplate tpl) {
			_Template = tpl;
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


		private void CheckhHierarchy() {

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