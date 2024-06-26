using System.Collections.Generic;

namespace net.roomful.assets.editor
{
    internal class GetSkinUploadLink : BaseWebPackage
    {
        private const string PACK_URL = "/api/v0/asset/skin/upload/link";

        private readonly string m_assetId;
        private readonly string m_skinId;
        private readonly string m_platform;
        private readonly string m_assetTitle;

        public GetSkinUploadLink(string assetId, string skinId, string platform, string skinName) : base(PACK_URL) {
            m_assetId = assetId;
            m_skinId = skinId;
            m_platform = platform;
            m_assetTitle = skinName;
        }

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object>();

            originalJSON.Add("assetId", m_assetId);
            originalJSON.Add("skinId", m_skinId);
            originalJSON.Add("platform", m_platform);
            originalJSON.Add("fileName", m_assetTitle);

            return originalJSON;
        }
    }
}