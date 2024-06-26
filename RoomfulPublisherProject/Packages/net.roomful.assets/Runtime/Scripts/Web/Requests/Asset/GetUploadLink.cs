using System.Collections.Generic;

namespace net.roomful.assets.editor {
	class GetUploadLink : BaseWebPackage {
		const string k_PackURL = "/api/v0/asset/upload/link";

		readonly string m_AssetId;
		readonly string m_Platform;
		readonly string m_AssetTitle;

		public GetUploadLink (string assetId, string platform, string fileName) : base (k_PackURL) {
			m_AssetId = assetId;
			m_Platform = platform;
			m_AssetTitle = fileName;
		}

		public override Dictionary<string, object> GenerateData () {
			var originalJson =  new Dictionary<string, object>();

			originalJson.Add ("asset", m_AssetId);
			originalJson.Add ("platform", m_Platform);
			originalJson.Add ("fileName", m_AssetTitle);

			return originalJson;
		}
	}
}
