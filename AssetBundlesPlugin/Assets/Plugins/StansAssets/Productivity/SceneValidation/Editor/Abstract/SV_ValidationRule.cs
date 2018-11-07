using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using SA.Foundation.Editor;

namespace SA.Productivity.SceneValidator
{

    /// <summary>
    /// Simplified abstract implementation of <see cref="SV_iValidationRule"/>
    /// </summary>
    /// <typeparam name="ComponentType"></typeparam>
    public abstract class SV_ValidationRule<ComponentType> : SV_iValidationRule where ComponentType : Component
    {

        public abstract SV_ResolutionState OnInspectorGUI(ComponentType component);
        public abstract SV_ValidationState OnValidate(ComponentType component);
        public abstract void OnResolve(ComponentType component);


        public void Resolve(Component component) {
            OnResolve((ComponentType)component);
        }

        public void InspectorGUI(Component component) {

            if(SV_Settings.UserSettings.DisplayRuleClassName) {
                EditorGUILayout.LabelField(Name, SA_Skin.LabelBold);
            }
           

            SV_ResolutionState resolutionState =  OnInspectorGUI((ComponentType)component);

            using (new SA_GuiBeginHorizontal()) {

                GUILayout.FlexibleSpace();
                if (resolutionState == SV_ResolutionState.HasResolution) {
                    bool click = GUILayout.Button("Resolve", EditorStyles.miniButton, GUILayout.Width(70));
                    if (click) {
                        Resolve(component);
                        EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
                    }
                }

                if (SV_Settings.UserSettings.AllowIssueIgnore) {
                    bool ignore = GUILayout.Button("Ignore", EditorStyles.miniButton, GUILayout.Width(70));
                    if (ignore) {
                        SV_IgnoreManager.Ignore(this, component);
                    }
                }
            }
        }

        public SV_ValidationState Validate(Component component) {
           return  OnValidate((ComponentType)component);
        }


        public Type ValidatedType {
            get {
                return (typeof(ComponentType));
            }
        }

        public virtual string Name {
            get {
                return  GetType().Name;
            }
        }
    }
}