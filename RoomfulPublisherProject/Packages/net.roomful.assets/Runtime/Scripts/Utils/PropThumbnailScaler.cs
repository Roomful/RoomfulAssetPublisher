using net.roomful.assets.serialization;
using UnityEngine;

namespace net.roomful.assets
{
    public static class PropThumbnailScaler
    {
        public static Vector3 GetScale(int width, int height, ThumbnailScaleMode scaleMode) {
            float ratio;
            Vector3 scale = Vector3.one;

            switch (scaleMode) {
                case ThumbnailScaleMode.Default:
                    if (width > height) {
                        ratio = height / (float) width;
                        scale = new Vector3(1f, 1f * ratio, 0.01f);
                    }
                    else {
                        ratio = width / (float) height;
                        scale = new Vector3(1f * ratio, 1f, 0.01f);
                    }

                    break;

                case ThumbnailScaleMode.PreserveWidth:
                    if (width > height) {
                        ratio = height / (float) width;
                        scale = new Vector3(1f, 1f * ratio, 0.01f);
                    }
                    else {
                        ratio = width / (float) height;
                        scale = new Vector3(1f, 1f / ratio, 0.01f);
                    }

                    break;

                case ThumbnailScaleMode.PreserveHeight:
                    if (width > height) {
                        ratio = height / (float) width;
                        scale = new Vector3(1f / ratio, 1f, 0.01f);
                    }
                    else {
                        ratio = width / (float) height;
                        scale = new Vector3(1f * ratio, 1f, 0.01f);
                    }

                    break;
            }

            return scale;
        }
    }
}
