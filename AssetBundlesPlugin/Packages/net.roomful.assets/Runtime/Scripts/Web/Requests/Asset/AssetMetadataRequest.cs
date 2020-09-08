using System.Collections.Generic;

namespace net.roomful.assets.Network.Request
{
    public abstract class AssetMetadataRequest : BaseWebPackage
    {

        private Template m_template;


        public AssetMetadataRequest(string url) : base(url) {}

        public override Dictionary<string, object> GenerateData() {
            var OriginalJSON = new Dictionary<string, object>();
            OriginalJSON.Add("data", m_template.ToDictionary());

            return OriginalJSON;
        }


        protected void SetTemplate(Template template) {
            m_template = template;
        }


    }
}
