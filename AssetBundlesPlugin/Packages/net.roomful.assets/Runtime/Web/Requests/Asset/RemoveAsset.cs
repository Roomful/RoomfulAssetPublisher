using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.Network.Request {
	public class RemoveAsset : BaseWebPackage {

		public const string PackUrl = "/api/v0/asset/remove";

		private string Id;

		public RemoveAsset(string id):base(PackUrl) {
			Id = id;
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("asset", Id);

			return OriginalJSON;
		}
	}
}

