using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard {
	
	[ExecuteInEditMode]
	public class Prop : MonoBehaviour {

		public bool DrawBounds = true;
		public bool AutoCollider = true;
		public Color EditorBoundsColor = Color.green;


		private AssetTemplate _Template = new AssetTemplate();

		//--------------------------------------
		// Initialization
		//--------------------------------------

		public void Awake() {
			if (Application.isPlaying) {

				ObjectRigidbody.isKinematic = true;
				ObjectRigidbody.useGravity = false;

				AutosizeCollider();
			}

			ObjectCollider.isTrigger = true;
		}

		public void Update() {
			if (!Application.isPlaying) {
				AutosizeCollider();
			}
		}
		
		//--------------------------------------
		// Unity Editor
		//--------------------------------------

		private void OnDrawGizmos() {
			if (Application.isPlaying || !DrawBounds) {
				return;
			}

			Gizmos.color = EditorBoundsColor;

			Vector3 pos = transform.position;
			pos.x += ObjectCollider.center.x;
			pos.y += ObjectCollider.center.y;
			pos.z += ObjectCollider.center.z;

			//Gizmos.DrawWireCube(ObjectCollider.center, ObjectCollider.size);

			DrawCube (ObjectCollider.center + transform.position, transform.rotation,  ObjectCollider.size);
		}

		public void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale) {
			Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
			Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

			Gizmos.matrix *= cubeTransform;

			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

			Gizmos.matrix = oldGizmosMatrix;
		}

		public void AutosizeCollider() {

			if(!AutoCollider) {
				return;
			}

			bool hasBounds = false;
			Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
			Renderer[] ChildrenRenderer = GetComponentsInChildren<Renderer>();

			Quaternion oldRotation = transform.rotation;
			transform.rotation = Quaternion.identity;

			foreach(Renderer child in ChildrenRenderer) {
//				if(child.GetComponent<BoundsIgnore>() != null) {
//					continue;
//				}

				if(!hasBounds) {
					bounds = child.bounds;
					hasBounds = true;
				} else {
					bounds.Encapsulate(child.bounds);
				}
			}

			transform.rotation = oldRotation;

			ObjectCollider.center = bounds.center - transform.position;
			ObjectCollider.size = bounds.size;

		}

		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public void SetTemplate(AssetTemplate tpl) {
			_Template = tpl;
		}

		//--------------------------------------
		// Get / Set
		//--------------------------------------

		public BoxCollider ObjectCollider {
			get {
				BoxCollider collider = gameObject.GetComponent<BoxCollider>();
				if(collider == null) {
					collider = gameObject.AddComponent<BoxCollider>();
				}

				return collider;
			}
		}

		public Rigidbody ObjectRigidbody {
			get {
				Rigidbody rb = GetComponent<Rigidbody>();
				if(rb == null) {
					rb = gameObject.AddComponent<Rigidbody>();
				}

				return rb;
			}
		}

		public AssetTemplate Template {
			get {
				return _Template;
			}
		}
	}
}