using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class UploadConfirmation : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/upload/link/complete";

		public UploadConfirmation (string assetId) : base (PackUrl) {
			_Headers.Add ("x-session-id", AssetBundlesSettings.Instance.SessionId);
			_Headers.Add ("x-asset-id", assetId);
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}
