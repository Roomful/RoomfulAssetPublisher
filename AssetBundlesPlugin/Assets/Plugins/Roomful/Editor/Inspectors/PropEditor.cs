using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace RF.AssetWizzard.Editor {

	[CustomEditor(typeof(PropAsset))]
	public class PropEditor : UnityEditor.Editor {


		SerializedProperty scaleProperty;
		SerializedProperty ShowBoundsProperty;
		SerializedProperty DisplayMode;

		void OnEnable() {
			scaleProperty = serializedObject.FindProperty("Scale");
			ShowBoundsProperty = serializedObject.FindProperty("ShowBounds");
			DisplayMode = serializedObject.FindProperty("DisplayMode");
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

			EditorGUILayout.PropertyField (DisplayMode);
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

				bool test = GUILayout.Button ("Test",  EditorStyles.miniButton, new GUILayoutOption[] {GUILayout.Width(120)});
				if(test) {

					Vector3 storedPos = Prop.transform.position;
					Prop.transform.position = Vector3.zero;

					Prop.GetLayer (HierarchyLayers.Silhouette).gameObject.SetActive (true);


					MeshFilter[] meshFilters = Prop.GetLayer (HierarchyLayers.Silhouette).GetComponentsInChildren<MeshFilter> ();
					CombineInstance[] combine = new CombineInstance[meshFilters.Length];
					int i = 0;
					while (i < meshFilters.Length) {
						combine [i].mesh = meshFilters [i].sharedMesh;
						combine [i].transform = meshFilters [i].transform.localToWorldMatrix;
						//meshFilters [i].transform.worldToLocalMatrix
						i++;
					}

					Mesh m = new Mesh ();
					m.CombineMeshes (combine);


				

					GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);
					go.name = "TEST_GENERATED";
					go.transform.parent = Prop.GetLayer (HierarchyLayers.Silhouette);
					go.transform.localPosition = Vector3.zero;

					go.GetComponent<MeshFilter> ().sharedMesh = m;
					go.GetComponent<MeshFilter> ().sharedMesh.name = "Silhouette";  



					Prop.GetLayer (HierarchyLayers.Silhouette).gameObject.SetActive (false);

					Prop.transform.position = storedPos;

				}

			} GUILayout.EndHorizontal ();



			EditorGUILayout.Space ();
			serializedObject.ApplyModifiedProperties ();

		}


		private void PrintPropState() {

			bool valid = true;


			if(Prop.DisplayMode == PropDisplayMode.Silhouette) {

				if(IsEmpty) {
					valid = false;
					EditorGUILayout.HelpBox("Silhouette is empty! Please add some graphics.", MessageType.Error);
				}

				return;
			}

			if(IsEmpty) {
				valid = false;
				EditorGUILayout.HelpBox("Asset is empty! Please add some graphics.", MessageType.Error);
			}

			if(Prop.GetLayer(HierarchyLayers.Silhouette).transform.childCount == 0) {
				valid = false;
				EditorGUILayout.HelpBox("Silhouette is empty! Please add some graphics.", MessageType.Error);
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

			if(HasLights) {
				EditorGUILayout.HelpBox("Please note, that light's range, spot angle, width and height will be scaled with accordinally to a prop scale in runtime. But that behaviour can't be tested inside the editor", MessageType.Info);
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


		public bool HasLights {
			get {
				Light[] lights = Prop.GetLayer(HierarchyLayers.Graphics).GetComponentsInChildren<Light> ();
				return lights.Length != 0;
			}
		}


	}
}
