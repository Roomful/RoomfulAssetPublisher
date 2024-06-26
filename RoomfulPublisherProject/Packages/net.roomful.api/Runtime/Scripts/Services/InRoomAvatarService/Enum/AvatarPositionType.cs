using System;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api.avatars
{
    /// <summary>
    /// Defines what pose the avatar should take at the positioning point.
    /// </summary>
    [Serializable]
    public enum AvatarPositionType
    {
        /// <summary>
        /// Avatar must sit.
        /// </summary>
        Sitting = 0,

        /// <summary>
        /// The avatar must be standing.
        /// </summary>
        Standing = 1,
        
        /// <summary>
        /// Prop should have AvatarPoseData to fetch Animation clip
        /// </summary>
        SourcedFromProp = 2,
    }
}
