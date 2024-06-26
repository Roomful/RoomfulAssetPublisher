using System;
using UnityEngine;

namespace net.roomful.api.settings
{
    public interface IRoomfulSettingsUIService
    {
        void Open(string title, TabbedControlsHolder controlsHolder, Action<ISettingsController> openCallback, Action<ISettingsController> closeCallback);
        void SetDeleteButton(Action onClick);
        void RegisterControlPrefab(string controlName, GameObject controlGameObject);
    }
}
