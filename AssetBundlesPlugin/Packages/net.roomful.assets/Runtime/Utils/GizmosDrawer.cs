using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace net.roomful.assets {
	
	public static class GizmosDrawer {

		public static void DrawCube (Vector3 position, Quaternion rotation, Vector3 scale, Color color) {

			Gizmos.color = color; 

			Matrix4x4 cubeTransform = Matrix4x4.TRS (position, rotation, scale);
			Matrix4x4 oldGizmosMatrix = Gizmos.matrix;

			Gizmos.matrix *= cubeTransform;

			Gizmos.DrawWireCube (Vector3.zero, Vector3.one);

			Gizmos.matrix = oldGizmosMatrix;


		}
	}

}