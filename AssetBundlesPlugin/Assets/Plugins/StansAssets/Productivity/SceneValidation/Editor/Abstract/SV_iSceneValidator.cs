using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


namespace SA.Productivity.SceneValidator
{
    public interface SV_iSceneValidator 
    {

        void ResolveAll();
        void ValidateRecursively(GameObject go);
        void ValidateSceneContext();
        SV_ValidationState GetGameObjectState(GameObject go);
        Dictionary<Component, Dictionary<SV_iValidationRule, SV_ValidationState>> ValidateGameObject(GameObject go, SV_ValidationPerfomanceReport report = null);
        void Validate(Scene scene, Dictionary<Type, List<SV_iValidationRule>> rules, Dictionary<Type, List<SV_iContextValidationRule>> contextRules, Action callback);

        List<int> GetParentIssues(int parentInstanceId);
        SV_ValidationState GetGameobjectnValidationState(GameObject go);
        SV_ValidationState GetChildrenValidationState(GameObject go);
        List<Component> GetComponentsCache(Type type);


        SV_ValidationPerfomanceReport ValidationReport { get; }
        Dictionary<SV_iContextValidationRule, SV_ValidationState> ContextIssues { get; }
        Dictionary<int, Dictionary<Component, Dictionary<SV_iValidationRule, SV_ValidationState>>> GameObjectsIssues { get; }

        Scene Scene { get; }



    }
}