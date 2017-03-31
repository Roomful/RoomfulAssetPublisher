using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class UpdateAsset : BaseWebPackage {

		public const string PackUrl = "/api/v0/asset/update";

//		private string Id;
//		private AssetTemplate Tpl;

		public UpdateAsset(string id, AssetTemplate tpl):base(PackUrl) {
			_Headers.Add("x-session-id", AssetBundlesSettings.Instance.SessionId);

//			Id = id;
//			Tpl = tpl;
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}
