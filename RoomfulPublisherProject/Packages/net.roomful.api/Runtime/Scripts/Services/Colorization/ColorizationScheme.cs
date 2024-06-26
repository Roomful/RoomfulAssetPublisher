using UnityEngine;

namespace net.roomful.api.colorization
{
    public abstract class ColorizationScheme
    {
        public abstract ColorizationSchemeType SchemeType { get; }

        public ColorsCollection DarkColors { get; protected set; }
        public ColorsCollection LightColors { get; protected set; }
        public ColorsCollection NavigationButtons { get; protected set; }

        public ColorsCollection SideMenuTabButtons { get; protected set; }

        public ColorsCollection SideMenuButtons { get; protected set; }

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
        public Color UnHoverColor;
        public Color HoverColor;
    }

    public static class ColorizationExtensions
    {
        public static Color GetAlternativeBackgroundColor(this ColorizationScheme @this) {
            const float multiplier = 0.35f;
            var color = @this.LightColors.DefaultColor;
            return new Color(color.r * multiplier, color.g * multiplier, color.b * multiplier);
        }

        public static Color GetDarkBlueColor(this ColorizationScheme @this) {
            return @this.SchemeType == ColorizationSchemeType.Fabuwood ? new Color(0.1058824f, 0.1215686f, 0.1411765f) : new Color(0.1058824f, 0.1215686f, 0.1411765f);
        }
    }
}