﻿using System.Collections;
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

				bool test = GUILayout.Button ("Test",  EditorStyles.miniButton, new GUILayoutOption[] {GUILayout.Width(120)});
				if(test) {
					GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
					go.name = "test";


					MeshFilter[] meshFilters = Prop.gameObject.GetComponentsInChildren<MeshFilter>();
					CombineInstance[] combine = new CombineInstance[meshFilters.Length];
					int i = 0;
					while (i < meshFilters.Length) {
						combine[i].mesh = meshFilters[i].sharedMesh;
						combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
						i++;
					}

					Mesh m = new Mesh();
					m.CombineMeshes(combine);

				






					Vector3[] vertices = m.vertices;
					var boundaryPath = EdgeHelpers.GetEdges(m.triangles).FindBoundary().SortEdges();

					List<Vector3> newVertices = new List<Vector3> ();
					List<int> newTriangels = new List<int> ();

					for(int j = 0; j < boundaryPath.Count; j++) {
						Vector3 pos = vertices[ boundaryPath[j].v1 ];
						newVertices.Add (pos);
						newTriangels.Add (m.triangles[boundaryPath[j].triangleIndex]);
						// do something with pos
					}



					Mesh m2 = new Mesh ();
					m2.vertices = newVertices.ToArray ();
					m2.triangles = newTriangels.ToArray ();
					//m2.uv = m.uv;


					go.GetComponent<MeshFilter> ().sharedMesh = m2;
					go.transform.position = Vector3.one * 2;


					//go.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);


					AssetDatabase.CreateAsset( m2, "Assets/generatedMesh.asset" );
					AssetDatabase.SaveAssets();


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
