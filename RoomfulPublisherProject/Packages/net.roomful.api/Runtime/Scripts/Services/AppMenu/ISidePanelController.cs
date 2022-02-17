using System;
using System.Collections.Generic;
using net.roomful.api.colorization;
using net.roomful.api.ui;
using UnityEngine;

namespace net.roomful.api.appMenu
{
    /// <summary>
    /// Provides API to work with side panel UI.
    /// </summary>
    public interface ISidePanelController : IPanelView, IColorizationSubject
    {
        /// <summary>
        /// Action is fired when tab is activated and contains name of the activated tab.
        /// </summary>
        event Action<string> OnTabActivated;

        /// <summary>
        /// When button is in the toggle state. See the <see cref="SetButtonToggleState"/>
        /// Button click event won't be fired.
        /// If you need to still get click on toggled button, this is the even you should go with.
        /// It will only be fired if button was clicked while in the toggle state
        /// and context menu for this button wasn't opened.
        /// </summary>
        event Action<IButtonView> OnToggledButtonClick;

        /// <summary>
        /// List of the panel buttons.
        /// </summary>
        IEnumerable<ISideMenuButtonView> Buttons { get; }

        /// <summary>
        /// Add button to the panel.
        /// </summary>
        /// <param name="tabName">Name of the tab where button should be added.</param>
        /// <param name="buttonData">Data of the new button.</param>
        /// <param name="options">Button options.</param>
        /// <returns>Instance of the button view model.</returns>
        IButtonView AddButton(string tabName, ButtonData buttonData, params ButtonOption[] options);

        /// <summary>
        /// Adds button to the bottom of the panel
        /// </summary>
        /// <param name="buttonData">Data of the new button.</param>
        /// <returns>Instance of the button view model.</returns>
        IButtonView AddBottomButton(ButtonData buttonData);

        /// <summary>
        /// Sets icon for a tab.
        /// </summary>
        /// <param name="tabName">The of the new tab.</param>
        /// <param name="icon">New tab icon.</param>
        void SetIconForTab(string tabName, Texture2D icon);

        /// <summary>
        /// Removes tab from the side panel.
        /// </summary>
        /// <param name="tabName">The name of the tab that has to be removed.</param>
        void RemoveTab(string tabName);

        /// <summary>
        /// Hides tab from the side panel.
        /// </summary>
        /// <param name="tabName">The name of the tab that you want hide.</param>
        void HideTab(string tabName);

        /// <summary>
        /// Shows tab from the side panel.
        /// </summary>
        /// <param name="tabName">The name of the tab that you want show.</param>
        void ShowTab(string tabName);

        /// <summary>
        /// Activate tab. Makes tab selected.
        /// The same if user will click on a tab.
        /// </summary>
        /// <param name="tabName">Name of the tab that you want to activate.</param>
        /// <param name="fireTabActivated">Defines if <see cref="OnTabActivated"/> will be fired.</param>
        void ActivateTab(string tabName, bool fireTabActivated = true);

        /// <summary>
        /// Sets tab priority.
        /// Priority defines tab position among other side panel tabs.
        /// Higher priority will appear higher in the ui.
        /// </summary>
        /// <param name="tabName">Name of the target tab.</param>
        /// <param name="priority">Int priority number.</param>
        void SetPriorityTab(string tabName, int priority);

        /// <summary>
        /// Hide all buttons in the tab.
        /// </summary>
        /// <param name="tabName">Target tab.</param>
        void HideButtons(string tabName);

        /// <summary>
        /// Get view indent.
        /// </summary>
        /// <returns>Returns indent and canvasScale.</returns>
        (float indent, Vector3 canvasScale) GetIndent();

        /// <summary>
        /// Remove button.
        /// </summary>
        /// <param name="button">Button to remove.</param>
        void RemoveButton(IButtonView button);

        /// <summary>
        /// Shoe side menu near the button.
        /// </summary>
        /// <param name="button">Target button.</param>
        /// <param name="customMenuRoot">Custom root transform for the side menu.</param>
        /// <param name="deleteWhenClose">Defines if custom menu object should be deleted on close.</param>
        void ShowSideMenu(IButtonView button, RectTransform customMenuRoot, bool deleteWhenClose = false);

        /// <summary>
        /// Show default side menu for the button.
        /// </summary>
        /// <param name="button">Target button.</param>
        /// <param name="buttons">Menu options list.</param>
        void ShowSideMenu(IButtonView button, IEnumerable<ContextMenuButtonData> buttons);

        /// <summary>
        /// Show default gird-like side menu for the button.
        /// </summary>
        /// <param name="button">Target button.</param>
        /// <param name="buttons">Menu options list.</param>
        void ShowGridSideMenu(IButtonView button, IEnumerable<ContextMenuButtonData> buttons);

        /// <summary>
        /// Returns `true` if button has side menu.
        /// </summary>
        /// <param name="button">Target button.</param>
        void HideSideMenu(IButtonView button);

        /// <summary>
        /// Set toggle state for a button.
        /// </summary>
        /// <param name="button">Target button.</param>
        /// <param name="isActive">The toggle state.</param>
        void SetButtonToggleState(IButtonView button, bool isActive);

        /// <summary>
        /// Use to check if button is in toggle state.
        /// </summary>
        /// <param name="button">Target button.</param>
        /// <returns>Returns `true` if button is toggled, `false` otherwise.</returns>
        bool IsButtonToggled(IButtonView button);

        /// <summary>
        /// Set button animated state.
        /// When set to `true` button will blink.
        /// </summary>
        /// <param name="button">Target button.</param>
        /// <param name="state">Set `true` to enable animation and `false` to disable it.</param>
        void SetAnimatedState(IButtonView button, bool state);

        /// <summary>
        /// Makes button background white in the panel.
        /// </summary>
        void SetButtonsPanelBackgroundToWhite();

        /// <summary>
        /// Makes button background dark in the panel.
        /// </summary>
        void SetButtonsPanelBackgroundToDark();

        /// <summary>
        /// Show panel with animation.
        /// </summary>
        void ShowAnimated();

        /// <summary>
        /// Hide panel with animation.
        /// </summary>
        void HideAnimated();

        /// <summary>
        /// Current panel color.
        /// </summary>
        Color Color { get; set; }
    }
}
