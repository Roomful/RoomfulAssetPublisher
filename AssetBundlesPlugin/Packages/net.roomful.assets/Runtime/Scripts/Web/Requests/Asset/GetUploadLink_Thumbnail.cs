using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
	public class GetUploadLink_Thumbnail : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/upload/thumbnail/link";

		private readonly string _AssetId;
	

		public GetUploadLink_Thumbnail (string assetId) : base (PackUrl) {
			_AssetId = assetId;

		}

		public override Dictionary<string, object> GenerateData () {
			var OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("asset", _AssetId);
			OriginalJSON.Add ("contentType", "image/png");

			return OriginalJSON;
		}
	}
}
