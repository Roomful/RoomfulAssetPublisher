
// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface IUserRoomPosition {
        
        string RoomId { get; }
        string PropId { get; set; }
        IPositionTemplate Position { get; set; }
    }
}
