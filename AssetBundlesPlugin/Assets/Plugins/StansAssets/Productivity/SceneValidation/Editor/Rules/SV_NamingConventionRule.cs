using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;

namespace SA.Productivity.SceneValidator
{
    public class SV_NamingConventionRule : SV_ValidationRule<Transform>
    {

        public interface SV_iNamingConventionResolver
        {
            string GetNameSuggestion(Transform transform);
        }

        public enum StringValidationMethod
        {
            Equals,
            StartsWith,
            EndsWith,
            Contains,
            RegEx
        }

        [Serializable]
        public class ConventionRule
        {

            public string Value;
            public StringValidationMethod Method;

            public ConventionRule(string val, StringValidationMethod method) {
                Value = val;
                Method = method;
            }
        }

        public override SV_ValidationState OnValidate(Transform component) {

            foreach(var rule in SV_Settings.Instance.ConventionRules) {

                if(rule == null) {
                    continue;
                }

                string componentName = component.name.ToLower();
                string ruleValue = rule.Value.ToLower();

                bool valid = true;
                switch(rule.Method) {
                    case StringValidationMethod.Contains:
                        valid = !componentName.Contains(ruleValue);
                        break;
                    case StringValidationMethod.StartsWith:
                        valid = !componentName.StartsWith(ruleValue);
                        break;
                    case StringValidationMethod.EndsWith:
                        valid = !componentName.EndsWith(ruleValue);
                        break;
                    case StringValidationMethod.Equals:
                        valid = !componentName.Equals(ruleValue);
                        break;
                }

                if (!valid) {
                    return SV_ValidationState.Warning;
                }
            }
            return SV_ValidationState.Ok;
        }

        public override SV_ResolutionState OnInspectorGUI(Transform component) {
            EditorGUILayout.HelpBox("The '" + component.name + "' is a bad Game Object name. Consider using another one.", MessageType.Warning);

            string nameSuggestion = GetNameSuggestion(component);
            if (!string.IsNullOrEmpty(nameSuggestion)) {
                EditorGUILayout.LabelField("Name Suggestion: " + nameSuggestion, SA_Skin.LabelBold);
                return SV_ResolutionState.HasResolution;
            } else {
                return SV_ResolutionState.NoResolution;
            }
        }

        public override void OnResolve(Transform component) {
            component.name = GetNameSuggestion(component);
        }

        public override string Name {
            get {
                return "Naming Convention";
            }
        }


        private string GetNameSuggestion(Transform component) {
            string nameSuggestion = string.Empty;
            foreach (var script in SV_Settings.Instance.ConventionResolvers) {
                if (script != null) {
                    var scriptInstance = Activator.CreateInstance(script.GetClass());
                    if ((scriptInstance is SV_iNamingConventionResolver)) {

                        SV_iNamingConventionResolver resolver = (SV_iNamingConventionResolver)scriptInstance;
                        nameSuggestion = resolver.GetNameSuggestion(component);
                        if (!string.IsNullOrEmpty(nameSuggestion)) {
                            break;
                        }
                    }
                }
            }

            return nameSuggestion;
        }

    }
}