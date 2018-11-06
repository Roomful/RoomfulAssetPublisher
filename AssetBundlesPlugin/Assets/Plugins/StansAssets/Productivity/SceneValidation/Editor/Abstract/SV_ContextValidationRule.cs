using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SA.Productivity.SceneValidator
{

    /// <summary>
    /// Simplified abstract implementation of <see cref="SV_iContextValidationRule"/>
    /// </summary>
    /// <typeparam name="ComponentType"></typeparam>
    public abstract class SV_ContextValidationRule<ComponentType> : SV_iContextValidationRule where ComponentType : Component
    {

        public abstract SV_ResolutionState OnInspectorGUI(Scene context, List<ComponentType> components);
        public abstract SV_ValidationState OnValidate(Scene context, List<ComponentType> components);
        public abstract void OnResolve(Scene context, List<ComponentType> components);


        public void Resolve(Scene context, List<Component> components) {
            OnResolve(context, Convert(components));
        }

        public SV_ResolutionState InspectorGUI(Scene context, List<Component> components) {
            return OnInspectorGUI(context, Convert(components));
        }

        public SV_ValidationState Validate(Scene context, List<Component> components) {
            return OnValidate(context, Convert(components));
        }


        public Type ValidatedType {
            get {
                return (typeof(ComponentType));
            }
        }

        public virtual string Name {
            get {
                return GetType().Name;
            }
        }

        private List<ComponentType> Convert(List<Component> components) {
            List<ComponentType> cp = new List<ComponentType>();
            foreach (Component component in components) {
                cp.Add((ComponentType)component);
            }

            return cp;
        }
    }
}