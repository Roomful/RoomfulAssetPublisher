using UnityEngine;

namespace net.roomful.assets.serialization
{

	public class SerializedTiledFrame : MonoBehaviour, IRecreatableOnLoad {
        public float FrameOffset = 0f;
        public float BackOffset = 0f;
        public Color FillerColor = Color.white;
    }
}