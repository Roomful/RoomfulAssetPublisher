using net.roomful.api.ui;
using UnityEngine;

namespace net.roomful.api.appMenu
{
    /// <summary>
    /// Provide additional options for the side button.
    /// </summary>
    public interface ISideMenuButtonView : IButtonView
    {
        /// <summary>
        /// Main button icon.
        /// </summary>
        Texture Icon { get; }

        /// <summary>
        /// Returns `true` if button is toggled, `false` otherwise.
        /// </summary>
        bool IsToggled { get; }
    }
}
