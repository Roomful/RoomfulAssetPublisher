using UnityEngine;

namespace net.roomful.api.ui
{
    /// <summary>
    /// Data to define new button instance properties.
    /// </summary>
    public class ButtonData
    {
        /// <summary>
        /// Button Icon.
        /// </summary>
        public Texture Icon { get; set; }

        /// <summary>
        /// Button Tittle.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Button Propriety.
        /// If container supports it priority will define button order of placement.
        /// </summary>
        public int Priority { get; set; }

        public ButtonData() {
            Title = "";
        }

        public ButtonData(string title, Texture icon, int priority = 1) {
            Icon = icon;
            Title = title;
            Priority = priority;
        }
    }
}
