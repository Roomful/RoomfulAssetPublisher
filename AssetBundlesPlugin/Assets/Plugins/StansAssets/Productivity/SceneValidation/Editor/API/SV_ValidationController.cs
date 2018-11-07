using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEditor.IMGUI.Controls;

using SA.Foundation.UtilitiesEditor;
using System.Linq;

namespace SA.Productivity.SceneValidator
{
    public class SV_ValidationController : SV_iValidationAPI
    {

        private Dictionary<Scene, SV_iSceneValidator> m_activeValidators = new Dictionary<Scene, SV_iSceneValidator>();

        private Dictionary<Type, List<SV_iValidationRule>> m_rules = new Dictionary<Type, List<SV_iValidationRule>>();
        private Dictionary<Type, List<SV_iContextValidationRule>> m_contextRules = new Dictionary<Type, List<SV_iContextValidationRule>>();


        private SV_HierarchyUI m_hierarchyUI;
        private GameObject[] m_selectedGameObjects;


        //--------------------------------------
        //  SV_iValidationAPI
        //--------------------------------------


        public void Restart() {
            if (!SV_Settings.IsValidationActive) { return; }

            m_rules.Clear();
            m_contextRules.Clear();

            if (SV_Settings.Instance.NamingConventionRule) {
                RegisterValidationRule(new SV_NamingConventionRule());
            }

            if (SV_Settings.Instance.MissingReferenceDetectionRule) {
                RegisterValidationRule(new SV_MissingReferenceDetectionRule());
            }

            //Registring custom rules
            foreach (var script in SV_Settings.Instance.CustomRules) {
                if (script != null) {
                    try {
                        var rule = Activator.CreateInstance(script.GetClass());
                        if ((rule is SV_iValidationRule)) {
                            RegisterValidationRule(rule as SV_iValidationRule);
                        }
                    } catch (Exception ex) {
                        Debug.LogWarning("Failed to register rule from the " + script.name + " script.");
                        Debug.LogWarning(ex.Message);
                    }
                }
            }

            //Registering custom rules via code
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies) {
#pragma warning disable 168
                try {
                    foreach (Type type in assembly.GetTypes()) {
                        if (type.GetInterface(typeof(SV_iAutoRegistredValidationRule).FullName) != null) {
                            var rule = Activator.CreateInstance(type);
                            RegisterValidationRule(rule as SV_iValidationRule);
                        }

                        if (type.GetInterface(typeof(SV_iAutoContextRegistredValidationRule).FullName) != null) {
                            var rule = Activator.CreateInstance(type);
                            RegisterContextValidationRule(rule as SV_iContextValidationRule);
                        }
                    }
                } catch(Exception ex) {
                    //Wasn't able to load assebly, probably some cusotm assebly,
                    //we arent intrested in anyway. 
                }
#pragma warning restore 168
            }


            EditorSceneManager.sceneOpened -= OnSceneOpened;
            Selection.selectionChanged -= OnSelectionChanged;


            EditorSceneManager.sceneOpened += OnSceneOpened;
            Selection.selectionChanged += OnSelectionChanged;


            //Make Validators for all the scenes
            for (int i = 0; i < SceneManager.sceneCount; i++) {
                Scene scene = EditorSceneManager.GetSceneAt(i);
                ValidateScene(scene);
            }

            HierarchyUI.Repaint();
        }


        public SV_iSceneValidator GetSceneValidator(Scene scene) {

            if (!SV_Settings.IsValidationActive) { return null; }

            if (!m_activeValidators.ContainsKey(scene)) {
                ValidateScene(scene);
            }

            return m_activeValidators[scene];
        }

        public void ValidateScene(Scene scene, Action callback = null) {

            if (!scene.IsValid() || !scene.isLoaded) {
                return;
            }

            SV_iSceneValidator validator = null;

            //In case n we already have and Active validator for a scene
            if (m_activeValidators.ContainsKey(scene)) {
                validator = m_activeValidators[scene];
            } else {
                //create new one
                validator = new SV_SceneValidator();
                m_activeValidators.Add(scene, validator);
            }

            validator.Validate(scene, m_rules, m_contextRules, () => {
                if(callback != null) {
                    callback.Invoke();
                }
            });
        }


        //--------------------------------------
        //  Validation
        //--------------------------------------



        public void ValidateGameObject(GameObject go) {
            var validator = GetSceneValidator(go.scene);
            validator.ValidateGameObject(go);
        }

        public void ValidateRecursively(GameObject go) {
            var validator = GetSceneValidator(go.scene);
            validator.ValidateRecursively(go);
        }

        private void ValidateSceneContext() {
            foreach (var pair in m_activeValidators) {
                pair.Value.ValidateSceneContext();
            }
        }

       

        //--------------------------------------
        //  Unity Events
        //--------------------------------------

        private void OnSceneOpened(Scene scene, OpenSceneMode mode) {

            if (!SV_Settings.IsValidationActive) { return; }

            //We just want to make sure that all the scene initialization things are completed
            EditorApplication.delayCall += () => {
                ValidateScene(scene);
            };
        }



        private void OnSelectionChanged() {
            if (m_selectedGameObjects != null) {
                foreach (var go in m_selectedGameObjects) {
                    //could be deleted
                    if (go == null) { continue; }

                    //we only care about pbjects on scene, not on dis (project view selected prefabs)
                    if (EditorUtility.IsPersistent(go)) { continue; }

                    //Let's see if that is a new object. That wasn't here when scene was loaded.
                    //In this case we can say that all childs of this object are also new
                    int localId = SA_AssetDatabase.GetLocalIdentifierInFile(go);
                    if (localId == 0) {
                        ValidateRecursively(go);
                    } else {
                        ValidateGameObject(go);
                    }
                }
                ValidateSceneContext();
            }

            m_selectedGameObjects = Selection.gameObjects;

        }



        //--------------------------------------
        // Get / set
        //--------------------------------------

        public SV_HierarchyUI HierarchyUI {
            get {
                if (m_hierarchyUI == null) {
                    m_hierarchyUI = new SV_HierarchyUI();
                }
                return m_hierarchyUI;
            }
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------

       

        private void RegisterValidationRule(SV_iValidationRule rule) {

            List<SV_iValidationRule> typeRules;
            if (m_rules.ContainsKey(rule.ValidatedType)) {
                typeRules = m_rules[rule.ValidatedType];
            } else {
                typeRules = new List<SV_iValidationRule>();
                m_rules.Add(rule.ValidatedType, typeRules);
            }

            typeRules.Add(rule);
        }


        private void RegisterContextValidationRule(SV_iContextValidationRule rule) {

            List<SV_iContextValidationRule> typeRules;
            if (m_contextRules.ContainsKey(rule.ValidatedType)) {
                typeRules = m_contextRules[rule.ValidatedType];
            } else {
                typeRules = new List<SV_iContextValidationRule>();
                m_contextRules.Add(rule.ValidatedType, typeRules);
            }

            typeRules.Add(rule);
        }

    }
}