namespace net.roomful.assets.serialization
{

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