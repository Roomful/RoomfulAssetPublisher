using System;
using net.roomful.api.native;
using UnityEngine.Serialization;

namespace net.roomful.api
{
    public struct VideoChatPropertyUpdatedArgs
    {
        public string PropertyName;
        public IProperty Property;
    }

    public interface IVideoChatNativeUI
    {
        void HideVideoChatControls();
        void ShowVideoChatControls();
    }

    public struct ParticipantUpdateArgs
    {
        /// <summary>
        /// Related videochat id
        /// </summary>
        public string VideochatId;
        
        /// <summary>
        /// Target chat participant
        /// </summary>
        public IChatParticipantModel Participant;
    }
    
    public delegate void VideoChatPresenterChanged(VideochatPresenterChangedEvent @event);
    public delegate void VideoChatCoPresenterChanged(VideochatCoPresenterChangedEvent @event);
    public delegate void VideoChatPropertyUpdated(VideoChatPropertyUpdatedArgs args);
    
    public interface IVideoChatService
    {
        event Action OnChatStatusUpdatedEvent;
        event Action OnActiveChatClosedEvent;
        event Action<IChatParticipantModel> OnVideoChatParticipantUpdated;
        event Action<VideoChatScreenShareStartedArgs> OnVideoChatScreenShareStarted;
        event Action<VideoChatScreenShareEndedArgs> OnVideoChatScreenShareEnded;
        event VideoChatPresenterChanged OnVideoChatPresenterChanged;
        event VideoChatCoPresenterChanged OnVideoChatCoPresenterChanged;
        event VideoChatPropertyUpdated OnVideoChatPropertyUpdated;

        /// <summary>
        /// Event is fired when participant is connected to the chat.
        /// Please note that even will be fired for every chat in the room,
        /// so you need to check <see cref="ParticipantUpdateArgs.VideochatId"/> to handle event properly,
        /// </summary>
        event Action<ParticipantUpdateArgs> OnChatParticipantConnected;
        
        /// <summary>
        /// Event is fired when chat participant is updated.
        /// Please note that even will be fired for every chat in the room,
        /// so you need to check <see cref="ParticipantUpdateArgs.VideochatId"/> to handle event properly,
        /// </summary>
        event Action<ParticipantUpdateArgs> OnChatParticipantUpdatedEvent;
        
        /// <summary>
        /// Event is fired when participant was disconnected from the chat.
        /// Please note that even will be fired for every chat in the room,
        /// so you need to check <see cref="ParticipantUpdateArgs.VideochatId"/> to handle event properly,
        /// </summary>
        event Action<ParticipantUpdateArgs> OnChatParticipantDisconnectedEvent;
        
        IVideoChat ActiveChat { get; }
        IVideoChatStatus RoomVideoChatStatus { get; }
        IVideoChatNativeUI NativeUI { get; }
        bool IsChatActive { get; }
        bool CurrentUserHasScreenShared { get; }
        SharedScreenInfo SharedScreenInfo { get; }

        void StartOrJoinCurrentRoomChat();
        void StopActiveVideoChat();

        void HideControls();
        void RestoreControls();

        void SendMuteAllRequest(string videoChatId, bool muteModerator = false);
        void SendUnmuteAllRequest(string videoChatId, bool unmuteModerator = true);
    }
}
