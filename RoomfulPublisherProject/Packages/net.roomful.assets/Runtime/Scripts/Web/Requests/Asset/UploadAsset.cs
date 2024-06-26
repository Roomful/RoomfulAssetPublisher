using System.Collections.Generic;

namespace net.roomful.assets
{
    class UploadAsset : BaseWebPackage
    {
        public UploadAsset(string packUrl, byte[] data) : base(packUrl, RequestMethods.PUT) {
            m_PackData = data;
        }

        public override bool IsDataPack => true;

        public override Dictionary<string, object> GenerateData() {
            var originalJson = new Dictionary<string, object>();
            return originalJson;
        }
    }
}