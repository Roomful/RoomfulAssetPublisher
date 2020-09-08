using UnityEngine;
using System;

namespace net.roomful.assets {
	[Serializable]
	public class AssetUrl  {

		[SerializeField] string _Platform;
		[SerializeField] string _Url;

		public AssetUrl(string platform, string url) {
			_Platform = platform;
			_Url = url;
		}

		public string Platform => _Platform;

		public string Url => _Url;
	}
}
