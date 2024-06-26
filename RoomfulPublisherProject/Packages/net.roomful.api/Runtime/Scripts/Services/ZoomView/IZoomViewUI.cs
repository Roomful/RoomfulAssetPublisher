using System;
using System.Collections.Generic;
using net.roomful.api.appMenu;
using net.roomful.api.ui;
using UnityEngine;
using UnityEngine.UI;

namespace net.roomful.api.zoom
{
    public interface IZoomViewUITabDelegate
    {
        void OnTabActivated(string tabName);
    }

    public interface IZoomViewUI
    {
        IButtonView NextButton { get; }
        IButtonView PrevButton { get; }
        IButtonView EnterButton { get; }
        IButtonView WebLinkButton { get; }
        IButtonView BackButton { get; }
        IButtonView BackMenuButton { get; }

        /// <summary>
        /// If you want to face your side bar has zoom view tab, you might need that.
        /// </summary>
        Texture2D MainTabIcon { get; }
        Texture2D TeamMembersTabIcon { get; }

        /// <summary>
        /// When button is in the toggle state. See the <see cref="SetButtonToggleState"/>
        /// Button click event won't be fired.
        /// If you need to still get click on toggled button, this is the even you should go with.
        /// It will only be fired if button was clicked while in the toggle state
        /// and context menu for this button wasn't opened.
        /// </summary>
        void AddToggledButtonClickCallback(Action<IButtonView> callback);
        
        IButtonView AddButton(ButtonData data, params ButtonOption[] options);
        IButtonView AddTabButton(ButtonData data);
        IButtonView AddButtonToTab(string tabName, ButtonData data);
        void HideTab(string tabName);
        void ShowTab(string tabName);
        void RemoveButton(IButtonView button);
        void ShowSideMenu(IButtonView button, IEnumerable<ContextMenuButtonData> buttons);
        void ShowSideMenu(IButtonView button, RectTransform customMenuRoot, bool deleteWhenClose = false);
        void HideSideMenu(IButtonView button);
        void SetButtonToggleState(IButtonView button, bool isActive);
        void AddTabDelegate(IZoomViewUITabDelegate @delegate);
        void RemoveTabDelegate(IZoomViewUITabDelegate @delegate);
        void SetButtonAnimatedState(IButtonView button, bool state);
        void AddBottomRightLayoutElement(LayoutElement layoutElement);
    }
}
