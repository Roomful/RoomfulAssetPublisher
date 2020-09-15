﻿using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
	public class RemoveAsset : BaseWebPackage {

		public const string PackUrl = "/api/v0/asset/remove";

		private readonly string Id;

		public RemoveAsset(string id):base(PackUrl) {
			Id = id;
		}

		public override Dictionary<string, object> GenerateData () {
			var OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("asset", Id);

			return OriginalJSON;
		}
	}
}
