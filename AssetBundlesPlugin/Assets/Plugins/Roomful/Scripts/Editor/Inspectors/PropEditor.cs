using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace RF.AssetWizzard.Editor {

	[CustomEditor(typeof(PropAsset))]
	public class PropEditor : UnityEditor.Editor {


		SerializedProperty scaleProperty;
		SerializedProperty ShowBoundsProperty;

		void OnEnable() {
			scaleProperty = serializedObject.FindProperty("Scale");
			ShowBoundsProperty = serializedObject.FindProperty("ShowBounds");
		}


		public override void OnInspectorGUI() {

			serializedObject.Update();

			EditorGUILayout.Space ();
			PrintPropState ();
			EditorGUILayout.Space ();


			GUILayout.BeginHorizontal ();
			Vector3 def = Prop.Size * 100f;

			EditorGUILayout.LabelField ("Size(mm): ");
			EditorGUILayout.LabelField ((int)def.x + "x" + (int)def.y + "x" + (int)def.z);
			GUILayout.EndHorizontal ();



			EditorGUILayout.Slider (scaleProperty, Prop.MinScale, Prop.MaxScale);
			EditorGUILayout.PropertyField (ShowBoundsProperty);

			EditorGUILayout.Space ();
			GUILayout.BeginHorizontal (); {
				
			
				GUILayout.FlexibleSpace ();
				bool wizzard = GUILayout.Button ("Wizzard", EditorStyles.miniButton, new GUILayoutOption[] {GUILayout.Width(120)});
				if(wizzard) {
					WindowManager.ShowWizard ();
					WindowManager.Wizzard.SiwtchTab (WizzardTabs.Wizzard);
				}


				bool save = GUILayout.Button ("Save",  EditorStyles.miniButton, new GUILayoutOption[] {GUILayout.Width(120)});
				if(save) {
					AssetBundlesManager.SavePrefab (Prop);
				}

			} GUILayout.EndHorizontal ();



			EditorGUILayout.Space ();
			serializedObject.ApplyModifiedProperties ();

		}


		private void PrintPropState() {

			bool valid = true;
			if(IsEmpty) {
				valid = false;
				EditorGUILayout.HelpBox("Asset is empty! Please add some graphics.", MessageType.Error);
			}

			if(!HasCollisison) {
				valid = false;
				EditorGUILayout.HelpBox("Your asset has no colliders, consider to add one.", MessageType.Warning);
			}


		
			if(HasMeshCollisison) {
				valid = false;
				EditorGUILayout.HelpBox("Hving MeshColliders inside your asset mey cause a low performance. Consider replasing it with primitive colliders.", MessageType.Warning);
			}


			if(valid) {
				EditorGUILayout.HelpBox("Asset is valid. No issues was found so far.", MessageType.Info);
			}

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


		public bool IsEmpty {
			get {
				Renderer[] renderers = Prop.GetComponentsInChildren<Renderer> ();
				return renderers.Length == 0;
			}
		}

		public bool HasCollisison {
			get {
				Collider[] colliders = Prop.GetComponentsInChildren<Collider> ();
				PropThumbnail[] thumbnails = Prop.GetComponentsInChildren<PropThumbnail> ();

				if(colliders.Length == 0 && thumbnails.Length == 0) {
					return false;
				}

				return true;
			}
		}

		public bool HasMeshCollisison {
			get {
				MeshCollider[] colliders = Prop.GetLayer(HierarchyLayers.Graphics).GetComponentsInChildren<MeshCollider> ();
				return colliders.Length != 0;
			}
		}


	}
}
