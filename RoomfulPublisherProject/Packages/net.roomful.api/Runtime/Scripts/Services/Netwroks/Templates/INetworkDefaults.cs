using net.roomful.api.colorization;

namespace net.roomful.api.networks
{
    public interface INetworkDefaults
    {
        /// <summary>
        /// informs clients that the network has expo.
        /// </summary>
        bool HasExpo { get; }
        
        /// <summary>
        /// if true, all users in network are friends by default.
        /// </summary>
        bool AllUsersAreConnected { get; }
        
        /// <summary>
        /// enables gaming balance.
        /// </summary>
        bool HasGamingBalance { get; }
        
        /// <summary>
        /// if true, client shouldn't show textchat UI.
        /// </summary>
        bool HideTextChat { get; }
        
        /// <summary>
        /// if true, client shouldn't show videochat UI
        /// </summary>
        bool HideVideoChat { get; }
        
        /// <summary>
        /// textchat cascade appearance in application.
        /// </summary>
        bool HasCascadeTextchatSkin { get; }
        ColorizationSchemeType ColorizationSchemeType { get; }
        
        /// <summary>
        /// animate back button icon in application.
        /// </summary>
        bool AnimatedBackButton { get; }
        bool DisableFavoriteRooms { get; }
        
        
        /// <summary>
        ///  0 - default, 1 - epam
        /// </summary>
        int GamificationParticlesIndex { get; }
        
        /// <summary>
        /// if true, disables room sharing.
        /// </summary>
        bool DisableShareRoom  { get; }
        
        /// <summary>
        /// if true, users can access network without authorization.
        /// </summary>
        bool AllowAnonymous { get; }
        bool HideSocialSessions { get; }
        
        /// <summary>
        /// enables additional (Open for Attendee) room privacy settings
        /// </summary>
        bool HasAttendees { get; }
    }
}
