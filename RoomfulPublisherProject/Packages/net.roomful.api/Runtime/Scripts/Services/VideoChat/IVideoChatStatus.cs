using System.Collections.Generic;

namespace net.roomful.api
{
    public interface IVideoChatStatus
    {
        /// <summary>
        /// Video chat id.
        /// </summary>
        string VideochatId { get; }

        /// <summary>
        /// Video chat name made based on room name or prop name.
        /// </summary>
        string VideochatName { get; }

        /// <summary>
        /// Video chat mode.
        /// </summary>
        VideoChatMode VideoChatMode { get; }

        /// <summary>
        /// Video chat engine.
        /// </summary>
        VideoChatEngine VideoChatEngine { get; }

        /// <summary>
        /// Video chat source.
        /// </summary>
        VideochatSource VideochatSource { get; }

        /// <summary>
        /// Room where the videochat is being held.
        /// </summary>
        string CurrentRoomId { get; }

        /// <summary>
        /// Room where the videochat was started.
        /// </summary>
        string OriginalRoomId { get; }

        /// <summary>
        /// Id of object chat is related to.
        /// </summary>
        IVideochatSourceInfo VideochatSourceInfo { get; }

        /// <summary>
        /// Property returns `true` if chat is active and false otherwise.
        /// </summary>
        bool IsActive { get; }
        
        bool IsStreamEnabled { get; }

        /// <summary>
        /// Video Chat has active presentation ongoing.
        /// </summary>
        bool IsPresentationActive { get; }

        /// <summary>
        /// Show only presenter in videochat.
        /// </summary>
        bool IsPresenterOnlyMode { get; }

        /// <summary>
        /// Participants are muted by moderator.
        /// </summary>
        bool IsMuted { get; }

        /// <summary>
        /// Amount of identities allowed in videochat (0 means unlimited).
        /// </summary>
        int VideochatLimit { get; }

        /// <summary>
        /// Video chat participants.
        /// </summary>
        IReadOnlyList<IChatParticipantModel> Participants { get; }

        /// <summary>
        /// List of user ids.
        /// </summary>
        List<string> PromotionsRequests { get; }

        /// <summary>
        /// For agora screen sharing
        /// </summary>
        IShareScreenIdentity ShareScreenIdentity { get; }

        /// <summary>
        /// Represents video chat status as json string.
        /// </summary>
        /// <returns>Json string.</returns>
        string ToJson();
    }
}
