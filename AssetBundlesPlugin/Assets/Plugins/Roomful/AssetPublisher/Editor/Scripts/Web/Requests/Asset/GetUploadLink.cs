using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class GetUploadLink : BaseWebPackage {

		private const string REQUEST_URL = "/api/v0/assetpublisher/upload/link";

		private string m_assetId;
		private string m_platform;
		private string m_filename;

		public GetUploadLink (string assetId, string platform, string filename) {
			m_assetId = assetId;
			m_platform = platform;
			m_filename = filename;
		}

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }

        public override Dictionary<string, object> GetRequestData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("asset", m_assetId);
			OriginalJSON.Add ("platform", m_platform);
			OriginalJSON.Add ("fileName", m_filename);

			return OriginalJSON;
		}
	}
}
