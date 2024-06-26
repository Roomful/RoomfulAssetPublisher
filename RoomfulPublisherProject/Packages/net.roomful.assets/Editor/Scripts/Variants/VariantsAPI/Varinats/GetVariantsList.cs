using System.Collections.Generic;
 
namespace net.roomful.assets.editor
{
    internal class GetVariantsList : BaseWebPackage {

        private const string RequestUrl = "/api/v0/asset/listVariants";
        private readonly string m_assetId;

        public GetVariantsList(string assetId) : base(RequestUrl)
        {
            m_assetId = assetId;
        }

        public override Dictionary<string, object> GenerateData() {
            return new Dictionary<string, object> {{"assetId", m_assetId}};
        }
    }
}