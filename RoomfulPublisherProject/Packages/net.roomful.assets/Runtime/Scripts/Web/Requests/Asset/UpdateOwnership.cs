using System.Collections.Generic;

namespace net.roomful.assets
{
    
    /// <summary>
    /// https://github.com/Roomful/RoomfulUnity1/wiki/RF_API-Assets#update-asset-ownership
    /// </summary>
    class UpdateOwnership : BaseWebPackage
    {
        const string k_PackURL = "/api/v0/asset/updateOwnership";

        readonly string m_AssetId;
        readonly string m_NetworkId;
        
        public UpdateOwnership(string assetId, string networkId) : base(k_PackURL)
        {
            m_AssetId = assetId;
            m_NetworkId = networkId;
        }

        public override Dictionary<string, object> GenerateData()
        {
            var originalJson = new Dictionary<string, object>();

            originalJson.Add("assetId", m_AssetId);
            originalJson.Add("networkId", m_NetworkId);
            return originalJson;
        }
    }
}
