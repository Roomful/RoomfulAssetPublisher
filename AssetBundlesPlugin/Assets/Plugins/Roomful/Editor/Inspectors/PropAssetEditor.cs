using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace RF.AssetWizzard.Editor {

	[CustomEditor(typeof(PropAsset))]
	public class PropAssetEditor : UnityEditor.Editor {


		SerializedProperty scaleProperty;
		SerializedProperty DrawGizmos;
		SerializedProperty ShowCenterProperty;
		SerializedProperty DisplayMode;

		void OnEnable() {
			scaleProperty = serializedObject.FindProperty("Scale");
			DrawGizmos = serializedObject.FindProperty("DrawGizmos");
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
			EditorGUILayout.PropertyField (DrawGizmos);


            Environment env = GameObject.FindObjectOfType<Environment>();
            if(env != null) {
                EditorGUI.BeginChangeCheck();
                env.RenderEnvironment = EditorGUILayout.Toggle("Render Environment", env.RenderEnvironment);
                if(EditorGUI.EndChangeCheck()) {
                    env.Update();
                }
            }

       
            



            EditorGUILayout.Space ();
			GUILayout.BeginHorizontal (); {

			
				GUILayout.FlexibleSpace ();
				bool wizzard = GUILayout.Button ("Wizzard", EditorStyles.miniButton, new GUILayoutOption[] {GUILayout.Width(120)});
				if(wizzard) {
					WindowManager.ShowWizard ();
					WindowManager.Wizzard.SiwtchTab (WizardTabs.Wizzard);
				}


				if (string.IsNullOrEmpty (Prop.Template.Id)) {

					bool upload = GUILayout.Button ("Upload", EditorStyles.miniButton, new GUILayoutOption[] {GUILayout.Width(120)});
					if(upload) {
						PropBundleManager.UploadAssets (Prop);
					}

				} else {
					bool re_upload = GUILayout.Button ("Re Upload", EditorStyles.miniButton, new GUILayoutOption[] {GUILayout.Width(120)});
					if (re_upload) {
						PropBundleManager.ReuploadAsset (Prop);
					}

					bool refresh =GUILayout.Button ("Refresh", EditorStyles.miniButton, new GUILayoutOption[] {GUILayout.Width(120)});
					if (refresh) {
						PropBundleManager.DownloadAssetBundle (Prop.Template);
					}
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
				EditorGUILayout.HelpBox("Having MeshColliders inside your asset mey cause a low performance. Consider replacing it with primitive colliders.", MessageType.Warning);
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

		public PropTemplate Template {
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

				foreach(MeshCollider c in colliders) {
					if(c.transform.parent != null) {
						if(c.transform.parent.GetComponent<PropThumbnail>() != null) {
							continue;
						}
					}

					return true;
				}

				return false;
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
