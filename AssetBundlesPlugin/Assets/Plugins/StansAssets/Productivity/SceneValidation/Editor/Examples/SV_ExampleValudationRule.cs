using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Productivity.SceneValidator;

public class SV_ExampleValudationRule : SV_ValidationRule<BoxCollider>
{
    public override SV_ResolutionState OnInspectorGUI(BoxCollider component) {

        EditorGUILayout.HelpBox("Using of BoxCollider not allowed", MessageType.Error);


        return SV_ResolutionState.HasResolution;
    }

    public override void OnResolve(BoxCollider component) {
        GameObject.DestroyImmediate(component);
    }

    public override SV_ValidationState OnValidate(BoxCollider component) {
        return SV_ValidationState.Error;
    }
}
