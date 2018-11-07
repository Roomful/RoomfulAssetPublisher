using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.Productivity.SceneValidator
{
    public class SV_MissingReferenceDetectionRule : SV_ValidationRule<Component>
    {

        public override SV_ValidationState OnValidate(Component component) {
      
            if(FindMissingReference(component) != null) {
                return SV_ValidationState.Error;
            }

            return SV_ValidationState.Ok;
        }

        public override SV_ResolutionState OnInspectorGUI(Component component) {

            SerializedProperty property = FindMissingReference(component);
            EditorGUILayout.HelpBox("Missing Reference Detected: \nproperty: " +
               property.propertyPath, MessageType.Error);


            return SV_ResolutionState.NoResolution;
        }

        public override void OnResolve(Component component) {}

        private SerializedProperty FindMissingReference(Component component) {

            SerializedObject serializedObject = new SerializedObject(component);
            SerializedProperty property = serializedObject.GetIterator();

            //var it  = prop.GetEnumerator();
            while (property.NextVisible(true)) {
                //This is Missing Reference
                if (property.propertyType == SerializedPropertyType.ObjectReference) {
                    if(property.objectReferenceValue == null && property.objectReferenceInstanceIDValue != 0) {
                        return property;
                    }
                }
            }

            return null;
        }


        public override string Name {
            get {
                return "Missing Reference";
            }
        }
    }
}