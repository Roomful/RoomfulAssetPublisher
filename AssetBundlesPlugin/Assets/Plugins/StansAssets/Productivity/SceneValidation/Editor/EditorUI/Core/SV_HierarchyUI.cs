using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEditor.IMGUI.Controls;

using SA.Foundation.UtilitiesEditor;
using System.Linq;



namespace SA.Productivity.SceneValidator
{

    public class SV_HierarchyUI
    {


        public SV_HierarchyUI() {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }


        public void Repaint() {
            EditorApplication.RepaintHierarchyWindow();
        }


        
        private void OnHierarchyWindowItemOnGUI(int instanceId, Rect selectionRect) {
            if (!SV_Settings.IsValidationActive) { return; }

            var go = (GameObject) EditorUtility.InstanceIDToObject(instanceId);
            if(go == null) {
                return;
            }

            var validator = SV_Validation.API.GetSceneValidator(go.scene);
           

            var state = validator.GetGameobjectnValidationState(go);
            switch (state) {

                case SV_ValidationState.Error:
                    DrawHierarchyIcon(ErrorIcon, selectionRect);
                    break;
                case SV_ValidationState.Warning:
                    DrawHierarchyIcon(WarningIcon, selectionRect);
                    break;

                case SV_ValidationState.Ok:
                    //Let's see if this object may be the parent of an issue

                    var childState = validator.GetChildrenValidationState(go);

                    //Let's see if we have to draw something
                    if (childState != SV_ValidationState.Ok) {
                        //It could be expanded already in this case we don't need to draw anything
                        if (IsGameObjectExpandedInHierarchy(instanceId)) {
                            //Preventing next switch from drawing
                            childState = SV_ValidationState.Ok;
                        }
                    }

                    switch (childState) {
                        case SV_ValidationState.Error:
                            DrawHierarchyIcon(ErrorIcon, selectionRect, fullSize: false, clickAction: () => {
                                HihglitChildIssues(go);
                            });
                            break;
                        case SV_ValidationState.Warning:
                            DrawHierarchyIcon(WarningIcon, selectionRect, fullSize: false, clickAction: () => {
                                HihglitChildIssues(go);
                            });
                            break;
                    }
                    break;
            }
        }


        private void DrawHierarchyIcon(Texture2D icon, Rect selectionRect, bool fullSize = true, Action clickAction = null) {


            float iconX;
            if (SV_Settings.UserSettings.HierarchyIconsAlligment == SV_IconAlligment.Left) {
                iconX = 0;
            } else {
                iconX = selectionRect.xMax - 17.0f;
            }

            Rect clickRect = new Rect(selectionRect) {
                x = iconX,
                width = 16
            };

            Rect drawRect;
            if (fullSize) {
                drawRect = clickRect;
            } else {
                drawRect = new Rect(4 + iconX, selectionRect.y + 5, 7, 7);
            }

            GUI.DrawTexture(drawRect, icon);

            if ((Event.current.type == EventType.MouseDown) && (Event.current.button == 0) && clickRect.Contains(Event.current.mousePosition)) {
                if (clickAction != null) {
                    clickAction.Invoke();
                }
            }

        }


        private void HihglitChildIssues(GameObject go) {
            var validator = SV_Validation.API.GetSceneValidator(go.scene);
            List<int> childrenIssues = validator.GetParentIssues(go.GetInstanceID());
            foreach (int child in childrenIssues) {
                EditorGUIUtility.PingObject(child);
                Selection.activeInstanceID = child;
                break;
            }
        }


        private TreeViewState hierarchyWindowTreeViewState = null;
        private EditorWindow m_sceneHierarchyWindow = null;
        private bool IsGameObjectExpandedInHierarchy(int instanceId) {


            if (m_sceneHierarchyWindow == null) {
                var SceneHierarchyWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
                m_sceneHierarchyWindow = EditorWindow.GetWindow(SceneHierarchyWindowType);

                if (m_sceneHierarchyWindow == null) {
                    return false;
                }

                FieldInfo field = SceneHierarchyWindowType.GetField("m_TreeViewState", BindingFlags.NonPublic | BindingFlags.Instance);
                hierarchyWindowTreeViewState = (TreeViewState)field.GetValue(m_sceneHierarchyWindow);
            }


            if (hierarchyWindowTreeViewState == null || hierarchyWindowTreeViewState.expandedIDs == null) {
                return false;
            }

            return hierarchyWindowTreeViewState.expandedIDs.Contains(instanceId);
        }



        public static Texture2D ErrorIcon {
            get {
                return EditorGUIUtility.FindTexture("d_console.erroricon.sml");
            }
        }

        public static Texture2D WarningIcon {
            get {
                return EditorGUIUtility.FindTexture("d_console.warnicon.sml");
            }
        }

    }
}