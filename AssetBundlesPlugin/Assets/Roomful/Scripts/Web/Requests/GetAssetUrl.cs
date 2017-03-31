using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class GetAssetUrl : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/url/";

		private const RequestMethods PackMethodName = RequestMethods.GET;

		public GetAssetUrl (string assetId) : base (PackUrl, PackMethodName) {
			_Headers.Add ("x-session-id", AssetBundlesSettings.Instance.SessionId);

			AddToUrl (assetId);
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}