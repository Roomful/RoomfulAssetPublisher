using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api
{
    public class UserRoomPosition : IUserRoomPosition
    {
        public string RoomId { get; set; }
        public string PropId { get; set; }
        public IPositionTemplate Position { get; set; }

        public UserRoomPosition() { }

        public UserRoomPosition(JSONData data) {
            if (data.HasValue("roomId")) {
                RoomId = data.GetValue<string>("roomId");
            }

            if (data.HasValue("propId")) {
                PropId = data.GetValue<string>("propId");
            }

            if (data.HasValue("position")) {
                Position = new PositionTemplate(new JSONData(data.GetValue<Dictionary<string, object>>("position")));
            }
        }
    }
}
