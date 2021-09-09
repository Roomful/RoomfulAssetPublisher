
// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    /// <summary>
    /// Describes user position in the room.
    /// </summary>
    public interface IUserRoomPosition {

        /// <summary>
        /// Id of the room.
        /// </summary>
        string RoomId { get; }

        /// <summary>
        /// Property contains id of the prop if user currently in this prop zoom view.
        /// `string.Empty` otherwise.
        /// </summary>
        string PropId { get; }
        IPositionTemplate Position { get; }
    }
}
