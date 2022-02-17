using System.Collections.Generic;
using net.roomful.api.colorization;

namespace net.roomful.api.networks
{
    public interface INetworkDefaults
    {
        bool HasExpo { get; }
        bool AllUsersAreConnected { get; }
        bool HasGamingBalance { get; }
        bool HideTextChat { get; }
        bool HideVideoChat { get; }
        bool HasCascadeTextchatSkin { get; }
        ColorizationSchemeType ColorizationSchemeType { get; }
        bool AnimatedBackButton { get; }
        string DefaultRoomId { get; }
        bool DisableFavoriteRooms { get; }
        IReadOnlyList<string> AvatarsTags { get; }
        int GamificationParticlesIndex { get; }
        bool DisableShareRoom  { get; }
    }
}
