// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api.activity
{
    /// <summary>
    /// Supported Activity types.
    /// </summary>
    public enum ActivityType
    {
        /// <summary>
        /// Unknown type.
        /// </summary>
        None,

        /// <summary>
        /// Invitation type.
        /// </summary>
        Invitation = 1,

        /// <summary>
        /// Invitation Accepted type.
        /// </summary>
        InvitationAccepted = 2,

        /// <summary>
        /// Invitation Declined type.
        /// </summary>
        InvitationDeclined = 3,

        /// <summary>
        /// Friend Request type.
        /// </summary>
        FriendRequest = 4,

        /// <summary>
        /// Thought Request type.
        /// </summary>
        ThoughtRequest = 9,

        /// <summary>
        /// Room Access Request type.
        /// </summary>
        RoomAccessRequest = 10,

        /// <summary>
        /// Welcome type.
        /// </summary>
        Welcome = 11,

        /// <summary>
        /// Request Accepted type.
        /// </summary>
        RequestAccepted = 12,

        /// <summary>
        /// Request Declined type.
        /// </summary>
        RequestDeclined = 13,

        /// <summary>
        /// Donate Token type.
        /// </summary>
        DonateToken = 14
    }
}
