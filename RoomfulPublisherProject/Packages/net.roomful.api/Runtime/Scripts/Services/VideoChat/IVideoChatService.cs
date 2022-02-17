using System;

namespace net.roomful.api
{
    public interface IVideoChatService
    {
        event Action OnChatStatusUpdatedEvent;
        IVideoChat ActiveChat { get; }
        IVideoChatStatus RoomVideoChatStatus { get; }
        bool IsChatActive { get; }

        void StartOrJoinCurrentRoomChat();
        void StopActiveVideoChat();

        void HideControls();
        void RestoreControls();
    }
}
