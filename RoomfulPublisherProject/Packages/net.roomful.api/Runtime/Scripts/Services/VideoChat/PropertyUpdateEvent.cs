using System.Collections.Generic;
using net.roomful.api.socket;

namespace net.roomful.api
{
    public class PropertyUpdateEvent : ISocketEventListener
    {
        public string Id => "videochat:action";

        public string PropertyName { get; private set; }

        public string VideoChatId { get; private set; }

        public string RoomId { get; private set; }
        public string Route { get; private set; }

        public Dictionary<string, object> Data { get; private set; }

        public void HandleData(JSONData data) {
            PropertyName = data.GetValue<string>("dataKey");
            VideoChatId = data.GetValue<string>("videochatId");
            RoomId = data.GetValue<string>("roomId");
            Route = data.GetValue<string>("route");
            Data = data.GetValue<Dictionary<string, object>>("data");
        }
    }
}