// Copyright Roomful 2013-2020. All rights reserved.

using System;

namespace net.roomful.api.textChat
{
    public interface ITextChatOpponent
    {
        IUserTemplate User { get; set; }
        DateTime LastReadMessageTime { get; } // timestamp of last read message in textchat
    }
}
