using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEditor.IMGUI.Controls;


namespace SA.Productivity.SceneValidator
{

    /// <summary>
    /// Main entry point for the Validator APIs. 
    /// </summary>
    [InitializeOnLoad]
    public static class SV_Validation 
    {
        private static SV_iValidationAPI m_api;


        static SV_Validation() {
            EditorApplication.delayCall += () => {
                API.Restart();
            };
        }


        /// <summary>
        /// Scene Validation API
        /// </summary>
        public static SV_iValidationAPI API {
            get {
                if(m_api == null) {
                    m_api = new SV_ValidationController();
                }

                return m_api;
            }
        }

    }
}