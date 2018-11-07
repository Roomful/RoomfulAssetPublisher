
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

using SA.Foundation.Editor;
using Rotorz.ReorderableList;


namespace SA.Productivity.SceneValidator
{
    [Serializable]
    public class SV_ReportTab : SA_GUILayoutElement
    {


        public override void OnAwake() {
          
        }

        public override void OnGUI() {

            if(EditorApplication.isPlaying) {
                EditorGUILayout.HelpBox("Scene Validator not avaloiable during Play Mode", MessageType.Info);
                return;
            }

            if (!SV_Settings.IsValidationActive) {
                EditorGUILayout.HelpBox("Report not avaliable. Scene Validator disabled, you can enbale it with the Settings tab", MessageType.Warning);
                return;
            }

            Stats();
        }


        private void Stats() {
          
            for (int i = 0; i < SceneManager.sceneCount; i++) {
                Scene scene = EditorSceneManager.GetSceneAt(i);
                if(!scene.IsValid() || !scene.isLoaded) {
                    continue;
                }

                var validator = SV_Validation.API.GetSceneValidator(scene);

                var perfReport = validator.ValidationReport;

                if (perfReport == null) {
                    continue;
                }


                using (new SV_SceneReportBlock(scene)) {

                    using (new SA_H2WindowBlockWithSpace(new GUIContent("STATS"))) {
                        SA_EditorGUILayout.LabelField("Total Gameobjects: ", perfReport.GameobjectsCount.ToString());
                        SA_EditorGUILayout.LabelField("Total Components: ", perfReport.ComponentsCount.ToString());

                        string time;
                        if(perfReport.Time3DigitsRound < 0) {
                            time = "In Progress";
                        } else {
                            time = perfReport.Time3DigitsRound.ToString();
                        }
                        SA_EditorGUILayout.LabelField("Validation Time (sec):  ", time);

                    }

                    bool sceneHasNoIssues = true;
   
                    if (validator.ContextIssues.Count > 0) {
                        sceneHasNoIssues = false;
                        using (new SA_H2WindowBlockWithSpace(new GUIContent("SCENE"))) {
                            foreach (var issuesPair in validator.ContextIssues) {
                                SV_iContextValidationRule rule = issuesPair.Key;
                                List<Component> sceneComponents = validator.GetComponentsCache(rule.ValidatedType);
                                rule.InspectorGUI(scene, sceneComponents);
                            }
                        }
                    }

                    var gameObjectIssues = GetSceneGameObjectIssues(scene);
                    if (gameObjectIssues.Count > 0) {
                        sceneHasNoIssues = false;

                        using (new SA_H2WindowBlockWithSpace(new GUIContent("COMPONENTS"))) {
                            DrawSceneGameObjectIssue(gameObjectIssues);
                        }
                    }

                  

                    if (sceneHasNoIssues) {
                        EditorGUILayout.Space();
                        EditorGUILayout.HelpBox("No issue found on a scene", MessageType.Info);
                    }
                }
            }


        }


        public static void DrawSceneGameObjectIssue(Dictionary<SV_iValidationRule, GameObjectIssuesList> gameObjectIssues) {


            foreach (var goIssuesPair in gameObjectIssues) {
                SV_iValidationRule rule = goIssuesPair.Key;
                GameObjectIssuesList list = goIssuesPair.Value;
                string title = "(" + list.GameObjects.Count + ") " + rule.Name;
                Texture2D icon;
                if (list.State == SV_ValidationState.Error) {
                    icon = SV_HierarchyUI.ErrorIcon;
                } else {
                    icon = SV_HierarchyUI.WarningIcon;
                }


                using (new SA_GuiBeginHorizontal()) {

                    GUIContent context = new GUIContent(title, icon);
                    var textDimensions = EditorStyles.label.CalcSize(context);

                    EditorGUILayout.LabelField(context, GUILayout.Width(textDimensions.x));
                    GUILayout.FlexibleSpace();
                    bool select = GUILayout.Button("Show", EditorStyles.miniButton, GUILayout.Width(50));
                    if (select) {
                        bool selectNext = false;

                        GameObject initialSelection = Selection.activeGameObject;
                        Selection.activeGameObject = null;
                        foreach (GameObject go in list.GameObjects) {
                            if (selectNext) {
                                EditorGUIUtility.PingObject(go);
                                Selection.activeGameObject = go;
                                break;
                            }

                            if (initialSelection == go) {
                                selectNext = true;
                                continue;
                            }
                        }

                        if (Selection.activeGameObject == null) {
                            GameObject go = list.GameObjects[0];
                            EditorGUIUtility.PingObject(go);
                            Selection.activeGameObject = go;
                        }

                    }

                }
            }
        }


        public static Dictionary<SV_iValidationRule, GameObjectIssuesList> GetSceneGameObjectIssues(Scene scene) {

            var validator = SV_Validation.API.GetSceneValidator(scene);
            var result = new Dictionary<SV_iValidationRule, GameObjectIssuesList>();


            foreach (var gameObjectPair in validator.GameObjectsIssues) {
                GameObject go = (GameObject) EditorUtility.InstanceIDToObject(gameObjectPair.Key);
                if (go == null) {
                    continue;
                }

                if(go.scene != scene) {
                    continue;
                }

                foreach(var componentsPair in gameObjectPair.Value) {
                    foreach(var rulePair in componentsPair.Value) {
                        SV_iValidationRule rule = rulePair.Key;
                        GameObjectIssuesList list;
                        if (result.ContainsKey(rule)) {
                            list = result[rule];
                        } else {
                            list = new GameObjectIssuesList();
                            result.Add(rule, list);
                        }

                        list.Add(go, rulePair.Value);
                    }
                }
            }


            return result;

        }


        public class GameObjectIssuesList
        {
            private List<GameObject> m_gameObjects = new List<GameObject>();
            private SV_ValidationState m_state = SV_ValidationState.Ok;

          

            public void Add(GameObject gameObject, SV_ValidationState state) {
                m_gameObjects.Add(gameObject);

                if(m_state != SV_ValidationState.Error) {
                    m_state = state;
                }
            }


            public List<GameObject> GameObjects {
                get {
                    return m_gameObjects;
                }
            }

            public SV_ValidationState State {
                get {
                    return m_state;
                }
            }
        }

    } 
}
