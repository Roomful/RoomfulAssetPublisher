using UnityEngine;

namespace net.roomful.api.appMenu
{
    /// <summary>
    /// Predefined Button options
    /// </summary>
    public static class ButtonOptions
    {
        /// <summary>
        /// Button with the round mask.
        /// </summary>
        /// <param name="value">Set `true` if rounds mask is needed.</param>
        /// <returns>Instance of button option.</returns>
        public static ButtonOption RoundMask(bool value) => new ButtonOption(ButtonOption.Type.RoundMask, value);

        /// <summary>
        /// Button without an icon.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Instance of button option.</returns>
        public static ButtonOption ClickWithoutIcon(bool value) => new ButtonOption(ButtonOption.Type.ClickWithoutIcon, value);

        /// <summary>
        /// Button with the central counter.
        /// </summary>
        /// <param name="value">Set `true` if central counter is needed.</param>
        /// <returns>Instance of button option.</returns>
        public static ButtonOption CentralCounter(bool value) => new ButtonOption(ButtonOption.Type.CentralCounter, value);

        /// <summary>
        /// Button with the custom background.
        /// </summary>
        /// <param name="color">Custom background color.</param>
        /// <returns>Instance of button option.</returns>
        public static ButtonOption WithBackground(Color color) => new ButtonOption(ButtonOption.Type.BackgroundColor, color);


        /// <summary>
        /// The special white button style.
        /// </summary>
        /// <param name="value">Set `true` if this has to be a white button style.</param>
        /// <returns>Instance of button option.</returns>
        public static ButtonOption WhiteButton(bool value) => new ButtonOption(ButtonOption.Type.WhiteButton, value);
    }
}
