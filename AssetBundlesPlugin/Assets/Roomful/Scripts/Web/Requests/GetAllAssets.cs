using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class GetAllAssets : BaseWebPackage {

		public const string PackUrl = "/api/v0/asset/list";

		public GetAllAssets():base(PackUrl) {
			_Headers.Add("x-session-id", AssetBundlesSettings.Instance.SessionId);
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}
