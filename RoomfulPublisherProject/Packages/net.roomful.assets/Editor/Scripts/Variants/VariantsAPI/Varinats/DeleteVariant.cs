using System.Collections.Generic;
 
namespace net.roomful.assets.editor
{
    internal class DeleteVariant : BaseWebPackage {
 
        private const string REQUEST_URL = "/api/v0/asset/deleteVariant";
        private readonly string m_assetId;
        private readonly string m_variantId;
        
        public DeleteVariant(string assetId, string variantId) : base(REQUEST_URL) {
            m_assetId = assetId;
            m_variantId = variantId;
        }
 
        public override Dictionary<string, object> GenerateData() { 
            return new Dictionary<string, object>
            {
                {"assetId", m_assetId},
                {"variantId", m_variantId}
            };
        }
    }
}