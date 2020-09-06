using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.Network.Request {
	public class GetUploadLink_Thumbnail : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/upload/thumbnail/link";

		private string _AssetId;
	

		public GetUploadLink_Thumbnail (string assetId) : base (PackUrl) {
			_AssetId = assetId;

		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("asset", _AssetId);
			OriginalJSON.Add ("contentType", "image/png");

			return OriginalJSON;
		}
	}
}
