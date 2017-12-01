using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace RF.AssetWizzard.Editor {

	[CustomEditor(typeof(StyleAsset))]
	public class StyleAssetInspector : AssetInspector<StyleTemplate, StyleAsset>
    {

        SerializedProperty ShowWalls;
        SerializedProperty ShowEditUI;




        void OnEnable() {
            ShowWalls = serializedObject.FindProperty("ShowWalls");
            ShowEditUI = serializedObject.FindProperty("ShowEditUI");
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
            var panel = new GameObject("NewPanel").AddComponent<StylePanel>();
            panel.transform.parent = Asset.transform;
            panel.transform.SetSiblingIndex(1);
        }



        private void AlignPanels() {
           
            Vector3 movePoint = Vector3.zero;

            foreach (StylePanel panel in Asset.Panels) {
                panel.SetPosition(movePoint, false);
                movePoint = panel.Bounds.GetVertex(VertexX.Right, VertexY.Bottom, VertexZ.Front); 
            }
        }



    }
}
