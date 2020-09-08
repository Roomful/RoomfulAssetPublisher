using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.Editor
{
    public static class IconManager
    {
        private static readonly Dictionary<Icon, Texture2D> s_icons = new Dictionary<Icon, Texture2D>();
        private static readonly Dictionary<float, Texture2D> s_colorIcons = new Dictionary<float, Texture2D>();

        public static Texture2D GetIcon(Icon icon) {
            if (s_icons.ContainsKey(icon)) {
                return s_icons[icon];
            }

            var tex = Resources.Load(icon.ToString()) as Texture2D;
            if (tex == null) {
                tex = new Texture2D(1, 1);
            }

            s_icons.Add(icon, tex);
            return GetIcon(icon);
        }
    }
}