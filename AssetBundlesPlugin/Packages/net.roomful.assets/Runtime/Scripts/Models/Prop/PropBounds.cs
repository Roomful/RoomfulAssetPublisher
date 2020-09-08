using UnityEngine;


namespace net.roomful.assets
{

    public class PropBounds : AssetBounds
    {

        private Transform m_silhouetteLayer = null;


        public void SetSilhouetteLayer(Transform silhouetteLayer) {
            m_silhouetteLayer = silhouetteLayer;
        }


        public override bool IsValidForBounds(Renderer renderer) {
            var isValid = base.IsValidForBounds(renderer);
            if(!isValid) {
                return false;
            }


            if(m_silhouetteLayer != null) {
                if (renderer.transform.IsChildOf(m_silhouetteLayer)) {
                    return false; ;
                }
            }
           

            return true;
        }


    }
}