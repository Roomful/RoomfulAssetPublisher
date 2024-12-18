using System.Collections.Generic;

namespace net.roomful.assets.editor
{
    internal class ConfirmSkinThumbnailUpload : BaseWebPackage
    {
        private const string PACK_URL = "/api/v0/asset/upload/skin/thumbnail/link/complete";

        private readonly string m_assetId;
        private readonly string m_skinId;

        public ConfirmSkinThumbnailUpload(string assetId, string skinId) : base(PACK_URL) {
            m_assetId = assetId;
            m_skinId = skinId;
        }

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object>();

            originalJSON.Add("assetId", m_assetId);
            originalJSON.Add("skinId", m_skinId);
            return originalJSON;
        }
    }
}