using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard
{
    public class PanelBounds : AssetBounds
    {

        public override bool IsValidForBounds(Renderer renderer) {
            bool isValid = base.IsValidForBounds(renderer);
            if (!isValid) {
                return false;
            }

            if (renderer.GetComponent<StylePanel>() != null) {
                return false; ;
            }

            return true;
        }
    }
}