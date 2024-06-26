using System;
using net.roomful.api.appMenu;
using UnityEngine;

namespace net.roomful.uiSynchronizer
{
    public interface IUISynchronizerService
    {
        void RegisterSidePanel(string name, ISidePanelController sidepanel);
        ISynchronizerUnsubscriber RegisterSubject(IUISynchronizedSubject subject);
        void MinimizeUI(bool isMinimize, UIComponents components = UIComponents.All);
        Vector3 GetCanvasScale();
        float GetMaxPadding();
        (float indent, Vector3 canvasScale) GetTextChatPadding();
        void HideNativeVideoChat();
        void ShowNativeVideoChat();
    }

    public enum UIComponents { All, LeftMenu }

    public interface ISynchronizerUnsubscriber : IDisposable { }
}

