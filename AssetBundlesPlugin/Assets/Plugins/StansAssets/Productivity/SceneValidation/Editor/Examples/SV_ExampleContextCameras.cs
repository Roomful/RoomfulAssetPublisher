using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Productivity.SceneValidator;
using UnityEngine.SceneManagement;

public class SV_ExampleContextCameras : SV_ContextValidationRule<MeshCollider>, SV_iAutoContextRegistredValidationRule
{
    public override SV_ResolutionState OnInspectorGUI(Scene context, List<MeshCollider> components) {

        EditorGUILayout.HelpBox("Used " + components.Count + " MeshCollider when limit is: " + 2, MessageType.Error);
        return SV_ResolutionState.NoResolution;
    }

    public override void OnResolve(Scene context, List<MeshCollider> components) {
       
    }

    public override SV_ValidationState OnValidate(Scene context, List<MeshCollider> components) {
        if (components.Count >= 3) {
            return SV_ValidationState.Error;
        } else {
            return SV_ValidationState.Ok;
        }
            
    }
}
