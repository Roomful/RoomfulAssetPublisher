using System;
using net.roomful.api.assets;
using net.roomful.api.lobby;
using UnityEngine;

namespace net.roomful.api.activity
{
    /// <summary>
    /// The activity notification template.
    /// </summary>
    public interface IActivityNotification : ITemplate
    {
        /// <summary>
        /// Prefab name.
        /// </summary>
        string PrefabName { get; }

        /// <summary>
        /// Prefab game object.
        /// </summary>
        GameObject Prefab { get; }

        /// <summary>
        /// Notification display tet.
        /// </summary>
        string DaysPassedText { get; }

        /// <summary>
        /// Notification message
        /// </summary>
        string Message { get; }
    }

    /// <summary>
    /// Activity Notification UI.
    /// </summary>
    public interface IActivityNotificationUI : ILobbyElement
    {
        /// <summary>
        /// Event fired when delete activity button is clicked.
        /// </summary>
        event Action<IActivityNotificationUI> OnDeleteActivityClicked;

        /// <summary>
        /// Sets activity template to the UI.
        /// </summary>
        /// <param name="template">Target template.</param>
        void SetTemplate(IActivityNotification template);
    }
}
