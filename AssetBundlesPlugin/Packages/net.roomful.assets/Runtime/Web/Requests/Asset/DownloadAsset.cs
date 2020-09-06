using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.Network.Request {
	public class DownloadAsset : BaseWebPackage {
		
		private const RequestMethods PackMethodName = RequestMethods.GET;

		public DownloadAsset (string url) : base (url, PackMethodName) {
			
		}

		public override bool IsDataPack {
			get {
				return true;
			}
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}
