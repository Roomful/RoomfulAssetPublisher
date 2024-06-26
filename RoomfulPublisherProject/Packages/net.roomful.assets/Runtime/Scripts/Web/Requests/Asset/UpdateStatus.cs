using System.Collections.Generic;

namespace net.roomful.assets
{
    class UpdateStatus : BaseWebPackage
    {
        const string k_PackURL = "/api/v0/asset/updateStatus";

        readonly string m_AssetId;
        readonly AssetStatus m_Status;

        public UpdateStatus(string assetId, AssetStatus status)
            : base(k_PackURL)
        {
            m_AssetId = assetId;
            m_Status = status;
        }

        public override Dictionary<string, object> GenerateData()
        {
            var originalJson = new Dictionary<string, object>();

            originalJson.Add("assetId", m_AssetId);
            originalJson.Add("status", m_Status.ToString());
            return originalJson;
        }
    }
}
