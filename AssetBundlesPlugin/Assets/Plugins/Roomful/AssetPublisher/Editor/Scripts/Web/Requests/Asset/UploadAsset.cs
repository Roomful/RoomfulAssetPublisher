using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class UploadAsset : BaseWebPackage {

		private const RequestMethods PackMethodName = RequestMethods.PUT;
        private string m_url;
		public UploadAsset (string packUrl, byte[] data) {
            m_url = packUrl;
            m_packData = data;
		}

		public override bool IsDataPack {
			get {
				return true;
			}
		}

        public override string Url {
            get {
                return m_url;
            }
        }

        public override RequestMethods MethodName {
            get {
                return RequestMethods.PUT;
            }
        }

        public override Dictionary<string, object> GetRequestData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}