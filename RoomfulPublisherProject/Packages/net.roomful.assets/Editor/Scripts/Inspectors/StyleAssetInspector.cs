using net.roomful.api;
using StansAssets.Foundation.Extensions;
using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    [CustomEditor(typeof(StyleAsset))]
    internal class StyleAssetInspector : AssetInspector<StyleAssetTemplate, StyleAsset>
    {
        private SerializedProperty m_showWalls;
        private SerializedProperty m_showEditUI;

        private void OnEnable() {
            m_showWalls = serializedObject.FindProperty("ShowWalls");
            m_showEditUI = serializedObject.FindProperty("ShowEditUI");

            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable() {
            EditorApplication.update -= OnEditorUpdate;
        }

        private void OnEditorUpdate() {
            Asset.transform.Reset();
            if (Asset.transform.childCount == 0) {
                GenerateDefaultPanels();
            }

            var env = Asset.Environment.GetComponentInChildren<Environment>();
            env.RenderEnvironment = false;
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DrawGizmosSwitch();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_showWalls);
            EditorGUILayout.PropertyField(m_showEditUI);
            if (EditorGUI.EndChangeCheck()) {
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
            }
            GUILayout.EndHorizontal();

            DrawActionButtons();
            serializedObject.ApplyModifiedProperties();
        }

        protected override StyleAsset Asset => target as StyleAsset;

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
                EditorApplication.delayCall += AlignPanels;
            };
        }

        private void AlignPanels() {
            var movePoint = Vector3.zero;

            bool isNoMirrorStyle = Asset.Template.StyleType == StyleType.NoMirror;
            foreach (var panel in Asset.Panels) {
                panel.SetPanelBoundsCenter(movePoint);

                var panelPoint = panel.Bounds.GetVertex(SA_VertexX.Right, SA_VertexY.Bottom, SA_VertexZ.Front);
                movePoint = isNoMirrorStyle ? new Vector3(panelPoint.x, panelPoint.y, 0.0f) : panelPoint;
                movePoint.x += 0.01f;
            }
        }
    }
}