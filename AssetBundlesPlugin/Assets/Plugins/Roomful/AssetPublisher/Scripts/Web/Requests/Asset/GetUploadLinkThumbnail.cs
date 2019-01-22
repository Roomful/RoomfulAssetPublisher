using System.Collections.Generic;

namespace RF.AssetWizzard.Network.Request {
    public class GetUploadLinkThumbnail : BaseWebPackage {

        private const string REQUEST_URL = "/api/v0/asset/upload/thumbnail/link";

        private string m_assetId;

        public GetUploadLinkThumbnail(string assetId) {
            m_assetId = assetId;

        }

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }

        public override Dictionary<string, object> GetRequestData() {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            fields.Add("asset", m_assetId);
            fields.Add("contentType", "image/png");
            return fields;
        }
    }
}
