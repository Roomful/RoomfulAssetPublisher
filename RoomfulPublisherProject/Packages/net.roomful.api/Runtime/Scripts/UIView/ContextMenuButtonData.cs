using System;
using UnityEngine;

namespace net.roomful.api.ui
{
    /// <summary>
    /// Data to define new context button instance properties.
    /// </summary>
    public class ContextMenuButtonData : ButtonData
    {
        /// <summary>
        /// Button On Click Actions
        /// </summary>
        public Action OnClick { get; set; }
        
        /// <summary>
        /// Additional icon on the right side of the element that can serve as a button. If you do not specify values, then it will not be displayed
        /// <summary>
        public Sprite SecondaryIcon { get; set; }
        
        public Action OnRightSideButtonClick { get; set; }
        
        /// <summary>
        /// Sets the color for the sprite, allows you not to duplicate the same icon for white and black backgrounds
        /// </summary>
        public Color RightSideIconColor { get; set; } = Color.white;
        
        public string RightSideButtonTitle { get; set; }

        /// <summary>
        /// Flag indicates that action should be launched to load the icon
        /// </summary>
        public bool IsNeedLoadIcon { get; set; }
        
        public Action<Action<Texture, bool>> LoadingAction { get; set; }
    }
}
