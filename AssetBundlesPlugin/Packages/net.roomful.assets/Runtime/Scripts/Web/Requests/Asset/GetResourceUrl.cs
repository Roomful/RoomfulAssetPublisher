using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
	internal class GetResourceUrl : BaseWebPackage {

		private const string PackUrl = "/api/v0/resource/url/";

		private const RequestMethods PackMethodName = RequestMethods.GET;

		public GetResourceUrl (string id) : base (PackUrl, PackMethodName) {
			AddToUrl (id);
		}

		public override Dictionary<string, object> GenerateData () {
			var OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}