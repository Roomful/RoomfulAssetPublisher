using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace RF.AssetWizzard.Editor {

	[CustomEditor(typeof(PropAsset))]
	public class PropAssetInspector : AssetInspector<PropTemplate, PropAsset>
    {


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
			Vector3 def = Asset.Size * 100f * Asset.Scale;

			EditorGUILayout.LabelField ("Size(mm): ");
			EditorGUILayout.LabelField ((int)def.x + "x" + (int)def.y + "x" + (int)def.z);
			GUILayout.EndHorizontal ();

            
			EditorGUILayout.Slider (scaleProperty, Asset.MinScale, Asset.MaxScale);

			EditorGUILayout.PropertyField (DisplayMode);
			EditorGUILayout.PropertyField (DrawGizmos);


          

            DrawEnvironmentSiwtch();
            DrawActionButtons();

            serializedObject.ApplyModifiedProperties ();

		}


		private void PrintPropState() {

			bool valid = true;


			if(Asset.DisplayMode == PropDisplayMode.Silhouette) {

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

			if(Asset.GetLayer(HierarchyLayers.Silhouette).transform.childCount == 0) {
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



        public override PropAsset Asset {
            get {
                return target as PropAsset;
            }
        }


        public bool IsEmpty {
			get {
				Renderer[] renderers = Asset.GetComponentsInChildren<Renderer> ();
				return renderers.Length == 0;
			}
		}

		public bool HasCollisison {
			get {
				Collider[] colliders = Asset.GetComponentsInChildren<Collider> ();
				PropThumbnail[] thumbnails = Asset.GetComponentsInChildren<PropThumbnail> ();

				if(colliders.Length == 0 && thumbnails.Length == 0) {
					return false;
				}

				return true;
			}
		}

		public bool HasMeshCollisison {
			get {
				MeshCollider[] colliders = Asset.GetLayer(HierarchyLayers.Graphics).GetComponentsInChildren<MeshCollider> ();

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
				Light[] lights = Asset.GetLayer(HierarchyLayers.Graphics).GetComponentsInChildren<Light> ();
				return lights.Length != 0;
			}
		}


	}
}
