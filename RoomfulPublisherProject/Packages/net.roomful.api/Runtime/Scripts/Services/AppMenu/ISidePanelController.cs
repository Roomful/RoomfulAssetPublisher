using System;
using System.Collections.Generic;
using net.roomful.api.ui;
using RF.UI.SideMenu;
using UnityEngine;

namespace net.roomful.api.appMenu
{
    public interface ISidePanelController : IPanelView
    {
        event Action<string> OnTabActivated;

        /// <summary>
        /// When button is in the toggle state. See the <see cref="SetButtonToggleState"/>
        /// Button click event won't be fired.
        /// If you need to still get click on toggled button, this is the even you should go with.
        /// It will only be fired if button was clicked while in the toggle state
        /// and context menu for this button wasn't opened.
        /// </summary>
        event Action<IButtonView> OnToggledButtonClick;

        IEnumerable<ISideMenuButtonView> Buttons { get; }

        IButtonView AddButton(string tabName, ButtonData buttonData, params ButtonOption[] options);
        IButtonView AddBottomButton(ButtonData buttonData);
        void SetIconForTab(string tabName, Texture2D icon);
        void RemoveTab(string tabName);
        void HideTab(string tabName);
        void ShowTab(string tabName);
        void ActivateTab(string tabName, bool fireTabActivated = true);
        void SetPriorityTab(string tabName, int priority);
        void HideButtons(string tabName);
        (float indent, Vector3 canvasScale) GetIndent();
        void RemoveButton(IButtonView button);
        void ShowSideMenu(IButtonView button, RectTransform customMenuRoot);
        void ShowSideMenu(IButtonView button, IEnumerable<ContextMenuButtonData> buttons);
        void ShowGridSideMenu(IButtonView button, IEnumerable<ContextMenuButtonData> buttons);
        void HideSideMenu(IButtonView button);
        void SetButtonToggleState(IButtonView button, bool isActive);
        void SetAnimatedState(IButtonView button, bool state);
        Color Color { get; set; }
    }
}
