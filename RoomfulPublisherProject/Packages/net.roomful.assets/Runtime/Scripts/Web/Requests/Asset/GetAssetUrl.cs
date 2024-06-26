using System.Collections.Generic;

namespace net.roomful.assets.editor {
	internal class GetAssetUrl : BaseWebPackage {

		private const string PACK_URL = "/api/v0/asset/url/";

		private const RequestMethods PackMethodName = RequestMethods.GET;

		public GetAssetUrl (string assetId, string platform) : base (PACK_URL, PackMethodName) {
			
			AddToUrl (assetId);
			AddToUrl ("/platform/"+platform);
		}

		public override Dictionary<string, object> GenerateData () {
			var originalJSON =  new Dictionary<string, object>();
			return originalJSON;
		}
	}
}