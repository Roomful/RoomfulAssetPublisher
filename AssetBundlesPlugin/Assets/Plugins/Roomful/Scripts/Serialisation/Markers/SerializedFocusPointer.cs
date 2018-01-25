using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RF.AssetWizzard;

namespace RF.AssetBundles.Serialization
{
    [ExecuteInEditMode]
    [AddComponentMenu("Roomful/Focus Pointer")]
    public class SerializedFocusPointer : MonoBehaviour, IRecreatableOnLoad
    {

        protected  void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.04f);
        }
    }
}
