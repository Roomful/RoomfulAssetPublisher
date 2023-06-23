using UnityEngine;

namespace net.roomful.api
{
    public static class CameraExtensions
    {
        private static readonly int DEFAULT_WIDTH = 256;
        private static readonly int DEFAULT_HEIGHT = 256;
        
        public static Texture2D MakeScreenshotWithBackground(this Camera @this, Color background, int width, int height) {
            @this.clearFlags = CameraClearFlags.SolidColor;
            @this.backgroundColor = background;
            
            var rt = new RenderTexture(width, height, 32);
            var screenshot = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
            @this.targetTexture = rt;
            @this.Render();

            RenderTexture.active = rt;
            screenshot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            screenshot.Apply();
            RenderTexture.active = null;
            rt.DiscardContents();
            return screenshot;
        }
        
        public static Texture2D MakeScreenshotWithBackground(this Camera @this, Color background) {
            return @this.MakeScreenshotWithBackground(background, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }
        
        public static Texture2D MakeScreenshotWithBackground(this Camera @this, Color background, int cullingMask) {
            return @this.MakeScreenshotWithBackground(background, cullingMask, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public static Texture2D MakeScreenshotWithBackground(this Camera @this, Color background, int cullingMask, int width, int height) {
            @this.cullingMask = cullingMask;
            return @this.MakeScreenshotWithBackground(background, width, height);
        }
    }
}