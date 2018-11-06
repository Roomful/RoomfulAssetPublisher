
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;
using Rotorz.ReorderableList;


namespace SA.Productivity.SceneValidator
{
    [Serializable]
    public class SV_SettingsTab : SA_GUILayoutElement
    {

        private const string CUSTOM_RULES_DESC = "You may define your custom validation rules using this menu. " +
            "Please not that your rule has to implement SV_iValidationRule interface.";


        [SerializeField] SA_AnimatedFoldoutBlock m_convetionRulesBlock;
        [SerializeField] SA_AnimatedFoldoutBlock m_convetionResolversBlock;


        public override void OnAwake() {
            m_convetionRulesBlock = new SA_AnimatedFoldoutBlock(new GUIContent("Validation Rules"));
            m_convetionResolversBlock = new SA_AnimatedFoldoutBlock(new GUIContent("Resolvers"));
        }

        public override void OnGUI() {

            Properties();
            DefaultRules();

            CustomRules();

        }

        private void Properties() {

            EditorGUI.BeginChangeCheck();

            var userSettings = SV_Settings.UserSettings; 
            using (new SA_WindowBlockWithSpace(new GUIContent("API"))) { 
                userSettings.ValidationEnabled = SA_EditorGUILayout.ToggleFiled("Scene Validation", userSettings.ValidationEnabled, SA_StyledToggle.ToggleType.EnabledDisabled);
                GUI.enabled = userSettings.ValidationEnabled;

                userSettings.AllowIssueIgnore = SA_EditorGUILayout.ToggleFiled("Allow Issue Ignore", userSettings.AllowIssueIgnore, SA_StyledToggle.ToggleType.EnabledDisabled);
            }


            using (new SA_WindowBlockWithSpace(new GUIContent("Hierarchy UI"))) {
                userSettings.HierarchyIconsAlligment = (SV_IconAlligment)SA_EditorGUILayout.EnumPopup("Hierarchy Icons Alligment", userSettings.HierarchyIconsAlligment);
            }

            using (new SA_WindowBlockWithSpace(new GUIContent("Inspector UI"))) {
                userSettings.DisplayIssuesCount = SA_EditorGUILayout.ToggleFiled("UI Display Issues Count", userSettings.DisplayIssuesCount, SA_StyledToggle.ToggleType.EnabledDisabled);
                userSettings.DisplayIssueRelatedComponent = SA_EditorGUILayout.ToggleFiled("UI Display Related Component", userSettings.DisplayIssueRelatedComponent, SA_StyledToggle.ToggleType.EnabledDisabled);
                userSettings.DisplayRuleClassName = SA_EditorGUILayout.ToggleFiled("UI Display Rule Class Name", userSettings.DisplayRuleClassName, SA_StyledToggle.ToggleType.EnabledDisabled);
            }


            using (new SA_WindowBlockWithSpace(new GUIContent("Scene View"))) {
                userSettings.DisplaySceneViewWindow = SA_EditorGUILayout.ToggleFiled("Scene View Window", userSettings.DisplaySceneViewWindow, SA_StyledToggle.ToggleType.EnabledDisabled);
            }


         

            if (EditorGUI.EndChangeCheck()) {
                SV_Settings.Save();
                SV_Settings.SaveUserSettings(userSettings);
                SV_Validation.API.Restart();
            }
        }


        private void DrawNamingConventionRules() {
            ReorderableListGUI.ListField(SV_Settings.Instance.ConventionRules,
                (Rect position, SV_NamingConventionRule.ConventionRule item) => {
                    //draw item

                    if (item == null) {
                        item = new SV_NamingConventionRule.ConventionRule(string.Empty, SV_NamingConventionRule.StringValidationMethod.Equals);
                    }

                    float enumWidth = 100;

                    var textFieldWidth = new Rect(position);
                    textFieldWidth.width -= enumWidth;

                    var enumRect = new Rect(position);
                    enumRect.width = enumWidth - 5;
                    enumRect.x = textFieldWidth.x + textFieldWidth.width + 5;


                    item.Value = EditorGUI.TextField(textFieldWidth, item.Value);
                    item.Method = (SV_NamingConventionRule.StringValidationMethod)EditorGUI.EnumPopup(enumRect, item.Method);


                    return item;
                },
                () => {

            });
        }


        private void DrawNamingConventionResolvers() {
            ReorderableListGUI.ListField(SV_Settings.Instance.ConventionResolvers, DrawConventionScriptsList, () => {
                GUILayout.Label("No automatic name resolvers yet added", EditorStyles.miniLabel);
            });
        }

        private MonoScript DrawConventionScriptsList(Rect pos, MonoScript itemValue) {
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var assets = EditorGUI.ObjectField(pos, itemValue, typeof(MonoScript), false) as MonoScript;

            EditorGUI.indentLevel = indentLevel;
            return assets;
        }


        private void DefaultRules() {
            using (new SA_WindowBlockWithSpace(new GUIContent("Default Rules"))) {

                EditorGUI.BeginChangeCheck();

                EditorGUILayout.LabelField("Naming Convetion", SA_Skin.LabelBold);


                using(new SA_GuiHorizontalSpace(15)) {
                    m_convetionResolversBlock.OnGUI(() => {
                        DrawNamingConventionResolvers();
                    });

                    m_convetionRulesBlock.OnGUI(() => {
                        DrawNamingConventionRules();
                    });
                }
             
                SV_Settings.Instance.NamingConventionRule = SA_EditorGUILayout.ToggleFiled("", SV_Settings.Instance.NamingConventionRule, SA_StyledToggle.ToggleType.EnabledDisabled);



                EditorGUILayout.LabelField("Missing Reference Detection", SA_Skin.LabelBold);
                SV_Settings.Instance.MissingReferenceDetectionRule = SA_EditorGUILayout.ToggleFiled(string.Empty, SV_Settings.Instance.MissingReferenceDetectionRule, SA_StyledToggle.ToggleType.EnabledDisabled);


                EditorGUILayout.LabelField("Missing Component Detection", SA_Skin.LabelBold);

                using(new SA_GuiEnable(false)) {
                    SA_EditorGUILayout.ToggleFiled(string.Empty, true, SA_StyledToggle.ToggleType.EnabledDisabled);
                }

                if (EditorGUI.EndChangeCheck()) {
                    SV_Validation.API.Restart();
                }
            }
        }

        private void CustomRules() {
            using (new SA_WindowBlockWithSpace(new GUIContent("Custom Rules"))) {
                EditorGUILayout.HelpBox(CUSTOM_RULES_DESC, MessageType.Info);
                using (new SA_GuiBeginHorizontal()) {
                    GUILayout.Space(20);
                    using (new SA_GuiBeginVertical()) {
                        EditorGUI.BeginChangeCheck();
                        ReorderableListGUI.ListField(SV_Settings.Instance.CustomRules, DrawScriptsList, DrawEmptyScripts);
                        if (EditorGUI.EndChangeCheck()) {
                            SV_Validation.API.Restart();
                        }
                    }

                }
            }
        }

     

        private MonoScript DrawScriptsList(Rect pos, MonoScript itemValue) {
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var assets = EditorGUI.ObjectField(pos, itemValue, typeof(MonoScript), false) as MonoScript;
   
            EditorGUI.indentLevel = indentLevel;
            return assets;
        }

        private void DrawEmptyScripts() {
            GUILayout.Label("Add your first custom rule", EditorStyles.miniLabel);
        }

    }
}
