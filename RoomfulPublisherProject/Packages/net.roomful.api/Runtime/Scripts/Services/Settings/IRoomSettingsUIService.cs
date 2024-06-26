using System;
using UnityEngine;

namespace net.roomful.api.settings
{
    public interface IRoomSettingsUIService : ISettingsUIService
    {
        bool IsOpen { get; }
        event Action OnClose;
        event Action OnOpen;
        void RegisterBuildViewDelegate(IBuildSettingsViewDelegate<IRoomSettingsUIService> buildViewDelegate);
        void RegisterControlPrefab(string controlName, GameObject controlGameObject);
    }
}