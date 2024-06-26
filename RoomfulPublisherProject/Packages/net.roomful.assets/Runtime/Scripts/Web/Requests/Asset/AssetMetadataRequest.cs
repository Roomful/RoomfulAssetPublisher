using System.Collections.Generic;

namespace net.roomful.assets
{
    internal abstract class AssetMetadataRequest : BaseWebPackage
    {
        private AssetTemplate m_assetTemplate;

        protected AssetMetadataRequest(string url) : base(url) { }

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object> { { "data", m_assetTemplate.ToDictionary() } };

            return originalJSON;
        }

        protected void SetTemplate(AssetTemplate assetTemplate) {
            m_assetTemplate = assetTemplate;
        }
    }
}
