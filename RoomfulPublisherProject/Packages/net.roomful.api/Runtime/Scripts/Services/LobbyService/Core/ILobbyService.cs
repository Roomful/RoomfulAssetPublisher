using System;
using UnityEngine;

namespace net.roomful.api.lobby
{
    public interface ILobbyService
    {
        event Action OnLobbyOpen;
        void Show(Action callback = null);
        void Show(LobbyPage page, Action callback = null);
        void Show(LobbyPage page, LobbyTab tabIndexes);

        void DisablePage(LobbyPage lobbyPage);
        void EnablePage(LobbyPage lobbyPage);
        void SetDefaultPage(LobbyPage lobbyPage);
        void AddTab(LobbyPage pageId, string tabName, LobbyTab lobbyTabId, GameObject tabPrefab);
    }
}
