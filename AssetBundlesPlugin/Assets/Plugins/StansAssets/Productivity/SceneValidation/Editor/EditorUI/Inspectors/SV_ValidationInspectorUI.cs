using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;
using UnityEditor.SceneManagement;

namespace SA.Productivity.SceneValidator
{
    public static class SV_ValidationInspectorUI 
    {


        private static void DisplayIssuesCount(Dictionary<Component, Dictionary<SV_iValidationRule, SV_ValidationState>> issues, GameObject gameObject, SV_ValidationState state) {
            int issuesNum = 0;
           
            foreach (var pair in issues) {
                var component = pair.Key;
                issuesNum += pair.Value.Count;
            }

 
            var issuesLabelColor = Color.yellow;
            if (state == SV_ValidationState.Error) {
                issuesLabelColor = Color.red;
            }

            using (new SA_GuiChangeColor(issuesLabelColor)) {
                using (new SA_GuiHorizontalSpace(-10)) {
                    using (new SA_GuiBeginHorizontal(EditorStyles.helpBox)) {
                        string issueToken = "Issue";
                        if (issuesNum > 1) {
                            issueToken = "Issues";
                        }
                        string issuesLabelText = string.Format("{0} {1} found on {2}", issuesNum, issueToken, gameObject.name);
                        EditorGUILayout.LabelField(issuesLabelText, EditorStyles.boldLabel);
                    }
                }
            }
        }

        public static void DrawValidationUI(GameObject gameObject) {

            //Skip everything if validator is disabled
            if (!SV_Settings.IsValidationActive) { return; }

            //We only work with the scene files
            if (EditorUtility.IsPersistent(gameObject)) { return; }


            var validator = SV_Validation.API.GetSceneValidator(gameObject.scene);
            SV_ValidationState state = validator.GetGameObjectState(gameObject);
            if (state == SV_ValidationState.Ok) { return; }


            Dictionary<Component, Dictionary<SV_iValidationRule, SV_ValidationState>> issues = validator.ValidateGameObject(gameObject);

            if(SV_Settings.UserSettings.DisplayIssuesCount) {
                DisplayIssuesCount(issues, gameObject, state);
            }

         

            bool hasComponentAbove = false;
            foreach (var pair in issues) {

                var component = pair.Key;

                if(SV_Settings.UserSettings.DisplayIssueRelatedComponent) {
                    if (hasComponentAbove) {
                        using (new SA_GuiHorizontalSpace(-50)) {
                            SA_EditorGUILayout.HorizontalLineThin();
                        }
                        GUILayout.Space(5);
                    }
                    hasComponentAbove = true;
                }

                if(SV_Settings.UserSettings.DisplayIssueRelatedComponent) {
                    using (new SV_ComponentIssueBlock(component)) {
                        RenderIssuesInspector(component, pair.Value);
                    }
                } else {
                    RenderIssuesInspector(component, pair.Value);
                }

            }



            EditorGUILayout.Space();

        }

        private static void RenderIssuesInspector(Component component, Dictionary<SV_iValidationRule, SV_ValidationState> componentIssues) {
            foreach (var issuePair in componentIssues) {
                var issue = issuePair.Key;
                issue.InspectorGUI(component);
                if (SV_IgnoreManager.IsRuleIgnored(issue, component)) {
                    EditorGUILayout.LabelField("THIS RULE IS IGNORED YOU SHOULD NOT SEE IT HERE");
                }

                EditorGUILayout.Space();
            }
        }


    }
}