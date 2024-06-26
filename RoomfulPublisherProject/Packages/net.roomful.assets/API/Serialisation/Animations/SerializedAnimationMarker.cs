using System;
using UnityEngine;
using net.roomful.api.props;

// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.assets.serialization
{
    [Serializable]
    [RequireComponent(typeof(Collider))]
    public class SerializedAnimationMarker : PropComponent, IRecreatableOnLoad
    {
        public BoxCollider Collider;
        public string Key;
        public Transform TargetClickArea;
        public bool IsHidden;

        
        [SerializeField, Tooltip("When enabled, animation will be played when this prop is opened in zoom view. " +
                                 "Animation won't be played on animation area click anymore.")]
        private bool m_playWhenEnteringZoomView;
        
        public Animator Animator { get; set; }

        public bool PlayWhenEnteringZoomView => m_playWhenEnteringZoomView;
    }
}
