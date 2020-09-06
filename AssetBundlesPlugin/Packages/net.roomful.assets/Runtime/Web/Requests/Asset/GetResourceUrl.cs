using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.Network.Request {
	public class GetResourceUrl : BaseWebPackage {

		private const string PackUrl = "/api/v0/resource/url/";

		private const RequestMethods PackMethodName = RequestMethods.GET;

		public GetResourceUrl (string id) : base (PackUrl, PackMethodName) {
			AddToUrl (id);
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}