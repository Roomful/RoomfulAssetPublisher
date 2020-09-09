using UnityEngine;

namespace net.roomful.assets.serialization
{

    [ExecuteInEditMode]
    [AddComponentMenu("Roomful/Floor Marker")]
    public class SerializedFloorMarker : MonoBehaviour, IRecreatableOnLoad
    {
 
        protected void OnDrawGizmos() {

            if (Prop == null) {
                return;
            }

            if (!Prop.DrawGizmos) {
                return;
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(FloorCollider.bounds.center, FloorCollider.bounds.size);

        }

        private BoxCollider FloorCollider {
            get {
                var c = gameObject.GetComponent<BoxCollider>();
                if(c == null) {
                    c = gameObject.AddComponent<BoxCollider>();
                }

                return c;
            }
        }

        private PropAsset Prop {
            get {
                var go = gameObject.transform;
                while (go != null) {
                    if (go.GetComponent<PropAsset>() != null) {
                        return go.GetComponent<PropAsset>();
                    }

                    go = go.parent;
                }

                return null;
            }
        }
    }
}