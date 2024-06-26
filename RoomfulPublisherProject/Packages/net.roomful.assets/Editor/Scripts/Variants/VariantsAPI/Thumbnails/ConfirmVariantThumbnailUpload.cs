using System.Collections.Generic;

namespace net.roomful.assets.editor
{
    internal class ConfirmVariantThumbnailUpload : BaseWebPackage
    {
        private const string PACK_URL = "/api/v0/asset/upload/variant/thumbnail/link/complete";

        private readonly string m_assetId;
        private readonly string m_variantId;

        public ConfirmVariantThumbnailUpload(string assetId, string variantId) : base(PACK_URL) {
            m_assetId = assetId;
            m_variantId = variantId;
        }

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object>();

            originalJSON.Add("assetId", m_assetId);
            originalJSON.Add("variantId", m_variantId);

            return originalJSON;
        }
    }
}