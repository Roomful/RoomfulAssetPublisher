using System;
using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public class TextChat : ITextChat {

        public string Id { get; } = string.Empty;
        public DateTime Created { get; } = DateTime.MinValue;
        public DateTime Updated { get; } = DateTime.MinValue;
        public string Mode { get; } = string.Empty;// room/videochat/prop/direct
        public TextChatMode TextChatMode { get; set; }// room/videochat/prop/direct -> enum represantation
        public ITextChatSource Source { get; } // object textchat is related to
        public DateTime LastMessageTime { get; set; } = DateTime.MinValue; // timestamp of last message in textchat
        public DateTime LastReadMessageTime { get; } = DateTime.MinValue; // timestamp of last read message in textchat
        public int UnreadMessagesCount { get; set; }
        public int PaginationOffset { get; set; }
        public bool HasReceivedAllItems { get; set; }
        public bool HasReceivedAllUnreadItems { get; set; }
        public ITextChatOpponent OpponentUser { get; set; }
        public List<ITextChatMessage> Messages { get; set; } = new List<ITextChatMessage>();

        public TextChat(string textChatId) {
            Id = textChatId;
            Created = DateTime.Now;
        }

        public TextChat(ITextChat textChat) {
            Id = textChat.Id;
            Created = textChat.Created;
            Updated = textChat.Updated;
            Mode = textChat.Mode;
            Enum.TryParse(Mode, true, out TextChatMode tempMode);
            TextChatMode = tempMode;
            Source = textChat.Source;
            LastMessageTime = textChat.LastMessageTime;
            LastReadMessageTime = textChat.LastReadMessageTime;
            UnreadMessagesCount = textChat.UnreadMessagesCount;
            HasNewMessage = LastReadMessageTime.CompareTo(LastMessageTime) < 0;
            OpponentUser = textChat.OpponentUser;
        }

        public TextChat(JSONData data) {
            if (data.HasValue("id")) {
                Id = data.GetValue<string>("id");
            }
            if (data.HasValue("created")) {
                Created = data.GetValue<DateTime>("created");
            }
            if (data.HasValue("updated")) {
                Updated = data.GetValue<DateTime>("updated");
            }
            if (data.HasValue("textchatMode")) {
                Mode = data.GetValue<string>("textchatMode");
                Enum.TryParse(Mode, true, out TextChatMode tempMode);
                TextChatMode = tempMode;
            }
            if (data.HasValue("textchatSource")) {
                Source = new TextChatSource(new JSONData(data.GetValue<Dictionary<string, object>> ("textchatSource")));
            }
            if (data.HasValue("lastMessageTime")) {
                LastMessageTime = data.GetValue<DateTime>("lastMessageTime");
            }
            if (data.HasValue("lastReadMessageTime")) {
                LastReadMessageTime = data.GetValue<DateTime>("lastReadMessageTime");
            }
            if (data.HasValue("unreadMessageCount")) {
                UnreadMessagesCount = data.GetValue<int>("unreadMessageCount");
            }
            
            HasNewMessage = LastReadMessageTime.CompareTo(LastMessageTime) < 0;
        }
 
        public void Clear() {
            Messages.Clear();
        }

        public bool HasMoreMessage => !HasReceivedAllItems;
        
        public bool HasNewMessage { get; set; }
    }
}
