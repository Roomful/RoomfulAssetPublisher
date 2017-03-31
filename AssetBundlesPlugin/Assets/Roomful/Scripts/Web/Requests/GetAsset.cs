using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class GetAsset : BaseWebPackage {
		
		private const RequestMethods PackMethodName = RequestMethods.GET;

		public GetAsset (string url) : base (url, PackMethodName) {
			_Headers.Add ("x-session-id", AssetBundlesSettings.Instance.SessionId);
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
