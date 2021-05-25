using System;

namespace net.roomful.api
{
    public interface IVideoChatService
    {
        event Action OnChatStatusUpdatedEvent;
        IVideoChat ActiveChat { get; }
        bool IsChatActive { get; }
        void StopActiveVideoChat();
    }
}