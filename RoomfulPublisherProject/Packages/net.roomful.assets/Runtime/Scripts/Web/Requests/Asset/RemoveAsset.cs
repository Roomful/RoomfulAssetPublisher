using System.Collections.Generic;

namespace net.roomful.assets {
	class RemoveAsset : BaseWebPackage {

		public const string PackUrl = "/api/v0/asset/remove";
		readonly string m_Id;

		public RemoveAsset(string id):base(PackUrl) {
			m_Id = id;
		}

		public override Dictionary<string, object> GenerateData () {
			var originalJson =  new Dictionary<string, object>();
			originalJson.Add ("asset", m_Id);

			return originalJson;
		}
	}
}

