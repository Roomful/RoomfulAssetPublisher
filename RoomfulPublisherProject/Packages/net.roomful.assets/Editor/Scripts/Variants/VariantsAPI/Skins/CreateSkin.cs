using System.Collections.Generic;
 
namespace net.roomful.assets.editor
{
    internal class CreateSkin : BaseWebPackage {

        private const string REQUEST_URL = "/api/v0/asset/createSkin";
        private readonly string m_assetId;
        private readonly IPropSkin m_skinModel;

        public CreateSkin(string assetId, IPropSkin skinModel) : base(REQUEST_URL)
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