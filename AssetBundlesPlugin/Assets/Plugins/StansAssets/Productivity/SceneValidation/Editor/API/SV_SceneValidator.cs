using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace SA.Productivity.SceneValidator
{
    public class SV_SceneValidator : SV_iSceneValidator
    {
        private Scene m_scene;
        private SV_ValidationQueue m_validationQueue;
        private GameObject[] m_selectedGameObjects;
        private Dictionary<Type, List<SV_iValidationRule>> m_rules = new Dictionary<Type, List<SV_iValidationRule>>();
        private Dictionary<Type, List<SV_iContextValidationRule>> m_contextRules = new Dictionary<Type, List<SV_iContextValidationRule>>();


        private SV_IssuesCache m_issuesCache = new SV_IssuesCache();
        private SV_SceneComponentsCache m_scenesContextCache = new SV_SceneComponentsCache();

        private Dictionary<SV_iContextValidationRule, SV_ValidationState> m_contextIssues = new Dictionary<SV_iContextValidationRule, SV_ValidationState>();

        private SV_MissingComponentDetectionRule m_missingComponentDetectionRule = new SV_MissingComponentDetectionRule();

        //--------------------------------------
        // SV_iValidationAPI
        //--------------------------------------



        //Go trough all scene gameobjects including disabled onces
        public void Validate(Scene scene, Dictionary<Type, List<SV_iValidationRule>> rules, Dictionary<Type, List<SV_iContextValidationRule>> contextRules, Action callback) {

            m_scene = scene;
            m_rules = rules;
            m_contextRules = contextRules;
            m_validationQueue = new SV_ValidationQueue(this);

            Validate(callback);
        }

        private void Validate(Action callback) {

            m_issuesCache.Clear();
            m_contextIssues.Clear();

            m_validationQueue.Clear();
            foreach (var go in m_scene.GetRootGameObjects()) {
                FillQueue(go, m_validationQueue);
            }


            m_validationQueue.Run(() => {

                //TODO also make a time slice
                m_issuesCache.CleanUp();
                ValidateSceneContext();


                SV_SceneInspector.Repaint();
                callback.Invoke();

            });


           

        }


        public void ResolveAll() {

            foreach(var pair in m_issuesCache.FullIssuesCache) {
                Dictionary<Component, Dictionary<SV_iValidationRule, SV_ValidationState>> components = pair.Value;

                foreach(var componentPair in components) {
                    var component = componentPair.Key;
                    Dictionary<SV_iValidationRule, SV_ValidationState> issues = componentPair.Value;
                    foreach(var issuesPair in issues) {
                        var rule = issuesPair.Key;
                        rule.Resolve(component);
                        EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
                    }
                }
            }
            Validate(() => { });
        }


        public void ValidateSceneContext() {

            m_contextIssues.Clear();
            m_scenesContextCache.CleanUp();

            foreach (var typesPair in m_contextRules) {
                Type type = typesPair.Key;
                List<SV_iContextValidationRule> rules = typesPair.Value;

                List<Component> components = m_scenesContextCache.GetComponents(type);
                if (components == null) { continue; }
                foreach (var rule in rules) {
                    var state = rule.Validate(m_scene, components);
                    if (state != SV_ValidationState.Ok) {
                        m_contextIssues.Add(rule, state);
                    }
                }
            }
        }



        private void FillQueue(GameObject go, SV_ValidationQueue queue) {
            queue.AddGameObject(go);
            foreach (Transform child in go.transform) {
                FillQueue(child.gameObject, queue);
            }
        }


        public void ValidateRecursively(GameObject go) {
            ValidateGameObject(go);
            foreach (Transform child in go.transform) {
                ValidateRecursively(child.gameObject);
            }
        }


        public Dictionary<Component, Dictionary<SV_iValidationRule, SV_ValidationState>> ValidateGameObject(GameObject go, SV_ValidationPerfomanceReport report = null) {
            if(report != null) {
                report.GameobjectsCount++;
            }

            var issues = new Dictionary<Component, Dictionary<SV_iValidationRule, SV_ValidationState>>();
            foreach (Component component in go.GetComponents<Component>()) {
                if (report != null) {
                    report.ComponentsCount++;
                }
                if (component == null) {
                    var dict = new Dictionary<SV_iValidationRule, SV_ValidationState>();
                    dict.Add(m_missingComponentDetectionRule, SV_ValidationState.Error);
                    issues.Add(new SV_MissingComponent(), dict);
                    continue;
                }

                var componentIssues = GetIssuesFromComponent(component);
                if(componentIssues.Count > 0) {
                    issues.Add(component, componentIssues);
                }
            }

            m_issuesCache.SetGameobjectIssues(go, issues);

            return issues;
        }


        public SV_ValidationState GetGameObjectState(GameObject go) {
            return m_issuesCache.GetGameobjectnValidationState(go);
        }


        public SV_ValidationState GetGameobjectnValidationState(GameObject go) {
            return m_issuesCache.GetGameobjectnValidationState(go);
        }

        public SV_ValidationState GetChildrenValidationState(GameObject go) {
            return m_issuesCache.GetChildrenValidationState(go.GetInstanceID());
        }


        public List<int> GetParentIssues(int parentInstanceId) {
            return m_issuesCache.GetParentIssues(parentInstanceId);
        }


        public List<Component> GetComponentsCache(Type type) {
            return m_scenesContextCache.GetComponents(type);
        }

        public SV_ValidationPerfomanceReport ValidationReport {
            get {
                return m_validationQueue.ValidationReport;
            }
        }

        public Dictionary<SV_iContextValidationRule, SV_ValidationState> ContextIssues {
            get {
                return m_contextIssues;
            }
        }


        public Dictionary<int, Dictionary<Component, Dictionary<SV_iValidationRule, SV_ValidationState>>> GameObjectsIssues {
            get {
                return m_issuesCache.FullIssuesCache;
            }
        }

        public Scene Scene {
            get {
                return m_scene;
            }
        }


        //--------------------------------------
        // Private Methods
        //--------------------------------------





        private Dictionary<SV_iValidationRule, SV_ValidationState> GetIssuesFromComponent(Component component, Type type = null, Dictionary<SV_iValidationRule, SV_ValidationState> issues = null, List<SV_iValidationRule> excludes = null) {

            if (type == null) {
                type = component.GetType();
                issues = new Dictionary<SV_iValidationRule, SV_ValidationState>();
                excludes = new List<SV_iValidationRule>();
            }

            if(m_contextRules.ContainsKey(type)) {
                m_scenesContextCache.CacheComponent(type, component);
            }

            if (m_rules.ContainsKey(type)) {
                List<SV_iValidationRule> rules = m_rules[type];

                foreach (var rule in rules) {
                    if (excludes.Contains(rule)) {
                        continue;
                    }

                    if (SV_IgnoreManager.IsRuleIgnored(rule, component)) {
                        continue;
                    }
                    var validateState = rule.Validate(component);
                    if (validateState != SV_ValidationState.Ok) {
                        issues.Add(rule, validateState);
                        excludes.Add(rule);
                    }
                }
            }

            if (type != typeof(Component)) {
                return GetIssuesFromComponent(component, type.BaseType, issues, excludes);
            } else {
                return issues;
            }
        }


        //--------------------------------------
        // UI
        //--------------------------------------



      
    }
}