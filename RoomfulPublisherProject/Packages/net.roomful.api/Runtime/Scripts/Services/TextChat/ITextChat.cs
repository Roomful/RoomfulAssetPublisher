using System;
using System.Collections.Generic;
using net.roomful.api.assets;
using net.roomful.api.textChat;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api.textChat {

    public interface ITextChat : ITemplate {

        List<ITextChatMessage> Messages { get; set; }
        DateTime Created { get; }
        DateTime Updated { get; }
        string Mode { get; }                      // room/videochat/prop/direct
        TextChatMode TextChatMode { get; set; }   // room/videochat/prop/direct -> enum represantation
        ITextChatSource Source { get; }           // object textchat is related to
        DateTime LastMessageTime { get; set; }         // timestamp of last message in textchat
        DateTime LastReadMessageTime { get; }     // timestamp of last read message in textchat
        int UnreadMessagesCount { get; set; }
        int PaginationOffset { get; set; }
        bool HasReceivedAllItems { get; set; }
        bool HasReceivedAllUnreadItems { get; set; }
        void Clear();
        bool HasMoreMessage { get; }
        ITextChatOpponent OpponentUser { get; set; }
        bool HasNewMessage { get; set; }
    }
}
