using System;
using UnityEngine;

namespace RF.AssetBundles.Serialization {
    
    [Serializable]
    [RequireComponent(typeof(Collider))]
    public class AnimationMarker : MonoBehaviour {

        public BoxCollider Collider;
        public string Key;
    }
}
