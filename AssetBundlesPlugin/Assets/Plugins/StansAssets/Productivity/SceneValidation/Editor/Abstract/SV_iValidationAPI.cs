using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SA.Productivity.SceneValidator
{
    /// <summary>
    /// Scene Validation API
    /// </summary>
    public interface SV_iValidationAPI {

       

        /// <summary>
        /// Will restart validation API
        /// All rules will be re-assigned and UI updated
        /// </summary>
        void Restart();

        void ValidateScene(Scene scene, Action callback = null);


        SV_iSceneValidator GetSceneValidator(Scene scene);

        SV_HierarchyUI HierarchyUI { get; }

    }
}