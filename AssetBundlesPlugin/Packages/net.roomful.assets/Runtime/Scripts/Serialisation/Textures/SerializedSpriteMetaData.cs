using UnityEngine;

namespace net.roomful.assets.serialization
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