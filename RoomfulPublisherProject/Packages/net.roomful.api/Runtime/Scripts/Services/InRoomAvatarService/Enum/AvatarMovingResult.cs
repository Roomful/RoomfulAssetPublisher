// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api.avatars
{
    /// <summary>
    /// Defines result of the avatar's moving to the destination.
    /// </summary>
    public enum AvatarMovingResult
    {
        /// <summary>
        /// Avatar reached the destination.
        /// </summary>
        ReachedDestination,

        /// <summary>
        /// Avatar did not reach the destination.
        /// </summary>
        CanNotReachedDestination
    }
}
