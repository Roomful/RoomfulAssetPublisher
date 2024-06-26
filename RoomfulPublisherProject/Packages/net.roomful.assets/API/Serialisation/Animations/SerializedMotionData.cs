using System;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    [Serializable]
    public class SerializedMotionData
    {
        public string Layer;
        public string State;
        public string AnimationName;
        public bool IsInsideBlendTree;
        public string BlendTreeName;
        public int BlendTreeChildIndex;
        public Vector2 BlendTreeChildPosition;
        public bool BlendTreeChildMirrored;
        public float BlendTreeChildTimeScale;
    }
}