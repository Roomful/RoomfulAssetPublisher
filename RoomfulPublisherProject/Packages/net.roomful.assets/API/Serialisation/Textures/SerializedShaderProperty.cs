using UnityEngine;
using System;

namespace net.roomful.assets.serialization
{
    [Serializable]
    public class SerializedShaderProperty
    {
        public string PropertyName = string.Empty;
        public string PropertyType = string.Empty;

        public Vector4 VectorValue;
        public float FloatValue;
        public SerializedTexture SerializedTextureValue = null;
    }
}