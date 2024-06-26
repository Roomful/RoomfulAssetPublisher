using System.Collections.Generic;

namespace net.roomful.assets.editor
{
    internal class SkinUploadConfirmation : BaseWebPackage
    {
        private const string PACK_URL = "/api/v0/asset/skin/upload/link/complete";

        private readonly string m_assetId;
        private readonly string m_skinId;
        private readonly string m_platform;

        public SkinUploadConfirmation(string assetId, string skinId, string platform) : base(PACK_URL) {
            m_assetId = assetId;
            m_skinId = skinId;
            m_platform = platform;
        }

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object>();

            originalJSON.Add("assetId", m_assetId);
            originalJSON.Add("skinId", m_skinId);
            originalJSON.Add("platform", m_platform);

            return originalJSON;
        }
    }
}