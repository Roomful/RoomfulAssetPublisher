using System.Collections.Generic;
 
namespace net.roomful.assets.editor
{
    internal class DeleteSkin : BaseWebPackage {
 
        private const string REQUEST_URL = "/api/v0/asset/deleteSkin";
        private readonly string m_assetId;
        private readonly string m_skinId;
        
        public DeleteSkin(string assetId, string skinId) : base(REQUEST_URL) {
            m_assetId = assetId;
            m_skinId = skinId;
        }
 
        public override Dictionary<string, object> GenerateData() { 
            return new Dictionary<string, object>
            {
                {"assetId", m_assetId},
                {"skinId", m_skinId}
            };
        }
     }
}