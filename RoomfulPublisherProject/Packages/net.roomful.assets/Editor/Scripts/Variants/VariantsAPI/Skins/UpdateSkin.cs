using System.Collections.Generic;

namespace net.roomful.assets.editor
{
    internal class UpdateSkin : BaseWebPackage {

        private const string REQUEST_URL = "/api/v0/asset/updateSkin";
        private readonly IPropSkin m_skinModel;
        private readonly string m_assetId;
  
        public UpdateSkin(string assetId, IPropSkin skinModel) : base(REQUEST_URL)
        {
            m_assetId = assetId;
            m_skinModel = skinModel;
        }
  
        public override Dictionary<string, object> GenerateData() {
            return new Dictionary<string, object>
            {
                {"assetId", m_assetId},
                {"skin", m_skinModel.ToDictionary()}
            };
        }
    }
}