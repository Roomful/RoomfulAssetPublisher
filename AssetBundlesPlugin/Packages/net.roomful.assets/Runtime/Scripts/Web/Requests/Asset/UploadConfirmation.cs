using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
	public class UploadConfirmation : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/upload/link/complete";

		private readonly string _AssetId;
		private readonly string _Platform;

		public UploadConfirmation (string assetId, string platform) : base (PackUrl) {
			_AssetId = assetId;
			_Platform = platform;
		}

		public override Dictionary<string, object> GenerateData () {
			var OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("asset", _AssetId);
			OriginalJSON.Add ("platform", _Platform);

			return OriginalJSON;
		}
	}
}
