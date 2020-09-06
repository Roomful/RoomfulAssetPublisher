using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.serialization {

    [System.Serializable]
    public class SerializedAnimationClip {
        public byte[] ClipData;
        public string AnimationClipName;
    }
}
