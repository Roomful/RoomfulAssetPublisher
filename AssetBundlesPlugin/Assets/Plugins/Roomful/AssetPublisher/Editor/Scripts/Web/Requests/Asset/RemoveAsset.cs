using System.Collections.Generic;

namespace RF.AssetWizzard.Network.Request {
    public class RemoveAsset : BaseWebPackage {

        private const string REQUEST_URL = "/api/v0/assetpublisher/remove";

        private string m_id;

        public RemoveAsset(string id) {
            m_id = id;
        }

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }

        public override Dictionary<string, object> GetRequestData() {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            fields.Add("asset", m_id);
            return fields;
        }
    }
}

