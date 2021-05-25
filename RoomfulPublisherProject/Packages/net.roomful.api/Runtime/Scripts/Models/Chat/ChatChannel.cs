using System;

namespace net.roomful.api {

    public class ChatChannel : IChatChannel {

        public enum ChatChannelType {
            Room,
            User
        }

        public string Id { get; set; }
        
        public string InternalId { get; set; }
        
        public string Type { get; set; }
        
        public string Name { get; set; }
        
        public int UnreadCounter { get; set; }
        
        public string Thumbnail { get; set; }
        
        public ChatChannelType ChannelType { get; set; } = ChatChannelType.Room;
        
        public TextChatMode TextChatMode { get; set; }

        public void SetId(string id) {
            Id = id;
        }
        
        public DateTime LastMessageTime { get; set; } = DateTime.MinValue; // timestamp of last message in textchat
    }
}