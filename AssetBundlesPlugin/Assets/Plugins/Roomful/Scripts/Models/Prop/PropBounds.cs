using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard
{

    public class PropBounds : AssetBounds
    {

        private Transform m_silhouetteLayer = null;


        public void SetSilhouetteLayer(Transform silhouetteLayer) {
            m_silhouetteLayer = silhouetteLayer;
        }


        public override bool IsValidForBounds(Renderer renderer) {
            bool isValid = base.IsValidForBounds(renderer);
            if(!isValid) {
                return false;
            }

            if (renderer.transform.IsChildOf(m_silhouetteLayer)) {
                return false; ;
            }

            return true;
        }


    }
}