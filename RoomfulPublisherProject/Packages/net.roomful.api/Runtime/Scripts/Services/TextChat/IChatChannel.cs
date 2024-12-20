﻿using System;
using net.roomful.api.assets;
using net.roomful.api.textChat;

namespace net.roomful.api {

    public interface IChatChannel : ITemplate {
        string InternalId { get; set; }
        string Type { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int UnreadCounter { get; set; }
        string Thumbnail { get; set; }
        ChatChannel.ChatChannelType ChannelType { get; set; }
        TextChatMode TextChatMode { get; set; }   // room/videochat/prop/direct -> enum represantation
        void SetId(string id);
        DateTime LastMessageTime { get; set; } // timestamp of last message in textchat
    }
}
