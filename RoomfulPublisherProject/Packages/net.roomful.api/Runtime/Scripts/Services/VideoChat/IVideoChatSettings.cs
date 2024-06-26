// Copyright Roomful 2013-2020. All rights reserved.
using net.roomful.api.room;

namespace net.roomful.api {

    public interface IVideoChatSettings {

        string VideoChatMode { get; set; }
        string VideoChatType { get; set; }
        string VideoChatEngine { get; set; }
        IVideoChatSettingsAutoPromotion AutoPromotion { get; }
        bool IsStreamingEnabled { get; set; }
        bool ExcludeListeners { get; set; }
        bool UsePresentationBoard { get; }
        VideoChatNotificationMode NotificationMode { get; set; }
    }
}
