using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Productivity.SceneValidator;

public class SV_ExampleCameraValudationMinRule2 : SV_ValidationRuleMinUI<Camera>
{
    public override SV_ValidationInfo GetValidationInfo(Camera component) {
        SV_ValidationInfo info = new SV_ValidationInfo();
        info.Message = "SV Example Camera Valudation Min Rule 2";
        info.ResolutionState = SV_ResolutionState.NoResolution;


        return info;
    }

    public override void OnResolve(Camera component) {
       
    }

    public override SV_ValidationState OnValidate(Camera component) {
        return SV_ValidationState.Error;
    }
}
