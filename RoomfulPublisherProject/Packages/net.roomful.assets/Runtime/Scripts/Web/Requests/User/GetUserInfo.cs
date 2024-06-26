using System.Collections.Generic;

namespace net.roomful.assets
{
    
    class GetUserInfo : BaseWebPackage  {

        public const string PackUrl = "/api/v0/rpc/user.info";
        
        public GetUserInfo():base(PackUrl, RequestMethods.POST) {
            
        }
        
        public override Dictionary<string, object> GenerateData () {
            var originalJson =  new Dictionary<string, object>();
            return originalJson;
        }
    }
}
