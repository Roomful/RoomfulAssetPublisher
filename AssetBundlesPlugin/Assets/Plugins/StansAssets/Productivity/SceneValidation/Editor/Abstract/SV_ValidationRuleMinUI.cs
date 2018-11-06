using System;
using System.Collections;
using System.Collections.Generic;
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
    public abstract class SV_ValidationRuleMinUI<ComponentType> : SV_iValidationRule where ComponentType : Component
    {

        public class  SV_ValidationInfo
        {
            public string Message;
            public SV_ResolutionState ResolutionState;
        }

        public abstract SV_ValidationInfo GetValidationInfo(ComponentType component);
        public abstract SV_ValidationState OnValidate(ComponentType component);
        public abstract void OnResolve(ComponentType component);


        public void Resolve(Component component) {
            OnResolve((ComponentType)component);
        }

        public void InspectorGUI(Component component) {

            SV_ValidationInfo validationInfo = GetValidationInfo((ComponentType)component);
            var validationState = Validate(component);


            string message = validationInfo.Message;


            switch (validationState) {
                case SV_ValidationState.Error:
                    EditorGUILayout.HelpBox(message, MessageType.Error);
                    break;
                case SV_ValidationState.Warning:
                    EditorGUILayout.HelpBox(message, MessageType.Warning);
                    break;
            }


            GUILayout.Space(-4);
            using (new SA_GuiBeginHorizontal()) {
                GUILayout.FlexibleSpace();
                if (validationInfo.ResolutionState == SV_ResolutionState.HasResolution) {
                    bool click = GUILayout.Button("Resolve", EditorStyles.miniButton, GUILayout.Width(70));
                    if (click) {
                        Resolve(component);
                        EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
                    }
                }
                
                bool ignore = GUILayout.Button("Ignore", EditorStyles.miniButton, GUILayout.Width(70));
                if (ignore) {
                    SV_IgnoreManager.Ignore(this, component);
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
                return GetType().Name;
            }
        }
    }
}