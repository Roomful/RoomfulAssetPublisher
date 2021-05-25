using net.roomful.api.props;
using UnityEngine;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api.avatars
{
    ///<summary>
    /// Marker describing the position, direction, pose of the avatar.
    ///</summary>
    public interface IAvatarPositionMarker : IPropComponent
    {

        ///<summary>
        /// Avatar pose type.
        ///</summary>
        AvatarPositionType PositionType { get; }

        ///<summary>
        /// Position of the marker.
        ///</summary>
        Vector3 Position { get; }

        ///<summary>
        /// Forward direction of the marker.
        ///</summary>
        Vector3 Forward { get; }
    }
}
