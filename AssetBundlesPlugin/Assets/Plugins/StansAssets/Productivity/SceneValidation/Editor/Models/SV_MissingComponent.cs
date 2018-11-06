using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SA.Productivity.SceneValidator
{

    /// <summary>
    /// Dummy component that used as replacment to a null component on gameobject.
    /// Do not refer to any fields of that component.
    /// </summary>
    public class SV_MissingComponent : Component
    {
        public SV_MissingComponent() {

        }
    }
}