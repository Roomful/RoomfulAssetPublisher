using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace net.roomful.assets.Editor {

	[CanEditMultipleObjects, CustomEditor(typeof(RoomfulText))]
	public class TextEditor : UnityEditor.Editor {



		void OnEnable() {
			
		}


		public override void OnInspectorGUI() {

			DrawDefaultInspector ();

		}




	}
}
