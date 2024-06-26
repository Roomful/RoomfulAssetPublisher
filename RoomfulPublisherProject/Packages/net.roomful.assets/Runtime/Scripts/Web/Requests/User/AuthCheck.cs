using System.Collections.Generic;

namespace net.roomful.assets {
    
    class AuthCheck : BaseWebPackage  {

        public const string PackUrl = "/auth/check";
        public override bool ShouldDisplayAnErrorPopup => false;

        public AuthCheck():base(PackUrl, RequestMethods.GET) {
            
        }
        
        public override Dictionary<string, object> GenerateData () {
            var originalJson =  new Dictionary<string, object>();
            return originalJson;
        }
    }
}