using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.Editor
{
    [CustomEditor(typeof(PropAsset))]
    internal class PropAssetInspector : AssetInspector<PropTemplate, PropAsset>
    {
        private static bool s_playAnimation = false;

        private SerializedProperty m_scaleProperty;
        private SerializedProperty m_showCenterProperty;

        private void Awake() {
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDestroy() {
            EditorApplication.update -= OnEditorUpdate;
        }

        void OnEnable() {
            m_scaleProperty = serializedObject.FindProperty("m_scale");
        }

        private void OnEditorUpdate() {
            if (s_playAnimation) {
                foreach (var animator in Asset.AnimatorControllers) {
                    animator.Update(Time.deltaTime);
                }
            }
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUILayout.Space();
            PrintPropState();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            var def = Asset.Size * 100f * Asset.Scale;

            EditorGUILayout.LabelField("Size(mm): ");
            EditorGUILayout.LabelField((int) def.x + "x" + (int) def.y + "x" + (int) def.z);
            GUILayout.EndHorizontal();

            EditorGUILayout.Slider(m_scaleProperty, Asset.MinScale, Asset.MaxScale);
            
            DrawGizmosSwitch();

            DrawEnvironmentSwitch();
            DrawAnimationInfo();
            DrawActionButtons();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAnimationInfo() {
            foreach (var animator in Asset.AnimatorControllers) {
                EditorGUILayout.Space();

                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(animator.name + " Animator: ", EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();

                    var buttonName = "Play";
                    if (s_playAnimation) {
                        buttonName = "Stop";
                    }

                    var play = GUILayout.Button(buttonName, EditorStyles.miniButton, GUILayout.Width(60));
                    if (play) {
                        if (s_playAnimation) {
                            animator.Rebind();
                        }

                        s_playAnimation = !s_playAnimation;
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUI.indentLevel++;

                for (var i = 0; i < animator.parameterCount; i++) {
                    var paramName = animator.GetParameter(i).name;
                    switch (animator.GetParameter(i).type) {
                        case AnimatorControllerParameterType.Bool:

                            var boolVal = animator.GetBool(paramName);
                            boolVal = EditorGUILayout.Toggle(paramName, boolVal);

                            animator.SetBool(paramName, boolVal);
                            break;
                        case AnimatorControllerParameterType.Trigger:
                            GUILayout.BeginHorizontal(); {
                            //    GUILayout.FlexibleSpace();
                            var click = GUILayout.Button(paramName, EditorStyles.miniButton, GUILayout.Width(120));
                            if (click) {
                                animator.SetTrigger(paramName);
                            }
                        }
                            GUILayout.EndHorizontal();

                            break;

                        case AnimatorControllerParameterType.Float:

                            var val = animator.GetFloat(paramName);
                            val = EditorGUILayout.Slider(paramName, val, -1, 1);

                            animator.SetFloat(paramName, val);
                            break;
                    }
                }

                EditorGUI.indentLevel--;
            }
        }

        private void PrintPropState() {
            var valid = true;
            
            if (!Asset.ValidSize) {
                valid = false;
                EditorGUILayout.HelpBox("Your prop's default size doesn't follow our guidelines. We recommend you keep your prop between 50cm and 3m", MessageType.Error);
            }

            if (Asset.IsEmpty) {
                valid = false;
                EditorGUILayout.HelpBox("Asset is empty! Please add some graphics.", MessageType.Error);
            }
            
            if (!Asset.HasCollision) {
                valid = false;
                EditorGUILayout.HelpBox("Your asset has no colliders, consider adding one.", MessageType.Error);
            }

            if (HasMeshCollision) {
                valid = false;
                EditorGUILayout.HelpBox("Using Mesh Colliders in your asset may cause low performance. Consider replacing them with primitive colliders.", MessageType.Warning);
            }

            if (valid) {
                EditorGUILayout.HelpBox("Asset is valid. No issues were found", MessageType.Info);
            }

            if (HasLights) {
                EditorGUILayout.HelpBox("Please note that in Roomful the light's range, spot angle, width and height will be scaled with the prop's scale. This behaviour can't be tested here", MessageType.Info);
            }
        }

        protected override PropAsset Asset => target as PropAsset;

        private bool HasMeshCollision {
            get {
                var colliders = Asset.GetLayer(HierarchyLayers.Graphics).GetComponentsInChildren<MeshCollider>();

                foreach (var c in colliders) {
                    if (c.transform.parent != null) {
                        if (c.transform.parent.GetComponent<PropThumbnail>() != null) {
                            continue;
                        }
                    }

                    return true;
                }

                return false;
            }
        }

        private bool HasLights {
            get {
                var lights = Asset.GetLayer(HierarchyLayers.Graphics).GetComponentsInChildren<Light>();
                return lights.Length != 0;
            }
        }
    }
}