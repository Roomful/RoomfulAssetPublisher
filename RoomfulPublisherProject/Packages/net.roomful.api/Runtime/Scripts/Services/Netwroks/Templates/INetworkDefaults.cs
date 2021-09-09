using System.Collections.Generic;

namespace net.roomful.api.networks
{
    public interface INetworkDefaults
    {
        bool HasExpo { get; }
        bool AllUsersAreConnected { get; }
        bool HasGamingBalance { get; }
        bool HideTextChat { get; }
        bool HideVideoChat { get; }
        string DefaultRoomId { get; }
        bool DisableFavoriteRooms { get; }
        IReadOnlyList<string> AvatarsTags { get; }
        int GamificationParticlesIndex { get; }
        bool DisableShareRoom  { get; }
    }
}
