using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetBundles {

	[System.Serializable]
	public class SerializedDefaultTextureSettings {
		public bool 	AllowsAlphaSplitting;
		public int     	CompressionQuality;
		public bool 	CrunchedCompression;
		public string  	TextureFormat;
		public int     	MaxTextureSize;
		public string  	Platform;
		public bool 	Overridden;
		public string	TextureCompression;
	}
}