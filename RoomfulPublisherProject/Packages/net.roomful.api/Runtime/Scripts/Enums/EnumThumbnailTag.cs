using System;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api
{
    [Flags]
    public enum EnumThumbnailTag : int
    {
        Room = 1 << 0,
        UI = 1 << 1,
        Notification = 1 << 2,
        Social = 1 << 3,
        Static = 1 << 4,
        Lobby = 1 << 5,
        Avatar = 1 << 6,
        RoomDecoration = 1 << 7,
        RoomStyles = 1 << 8,
        Editor = 1 << 9,

        Max = 1 << 30 // Should be at end and greatest
    }
}
