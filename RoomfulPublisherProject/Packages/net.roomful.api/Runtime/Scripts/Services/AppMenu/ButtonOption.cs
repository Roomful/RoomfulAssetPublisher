namespace net.roomful.api.appMenu
{
    public sealed class ButtonOption
    {
        public ButtonOption.Type type;
        public object value;

        internal ButtonOption(ButtonOption.Type type, object value)
        {
            this.type = type;
            this.value = value;
        }

        public enum Type
        {
            roundMask,
            clickWithoutIcon,
            centralCounter,
            backgroundColor
        }
    }
}

