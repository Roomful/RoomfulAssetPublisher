using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class GetAssetUrl : BaseWebPackage {

		private const string PackUrl = "/api/v0/assetpublisher/url/{0}/platform/{1}";

		private const RequestMethods PackMethodName = RequestMethods.GET;
        private string m_assetId;
        private string m_platform;
		public GetAssetUrl (string assetId, string platform) {
            m_assetId = assetId;
            m_platform = platform;

		}

        public override string Url {
            get {
                return string.Format(PackUrl, m_assetId, m_platform);
            }
        }

        public override RequestMethods MethodName {
            get {
                return RequestMethods.GET;
            }
        }
    }
}