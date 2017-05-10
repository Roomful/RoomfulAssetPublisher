using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace RF.AssetWizzard {

	[CustomEditor(typeof(PropAsset))]
	public class PropEditor : UnityEditor.Editor {


		SerializedProperty scaleProperty;

		void OnEnable() {
			scaleProperty = serializedObject.FindProperty("Scale");
		}


		public override void OnInspectorGUI() {

			serializedObject.Update();

			EditorGUILayout.Space ();

			GUILayout.BeginHorizontal ();
			Vector3 def = Prop.Size * 100f;

			EditorGUILayout.LabelField ("Size(mm): ");
			EditorGUILayout.LabelField ((int)def.x + "x" + (int)def.y + "x" + (int)def.z);
			GUILayout.EndHorizontal ();


			GUILayout.BeginHorizontal ();

			//EditorGUILayout.LabelField ("Scale: ");
			EditorGUILayout.Slider (scaleProperty, Prop.MinScale, Prop.MaxScale);
			GUILayout.EndHorizontal ();


			serializedObject.ApplyModifiedProperties ();

		}


		public PropAsset Prop {
			get {
				return target as PropAsset;
			}
		}

		public AssetTemplate Template {
			get {
				return Prop.Template;
			}
		}


	}
}
