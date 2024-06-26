namespace net.roomful.api.appMenu
{
    /// <summary>
    /// Button options.
    /// </summary>
    public sealed class ButtonOption
    {
        public readonly Type type;
        public readonly object value;

        /// <summary>
        /// Creates new button options.
        /// </summary>
        /// <param name="type">Options type.</param>
        /// <param name="value">Option value.</param>
        internal ButtonOption(Type type, object value)
        {
            this.type = type;
            this.value = value;
        }

        /// <summary>
        /// Available button types.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Button with round mask.
            /// </summary>
            RoundMask,

            /// <summary>
            /// Click Without Icon.
            /// </summary>
            ClickWithoutIcon,

            /// <summary>
            /// Button contains counter.
            /// </summary>
            CentralCounter,

            /// <summary>
            /// Button has custom background color.
            /// </summary>
            BackgroundColor,

            /// <summary>
            /// This si roomful "white" button.
            /// </summary>
            WhiteButton
        }
    }
}

