using UnityEngine;

namespace net.roomful.api.appMenu
{
    public static class ButtonOptions
    {
        public static ButtonOption RoundMask(bool value) => new ButtonOption(ButtonOption.Type.roundMask, value);

        public static ButtonOption ClickWithoutIcon(bool value) => new ButtonOption(ButtonOption.Type.clickWithoutIcon, value);

        public static ButtonOption CentralCounter(bool value) => new ButtonOption(ButtonOption.Type.centralCounter, value);

        public static ButtonOption WithBackground(Color color) => new ButtonOption(ButtonOption.Type.backgroundColor, color);
    }
}
