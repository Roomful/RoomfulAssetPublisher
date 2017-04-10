using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class UpdateAsset : BaseWebPackage {

		public const string PackUrl = "/api/v0/asset/update";

		private AssetTemplate Tpl;

		public UpdateAsset(AssetTemplate tpl):base(PackUrl) {
			_Headers.Add("x-session-id", AssetBundlesSettings.Instance.SessionId);

			Tpl = tpl;
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("data", Tpl.ToDictionaryWithoutInfo ());

			return OriginalJSON;
		}
	}
}
