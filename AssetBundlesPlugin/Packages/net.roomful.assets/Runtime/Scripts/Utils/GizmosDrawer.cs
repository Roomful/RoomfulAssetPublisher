using UnityEngine;


namespace net.roomful.assets {
	
	public static class GizmosDrawer {

		public static void DrawCube (Vector3 position, Quaternion rotation, Vector3 scale, Color color) {

			Gizmos.color = color; 

			var cubeTransform = Matrix4x4.TRS (position, rotation, scale);
			var oldGizmosMatrix = Gizmos.matrix;

			Gizmos.matrix *= cubeTransform;

			Gizmos.DrawWireCube (Vector3.zero, Vector3.one);

			Gizmos.matrix = oldGizmosMatrix;


		}
	}

}