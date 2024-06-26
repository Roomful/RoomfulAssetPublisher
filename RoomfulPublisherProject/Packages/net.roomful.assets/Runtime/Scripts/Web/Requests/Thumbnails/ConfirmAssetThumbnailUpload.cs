using System.Collections.Generic;

namespace net.roomful.assets.editor
{
    internal class ConfirmAssetThumbnailUpload : BaseWebPackage
    {
        private const string PACK_URL = "/api/v0/asset/upload/thumbnail/link/complete";

        private readonly string m_assetId;

        public ConfirmAssetThumbnailUpload(string assetId) : base(PACK_URL) {
            m_assetId = assetId;
        }

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object>();

            originalJSON.Add("asset", m_assetId);

            return originalJSON;
        }
    }
}