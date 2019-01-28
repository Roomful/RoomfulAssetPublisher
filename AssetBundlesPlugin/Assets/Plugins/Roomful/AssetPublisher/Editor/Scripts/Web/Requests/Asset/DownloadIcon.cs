using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard.Network.Request {
	public class DownloadIcon : BaseWebPackage {

        private string m_url;

        public DownloadIcon(string url) {
            m_url = url;
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
                return RequestMethods.GET;
            }
        }
    }
}