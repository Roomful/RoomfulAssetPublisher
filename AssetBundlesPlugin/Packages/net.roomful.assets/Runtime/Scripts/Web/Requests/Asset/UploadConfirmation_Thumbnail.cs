using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
	internal class UploadConfirmation_Thumbnail : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/upload/thumbnail/link/complete";

		private readonly string _AssetId;

		public UploadConfirmation_Thumbnail (string assetId) : base (PackUrl) {
			_AssetId = assetId;
		}

		public override Dictionary<string, object> GenerateData () {
			var OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("asset", _AssetId);

			return OriginalJSON;
		}
	}
}
