using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class SerializedEnvironment : MonoBehaviour, IRecreatableOnLoad
    {


        public float AmbientIntensity = 1f;
        public float ReflectionIntensity = 1f;

        public Cubemap ReflectionCubemap = null;
        public byte[] ReflectionCubemapFileData = null;
        public string ReflectionCubemapFileName = string.Empty;
        public SerializedTexture ReflectionCubemapSettings = null;


    }
}