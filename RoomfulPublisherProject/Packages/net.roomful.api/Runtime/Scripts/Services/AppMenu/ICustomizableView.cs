using System;
using System.Collections.Generic;
using net.roomful.api.ui;
using UnityEngine;

namespace net.roomful.api.appMenu
{
    /// <summary>
    /// Menu customizable view.
    /// </summary>
    public interface ICustomizableView
    {
        /// <summary>
        /// Add button to the new.
        /// </summary>
        /// <param name="data">Button of the data.</param>
        /// <param name="options">Button option.</param>
        /// <returns></returns>
        IButtonView AddButton(ButtonData data, params ButtonOption[] options);

        /// <summary>
        /// Remove button.
        /// </summary>
        /// <param name="button">Button to remove.</param>
        void RemoveButton(IButtonView button);

        /// <summary>
        /// Shoe side menu near the button.
        /// </summary>
        /// <param name="button">Target button.</param>
        /// <param name="customMenuRoot">Custom root transform for the side menu</param>
        ISideMenuDelegate ShowSideMenu(IButtonView button, RectTransform customMenuRoot);

        /// <summary>
        /// Show default side menu for the button.
        /// </summary>
        /// <param name="button">Target button.</param>
        /// <param name="buttons">Menu options list.</param>
        void ShowSideMenu(IButtonView button, IEnumerable<ContextMenuButtonData> buttons);
        
        /// <summary>
        /// Show default tabbed side menu for the button.
        /// </summary>
        /// <param name="button">Target button.</param>
        /// <param name="items">Menu items list.</param>
        ISideMenuDelegate ShowTabbedSideMenu(IButtonView button, Dictionary<string, List<GameObject>> items);


        /// <summary>
        /// Set toggle state for a button.
        /// </summary>
        /// <param name="button">Target button.</param>
        /// <param name="state">The toggle state.</param>
        void SetToggleState(IButtonView button, bool state);

        /// <summary>
        /// Use to check if button is in toggle state.
        /// </summary>
        /// <param name="button">Target button.</param>
        /// <returns>Returns `true` if button is toggled, `false` otherwise.</returns>
        bool IsButtonToggled(IButtonView button);

        /// <summary>
        /// Returns `true` if button has side menu.
        /// </summary>
        /// <param name="button">Target button.</param>
        void HideSideMenu(IButtonView button);
        
        /// <summary>
        /// Set button animated state.
        /// When set to `true` button will blink.
        /// </summary>
        /// <param name="button">Target button.</param>
        /// <param name="state">Set `true` to enable animation and `false` to disable it.</param>
        void SetButtonAnimatedState(IButtonView button, bool state);

        /// <summary>
        /// Event is fired when toggle button is clicked.
        /// See <see cref="SetToggleState"/> method.
        /// When button in the toggle state it won't generate default click event.
        /// So you can use this action to subscribe to the click event of the toggled button.
        /// </summary>
        event Action<IButtonView> OnToggledButtonClick;
        
        /// <summary>
        /// Tab name
        /// </summary>
        string TabName { get; }
        
        /// <summary>
        /// Tab icon
        /// </summary>
        Texture2D TabIcon { get; }

        void ActivateTab();
    }
}
