using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SA.Productivity.SceneValidator
{

    /// <summary>
    /// The validation rules interface
    /// </summary>
    public interface SV_iContextValidationRule
    {
        /// <summary>
        /// Draw UI where you desribed the component issue
        /// </summary>
        SV_ResolutionState InspectorGUI(Scene context, List<Component> components);

        /// <summary>
        /// Weill be only called if <see cref="OnInspectorGUI"/> had returened <see cref="SV_ResolutionState.HasResolution"/>
        /// </summary>
        /// <param name="component"></param>
        void Resolve(Scene context, List<Component> components);

        /// <summary>
        /// Validation Rule Display name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Component Type to validate for a current rule
        /// </summary>
        Type ValidatedType { get; }


        /// <summary>
        /// Method will valiate component, and return the validation state result.
        /// </summary>
        /// <param name="component">The component to validate</param>
        SV_ValidationState Validate(Scene context, List<Component> components);
    }
}