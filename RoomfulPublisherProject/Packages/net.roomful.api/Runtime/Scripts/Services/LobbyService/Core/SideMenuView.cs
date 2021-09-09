using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace net.roomful.api.lobby
{
    public class SideMenuView : ISideMenuView
    {
        private readonly List<ILobbyButtonTemplate> m_buttons = new List<ILobbyButtonTemplate>();

        public ReadOnlyCollection<ILobbyButtonTemplate> Buttons => m_buttons.AsReadOnly();

        public void AddCustomButton(ILobbyButtonTemplate buttonTemplate) {
            if (m_buttons.Contains(buttonTemplate)) {
                Debug.LogWarning($"{buttonTemplate.Title} button already registered in ISideMenuView");
                return;
            }

            m_buttons.Add(buttonTemplate);
        }

        public void RemoveCustomButton(ILobbyButtonTemplate buttonTemplate) {
            if (!m_buttons.Contains(buttonTemplate)) {
                Debug.LogWarning($"{buttonTemplate.Title} button is not registered in ISideMenuView");
                return;
            }

            m_buttons.Remove(buttonTemplate);
        }
    }
}