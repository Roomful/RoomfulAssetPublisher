using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class CreateEnvironmentMetaData : BaseWebPackage {

		private const string PackUrl = "/api/v0/environment/create";

		private EnvironmentTemplate _Template;

		public CreateEnvironmentMetaData(EnvironmentTemplate template):base(PackUrl) {
			_Template = template;
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();
			OriginalJSON.Add ("data", _Template.ToDictionary());

			return OriginalJSON;
		}

	}
}
