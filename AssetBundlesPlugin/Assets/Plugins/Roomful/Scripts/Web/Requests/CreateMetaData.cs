using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class CreateMetaData : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/create";

		private AssetTemplate _Template;

		public CreateMetaData(AssetTemplate template):base(PackUrl) {
			_Template = template;
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();
			OriginalJSON.Add ("data", _Template.ToDictionary());

			return OriginalJSON;
		}

	}
}
