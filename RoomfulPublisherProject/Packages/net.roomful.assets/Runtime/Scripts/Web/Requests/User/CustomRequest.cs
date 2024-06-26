using System.Collections.Generic;

namespace net.roomful.assets
{
    public class CustomRequest : BaseWebPackage
    {
        bool m_IsDataPack;
        public override bool IsDataPack => m_IsDataPack;
        
        public CustomRequest(string url, RequestMethods methods):base(url, methods) {
            
        }

        public void SetIsDataPack(bool isDataPack)
        {
            m_IsDataPack = isDataPack;
        }

        public override Dictionary<string, object> GenerateData()
        {
            return new Dictionary<string, object>();
        }
    }
}
