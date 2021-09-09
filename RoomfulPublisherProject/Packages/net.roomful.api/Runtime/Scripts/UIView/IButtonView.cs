using System;
using UnityEngine;

namespace net.roomful.api.ui
{
    /// <summary>
    /// Button View interfaces.
    /// </summary>
    public interface IButtonView : IElementView
    {
        /// <summary>
        /// Button click event.
        /// </summary>
        event Action OnClick;

        /// <summary>
        /// Simulate button click.
        /// </summary>
        void SimulateClick();

        /// <summary>
        /// User to set if button is interactable.
        /// </summary>
        bool Interactable { get; set; }

        /// <summary>
        /// Set button text.
        /// </summary>
        /// <param name="text">The preferred button text.</param>
        void SetText(string text);

        /// <summary>
        /// Set color of a button text.
        /// </summary>
        /// <param name="color"></param>
        void SetTextColor(Color color);

        /// <summary>
        /// Set button texture.
        /// </summary>
        /// <param name="texture">Texture for a button.</param>
        void SetIcon(Texture texture);

        /// <summary>
        /// Show counter bubble. If value 0 then bubble hidden.
        /// </summary>
        /// <param name="value">Value to be shown </param>
        void SetCounter(int value);

        void SetCounterColor(Color color);

        /// <summary>
        /// Shows preloader over the button.
        /// Use this method if you need to load a button icon for the <see cref="SetIcon"/> method.
        /// </summary>
        void ShowPreloader();

        /// <summary>
        /// Hides button preloader.
        /// Use this method if you need to load a button icon for the <see cref="SetIcon"/> method.
        /// </summary>
        void HidePreloader();
    }
}
