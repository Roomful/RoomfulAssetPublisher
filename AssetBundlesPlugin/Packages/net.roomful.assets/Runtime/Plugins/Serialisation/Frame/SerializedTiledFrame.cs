using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetBundles.Serialization
{

	public class SerializedTiledFrame : MonoBehaviour, IRecreatableOnLoad {
        public float FrameOffset = 0f;
        public float BackOffset = 0f;
        public Color FillerColor = Color.white;
    }
}