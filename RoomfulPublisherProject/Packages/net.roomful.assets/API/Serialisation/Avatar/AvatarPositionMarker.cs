using System;
using net.roomful.api.avatars;
using net.roomful.api.props;
using UnityEngine;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.assets.serialization
{
    [Serializable]
    public class AvatarPositionMarker : PropComponent, IRecreatableOnLoad, IAvatarPositionMarker
    {
        [SerializeField] private AvatarPositionType m_positionType = AvatarPositionType.Sitting;
        [SerializeField] private int m_OccupiedById;

        public AvatarPositionType PositionType => m_positionType;
        public Vector3 Position => transform.position;
        public Vector3 Forward => transform.forward;
        public Vector3 EulerAngles => transform.eulerAngles;
        public int OccupiedById
        {
            get => m_OccupiedById;
            set => m_OccupiedById = value;
        }
    }
}
