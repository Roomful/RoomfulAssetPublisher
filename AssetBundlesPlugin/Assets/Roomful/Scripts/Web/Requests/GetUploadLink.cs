using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class GetUploadLink : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/upload/link";

		public GetUploadLink (string assetId) : base (PackUrl) {
			_Headers.Add ("x-session-id", AssetBundlesSettings.Instance.SessionId);
			_Headers.Add ("x-asset-id", assetId);
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}

	}
}
