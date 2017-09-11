using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RF.AssetWizzard {
	[Serializable]
	public class AssetUrl  {

		[SerializeField] string _Platform;
		[SerializeField] string _Url;

		public AssetUrl(string platform, string url) {
			_Platform = platform;
			_Url = url;
		}

		public string Platform {
			get {
				return _Platform;
			}
		}

		public string Url {
			get {
				return _Url;
			}
		}
	}
}
