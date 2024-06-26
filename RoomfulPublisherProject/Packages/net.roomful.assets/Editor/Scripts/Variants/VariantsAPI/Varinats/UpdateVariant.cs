using System.Collections.Generic;

namespace net.roomful.assets.editor
{
    internal class UpdateVariant : BaseWebPackage {

        private const string RequestUrl = "/api/v0/asset/updateVariant";
        private readonly IPropVariant m_variantModel;
        private readonly string m_assetId;
  
        public UpdateVariant(string assetId, IPropVariant variantModel) : base(RequestUrl)
        {
            m_assetId = assetId;
            m_variantModel = variantModel;
        }
  
        public override Dictionary<string, object> GenerateData() {
            return new Dictionary<string, object>
            {
                {"assetId", m_assetId},
                {"variant", m_variantModel.ToDictionary()}
            };
        }
    }
}