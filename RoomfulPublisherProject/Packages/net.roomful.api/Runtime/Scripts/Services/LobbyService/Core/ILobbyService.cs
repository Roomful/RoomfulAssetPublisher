using System;
using System.Collections.Generic;

namespace net.roomful.api.lobby
{
    public interface ILobbyService
    {
        void Show(Action callback = null);
        void Show(LobbyPage page, Action callback);
        void Show(LobbyPage page, List<LobbyTab> tabIndexes = null);

        void DisablePage(LobbyPage lobbyPage);
        void EnablePage(LobbyPage lobbyPage);
        void SetDefaultPage(LobbyPage lobbyPage);
    }
}
