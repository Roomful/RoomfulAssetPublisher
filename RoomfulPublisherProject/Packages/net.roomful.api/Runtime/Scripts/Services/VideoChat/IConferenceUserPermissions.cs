using System.Collections.Generic;

namespace net.roomful.api {
    public interface IConferenceUserPermissions {
        bool IsModerator { get; }
        bool IsOwner { get; }
        bool IsPanelist { get; }
        
        /// <summary>
        /// IMPORTANT! I added public 'getter' to <see cref="IsPresenter"/> property for only one reason
        /// - we need to set this property, when we want to switch Singer for Karaoke.
        /// BUT <see cref="ConferenceUserPermissions"/> class (the implementation of <see cref="IConferenceUserPermissions"/> interface)
        /// is in Assembly-CSharp and can't be referenced in <see cref="RF.Karaoke.Core"/> assembly.
        /// This has to be refactored after will wil migrate <see cref="VideoChatService"/> to a separate assembly
        /// - Alexey Yaremenko
        /// </summary>
        bool IsPresenter { get; set; }
        bool IsCoPresenter { get; }
        bool IsPromoted { get;}
        bool IsParticipant { get; }
        bool IsListener { get; }

        Dictionary<string, object> ToDictionary();
    }
}