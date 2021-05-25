// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface ITextChatSource {
        
        string RoomId { get; }
        string PropId { get; }
        string VideoChatId { get; }
    }
}