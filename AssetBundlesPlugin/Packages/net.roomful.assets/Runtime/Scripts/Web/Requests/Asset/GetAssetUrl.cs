using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
	internal class GetAssetUrl : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/url/";

		private const RequestMethods PackMethodName = RequestMethods.GET;

		public GetAssetUrl (string assetId, string platform) : base (PackUrl, PackMethodName) {
			
			AddToUrl (assetId);
			AddToUrl ("/platform/"+platform);
		}

		public override Dictionary<string, object> GenerateData () {
			var OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}