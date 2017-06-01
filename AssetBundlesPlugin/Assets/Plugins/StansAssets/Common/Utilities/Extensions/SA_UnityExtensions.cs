////////////////////////////////////////////////////////////////////////////////
//  
// @module Assets Common Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;


public static class SA_UnityExtensions  {

	//--------------------------------------
	// GameObject
	//--------------------------------------


	public static void MoveTo(this GameObject go, Vector3 position, float time, SA.Common.Animation.EaseType easeType = SA.Common.Animation.EaseType.linear, System.Action OnCompleteAction = null ) {
		SA.Common.Animation.ValuesTween tw = go.AddComponent<SA.Common.Animation.ValuesTween>();

		tw.DestoryGameObjectOnComplete = false;
		tw.VectorTo(go.transform.position, position, time, easeType);

		tw.OnComplete += OnCompleteAction;
	}


	public static Bounds GetRendererBounds(this Transform t) {
		return t.gameObject.GetRendererBounds ();
	}


	public static Vector3 GetVertex(this Transform t, VertexX x, VertexY y, VertexZ z) {
		return t.gameObject.GetVertex (x, y, z);
	}


	public static Bounds GetRendererBounds(this GameObject go) {
		return CalculateBounds(go);
	}
		

	public static Vector3 GetVertex(this GameObject go, VertexX x, VertexY y, VertexZ z) {

		Bounds bounds = go.GetRendererBounds ();
		return bounds.GetVertex (x, y, z);
	}
		

	//--------------------------------------
	// Transaform
	//--------------------------------------

	public static Transform Clear(this Transform transform) {

		Transform[] children = transform.GetComponentsInChildren<Transform> ();

		foreach (Transform child in children) {
			if(child != transform) {
				GameObject.DestroyImmediate(child.gameObject);
			}
		}
		return transform;
	}



	public static Vector3 GetVertex(this Bounds bounds, VertexX x, VertexY y, VertexZ z) {


		Vector3 center = bounds.center;

		switch(x){
		case VertexX.Right:
			center.x -= bounds.extents.x;
			break;
		case VertexX.Left:
			center.x += bounds.extents.x;
			break;
		}


		switch(y) {
		case VertexY.Bottom:
			center.y -= bounds.extents.y;
			break;

		case VertexY.Top:
			center.y += bounds.extents.y;
			break;
		}

		switch(z) {
		case VertexZ.Back:
			center.z -= bounds.extents.z;
			break;

		case VertexZ.Front:
			center.z += bounds.extents.z;
			break;
		}

		return center;
	}




	public static Bounds CalculateBounds(GameObject obj) {

		bool hasBounds = false;
		Bounds Bounds = new Bounds(Vector3.zero, Vector3.zero);
		Renderer[] ChildrenRenderer = obj.GetComponentsInChildren<Renderer>();

		//Quaternion oldRotation = obj.transform.rotation;
		//obj.transform.rotation = Quaternion.identity;

		Renderer rnd = obj.GetComponent<Renderer> ();
		if(rnd != null) {
			Bounds = rnd.bounds;
			hasBounds = true;
		}

		foreach(Renderer child in ChildrenRenderer) {

			if(!hasBounds) {
				Bounds = child.bounds;
				hasBounds = true;
			} else {
				Bounds.Encapsulate(child.bounds);
			}
		}

		//obj.transform.rotation = oldRotation;

		return Bounds;
	}





	public static void ScaleTo(this GameObject go, Vector3 scale, float time, SA.Common.Animation.EaseType easeType = SA.Common.Animation.EaseType.linear, System.Action OnCompleteAction = null ) {
		SA.Common.Animation.ValuesTween tw = go.AddComponent<SA.Common.Animation.ValuesTween>();

		tw.DestoryGameObjectOnComplete = false;
		tw.ScaleTo(go.transform.localScale, scale, time, easeType);

		tw.OnComplete += OnCompleteAction;
	}

	//--------------------------------------
	// Texture2D
	//--------------------------------------

	public static Sprite ToSprite(this Texture2D texture) {
		return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f)); 
	}

	//--------------------------------------
	// Vector3
	//--------------------------------------

	public static Vector3 ResetXCoord(this Vector3 vec) {
		Vector3 newVec = vec;
		newVec.x = 0f;

		return newVec;
	}

	public static Vector3 ResetYCoord(this Vector3 vec) {
		Vector3 newVec = vec;
		newVec.y = 0f;

		return newVec;
	}

	public static Vector3 ResetZCoord(this Vector3 vec) {
		Vector3 newVec = vec;
		newVec.z = 0f;

		return newVec;
	}



		

}


