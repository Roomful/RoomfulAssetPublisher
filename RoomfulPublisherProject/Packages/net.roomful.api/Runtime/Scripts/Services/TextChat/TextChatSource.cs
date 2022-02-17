// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api.textChat {

    public class TextChatSource : ITextChatSource {

        public string RoomId { get; } = string.Empty;
        public string PropId { get; } = string.Empty;
        public string VideoChatId { get; } = string.Empty;

        public TextChatSource(JSONData data) {
            if (data.HasValue("roomId")) {
                RoomId = data.GetValue<string>("roomId");
            }
            if (data.HasValue("propId")) {
                PropId = data.GetValue<string>("propId");
            }
            if (data.HasValue("videochatId")) {
                VideoChatId = data.GetValue<string>("videochatId");
            }
        }

    }
}
