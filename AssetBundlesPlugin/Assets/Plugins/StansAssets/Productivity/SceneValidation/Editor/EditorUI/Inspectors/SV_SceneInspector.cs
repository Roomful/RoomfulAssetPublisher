using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEditor.IMGUI.Controls;

using SA.Foundation.Editor;


namespace SA.Productivity.SceneValidator
{

    [InitializeOnLoad]
    public static class SV_SceneInspector
    {

        private static int WINDOW_ID = 516789;

        private static Dictionary<SV_iContextValidationRule, SA_CollapsableBlockLayout> m_contextLayouts = new Dictionary<SV_iContextValidationRule, SA_CollapsableBlockLayout>();


        private static SceneView m_sceneView;

        private static Rect m_position = Rect.zero;
        private static Rect m_windowRect = Rect.zero;

        private static float m_lastWindowHeight = 0f;

        static SV_SceneInspector() {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }


        private static SA_CollapsableBlockLayout GetBlockForContextRule(SV_iContextValidationRule rule) {

            if (!m_contextLayouts.ContainsKey(rule)) {

                GUIContent context = new GUIContent(rule.Name, SV_HierarchyUI.WarningIcon);
                SA_CollapsableBlockLayout layout = new SA_CollapsableBlockLayout(context, () => {

                });

                m_contextLayouts.Add(rule, layout);
            }


            return m_contextLayouts[rule];
        }


        private static void OnSceneGUI(SceneView sceneView) {

            if (!SV_Settings.IsValidationActive || !SV_Settings.UserSettings.DisplaySceneViewWindow) { return; }
            m_sceneView = sceneView;
            Scene scene = SceneManager.GetActiveScene();
            var validator = SV_Validation.API.GetSceneValidator(scene);

            int issuesCount = validator.ContextIssues.Count + validator.GameObjectsIssues.Count;
            if (issuesCount != 0) {

                if (WindowX < 0) {
                    WindowX = 0;
                }


                float maxX = sceneView.position.width - m_windowRect.width;
                if (WindowX > maxX) {
                    WindowX = maxX;
                }


                m_position.x = WindowX;
                m_position.y = sceneView.position.height - m_windowRect.height;


                m_windowRect = GUILayout.Window(WINDOW_ID, m_position, OnWindowGui, scene.name + " Issues", GUILayout.ExpandHeight(true));
                WindowX = m_windowRect.x;

                if (Event.current.type == EventType.Repaint) {

                    if (!Mathf.Approximately(m_lastWindowHeight, m_windowRect.height)) {
                        m_lastWindowHeight = m_windowRect.height;
                        Repaint();
                    }
                }

            }

        }

        public static void Repaint() {
            SceneView.RepaintAll();
        }

        private static void OnWindowGui(int id) {

            Scene scene = SceneManager.GetActiveScene();
            var validator = SV_Validation.API.GetSceneValidator(scene);

            var gameObjectIssues = SV_ReportTab.GetSceneGameObjectIssues(scene);


            if (validator.ContextIssues.Count > 0) {
                GUILayout.Label(new GUIContent("Scene:"), SA_Skin.LabelBold);
                foreach (var issuesPair in validator.ContextIssues) {



                    SV_ValidationState state = issuesPair.Value;
                    SV_iContextValidationRule rule = issuesPair.Key;

                    Texture2D icon;
                    if (state == SV_ValidationState.Error) {
                        icon = SV_HierarchyUI.ErrorIcon;
                    } else {
                        icon = SV_HierarchyUI.WarningIcon;
                    }



                    var layout = GetBlockForContextRule(rule);
                    layout.Content.image = icon;
                    layout.OnGUIExpanded = () => {
                        List<Component> sceneComponents = validator.GetComponentsCache(rule.ValidatedType);
                        rule.InspectorGUI(scene, sceneComponents);
                    };
                    layout.OnGUI();






                }
            }

            if (gameObjectIssues.Count > 0) {
                GUILayout.Label(new GUIContent("Components:"), SA_Skin.LabelBold);
                SV_ReportTab.DrawSceneGameObjectIssue(gameObjectIssues);
            }

            GUI.DragWindow(new Rect(0, 0, m_sceneView.position.width, m_sceneView.position.height));
        }



        public static float WindowX {
            get {
                string key = "SV_SceneInspector_WindowX";
                if (EditorPrefs.HasKey(key)) {
                    return EditorPrefs.GetFloat(key);
                } else {
                    return 0f;
                }
            }

            set {
                string key = "SV_SceneInspector_WindowX";
                EditorPrefs.SetFloat(key, value);
            }
        }

    }
}