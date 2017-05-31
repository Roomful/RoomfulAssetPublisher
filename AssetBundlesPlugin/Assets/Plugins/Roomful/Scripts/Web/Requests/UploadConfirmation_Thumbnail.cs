using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class UploadConfirmation_Thumbnail : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/upload/thumbnail/link/complete";

		private string _AssetId;

		public UploadConfirmation_Thumbnail (string assetId) : base (PackUrl) {
			_AssetId = assetId;
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("asset", _AssetId);

			return OriginalJSON;
		}
	}
}
