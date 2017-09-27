using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RF.AssetBundles {

	[Serializable]
	public class ShaderProperty {

		public string PropertyName = string.Empty;
		public string  PropertyType = string.Empty;

		public Vector4 VectorValue;
		public float FloatValue;
		public SerializedTexture SerializedTextureValue = null;
	}
}