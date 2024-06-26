using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class SerializedThumbnail : SerializedThumbnailSettings, IRecreatableOnLoad
    {
        public int XRatio = 1;
        public int YRatio = 1;
    }
}
