using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace RF.AssetWizzard.Editor {

	[CustomEditor(typeof(PropAsset))]
	public class PropAssetInspector : AssetInspector<PropTemplate, PropAsset>
    {

        private static bool playAnimation = false;


		SerializedProperty scaleProperty;
		SerializedProperty ShowCenterProperty;
		SerializedProperty DisplayMode;

        private void Awake() {
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDestroy() {
            EditorApplication.update -= OnEditorUpdate;
        }

        void OnEnable() {

			scaleProperty = serializedObject.FindProperty("Scale");
			DisplayMode = serializedObject.FindProperty("DisplayMode");

          

        }

        private void OnEditorUpdate() {
            if (playAnimation) {
                foreach (var animator in Asset.AnimatorControllers) {
                    animator.Update(Time.deltaTime);
                }
            }
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
            DrawAnimationInfo();
            DrawActionButtons();

            

            serializedObject.ApplyModifiedProperties ();

		}


        private void DrawAnimationInfo() {

            foreach (var animator in Asset.AnimatorControllers) {
                EditorGUILayout.Space();

                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(animator.name + " Animator: ", EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();

                    string name = "Play";
                    if(playAnimation) {
                        name = "Stop";
                    }
                    bool play = GUILayout.Button(name, EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(60) });
                    if (play) {

                        if(playAnimation) {
                            animator.Rebind();
                        }
                        playAnimation = !playAnimation;
                    }



                }
                GUILayout.EndHorizontal();     
                EditorGUI.indentLevel++;

                for (int i = 0; i < animator.parameterCount; i++) {
                    string name = animator.GetParameter(i).name;
                    switch (animator.GetParameter(i).type) {
                        case AnimatorControllerParameterType.Bool:

                            var boolVal = animator.GetBool(name);
                            boolVal = EditorGUILayout.Toggle(name, boolVal);

                            animator.SetBool(name, boolVal);
                            break;
                        case AnimatorControllerParameterType.Trigger:
                            GUILayout.BeginHorizontal(); {
                            //    GUILayout.FlexibleSpace();
                                bool click = GUILayout.Button(name, EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(120) });
                                if(click) {
                                    animator.SetTrigger(name);
                                }
                                
                            }
                            GUILayout.EndHorizontal();
                                
                            break;

                        case AnimatorControllerParameterType.Float:

                            var val = animator.GetFloat(name);
                            val =  EditorGUILayout.Slider(name, val, -1, 1);

                            animator.SetFloat(name, val);
                            break;
                    }
                }

                EditorGUI.indentLevel--;
            }
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
