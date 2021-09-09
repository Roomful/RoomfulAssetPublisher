using System.Collections.ObjectModel;
using UnityEngine;

namespace net.roomful.api.lobby
{
    public enum RoomUserType
    {
        Owner,
        Visitor
    }

    public interface ILobbyButtonTemplate
    {
        Sprite Icon { get; }
        string Title { get; }

        bool IsActiveFor(RoomUserType roomUserType);

        void OnClick();
    }

    public interface ISideMenuView
    {
        ReadOnlyCollection<ILobbyButtonTemplate> Buttons { get; }

        void AddCustomButton(ILobbyButtonTemplate buttonTemplate);
        void RemoveCustomButton(ILobbyButtonTemplate buttonTemplate);
    }
}