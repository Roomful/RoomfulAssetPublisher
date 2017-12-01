using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace RF.AssetWizzard.Editor {

	[CustomEditor(typeof(PropAsset))]
	public class PropAssetInspector : AssetInspector<PropTemplate, PropAsset>
    {


		SerializedProperty scaleProperty;
		SerializedProperty ShowCenterProperty;
		SerializedProperty DisplayMode;

		void OnEnable() {
			scaleProperty = serializedObject.FindProperty("Scale");
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
            DrawGizmosSiwtch();





            DrawEnvironmentSiwtch();
            DrawActionButtons();

            serializedObject.ApplyModifiedProperties ();

		}


		private void PrintPropState() {

			bool valid = true;


            if(Asset.DisplayMode == PropDisplayMode.Silhouette || Asset.DisplayMode == PropDisplayMode.Hybrid) {
                EditorGUILayout.HelpBox("The Silhouette is a placeholder so the user knows your prop is being downloaded.\nWe recommend you create a simplified version of your object that fully envelops your prop. Use the 'Hybrid' Display Mode to check how your silhouette is working", MessageType.Info);
            }

			if(Asset.DisplayMode == PropDisplayMode.Silhouette) {

				if(Asset.IsEmpty) {
					valid = false;
					EditorGUILayout.HelpBox("Silhouette is empty! Please add some graphics.", MessageType.Error);
				}

				return;
			}


            if (!Asset.ValidSize) {
                valid = false;
                EditorGUILayout.HelpBox("Your prop's default size doesn't follow our guidelines. We recommend you keep your prop between 50cm and 3m", MessageType.Error);
            }


            if (Asset.IsEmpty) {
				valid = false;
				EditorGUILayout.HelpBox("Asset is empty! Please add some graphics.", MessageType.Error);
			}

			if(Asset.GetLayer(HierarchyLayers.Silhouette).transform.childCount == 0) {
				valid = false;
				EditorGUILayout.HelpBox("Silhouette is empty! Please add some graphics.", MessageType.Error);
			}

			if(!Asset.HasCollisison) {
				valid = false;
				EditorGUILayout.HelpBox("Your asset has no colliders, consider adding one.", MessageType.Error);
			}


		
			if(HasMeshCollisison) {
				valid = false;
				EditorGUILayout.HelpBox("Using Mesh Colliders in your asset may cause low performance. Consider replacing them with primitive colliders.", MessageType.Warning);
			}


			if(valid) {
				EditorGUILayout.HelpBox("Asset is valid. No issues were found", MessageType.Info);
			}

			if(HasLights) {
				EditorGUILayout.HelpBox("Please note that in Roomful the light's range, spot angle, width and height will be scaled with the prop's scale. This behaviour can't be tested here", MessageType.Info);
			}

		}



        public override PropAsset Asset {
            get {
                return target as PropAsset;
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
