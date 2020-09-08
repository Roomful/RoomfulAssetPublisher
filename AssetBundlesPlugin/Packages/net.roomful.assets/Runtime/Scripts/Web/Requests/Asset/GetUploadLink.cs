using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
	public class GetUploadLink : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/upload/link";

		private readonly string _AssetId;
		private readonly string _Platform;
		private readonly string _AssetTitle;

		public GetUploadLink (string assetId, string platform, string fileName) : base (PackUrl) {
			_AssetId = assetId;
			_Platform = platform;
			_AssetTitle = fileName;
		}

		public override Dictionary<string, object> GenerateData () {
			var OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("asset", _AssetId);
			OriginalJSON.Add ("platform", _Platform);
			OriginalJSON.Add ("fileName", _AssetTitle);

			return OriginalJSON;
		}
	}
}
