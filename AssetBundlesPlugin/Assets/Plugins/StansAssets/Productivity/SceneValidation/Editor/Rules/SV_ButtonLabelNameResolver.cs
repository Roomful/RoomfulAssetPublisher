using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA.Productivity.SceneValidator
{
    public class SV_ButtonLabelNameResolver : SV_NamingConventionRule.SV_iNamingConventionResolver
    {
        public string GetNameSuggestion(Transform transform) {
            if(transform.name.ToLower().Equals("text")) {
                var parent = transform.parent;
                if(parent != null) {
                    if(parent.GetComponent<Button>() != null && !parent.name.ToLower().Equals("button")) {
                        return parent.name + " Label";
                    }
                }
            }

            //no resolution
            return string.Empty;
        }
    }
}