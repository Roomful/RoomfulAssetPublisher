using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetBundles.Serialization
{
	
	[System.Serializable]
	public class SerializedSpriteMetaData {
		public string 	Name;
		public Rect 	Rect;
		public int 		Alignment;
		public Vector2 	Pivot;
		public Vector4 	Border;
	}
}