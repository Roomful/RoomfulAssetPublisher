using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard {
	
public class ExtendedBounds : MonoBehaviour {


		private Bounds Bounds = new Bounds(Vector3.zero, Vector3.zero);



		public Vector3 GetVertex(VertexX x, VertexY y, VertexZ z) {

			CalculateBounds ();

			Vector3 center = Bounds.center;

			switch(x){
			case VertexX.Right:
				center.x -= Bounds.extents.x;
				break;
			case VertexX.Left:
				center.x += Bounds.extents.x;
				break;
			}


			switch(y) {
			case VertexY.Bottom:
				center.y -= Bounds.extents.y;
				break;

			case VertexY.Top:
				center.y += Bounds.extents.y;
				break;
			}

			switch(z) {
			case VertexZ.Back:
				center.z -= Bounds.extents.z;
				break;

			case VertexZ.Front:
				center.z += Bounds.extents.z;
				break;
			}

			return Vector3.zero;
		}





		public void CalculateBounds() {

			bool hasBounds = false;
			Bounds = new Bounds(Vector3.zero, Vector3.zero);
			Renderer[] ChildrenRenderer = GetComponentsInChildren<Renderer>();

			Quaternion oldRotation = transform.rotation;
			transform.rotation = Quaternion.identity;

			foreach(Renderer child in ChildrenRenderer) {

				if(!hasBounds) {
					Bounds = child.bounds;
					hasBounds = true;
				} else {
					Bounds.Encapsulate(child.bounds);
				}
			}

			transform.rotation = oldRotation;
		}


	}

}