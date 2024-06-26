using System.Collections.Generic;

namespace net.roomful.assets {
	internal class UploadConfirmation : BaseWebPackage {

		private const string PACK_URL = "/api/v0/asset/upload/link/complete";

		private readonly string m_assetId;
		private readonly string m_platform;

		public UploadConfirmation (string assetId, string platform) : base (PACK_URL) {
			m_assetId = assetId;
			m_platform = platform;
		}

		public override Dictionary<string, object> GenerateData () {
			var originalJSON =  new Dictionary<string, object>();

			originalJSON.Add ("asset", m_assetId);
			originalJSON.Add ("platform", m_platform);

			return originalJSON;
		}
	}
}
