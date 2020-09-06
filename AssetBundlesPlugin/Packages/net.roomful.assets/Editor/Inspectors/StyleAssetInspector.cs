using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace net.roomful.assets.Editor {

	[CustomEditor(typeof(StyleAsset))]
	public class StyleAssetInspector : AssetInspector<StyleTemplate, StyleAsset>
    {

        SerializedProperty ShowWalls;
        SerializedProperty ShowEditUI;


        void OnEnable() {
            ShowWalls = serializedObject.FindProperty("ShowWalls");
            ShowEditUI = serializedObject.FindProperty("ShowEditUI");

            EditorApplication.update += OnEditorUpdate;
        }

        void OnDisable() {
            EditorApplication.update -= OnEditorUpdate;
        }

        void OnEditorUpdate() {
            Asset.transform.Reset();
            if (Asset.transform.childCount == 0) {
                GenerateDefaultPanels();
            }

            Environment env = Asset.Environment.GetComponentInChildren<Environment>();
            env.RenderEnvironment = false;
        }


        public override void OnInspectorGUI() {

			serializedObject.Update();

            DrawGizmosSiwtch();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(ShowWalls);
            EditorGUILayout.PropertyField(ShowEditUI);
            if(EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
                Asset.UpdateDefaultElements();
            }



            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("New Panel", EditorStyles.miniButton, GUILayout.Width(100))) {
                    NewPanel();
                }

                if (GUILayout.Button("Align Panels", EditorStyles.miniButton, GUILayout.Width(100))) {
                   AlignPanels();
                }

            } GUILayout.EndHorizontal();


            DrawActionButtons();
            serializedObject.ApplyModifiedProperties ();

		}


        public override StyleAsset Asset {
            get {
                return target as StyleAsset;
            }
        }


        private void NewPanel() {
            var panel = PrefabManager.CreatePrefab("Style/DefaultPanel");
            panel.transform.parent = Asset.transform;
            panel.transform.SetSiblingIndex(1);
            panel.name = "NewPanel";

            AlignPanelsWithDelay();

        }


        private void GenerateDefaultPanels() {
            var start = PrefabManager.CreatePrefab("Style/DefaultPanel");
            start.name = "Start";
            start.transform.parent = Asset.transform;

            var end = PrefabManager.CreatePrefab("Style/DefaultPanel");
            end.name = "End";
            end.transform.parent = Asset.transform;


            AlignPanelsWithDelay();
        }

        private void AlignPanelsWithDelay() {
            EditorApplication.delayCall += () => {
                EditorApplication.delayCall += () => {
                    AlignPanels();
                };

            };
        }


        private void AlignPanels() {

            Vector3 movePoint = Vector3.zero;

            foreach (StylePanel panel in  Asset.Panels) {
                panel.SetPosition(movePoint, false);
                movePoint = panel.Bounds.GetVertex(SA_VertexX.Right, SA_VertexY.Bottom, SA_VertexZ.Front);
                movePoint.x += 0.01f;
            }
        }

    }
}
