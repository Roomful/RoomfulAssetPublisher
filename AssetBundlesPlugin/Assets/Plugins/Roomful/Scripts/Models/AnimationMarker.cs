using System;
using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace RF.AssetBundles.Serialization {
    
    [Serializable]
    [RequireComponent(typeof(Collider))]
    public class AnimationMarker : MonoBehaviour, IRecreatableOnLoad {

        public BoxCollider Collider;
        public string Key;
    }
}
