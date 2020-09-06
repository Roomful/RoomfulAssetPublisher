using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using net.roomful.assets;

namespace net.roomful.assets.serialization
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
