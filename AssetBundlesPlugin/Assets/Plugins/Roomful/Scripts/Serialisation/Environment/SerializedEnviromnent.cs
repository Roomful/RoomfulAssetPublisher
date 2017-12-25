using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace RF.AssetBundles.Serialization
{
    public class SerializedEnviromnent : MonoBehaviour
    {


        public float AmbientIntensity = 1f;
        public float ReflectionIntensity = 1f;

        public Cubemap ReflectionCubemap = null;
        public byte[] ReflectionCubemapFileData = null;
        public string ReflectionCubemapFileName = string.Empty;
        public SerializedTexture ReflectionCubemapSettings = null;


    }
}