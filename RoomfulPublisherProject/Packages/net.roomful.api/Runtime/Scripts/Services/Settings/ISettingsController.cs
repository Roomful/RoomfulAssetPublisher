using System;
using UnityEngine;

namespace net.roomful.api.settings
{
    public interface ISettingsController
    {
        event Action<string> OnTabSelectRequest;
        void AddControl(string tabName, IControlTemplate template);
        void AddTab(string tabName, string label, int priority);
        void RegisterControlPrefab(string controlName, GameObject controlGameObject);
        event Action OnCloseRequest;
        void Close();
        void Open(TabbedControlsHolder controlsHolder, Action onClose);
        void SetDeleteButton(Action onClick);
        void SelectTab(string tabName);
        void HideTab(string tabName);
        void HideControl(string tabName, string controlName);
        void DisableInteraction(string tabName, string controlName);
        string DefaultTabName { get; }
        void SetTitle(string title);
        bool IsOpen { get; }
    }
}