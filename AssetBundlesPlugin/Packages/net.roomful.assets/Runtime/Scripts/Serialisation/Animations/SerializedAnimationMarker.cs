﻿using System;
using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.assets.serialization {
    
    [Serializable]
    [RequireComponent(typeof(Collider))]
    public class SerializedAnimationMarker : MonoBehaviour, IRecreatableOnLoad {

        public BoxCollider Collider;
        public string Key;
        
        
        public bool IsHidden;
        public Animator Animator { get; set; }

    }
}