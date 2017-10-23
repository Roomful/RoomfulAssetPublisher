using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetBundles.Serialization
{
	public class SerializedThumbnail : MonoBehaviour, IRecreatableOnLoad
    {

		public bool IsBoundToResourceIndex = false;
		public int ResourceIndex = 0;

		public bool IsFixedRatio = false;
		public int XRatio = 1;
		public int YRatio = 1;

    }
}




