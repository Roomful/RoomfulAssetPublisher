using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.serialization
{

	[System.Serializable]
	public class SerializedPlatformTextureSettings {
		public string  	Platform;
		public int     	MaxTextureSize;
		public string  	TextureFormat;
		public int     	CompressionQuality;
		public bool 	Etc1AlphaSplitEnabled;
	}
}