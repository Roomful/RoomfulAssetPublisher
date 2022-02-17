using System.Collections.Generic;

namespace net.roomful.api {
    public interface IVideoChatToken {
        string Identity { get; }
        IUserTemplate User { get;  }
        string RoomId { get; }
        string Token { get; }
        string VideoChatId { get;}
        VideoChatMode VideoChatMode { get; }
        VideoChatEngine VideoChatEngine { get; }
        string VideoChatAppId { get; }
        int UId { get; }
        IConferenceUserPermissions Permissions { get; }
        Dictionary<string, object> ToDictionary();
    }
}