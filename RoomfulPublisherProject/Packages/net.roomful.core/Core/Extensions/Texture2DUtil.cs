using System;
using UnityEngine;

namespace net.roomful.api
{
    public static class Texture2DUtil
    {
        public static Texture2D MakeTextureFromIntersection(Texture2D left, Texture2D right) {
            if (left.width != right.width || left.height != right.height) {
                throw new ArgumentException("Textures dimensions must be equal");
            }

            var resultIcon = new Texture2D(left.width, left.height, TextureFormat.RGBA32, false);
            var leftColors = left.GetPixels32();
            var rightColors = right.GetPixels32();
            var resultColors = new Color32[leftColors.Length];
            for (var i = resultColors.Length - 1; i >= 0; i--) {
                if (leftColors[i].Equals(rightColors[i])) {
                    resultColors[i] = leftColors[i];
                }
                else {
                    resultColors[i] = Color.clear;
                }
            }

            resultIcon.SetPixels32(resultColors);
            resultIcon.Apply();
            return resultIcon;
        }
    }
}