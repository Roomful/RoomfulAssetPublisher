using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class UploadConfirmation : BaseWebPackage {

		private const string REQUEST_URL = "/api/v0/assetpublisher/upload/link/complete";

		private string m_assetId;
		private string m_platform;

		public UploadConfirmation (string assetId, string platform) {
			m_assetId = assetId;
			m_platform = platform;
		}

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }

        public override Dictionary<string, object> GetRequestData () {
			Dictionary<string, object> fields =  new Dictionary<string, object>();
            fields.Add ("asset", m_assetId);
			fields.Add ("platform", m_platform);
            return fields;
		}
	}
}
