using System.Collections.Generic;

namespace net.roomful.assets.Network.Request
{
    internal abstract class AssetMetadataRequest : BaseWebPackage
    {

        private AssetTemplate m_assetTemplate;


        public AssetMetadataRequest(string url) : base(url) {}

        public override Dictionary<string, object> GenerateData() {
            var OriginalJSON = new Dictionary<string, object>();
            OriginalJSON.Add("data", m_assetTemplate.ToDictionary());

            return OriginalJSON;
        }


        protected void SetTemplate(AssetTemplate assetTemplate) {
            m_assetTemplate = assetTemplate;
        }


    }
}
