using System.Collections.Generic;

namespace net.roomful.assets.editor
{
    internal class CreateVariant : BaseWebPackage
    {
        private const string REQUEST_URL = "/api/v0/asset/createVariant";
        private readonly IPropVariant m_variantModel;
        private readonly string m_assetId;

        public CreateVariant(string assetId, IPropVariant variantModel) : base(REQUEST_URL)
        {
            m_assetId = assetId;
            m_variantModel = variantModel;
        }

        public override Dictionary<string, object> GenerateData()
        {
            return new Dictionary<string, object>
            {
                {"assetId", m_assetId},
                {"variant", m_variantModel.ToDictionary()}
            };
        }
    }
}