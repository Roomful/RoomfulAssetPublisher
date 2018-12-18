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
        private static int WINDOW_TEX_ID = 6162389;
        private static SceneView m_sceneView;

        private static Rect m_position = Rect.zero;
        private static Rect m_windowRect = Rect.zero;

        private static UnityEditor.Editor m_propEditor = null;
        private static bool m_isMouseOverIcon = false;

        private static Rect m_ionRect;
        private static PropAsset m_asset = null;

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
            m_windowRect = GUILayout.Window(WINDOW_ID, m_position, OnWindowGui, Asset.name, GUILayout.ExpandHeight(true));
            WindowX = m_windowRect.x;

          

            if(m_isMouseOverIcon) {
                m_ionRect = new Rect(
                            Event.current.mousePosition.x + 20,
                        Event.current.mousePosition.y + 40,
                        Asset.Icon.width,
                        Asset.Icon.height + 20);

                GUI.Window(WINDOW_TEX_ID, m_ionRect, OnTExtureWindowGui, "Asset Icon");

            }

            if (Event.current.type == EventType.Repaint) {
                if(m_windowRect.Contains(Event.current.mousePosition)) {
                    Repaint();
                }
            }
        }

        public static void Repaint() {
           
            SceneView.RepaintAll();
        }

        private static void OnTExtureWindowGui(int id) {
            var rect = new Rect(m_ionRect);
            rect.x = 0;
            rect.y = 20;
            rect.height -= 20;
            GUI.DrawTexture(rect,  Asset.Icon);
        }

        private static bool s_useEditorCameraPosition = false;
        private static void OnWindowGui(int id) {
           
            EditorGUILayout.LabelField("Icon: ", EditorStyles.boldLabel);
           
            using (new SA_GuiBeginHorizontal()) {
                Asset.Icon = (Texture2D) EditorGUILayout.ObjectField(Asset.Icon, typeof(Texture2D), false, new GUILayoutOption[] { GUILayout.Width(70), GUILayout.Height(70) });

                if (Event.current.type == EventType.Repaint) {
                    var  lastRect = GUILayoutUtility.GetLastRect();
                    m_isMouseOverIcon = lastRect.Contains(Event.current.mousePosition);
                }

            

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
            EditorGUILayout.LabelField("Asset: ", EditorStyles.boldLabel);
            
            EditorGUI.BeginChangeCheck();
            
            using (new SA_GuiBeginHorizontal()) {
                Vector3 def = Asset.Size * 100f * Asset.Scale;

                EditorGUILayout.LabelField("Size(mm): " + (int)def.x + "x" + (int)def.y + "x" + (int)def.z);
            }
           
            
            Asset.Scale =  EditorGUILayout.Slider(Asset.Scale, Asset.MinScale, Asset.MaxScale);
           
            Asset.DisplayMode = (PropDisplayMode) EditorGUILayout.EnumPopup("Display Mode", Asset.DisplayMode);

            DrawGizmosSiwtch();
            DrawEnvironmentSiwtch();

            if(EditorGUI.EndChangeCheck()) {
                Asset.Update();
            }
             

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Actions: ", EditorStyles.boldLabel);

            float btnWidth = 80;
            if (Asset.GetTemplate().IsNew) {
                bool upload = GUILayout.Button("Upload", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                if (upload) {
                    PropWizzard.UploadProp(Asset);
                }
            } else {
                using (new SA_GuiBeginHorizontal()) {
                    bool upload = GUILayout.Button("Re-Upload", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                    if (upload) {
                        PropWizzard.UploadProp(Asset);
                    }

                    bool meta = GUILayout.Button("Update Meta", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                    if (meta) {
                        PropWizzard.UpdateMeta(Asset);
                    }

                    bool refresh = GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                    if (refresh) {
                        PropWizzard.DownloadProp(Asset.Template);
                    }

                }
            }
                


               
        
            using (new SA_GuiBeginHorizontal()) {
                bool wizzard = GUILayout.Button("Wizzard", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                if (wizzard) {
                    WindowManager.ShowWizard();
                    WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);
                }

                bool create = GUILayout.Button("New", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                if (create) {
                    PropWizzard.CreateProp();
                }
            }


            




            GUI.DragWindow(new Rect(0, 0, m_sceneView.position.width, m_sceneView.position.height));
        }


        public static void DrawEnvironmentSiwtch() {
            Environment env = GameObject.FindObjectOfType<Environment>();
            if (env != null) {
                EditorGUI.BeginChangeCheck();
                env.RenderEnvironment = EditorGUILayout.Toggle("Render Environment", env.RenderEnvironment);
                if (EditorGUI.EndChangeCheck()) {
                    env.Update();
                }
            }
        }

        public static void DrawGizmosSiwtch() {
            Asset.DrawGizmos = EditorGUILayout.Toggle("Draw Gizmos", Asset.DrawGizmos);
        }

        private static PropAsset Asset {
            get {
                if(m_asset == null) {
                    m_asset = FindObjectWithType<PropAsset>();
                } 
                return m_asset;
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