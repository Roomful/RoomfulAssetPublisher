using UnityEngine;

namespace net.roomful.assets.serialization
{
    public abstract class SerializedThumbnailSettings : MonoBehaviour
    {
        public ThumbnailScaleMode ScaleMode;
        public bool IsBoundToUserAvatar = false;
        public bool IsBoundToResourceIndex = false;
        public int ResourceIndex = 0;
        public bool IsLogo;
    }
}
