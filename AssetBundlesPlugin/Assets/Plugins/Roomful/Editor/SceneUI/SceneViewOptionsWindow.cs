using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEditor.IMGUI.Controls;

using SA.Foundation.Editor;

namespace RF.AssetWizzard.Editor {

    [InitializeOnLoad]
    public static class SceneViewOptionsWindow  {

        private static int WINDOW_ID = 516789;
        private static SceneView m_sceneView;

        private static Rect m_position = Rect.zero;
        private static Rect m_windowRect = Rect.zero;

        static SceneViewOptionsWindow() {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }


        private static void OnSceneGUI(SceneView sceneView) {

            if (Asset == null) { return; }
            m_sceneView = sceneView;

           

            if (WindowX < 0) {
                WindowX = 0;
            }


            float maxX = sceneView.position.width - m_windowRect.width;
            if (WindowX > maxX) {
                WindowX = maxX;
            }


            m_position.x = WindowX;
            m_position.y = 20;


            m_windowRect = GUILayout.Window(WINDOW_ID, m_position, OnWindowGui,"Assset Wizard", GUILayout.ExpandHeight(true));
            WindowX = m_windowRect.x;

           

            

        }

        private static bool s_useEditorCameraPosition = false;
        private static void OnWindowGui(int id) {
            EditorGUILayout.LabelField("Asset Icon: ", EditorStyles.boldLabel);

            using (new SA_GuiBeginHorizontal()) {
                Asset.Icon = (Texture2D) EditorGUILayout.ObjectField(Asset.Icon, typeof(Texture2D), false, new GUILayoutOption[] { GUILayout.Width(70), GUILayout.Height(70) });
                EditorGUILayout.Space();
                using (new SA_GuiBeginVertical()) {
                    EditorGUILayout.Space();
                    s_useEditorCameraPosition = EditorGUILayout.Toggle("Used Editor Camera", s_useEditorCameraPosition);
                    bool createIcon = GUILayout.Button("Make icon");
                    if (createIcon) {
                        //CreateIcon();
                    }
                }  
            }
                

            GUI.DragWindow(new Rect(0, 0, m_sceneView.position.width, m_sceneView.position.height));
        }


        private static PropAsset Asset {
            get {
                return FindObjectWithType<PropAsset>();
            }
        }


        private static T FindObjectWithType<T>() {
            var allFindedObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (GameObject gameObject in allFindedObjects) {
                T target = gameObject.GetComponent<T>();

                if (target != null) {
                    return target;
                }
            }
            return default(T);
        }

        public static float WindowX {
            get {
                string key = "SceneViewOptionsWindow_WindowX";
                if (EditorPrefs.HasKey(key)) {
                    return EditorPrefs.GetFloat(key);
                } else {
                    return 0f;
                }
            }

            set {
                string key = "SceneViewOptionsWindow_WindowX";
                EditorPrefs.SetFloat(key, value);
            }
        }
    }
}