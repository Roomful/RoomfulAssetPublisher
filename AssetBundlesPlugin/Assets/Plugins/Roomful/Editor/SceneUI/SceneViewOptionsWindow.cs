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

        private static int WINDOW_ID = 5162389;
        private static SceneView m_sceneView;

        private static Rect m_position = Rect.zero;
        private static Rect m_windowRect = Rect.zero;

        private static UnityEditor.Editor m_propEditor = null;

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
            m_windowRect = GUILayout.Window(WINDOW_ID, m_position, OnWindowGui, Asset.name, GUILayout.ExpandHeight(true), GUILayout.Width(m_position.width), GUILayout.MaxWidth(m_position.width));
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
                    s_useEditorCameraPosition = EditorGUILayout.Toggle("Use Editor Camera", s_useEditorCameraPosition);
                    bool createIcon = GUILayout.Button("Make Icon");
                    if (createIcon) {
                        PropAssetScreenshotTool.CreateIcon(s_useEditorCameraPosition, Asset);
                    }
                }  
            }

           
           EditorGUILayout.Space();
           EditorGUILayout.LabelField("Actions: ", EditorStyles.boldLabel);
         //  PropEditor.OnInspectorGUI();
          

           float btnWidth = 80;
           using (new SA_GuiBeginHorizontal()) {
               if (Asset.GetTemplate().IsNew) {
                   bool upload = GUILayout.Button("Upload", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                   if (upload) {
                       PropWizzard.UploadProp(Asset);
                   }

               } else {
                   bool upload = GUILayout.Button("Re-Upload", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                   if (upload) {
                       PropWizzard.UploadProp(Asset);
                   }

                   bool refresh = GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                   if (refresh) {
                       PropWizzard.DownloadProp(Asset.Template);
                   }
               }


               bool create = GUILayout.Button("New", EditorStyles.miniButton, GUILayout.Width(btnWidth));
               if (create) {
                   PropWizzard.CreateProp();
               }
           }
            using (new SA_GuiBeginHorizontal()) {
                bool wizzard = GUILayout.Button("Wizzard", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                if (wizzard) {
                    WindowManager.ShowWizard();
                    WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);
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

        public static UnityEditor.Editor PropEditor {
            get {

                if(m_propEditor != null && m_propEditor.target != Asset) {
                    m_propEditor = null;
                }

                if(m_propEditor == null) {
                    m_propEditor = UnityEditor.Editor.CreateEditor(Asset);
                } 
                return m_propEditor;
            }

            
        }
    }
}