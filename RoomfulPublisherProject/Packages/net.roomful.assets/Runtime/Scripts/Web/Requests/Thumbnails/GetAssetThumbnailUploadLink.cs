using System.Collections.Generic;

namespace net.roomful.assets.editor
{
    internal class GetAssetThumbnailUploadLink : BaseWebPackage
    {
        private const string PACK_URL = "/api/v0/asset/upload/thumbnail/link";

        private readonly string m_assetId;

        public GetAssetThumbnailUploadLink(string assetId) : base(PACK_URL) {
            m_assetId = assetId;
        }

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object>();

            originalJSON.Add("asset", m_assetId);
            originalJSON.Add("contentType", "image/png");

            return originalJSON;
        }
    }
}