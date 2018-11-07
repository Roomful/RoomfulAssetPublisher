using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA.Productivity.SceneValidator
{
    public class SV_MissingComponentDetectionRule : SV_ValidationRule<SV_MissingComponent>
    {

        public override SV_ValidationState OnValidate(SV_MissingComponent component) {
            return SV_ValidationState.Error;
        }

        public override SV_ResolutionState OnInspectorGUI(SV_MissingComponent component) {
            EditorGUILayout.HelpBox("One of game-objects associated scripts can not be loaded",MessageType.Error);
            return SV_ResolutionState.NoResolution;
        }

        public override void OnResolve(SV_MissingComponent component) { }


        public override string Name {
            get {
                return "Missing Component Reference";
            }
        }
    }
}