using System.Collections;
using System.Collections.Generic;
using net.roomful.api.colorization;
using UnityEngine;
using UnityEngine.UI;

namespace net.roomful.api
{
    /// <summary>
    /// Unity Color Extension
    /// </summary>
    public static class ColorExtensions
    {
        public static Color WithAlpha(this Color @this, float alpha) {
            return new Color(@this.r, @this.g, @this.b, alpha);
        }
        
        public static Color Lighter(this Color @this, float additionalLight) {
            return new Color(@this.r + additionalLight, @this.g + additionalLight, @this.b + additionalLight, @this.a);
        }

        public static Color Inactive(this Color @this, float value) {
            return new Color(@this.r + value, @this.g + value,
                @this.b + value, @this.a);
        }

        public static ColorBlock AsColorBlock(this Color @this, float colorMultiplier = 1.0f) {
            Color.RGBToHSV(@this, out var h, out var s, out var v);
            var pressedColor = Color.HSVToRGB(h, s, 1.2f * v);

            var colorBlock = new ColorBlock {
                normalColor = @this,
                pressedColor = pressedColor.WithAlpha(0.8f),
                highlightedColor = pressedColor,
                selectedColor = pressedColor.WithAlpha(0.75f),
                disabledColor = @this.WithAlpha(0.65f),
                colorMultiplier = colorMultiplier
            };
            return colorBlock;
        }

        public static ColorBlock AsColorBlock(this ColorizationScheme @this) {
            var defaultColorBLock = ColorBlock.defaultColorBlock;
            return new ColorBlock {
                normalColor = Color.clear,
                highlightedColor = @this.DarkColors.DefaultColor.WithAlpha(0.25f),
                pressedColor = @this.LightColors.SelectionColor,
                selectedColor = @this.LightColors.SelectionColor,
                disabledColor = defaultColorBLock.disabledColor,
                colorMultiplier =  defaultColorBLock.colorMultiplier
            };
        }
        
        public static ColorBlock AsColorBlockB(this ColorizationScheme @this) {
            var defaultColorBLock = ColorBlock.defaultColorBlock;
            return new ColorBlock {
                normalColor = Color.clear,
                highlightedColor = @this.LightColors.SelectionColor,
                pressedColor = @this.LightColors.SelectionColor.WithAlpha(0.5f),
                selectedColor = @this.LightColors.SelectionColor,
                disabledColor = defaultColorBLock.disabledColor,
                colorMultiplier =  defaultColorBLock.colorMultiplier
            };
        }
        
        public static Dictionary<string, object> ToRGBAStructure(this Color @this) {
            return new Dictionary<string, object> {
                { "r", @this.r },
                { "g", @this.g },
                { "b", @this.b },
                { "a", @this.a }
            };
        }
        
        public static Color Parse(this Color _, JSONData data) {
            return new Color(data.GetValueSafe<float>("r"), data.GetValueSafe<float>("g"), data.GetValueSafe<float>("b"));
        }

        public static Color Invert(this Color @this) {
            return new Color(1.0f - @this.r, 1.0f - @this.g, 1.0f - @this.b, @this.a);
        }
        
        public static Color Desaturate(this Color @this) {
            Color.RGBToHSV(@this, out var h, out var s, out var v);
            var desaturatedColor = Color.HSVToRGB(h, 0f, v, false);
            return desaturatedColor;
        }
        
        /// <summary>
        /// This method made to solve issue when trying to invert grey color.
        /// </summary>
        /// <param name="this">Color to invert.</param>
        /// <returns>Inverted color with clamped to 0 or 1 rgb values, alpha stays the same.</returns>
        public static Color Invert01Rounded(this Color @this) {
            var inverted = @this.Invert();
            return new Color(Mathf.RoundToInt(inverted.r), Mathf.RoundToInt(inverted.g), Mathf.RoundToInt(inverted.b), inverted.a);
        }
    }
}
