using System;
using System.Collections.Generic;
using net.roomful.api.textChat;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api
{
    public interface ITextChatMessage
    {
        string Id { get; set; }
        string Guid { get; set; }
        string Type { get; set; }
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
        string UserId { get; set; }
        IUserTemplateSimple User { get; set; }
        string MessageBody { get; set; }
        int MessageType { get; set; }
        bool IsBlocked { get; set; }
        bool IsDeleted { get; set; }
        Dictionary<string, object> ToDictionary();
        void UpdateMessage(ITextChatMessage message);
        TextChatMessageState State { get; set; }
    }
}
