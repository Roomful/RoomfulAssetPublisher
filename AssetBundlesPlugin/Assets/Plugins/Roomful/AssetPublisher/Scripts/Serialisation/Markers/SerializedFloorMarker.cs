using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetWizzard;

namespace RF.AssetBundles.Serialization
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

        public BoxCollider FloorCollider {
            get {
                BoxCollider c = gameObject.GetComponent<BoxCollider>();
                if(c == null) {
                    c = gameObject.AddComponent<BoxCollider>();
                }

                return c;
            }
        }

       
        public PropAsset Prop {
            get {
                Transform go = gameObject.transform;
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