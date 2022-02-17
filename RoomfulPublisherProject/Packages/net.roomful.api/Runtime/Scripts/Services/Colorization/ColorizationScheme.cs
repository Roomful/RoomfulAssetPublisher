using UnityEngine;

namespace net.roomful.api.colorization
{
    public abstract class ColorizationScheme
    {
        public abstract ColorizationSchemeType SchemeType { get; }

        public ColorsCollection DarkColors { get; protected set; }
        public ColorsCollection LightColors { get; protected set; }
        public ColorsCollection NavigationButtons { get; protected set; }

        protected static readonly Color s_defaultDark = new Color(0.1843137f, 0.1843137f, 0.1843137f);
        protected static readonly Color s_defaultBlue = new Color(0.07843138f, 0.6980392f, 0.7372549f);

        protected ColorizationScheme() {
            DarkColors = new ColorsCollection();
            LightColors = new ColorsCollection();
            NavigationButtons = new ColorsCollection();
        }
    }

    public struct ColorsCollection
    {
        public Color DefaultColor;
        public Color SelectionColor;
        public Color DefaultTextColor;
        public Color DefaultIconColor;
        public Color SelectionTextColor;
        public Color SelectionIconColor;
    }
}