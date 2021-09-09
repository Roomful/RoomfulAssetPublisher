using System.Collections.Generic;
using net.roomful.api.ui;
using UnityEngine;

namespace net.roomful.api.appMenu
{
    public interface ICustomizableView
    {
        IButtonView AddButton(ButtonData data, params ButtonOption[] options);
        void RemoveButton(IButtonView button);
        void ShowSideMenu(IButtonView button, RectTransform customMenuRoot);
        void ShowSideMenu(IButtonView button, IEnumerable<ContextMenuButtonData> buttons);
        void SetToggleState(IButtonView button, bool state);
        void HideSideMenu(IButtonView button);
        (float indent, Vector3 canvasScale) GetIndent();
        void SetButtonAnimatedState(IButtonView button, bool state);
    }
}
